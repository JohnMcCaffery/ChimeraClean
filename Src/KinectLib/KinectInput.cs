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

namespace Chimera.Kinect {
    public class KinectInput : IInput {
        private readonly List<WindowInput> mWindowInputs = new List<WindowInput>();

        private Coordinator mCoordinator;
        private bool mEnabled;
        private bool mPointEnabled;
        private bool mMoveEnabled;
        private bool mKinectStarted;
        private Vector mPointEnd = Vector.Create(0f, 0f, 0f);
        private Vector mPointStart = Vector.Create(0f, 0f, 0f);
        private Vector mPointDir = Vector.Create(0f, 0f, 0f);
        private Vector3 mKinectPosition;
        private Rotation mKinectOrientation;
        private KinectPanel mPanel;

        public event Action<Vector3> PositionChanged;
        public event Action<Quaternion> OrientationChanged;
        public event Action<Vector, Vector> VectorsAssigned;

        /// <summary>
        /// Get the window input associated with a specific window.
        /// </summary>
        /// <param name="window">The name of the window to get the input for.</param>
        /// <returns>The WindowInput object which is calculating whether the user is pointing at the specified window.</returns>
        public WindowInput this[string window] {
            get { return mWindowInputs.FirstOrDefault(i => i.Window.Name.Equals(window)); }
        }
        /// <summary>
        /// All the window inputs this input associates with the windows the system renders to.
        /// </summary>
        public WindowInput[] WindowInputs {
            get { return mWindowInputs.ToArray(); }
        }

        public Vector PointStart {
            get { return mPointStart; }
        }

        public Vector PointDir {
            get { return mPointDir; }
        }

        public Vector3 Position {
            get { return mKinectPosition; }
            set {
                mKinectPosition = value;
                if (PositionChanged != null)
                    PositionChanged(value);
            }
        }

        public Rotation Orientation {
            get { return mKinectOrientation; }
        }

        public bool KinectStarted {
            get { return mKinectStarted; }
        }

        public KinectInput() {
            KinectConfig cfg = new KinectConfig();

            mKinectPosition = cfg.Position;
            mKinectOrientation = new Rotation(cfg.Pitch, cfg.Yaw);

            mKinectOrientation.Changed += (source, args) => OrientationChanged(mKinectOrientation.Quaternion);

            if (cfg.Autostart)
                StartKinect();
        }

        private Condition mWalkActive, mFlyActive, mYawActive;
        private Scalar mYaw;
        private Condition mFy, mFlyUp;
        private Condition mMoveActive, mForward;

