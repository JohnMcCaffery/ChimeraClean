using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.XInput;
using Chimera.Inputs;
using Chimera;
using Chimera.Interfaces;

namespace Joystick {
    public class ThumbstickAxis : ConstrainedAxis, IInputListener {
        private Controller mController;
        private bool mLeft;
        private bool mX;

        public ThumbstickAxis(bool left, bool x)
            : base(short.MaxValue / 3f, short.MaxValue / 2f, short.MaxValue / 6f, .00005f) {
            Init(left, x);

            mController = JoystickInput.GetController();
        }

        public ThumbstickAxis(UserIndex index, bool left, bool x)
            : this(left, x) {

            mController = new Controller(index);
            if (!mController.IsConnected)
                mController = null;
        }

        public ThumbstickAxis(Controller controller, bool left, bool x)
            : this(left, x) {

            mController = controller;
        }

        private void Init(bool left, bool x) {
            mLeft = left;
            mX = x;
        }

        public void Init(IInputSource source) {
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
