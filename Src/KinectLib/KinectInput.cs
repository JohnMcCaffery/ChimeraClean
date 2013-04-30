/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NuiLibDotNet;
using Chimera.Util;
using OpenMetaverse;
using Chimera.Kinect.GUI;
using C = NuiLibDotNet.Condition;
using Chimera.Kinect.Interfaces;
using System.Drawing;
using Chimera.Plugins;

namespace Chimera.Kinect {
    public class KinectInput : ISystemPlugin, IKinectController {
        private readonly Dictionary<string, DeltaBasedInput> mMovementControllers = new Dictionary<string, DeltaBasedInput>();
        private readonly Dictionary<string, IHelpTrigger> mHelpTriggers = new Dictionary<string, IHelpTrigger>();
        private readonly List<IKinectCursorFactory> mCursorFactories = new List<IKinectCursorFactory>();
        private readonly Dictionary<string, Dictionary<string, IKinectCursor>> mCursors = new Dictionary<string, Dictionary<string, IKinectCursor>>();
        private readonly HashSet<Action> mRedraws = new HashSet<Action>();

        private Coordinator mCoordinator;
        private bool mEnabled;
        private bool mKinectStarted;
        private bool mHeadEnabled;
        private Vector3 mKinectPosition;
        private Rotation mKinectOrientation;
        private KinectPanel mPanel;
        private Matrix4 mKinectToRealSpace;
        private EventHandler mOrientationChangeHandler;

        private Vector mHead;

        private DeltaBasedInput mCurrentMovementController;
        private IHelpTrigger mCurrentHelpTrigger;
        private Dictionary<string, IKinectCursor> mCurrentCursors = null;

        public event Action<Vector3> PositionChanged;
        public event Action<Rotation> OrientationChanged;

        /// <summary>
        /// Get the cursor input associated with a specific window.
        /// </summary>
        /// <param name="window">The name of the window to get the input for.</param>
        /// <returns>The WindowInput object which is calculating whether the user is pointing at the specified window.</returns>
        public IKinectCursor this[string window] {
            get { return mCurrentCursors.ContainsKey(window) ? mCurrentCursors[window] : null; }
        }

        public string[] CursorNames {
            get { return mCursors.Keys.ToArray(); }
        }
        public DeltaBasedInput[] MovementControllers {
            get { return mMovementControllers.Values.ToArray(); }
        }
        public IHelpTrigger[] HelpTriggers {
            get { return mHelpTriggers.Values.ToArray(); }
        }

        /// <summary>
        /// All the window inputs this input associates with the windows the system renders to.
        /// </summary>
        public IKinectCursor[] Cursors {
            get { return mCurrentCursors.Values.ToArray(); }
        }
        public DeltaBasedInput MovementController {
            get { return mCurrentMovementController; }
        }
        public IHelpTrigger HelpTrigger {
            get { return mCurrentHelpTrigger; }
        }

        public Vector3 Position {
            get { return mKinectPosition; }
            set {
                mKinectPosition = value;
                KinectSetupChanged();
                if (PositionChanged != null)
                    PositionChanged(value);
            }
        }

        public Rotation Orientation {
            get { return mKinectOrientation; }
            set {
                if (mKinectOrientation != null)
                    mKinectOrientation.Changed -= mOrientationChangeHandler;
                mKinectOrientation = value;
                mKinectOrientation_Changed(value, null);
                mKinectOrientation.Changed += mOrientationChangeHandler;
            }
        }

        public bool KinectStarted {
            get { return mKinectStarted; }
        }

        public bool HeadEnabled {
            get { return mHeadEnabled; }
            set {
                mHeadEnabled = value;
                if (value)
                    mHead_OnChange();
                else
                    mCoordinator.EyePosition = Vector3.Zero;
                if (EnabledChanged != null)
                    EnabledChanged(this, mEnabled);
            }
        }