        public void StartKinect() {
            if (!mKinectStarted) {
                Nui.Init();
                Nui.SetAutoPoll(true);
                mPointEnd = Nui.joint(Nui.Hand_Right);
                mPointStart = Nui.joint(Nui.Elbow_Right);
                mPointDir = mPointStart - mPointEnd;
                mKinectStarted = true;



                //Get the primary vectors.
                Vector shoulderR = Nui.joint(Nui.Shoulder_Right);
                Vector shoulderL = Nui.joint(Nui.Shoulder_Left);
                Vector elbowR = Nui.joint(Nui.Elbow_Right);
                Vector elbowL = Nui.joint(Nui.Elbow_Left);
                Vector wristR = Nui.joint(Nui.Wrist_Right);
                Vector wristL = Nui.joint(Nui.Wrist_Left);
                Vector handR = Nui.joint(Nui.Hand_Right);
                Vector handL = Nui.joint(Nui.Hand_Left);
                Vector hipC = Nui.joint(Nui.Hip_Centre);
                Vector head = Nui.joint(Nui.Head);
                //Condition guard = closeGuard();

                Vector armR = handR - shoulderR;
                Vector armL = handL - shoulderL;

                //----------- Walk----------- 
                //Scalar walkThreshold = Nui.tracker("WalkStart", 40, .65f / 40f, 0f, 20);
                Scalar walkThreshold = Scalar.Create(.65f);
                //Left and right
                Scalar walkDiffR = Nui.z(armR);
                Scalar walkDiffL = Nui.z(armL);
                //Active
                Condition walkActiveR = C.Or(C.And(Nui.abs(walkDiffR) > walkThreshold,  walkDiffR < 0f), (walkDiffR > walkThreshold / 5f));
                Condition walkActiveL = C.Or(C.And(Nui.abs(walkDiffL) > walkThreshold,  walkDiffL < 0f), (walkDiffL > walkThreshold / 5f));
                mWalkActive = C.Or(walkActiveR, walkActiveL);
                //Direction
                mForward = C.Or(C.And(walkActiveR,  walkDiffR < 0f), C.And(walkActiveL,  walkDiffL < 0f));

                //----------- Fly----------- 
                //Scalar flyThreshold = .2f - Nui.tracker("FlyStart", 40, .2f / 40f, 0f, 20);
                //Scalar flyMax = Nui.tracker("FlyMax", 40, .2f / 40f, .2f, 20);
                Scalar flyThreshold = Scalar.Create(0f);
                Scalar flyMax = Scalar.Create(.2f);
                Scalar flyAngleR = Nui.constrain(Nui.dot(Vector.Create(0f, 1f, 0f), armR), flyThreshold, flyMax, 0f, true);
                Scalar flyAngleL = Nui.constrain(Nui.dot(Vector.Create(0f, 1f, 0f), armL), flyThreshold, flyMax, 0f, true);
                Condition flyActiveR = C.And(flyAngleR != 0f,  Nui.magnitude(armR) - Nui.magnitude(Nui.limit(armR, true, true, false)) < .1f);
                Condition flyActiveL = C.And(flyAngleL != 0f,  Nui.magnitude(armL) - Nui.magnitude(Nui.limit(armL, true, true, false)) < .1f);
                mFlyActive = C.And(C.Or(flyActiveR, flyActiveL),  C.And(Nui.z(armR) < 0f,  Nui.z(armR) < 0f));
                mFlyUp = C.Or(C.And(flyActiveR,  flyAngleR > 0f), C.And(flyActiveL,  flyAngleL > 0f));
                //mFlyActive = flyActiveR;
                //mFlyUp = (flyActiveR,  flyAngleR > 0f);

                //----------- Yaw----------- 
                //Scalar yawD = .1f - Nui.tracker("YawStart", 40, .1f / 40f, 0f, 20);
                //Scalar yawSpeed = 10f - Nui.tracker("YawSpeed", 40, 15f / 40f, 0f, 20);
                Scalar yawD = Scalar.Create(0f);
                Scalar yawSpeed = Scalar.Create(0f);
                Vector yawCore = Nui.limit(head - hipC, true, true, false);
                // Yaw is how far the user is leaning horizontally. This is calculated the angle between vertical and the vector between the hip centre and the head.
                Scalar yawLean = Nui.dot(Nui.normalize(yawCore), Vector.Create(1f, 0f, 0f));
                // Constrain the value, deadzone is provided by a slider.
                mYaw = Nui.constrain(yawLean, yawD, .2f, .3f, true) / yawSpeed;
                mYawActive = mYaw != 0f;

                mMoveActive = C.Or(C.Or(mWalkActive, mFlyActive), mYawActive);



                if (VectorsAssigned != null)
                    VectorsAssigned(mPointStart, mPointDir);
            }
        }

        #region IInput Members

        public UserControl ControlPanel {
            get {
                if (mPanel == null) {
                    mPanel = new KinectPanel(this);
                    /*
                    mPanel.Position = mKinectPosition;
                    mPanel.Orientation = mKinectOrientation;
                    mPanel.PointStart = new VectorUpdater(mPointStart);
                    mPanel.PointDir = new VectorUpdater(mPointDir);

                    foreach (var window in mWindowInputs)
                        mPanel.AddWindow(window.Panel, window.Window.Name);

                    mPanel.PositionChanged += newPos => {
                        mKinectPosition = newPos;
                        if (PositionChanged != null)
                            PositionChanged(newPos);
                    };

                    mPanel.Started += () => StartKinect();
                    */
                }
                return mPanel;
            }
        }

        public string Name {
            get { return "Kinect"; }
        }

        public bool Enabled {
            get { return mMoveEnabled; }
            set {
                mMoveEnabled = value;
            }
        }

        public string[] ConfigSwitches {
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

            foreach(var window in mCoordinator.Windows)
                mWindowInputs.Add(new WindowInput(this, window));

            mCoordinator.WindowAdded += new Action<Window,EventArgs>(mCoordinator_WindowAdded);
        }

        public void Close() {
            Nui.Close();
        }

        public void Draw(Perspective perspective, System.Drawing.Graphics graphics) {
            throw new NotImplementedException();
        }

        #endregion

        private void mCoordinator_WindowAdded(Window window, EventArgs args) {
            WindowInput input = new WindowInput(this, window);
            mWindowInputs.Add(input);
            if (mPanel != null)
                mPanel.AddWindow(input.Panel, window.Name);
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
