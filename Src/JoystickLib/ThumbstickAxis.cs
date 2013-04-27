using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.XInput;
using Chimera.Plugins;
using Chimera;
using Chimera.Interfaces;

namespace Joystick {
    public class ThumbstickAxis : ConstrainedAxis, ITickListener {
        private Controller mController;
        private bool mLeft;
        private bool mX;

        private static string MakeName(bool left, bool x) {
            return (left ? "Left" : "Right") + "Thumbstick" + (x ? "X" : "Y");
        }

        public ThumbstickAxis(Controller controller, bool left, bool x, AxisBinding binding)
            : base(MakeName(left, x), short.MaxValue / 3f, short.MaxValue / 2f, short.MaxValue / 6f, .00005f, binding) {

            mController = controller;
            mLeft = left;
            mX = x;
        }

        public ThumbstickAxis(Controller controller, bool left, bool x)
            : this (controller, left, x, AxisBinding.None) {
        }

        public void Init(ITickSource source) {
            source.Tick += new Action(source_Tick);
        }

        void source_Tick() {
            if (mController == null)
                return;

            Gamepad g = mController.GetState().Gamepad;
            if (mLeft)
                SetRawValue(mX ? g.LeftThumbX : g.LeftThumbY);
            else
                SetRawValue(mX ? g.LeftThumbX : g.LeftThumbY);
        }
    }
}