        public KinectInput(IEnumerable<IDeltaInput> movementControllers, IEnumerable<IHelpTrigger> helpTriggers, params IKinectCursorFactory[] cursors) {
            KinectConfig cfg = new KinectConfig();

            mOrientationChangeHandler = new EventHandler(mKinectOrientation_Changed);

            mKinectPosition = cfg.Position;
            mKinectOrientation = new Rotation(cfg.Pitch, cfg.Yaw);
            mKinectOrientation.Changed += new EventHandler(mKinectOrientation_Changed);
            mCursorFactories = new List<IKinectCursorFactory>(cursors);
            mEnabled = cfg.Enabled;
            mHeadEnabled = cfg.EnableHead;

            Nui.SkeletonLost += new SkeletonTrackDelegate(Nui_SkeletonLost);

            mHead = Nui.joint(Nui.Head);
            mHead.OnChange += new ChangeDelegate(mHead_OnChange);

            foreach (var movement in movementControllers) {
                DeltaBasedInput put = new DeltaBasedInput(movement);
                mMovementControllers.Add(movement.Name, put);
                if (mCurrentMovementController == null) {
                    mCurrentMovementController = put;
                    mCurrentMovementController.Enabled = true;
                } else
                    movement.Enabled = false;
            }

            foreach (var helpTrigger in helpTriggers) {
                mHelpTriggers.Add(helpTrigger.Name, helpTrigger);
                helpTrigger.Init(mCoordinator);
                helpTrigger.Triggered += new Action<IHelpTrigger>(helpTrigger_Triggered);
                if (mCurrentHelpTrigger == null)
                    mCurrentHelpTrigger = helpTrigger;
                else
                    helpTrigger.Enabled = false;
            }

            foreach (var factory in mCursorFactories) {
                mCursors.Add(factory.Name, new Dictionary<string, IKinectCursor>());

                if (mCurrentCursors == null)
                    mCurrentCursors = mCursors[factory.Name];
            }

            if (cfg.Autostart)
                StartKinect();
        }

        private void KinectSetupChanged() {
             mKinectToRealSpace = Matrix4.CreateFromQuaternion(mKinectOrientation.Quaternion) * Matrix4.CreateTranslation(mKinectPosition);
             foreach (var redraw in mRedraws)
                 redraw();
        }

        void mHead_OnChange() {
            if (mHeadEnabled) {
                Vector3 hKinect = new Vector3(mHead.Z, -mHead.X, mHead.Y) * 1000f;
                //if (Nui.HasSkeleton)
                if (!hKinect.Equals(Vector3.Zero))
                    mCoordinator.EyePosition = hKinect * mKinectToRealSpace;
                else
                    mCoordinator.EyePosition = Vector3.Zero;
            }
        }

        void mKinectOrientation_Changed(object sender, EventArgs e) {
            KinectSetupChanged();
            if (OrientationChanged != null)
                OrientationChanged(mKinectOrientation);
        }
        public void StartKinect() {
            if (!mKinectStarted) {
                Nui.Init();
                Nui.SetAutoPoll(true);
                Nui.SkeletonLost += new SkeletonTrackDelegate(Nui_SkeletonLost);
                mKinectStarted = true;
            }
        }

        void Nui_SkeletonLost() {
            mCoordinator.EyePosition = Vector3.Zero;
            if (mCoordinator.ControlMode == ControlMode.Delta)
                mCoordinator.Update(Vector3.Zero, Vector3.Zero, Rotation.Zero, Rotation.Zero);
            foreach (var window in mCoordinator.Windows)
                window.OverlayManager.MoveCursorOffScreen();
        }

        public void SetCursor(string cursorName) {
            if (mCursors.ContainsKey(cursorName)) {
                foreach (var cursor in Cursors)
                    cursor.Enabled = false;
                mCurrentCursors = mCursors[cursorName];
                foreach (var cursor in Cursors)
                    cursor.Enabled = true;
            }
        }

        public void SetMovement(string controllerName) {
            if (mMovementControllers.ContainsKey(controllerName)) {
                mCurrentMovementController.Enabled = false;
                mCurrentMovementController = mMovementControllers[controllerName];
                mCurrentMovementController.Enabled = true;
            }
        }

        public void SetHelpTrigger(string helpTriggerName) {
            if (mHelpTriggers.ContainsKey(helpTriggerName)) {
                mCurrentHelpTrigger.Enabled = false;
                mCurrentHelpTrigger = mHelpTriggers[helpTriggerName];
                mCurrentHelpTrigger.Enabled = true;
            }
        }

