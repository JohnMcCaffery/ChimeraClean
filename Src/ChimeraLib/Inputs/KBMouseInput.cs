using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;
using Chimera.Util;
using System.Drawing;

namespace Chimera.Inputs {
    public class KBMouseInput : IDeltaInput {
        internal int X, Y, CurrentX, CurrentY;
        internal bool MouseDown;
        private Rotation mOrientation = Rotation.Zero;
        private Vector3 mDeltas;
        private double mStartPitch, mStartYaw;
        private bool mIgnorePitch;
        private bool mWalkEnabled = true;
        private bool mStrafeEnabled = true;
        private bool mFlyEnabled = true;
        private bool mYawEnabled = true;
        private bool mPitchEnabled = true;
        private bool mActive = false;

        /// <summary>
        /// Triggered whenever the keyboard scale changes.
        /// </summary>
        public event Action<int> KBScaleChange;
        /// <summary>
        /// Triggered whenever the mouse scale changes.
        /// </summary>
        public event Action<int> MouseScaleChange;

        /// <summary>
        /// Amount to scale keyboard values by.
        /// </summary>
        private double mKBShift = 1.0;
        /// <summary>
        /// Amount to scale mouse values by.
        /// </summary>
        private double mMouseScale = 1.0;
        /// <summary>
        /// The output panel that controls the input.
        /// </summary>
        private KBMousePanel mControlPanel;
        /// <summary>
        /// Whether this input is enabled.
        /// </summary>
        private bool mEnabled = true;
        /// <summary>
        /// The source which this input is getting key events transition.
        /// </summary>
        IInputSource mInput;

        public KBMouseInput() {
            InputConfig cfg = new InputConfig();
            mEnabled = cfg.KeyboardEnabled;
        }

        /// <summary>
        /// How much to scale Keyboard inputs by.
        /// Should be a value between 1 and 1000.
        /// </summary>
        public int KBScale {
            get { return (int) (mKBShift / .002); }
            set { 
                mKBShift = .002 * value;
                if (KBScaleChange != null)
                    KBScaleChange(KBScale);
            }
        }

        /// <summary>
        /// How much to scale mouse inputs by.
        /// Should be a value between 1 and 1000.
        /// </summary>
        public int MouseScale {
            get { return (int) (mMouseScale / .01) * 5; }
            set { 
                mMouseScale = ((double) value / 5.0) * .01;
                if (MouseScaleChange != null)
                    MouseScaleChange(KBScale);
            }
        }

        internal IInputSource Source {
            get { return mInput; }
        }

        /// <summary>
        /// If set to true pitch will always be left at zero.
        /// </summary>
        public bool IgnorePitch {
            get { return mIgnorePitch; }
            set { mIgnorePitch = value; }
        }

        #region ISystemInput Members

        public event Action<IInput, bool> EnabledChanged;

        public UserControl ControlPanel {
            get {
                if (mControlPanel == null)
                    mControlPanel = new KBMousePanel(this);
                return mControlPanel;
            }
        }

        public string Name {
            get { return "Keyboard + Mouse"; }
        }

        public virtual bool Enabled {
            get { return mEnabled; }
            set { 
                mEnabled = value;
                if (EnabledChanged != null)
                    EnabledChanged(this, value);
            }
        }

        public ConfigBase Config {
            get { throw new NotImplementedException(); }
        }

        public string State {
            get { 
                string dump = "----------KB/Mouse Input----------" + Environment.NewLine;
                dump += "Mouse Scale: " + mMouseScale + Environment.NewLine;
                dump += "Keyboard Scale: " + mKBShift + Environment.NewLine;
                return dump;
            }
        }

        public void Close() {
            if (mControlPanel != null)
                mControlPanel.Stop();
        }

        public void Draw(Func<Vector3, Point> to2D, System.Drawing.Graphics graphics, Action redraw) {
            //Do nothing
        }

        #endregion

        #region IDeltaInput

        public event Action<IDeltaInput> Change;

        public Vector3 PositionDelta {
            get { return mDeltas; }
        }

        public Rotation OrientationDelta {
            get { return mOrientation; }
        }

        public bool WalkEnabled {
            get { return mWalkEnabled; }
            set { mWalkEnabled = value; }
        }

        public bool StrafeEnabled {
            get { return mStrafeEnabled; }
            set { mStrafeEnabled = value; }
        }

