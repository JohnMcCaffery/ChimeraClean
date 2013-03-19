using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;
using Chimera.Util;

namespace Chimera.Inputs {
    public class KBMouseInput : Chimera.ISystemInput {
        internal int X, Y, CurrentX, CurrentY;
        internal bool MouseDown;
        private Rotation mRotation = Rotation.Zero, mOldRotation;
        private double mStartPitch, mStartYaw;
        private bool mLeftDown, mRightDown, mForwardDown, mBackwardDown;
        private bool mUpDown, mDownDown;
        private bool mYawRightDown, mYawLeftDown, mPitchUpDown, mPitchDownDown;
        private bool mIgnorePitch;

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
        /// The input which this input controls.
        /// </summary>
        private Coordinator mCoordinator;

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
                string dump = "-KB/Mouse Input-" + Environment.NewLine;
                dump += "Mouse Scale: " + mMouseScale + Environment.NewLine;
                dump += "Keyboard Scale: " + mKBShift + Environment.NewLine;
                return dump;
            }
        }

        /// <summary>
        /// If set to true pitch will always be left at zero.
        /// </summary>
        public bool IgnorePitch {
            get { return mIgnorePitch; }
            set { mIgnorePitch = value; }
        }

        /// <summary>
        /// The system which this input is registered with.
        /// </summary>
        public Coordinator Coordinator {
            get { return mCoordinator; }
        }

        public void Init(Coordinator coordinator) {
            mCoordinator = coordinator;
            mCoordinator.KeyDown += new Action<Coordinator,KeyEventArgs>(mCoordinator_KeyDown);
            mCoordinator.KeyUp += new Action<Coordinator,KeyEventArgs>(mCoordinator_KeyUp);
        }

        public void Close() {
            if (mControlPanel != null)
                mControlPanel.Stop();
        }

        public void Draw(Perspective perspective, System.Drawing.Graphics graphics) {
            throw new NotImplementedException();
        }

        #endregion

        internal void panel_MouseDown(object sender, MouseEventArgs e) {
            X = e.X;
            Y = e.Y;
            CurrentX = e.X;
            CurrentY = e.Y;
            mStartPitch = mCoordinator.Orientation.Pitch;
            mStartYaw = mCoordinator.Orientation.Yaw;
            MouseDown = true;
        }

        internal void panel_MouseUp(object sender, MouseEventArgs e) {
            MouseDown = false;
        }

        internal void panel_MouseMove(object sender, MouseEventArgs e) {
            if (MouseDown) {
                CurrentX = e.X;
                CurrentY = e.Y;

                mRotation.Pitch = mIgnorePitch ? 0.0 : mStartPitch + ((e.Y - Y) * mMouseScale);
                mRotation.Yaw = mStartYaw + (((X - e.X) / 2) * mMouseScale);
            }
        }

        private void mCoordinator_KeyDown(Coordinator coord, KeyEventArgs args) {
            switch (args.KeyData) {
                case Keys.A: mLeftDown = true; break;
                case Keys.D: mRightDown = true; break;
                case Keys.W: mForwardDown = true; break;
                case Keys.S: mBackwardDown = true; break;
                case Keys.E: mUpDown = true; break;
                case Keys.Q: mDownDown = true; break;
                case Keys.Left: mYawLeftDown = true; break;
                case Keys.Right: mYawRightDown = true; break;
                case Keys.Up: mPitchUpDown = true; break;
                case Keys.Down: mPitchDownDown = true; break;
            }
        }

        private void mCoordinator_KeyUp(Coordinator coord, KeyEventArgs args) {
            switch (args.KeyData) {
                case Keys.A: mLeftDown = false; break;
                case Keys.D: mRightDown = false; break;
                case Keys.W: mForwardDown = false; break;
                case Keys.S: mBackwardDown = false; break;
                case Keys.E: mUpDown = false; break;
                case Keys.Q: mDownDown = false; break;
                case Keys.Left: mYawLeftDown = false; break;
                case Keys.Right: mYawRightDown = false; break;
                case Keys.Up: mPitchUpDown = false; break;
                case Keys.Down: mPitchDownDown = false; break;
            }
        }

        internal void panel_Tick(object sender, EventArgs e) {
            Vector3 move = Vector3.Zero;
            if (mForwardDown) move.X += (float) mKBShift;
            if (mBackwardDown) move.X -= (float) mKBShift;
            if (mLeftDown) move.Y += (float) mKBShift;
            if (mRightDown) move.Y -= (float) mKBShift;

            if (mYawLeftDown || mYawRightDown || mPitchUpDown || mPitchDownDown) {
                mRotation = new Rotation(mCoordinator.Orientation);
                if (mYawLeftDown) mRotation.Yaw += mKBShift;
                if (mYawRightDown) mRotation.Yaw -= mKBShift;
                if (mPitchUpDown) mRotation.Pitch -= mKBShift;
                if (mPitchDownDown) mRotation.Pitch += mKBShift;
            }

            //TODO - handle keyboard rotation
            if (
                mForwardDown || mBackwardDown || mLeftDown || mRightDown || 
                mUpDown || mDownDown || 
                mPitchUpDown || mPitchDownDown || mYawLeftDown || mYawRightDown || 
                MouseDown) {
                move *= mCoordinator.Orientation.Quaternion;
                if (mUpDown) move.Z = (float) mKBShift;
                if (mDownDown) move.Z = -(float) mKBShift;

                Vector3 pos = mCoordinator.Position + move;
                if (mOldRotation == null)
                    mOldRotation = new Rotation(mRotation);
                mCoordinator.Update(pos, move, mRotation, mRotation - mOldRotation);
                mOldRotation = new Rotation(mRotation);
            }
        }
    }
}