        public bool FlyEnabled {
            get { return mCurrentMovementController.FlyEnabled; }
            set { mCurrentMovementController.FlyEnabled = value; }
        }

        public bool WalkEnabled {
            get { return mCurrentMovementController.WalkEnabled; }
            set { mCurrentMovementController.WalkEnabled = value; }
        }

        public bool YawEnabled {
            get { return mCurrentMovementController.YawEnabled; }
            set { mCurrentMovementController.YawEnabled = value; }
        }

        #region ISystemPlugin Members

        public event Action<IPlugin, bool> EnabledChanged;

        public UserControl ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new KinectPanel(this);
                return mPanel;
            }
        }

        public string Name {
            get { return "Kinect"; }
        }

        public bool Enabled {
            get { return mEnabled; }
            set { 
                mEnabled = value;
                foreach (var redraw in mRedraws)
                    redraw();
                if (EnabledChanged != null)
                    EnabledChanged(this, value);
            }
        }

        public ConfigBase Config {
            get { throw new NotImplementedException(); }
        }

        public string State {
            get { return "Kinect State"; }
        }

        public Coordinator Coordinator {
            get { return mCoordinator; }
        }

        public void Init(Coordinator coordinator) {
            mCoordinator = coordinator;

            foreach (var window in mCoordinator.Windows) {
                mCoordinator_WindowAdded(window, null);
            }

            foreach (var input in mMovementControllers.Values)
                input.Init(coordinator);

            mCoordinator.WindowAdded += new Action<Window,EventArgs>(mCoordinator_WindowAdded);
        }

        public void Close() {
            Nui.SetAutoPoll(false);
            //Nui.Close();
        }

        public void Draw(Func<Vector3, Point> to2D, Graphics graphics, Action redraw) {
            if (!mRedraws.Contains(redraw))
                mRedraws.Add(redraw);
            if (mEnabled) {
                using (Pen p = new Pen(Color.Black, 3f)) {
                    Vector3 l = new Vector3(0f, -140, 0f) * mKinectOrientation.Quaternion;
                    Vector3 r = new Vector3(0f, 140, 0f) * mKinectOrientation.Quaternion;
                    graphics.DrawLine(p, to2D(mKinectPosition + l), to2D(mKinectPosition + r));
                }
                using (Brush b = new SolidBrush(Color.FromArgb(128, Color.Blue))) {
                    int hFoV = 57 / 2;
                    int vFoV = 43 / 2;
                    Vector3 range = new Vector3(3500f, 0f, 0f);
                    Vector3 topLeft = range * (new Rotation(vFoV, -hFoV) + mKinectOrientation).Quaternion;
                    Vector3 topRight = range * (new Rotation(vFoV, hFoV) + mKinectOrientation).Quaternion;
                    Vector3 bottomLeft = range * (new Rotation(-vFoV, -hFoV) + mKinectOrientation).Quaternion;
                    Vector3 bottomRight = range * (new Rotation(-vFoV, hFoV) + mKinectOrientation).Quaternion;

                    Point centreP = to2D(mKinectPosition);
                    Point topLeftP = to2D(topLeft);
                    Point topRightP = to2D(topRight);
                    Point bottomLeftP = to2D(bottomLeft);
                    Point bottomRightP = to2D(bottomRight);

                    graphics.FillPolygon(b, new Point[] { centreP, topLeftP, topRightP, centreP });
                    graphics.FillPolygon(b, new Point[] { centreP, bottomLeftP, bottomRightP, centreP });
                    graphics.FillPolygon(b, new Point[] { centreP, topLeftP, bottomLeftP, centreP });
                    graphics.FillPolygon(b, new Point[] { centreP, topRightP, bottomRightP, centreP });
                }
            }
        }

        #endregion

        private void mCoordinator_WindowAdded(Window window, EventArgs args) {
            foreach (var factory in mCursorFactories) {
                IKinectCursor cursor = factory.Make();
                cursor.Init(this, window);
                cursor.CursorMove += new Action<IKinectCursor, float,float>(cursor_CursorMove);
                mCursors[factory.Name].Add(window.Name, cursor);

                if (mCurrentCursors == null)
                    mCurrentCursors = mCursors[factory.Name];
                else
                    cursor.Enabled = false;
            }
        }

        private void helpTrigger_Triggered(IHelpTrigger trigger) {
            if (trigger.Enabled)
                mCoordinator.StateManager.TriggerCustom("Help");
        }

        private void cursor_CursorMove(IKinectCursor cursor, float x, float y) {
            if (mEnabled && Nui.HasSkeleton)
                cursor.Window.OverlayManager.UpdateCursor(x, y);
        }
    }
}