        public bool FlyEnabled {
            get { return mFlyEnabled; }
            set { mFlyEnabled = value; }
        }

        public bool YawEnabled {
            get { return mYawEnabled; }
            set { mYawEnabled = value; }
        }

        public bool PitchEnabled {
            get { return mPitchEnabled; }
            set { mPitchEnabled = value; }
        }

        public void Init(IInputSource input) {
            mInput = input;
            input.KeyDown += new Action<Coordinator,KeyEventArgs>(mCoordinator_KeyDown);
            input.KeyUp += new Action<Coordinator,KeyEventArgs>(mCoordinator_KeyUp);
            input.Tick += new Action(mCoordinator_Tick);
        }

        #endregion

        internal void panel_MouseDown(object sender, MouseEventArgs e) {
            CurrentX = e.X;
            CurrentY = e.Y;
            X = CurrentX;
            Y = CurrentY;
            MouseDown = true;
        }

        internal void panel_MouseUp(object sender, MouseEventArgs e) {
            MouseDown = false;
            mOrientation.Pitch = 0.0;
            mOrientation.Yaw = 0.0;
        }

        internal void panel_MouseMove(object sender, MouseEventArgs e) {
            if (MouseDown) {
                int xDiff = e.X - CurrentX;
                int yDiff = e.Y - CurrentY;
                mOrientation.Pitch = mIgnorePitch ? 0.0 : (yDiff * mMouseScale);
                mOrientation.Yaw = xDiff * -mMouseScale;
                //if (Change != null && (mDeltas != Vector3.Zero || mOrientation.Pitch != 0.0 || mOrientation.Yaw != 0.0))
                    //Change(this);
                //mOrientation.Pitch = 0;
                //mOrientation.Yaw = 0;
                CurrentX = e.X;
                CurrentY = e.Y;
            }
        }

        private void mCoordinator_KeyDown(Coordinator coord, KeyEventArgs args) {
            switch (args.KeyData) {
                case Keys.W: mDeltas.X = mWalkEnabled ? (float) mKBShift : 0f; break;
                case Keys.S: mDeltas.X = mWalkEnabled ? (float) -mKBShift : 0f; break;
                case Keys.D: mDeltas.Y = mStrafeEnabled ? (float) -mKBShift : 0f; break;
                case Keys.A: mDeltas.Y = mStrafeEnabled ? (float) mKBShift : 0f; break;
                case Keys.E: mDeltas.Z = mFlyEnabled ? (float) mKBShift : 0f; break;
                case Keys.Q: mDeltas.Z = mFlyEnabled ? (float) -mKBShift : 0f; break;
                case Keys.Left: mOrientation.Yaw = mYawEnabled ? mKBShift * 2.0 : 0.0; break;
                case Keys.Right: mOrientation.Yaw = mYawEnabled ? -mKBShift * 2.0 : 0.0; break;
                case Keys.Up: mOrientation.Pitch = mPitchEnabled ? -mKBShift * 2.0 : 0.0; break;
                case Keys.Down: mOrientation.Pitch = mPitchEnabled ? mKBShift * 2.0 : 0.0; break;
            }
        }

        private void mCoordinator_KeyUp(Coordinator coord, KeyEventArgs args) {
            switch (args.KeyData) {
                case Keys.W: mDeltas.X = 0f; break;
                case Keys.S: mDeltas.X = 0f; break;
                case Keys.D: mDeltas.Y = 0f; break;
                case Keys.A: mDeltas.Y = 0f; break;
                case Keys.E: mDeltas.Z = 0f; break;
                case Keys.Q: mDeltas.Z = 0f; break;
                case Keys.Left: mOrientation.Yaw = 0.0; break;
                case Keys.Right: mOrientation.Yaw = 0.0; break;
                case Keys.Up: mOrientation.Pitch = 0.0; break;
                case Keys.Down: mOrientation.Pitch = 0.0; break;
            }
        }

        private void mCoordinator_Tick() {
            if (Change != null && (mDeltas != Vector3.Zero || mOrientation.Pitch != 0.0 || mOrientation.Yaw != 0.0)) {
                mActive = true;
                Change(this);
            } else if (mActive) {
                mActive = false;
                Change(this);
            }
            if (MouseDown)
                mOrientation = Rotation.Zero;
        }
    }
}
