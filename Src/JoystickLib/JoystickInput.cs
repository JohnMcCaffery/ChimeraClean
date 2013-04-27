using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.DirectInput;
using JS = SlimDX.DirectInput.Joystick;
using Chimera;
using SlimDX;
using System.Drawing;
using System.Windows.Forms;
using Chimera.Util;
using Joystick.GUI;
using OSVector = OpenMetaverse.Vector3;
using SlimDX.XInput;

namespace Joystick{
    public class JoystickInput : IDeltaInput {
        private Controller mJS;
        private Action mTick;
        private JoystickPanel mControlPanel;
        private bool mEnabled;

        private int mDeadzone = 10000;
        private float mScaleX = .00005f;
        private float mScaleY = .00005f;
        private float mScaleZ = .05f;
        private double mScalePitch = -.0005;
        private double mScaleYaw = -.0005;

        public int[] Sliders {
            get { 
                if (mJS == null)
                    return new int[0];

                List<int> values = new List<int>();
                SlimDX.XInput.State state = mJS.GetState();
                values.Add(state.Gamepad.LeftThumbX);
                values.Add(state.Gamepad.LeftThumbY);
                values.Add(state.Gamepad.RightThumbX);
                values.Add(state.Gamepad.RightThumbY);
                values.Add(state.Gamepad.LeftTrigger);
                values.Add(state.Gamepad.RightTrigger);
                return values.ToArray(); 
            }
        }

        public JoystickInput() {
            List<DeviceInstance> devices = new List<DeviceInstance>();
            DirectInput input = new DirectInput();
            devices.AddRange(input.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly));

            mTick = new Action(Tick);

            mJS = new Controller(UserIndex.One);
            if (mJS.IsConnected)
                return;

            mJS = new Controller(UserIndex.Two);
            if (mJS.IsConnected)
                return;

            mJS = new Controller(UserIndex.Three);
            if (mJS.IsConnected)
                return;

            mJS = new Controller(UserIndex.Four);
            if (mJS.IsConnected)
                return;

            mJS = null;
        }

        private void Tick() {
            if (mJS != null) {
                OSVector posDelta = OSVector.Zero;
                Rotation rotDelta = Rotation.Zero;

                Gamepad g = mJS.GetState().Gamepad;

                int x = Math.Abs((int) g.RightThumbY) - mDeadzone;
                posDelta.X = x > 0 ? x * mScaleX * (g.RightThumbY > 0f ? 1f : -1f) : 0f;

                int y = Math.Abs((int) g.RightThumbX) - mDeadzone;
                posDelta.Y = y > 0 ? y * mScaleY * (g.RightThumbX > 0f ? 1f : -1f) : 0f;

                if (g.RightTrigger > 0)
                    posDelta.Z = g.RightTrigger * mScaleZ;
                else if (g.LeftTrigger > 0)
                    posDelta.Z = g.LeftTrigger * -mScaleZ;


                int pitch = Math.Abs((int) g.LeftThumbY) - mDeadzone;
                rotDelta.Pitch = pitch > 0 ? y * mScalePitch * (g.LeftThumbY > 0 ? 1.0 : -1.0) : 0.0;

                int yaw = Math.Abs((int) g.LeftThumbX) - mDeadzone;
                rotDelta.Yaw = yaw > 0 ? yaw * mScaleYaw * (g.LeftThumbX > 0 ? 1.0 : -1.0) : 0.0;

                mDelta = posDelta;
                mRotation = rotDelta;

                if (Change != null)
                    Change(this);
            }
        }

        private OSVector mDelta = OSVector.Zero;
        private Rotation mRotation = Rotation.Zero;


        #region IDeltaInput Members

        public event Action<IDeltaInput> Change;

        public OSVector PositionDelta {
            get { return mDelta; }
        }

        public Rotation OrientationDelta {
            get { return mRotation; }
        }

        public bool WalkEnabled {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public bool StrafeEnabled {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public bool FlyEnabled {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public bool YawEnabled {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public bool PitchEnabled {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public void Init(IInputSource input) {
            input.Tick += mTick;
        }

        #endregion

        #region IInput Members

        public event Action<IInput, bool> EnabledChanged;

        public UserControl ControlPanel {
            get {
                if (mControlPanel == null)
                    mControlPanel = new JoystickPanel(this);
                return mControlPanel; 
            }
        }

        public bool Enabled {
            get {
                return mEnabled;
            }
            set {
                if (value != mEnabled) {
                    mEnabled = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public string Name {
            get { return "Joystick"; }
        }

        public string State {
            get { throw new NotImplementedException(); }
        }

        public ConfigBase Config {
            get { throw new NotImplementedException(); }
        }

        public void Close() {
        }

        public void Draw(Func<OSVector, Point> to2D, Graphics graphics, Action redraw) { }

        #endregion
    }
}
