using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.XInput;
using Chimera.Plugins;
using Chimera;
using Chimera.Interfaces;

namespace Joystick {
    public class TriggerAxis : ConstrainedAxis, ITickListener {
        private Controller mController;
        private bool mLeftUp;

        public TriggerAxis(bool leftUp)
            : base("Trigger", 0, 255f, 0, .0005f) {
            Init(leftUp);

            mController = JoystickInput.GetController();
        }
        public TriggerAxis(UserIndex index, bool leftUp)
            : this(leftUp) {
            mController = new Controller(index);
            if (!mController.IsConnected)
                mController = null;
        }
        public TriggerAxis(Controller controller, bool leftUp)
            : this(leftUp) {
            mController = controller;
        }

        private void Init(bool leftUp) {
            mLeftUp = leftUp;
        }

        public void Init(ITickSource source) {
            source.Tick += new Action(source_Tick);
        }

        void source_Tick() {
            if (mController == null)
                return;

            Gamepad g = mController.GetState().Gamepad;

            if (g.RightTrigger > 0)
                SetRawValue(g.RightTrigger * (mLeftUp ? -1f : 1f));
            else if (g.LeftTrigger > 0)
               SetRawValue(g.LeftTrigger * (mLeftUp ? 1f : -1f));
            else
                SetRawValue(0f);
        }
    }
}
