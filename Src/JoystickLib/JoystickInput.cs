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
using Chimera.Plugins;

namespace Joystick{
    public class JoystickInput : DeltaBasedPlugin {
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
                if (!GamepadManager.Initialised)
                    return new int[0];

                List<int> values = new List<int>();
                Gamepad g = GamepadManager.Gamepad;
                values.Add(g.LeftThumbX);
                values.Add(g.LeftThumbY);
                values.Add(g.RightThumbX);
                values.Add(g.RightThumbY);
                values.Add(g.LeftTrigger);
                values.Add(g.RightTrigger);
                return values.ToArray(); 
            }
        }

        public JoystickInput() {
            List<DeviceInstance> devices = new List<DeviceInstance>();
            DirectInput input = new DirectInput();
            devices.AddRange(input.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly));

            mTick = new Action(Tick);
        }

        private void Tick() {
            if (GamepadManager.Initialised) {
                OSVector posDelta = OSVector.Zero;
                Rotation rotDelta = Rotation.Zero;

                Gamepad g = GamepadManager.Gamepad;

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

                TriggerChange(this);
            }
        }

        private OSVector mDelta = OSVector.Zero;
        private Rotation mRotation = Rotation.Zero;


        #region IDeltaInput Members

        public override OSVector PositionDelta {
            get { return mDelta; }
        }

         public override Rotation OrientationDelta {
            get { return mRotation; }
        }

        public override void Init(Coordinator input) {
            input.Tick += mTick;
        }

        #endregion

        #region IInput Members

        public override UserControl ControlPanel {
            get {
                if (mControlPanel == null)
                    mControlPanel = new JoystickPanel(this);
                return mControlPanel; 
            }
        }

        public override string Name {
            get { return "Joystick"; }
        }

        public override string State {
            get { throw new NotImplementedException(); }
        }

        public override ConfigBase Config {
            get { throw new NotImplementedException(); }
        }

        public override void Close() { }

        public override void Draw(Func<OSVector, Point> to2D, Graphics graphics, Action redraw) { }

        #endregion
    }
}
