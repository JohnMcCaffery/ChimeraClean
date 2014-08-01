using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.XInput;
using Chimera.Plugins;
using Chimera;
using Chimera.Interfaces;

namespace Joystick
{
    public class DPadX : DPadAxis {
        public DPadX() : base(true, AxisBinding.NotSet) {
        }
    }
    public class DPadY : DPadAxis {
        public DPadY() : base(false, AxisBinding.NotSet) {
        }
    }

    public abstract class DPadAxis : ConstrainedAxis, ITickListener
    {
        private bool mX;

        private static string MakeName(bool x)
        {
            return "DPad" + (x ? "X" : "Y");
        }

        public DPadAxis(bool x, AxisBinding binding)
            : base(MakeName(x), 0, .00005f, binding)
        {

            mX = x;
        }
       
        public DPadAxis(bool x)
            : this(x, AxisBinding.NotSet) {
        }

        public void Init(ITickSource source) {
            GamepadManager.Init(source);
        }

        private bool IsPressed(GamepadButtonFlags button)
        {
                return (GamepadManager.Gamepad.Buttons & button) == button;
        }

        protected override float RawValue {
            get {
                if (!GamepadManager.Initialised)
                    return 0f;

                Gamepad g = GamepadManager.Gamepad;
                float r = 0;
                if (mX)
                {
                    if (IsPressed(GamepadButtonFlags.DPadLeft))
                        r = short.MaxValue;
                    else if (IsPressed(GamepadButtonFlags.DPadRight))
                        r = short.MinValue;
                }
                else
                {
                    if (IsPressed(GamepadButtonFlags.DPadUp))
                        r = short.MaxValue;
                    else if (IsPressed(GamepadButtonFlags.DPadDown))
                        r = short.MinValue;
                }

                return r;
            }
        }
    }
}