/*

	
    //Get the primary vectors.
    Vector shoulderR = joint(SHOULDER_RIGHT);
    Vector shoulderL = joint(SHOULDER_LEFT);
    Vector elbowR = joint(ELBOW_RIGHT);
    Vector elbowL = joint(ELBOW_LEFT);
    Vector wristR = joint(WRIST_RIGHT);
    Vector wristL = joint(WRIST_LEFT);
    Vector handR = joint(HAND_RIGHT);
    Vector handL = joint(HAND_LEFT);
    Vector hipC = joint(HIP_CENTER);
    Vector head = joint(HEAD);
    //Condition guard = closeGuard();

    Vector armR = handR - shoulderR;
    Vector armL = handL - shoulderL;

    //----------- Walk----------- 
    Scalar walkThreshold = tracker("WalkStart", 40, .65f / 40.f , 0.f, 20);
    //Left and right
    Scalar walkDiffR = z(armR);
    Scalar walkDiffL = z(armL);
    //Active
    Condition walkActiveR = (abs(walkDiffR) > walkThreshold && walkDiffR < 0.f) || (walkDiffR > walkThreshold / 5.f);
    Condition walkActiveL = (abs(walkDiffL) > walkThreshold && walkDiffL < 0.f) || (walkDiffL > walkThreshold / 5.f);
    mWalkActive = walkActiveR || walkActiveL;
    //Direction
    mForward = (walkActiveR && walkDiffR < 0.f) || (walkActiveL && walkDiffL < 0.f);

    //----------- Fly----------- 
    Scalar flyThreshold = .2f - tracker("FlyStart", 40, .2f / 40.f, 0.f, 20);
    Scalar flyMax = tracker("FlyMax", 40, .2f / 40.f, .2f, 20);
    Scalar flyAngleR = constrain(dot(Vector(0.f, 1.f, 0.f), armR), flyThreshold, flyMax, 0.f, true);
    Scalar flyAngleL = constrain(dot(Vector(0.f, 1.f, 0.f), armL), flyThreshold, flyMax, 0.f, true);
    Condition flyActiveR = flyAngleR != 0.f && magnitude(armR) - magnitude(limit(armR, true, true, false)) < .1f;
    Condition flyActiveL = flyAngleL != 0.f && magnitude(armL) - magnitude(limit(armL, true, true, false)) < .1f;
    mFlyActive = (flyActiveR || flyActiveL) && z(armR) < 0.f && z(armR) < 0.f;
    mFlyUp = (flyActiveR && flyAngleR > 0.f) || (flyActiveL && flyAngleL > 0.f);
    //mFlyActive = flyActiveR;
    //mFlyUp = (flyActiveR && flyAngleR > 0.f);

    //----------- Yaw----------- 
    Scalar yawD = .1f - tracker("YawStart", 40, .1f / 40.f, 0.f, 20);
    Scalar yawSpeed = 10.f - tracker("YawSpeed", 40, 15.f / 40.f, 0.f, 20);
    Vector yawCore = limit(head - hipC, true, true, false);
    // Yaw is how far the user is leaning horizontally. This is calculated the angle between vertical and the vector between the hip centre and the head.
    Scalar yawLean = dot(normalize(yawCore), Vector(1.f, 0.f, 0.f));
    // Constrain the value, deadzone is provided by a slider.
    mYaw = constrain(yawLean, yawD, .2f, .3f, true) / yawSpeed;
    mYawActive = mYaw != 0.f;

    mMoveActive = mWalkActive || mFlyActive || mYawActive;


 
*/
