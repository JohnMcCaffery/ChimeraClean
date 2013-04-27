using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.XInput;
using Chimera.Inputs;
using Chimera;

namespace Joystick {
    public class TriggerAxis : ConstrainedAxis {
        private Controller mController;
        private bool mLeftUp;

        public TriggerAxis(bool leftUp)
            : base(short.MaxValue / 3f, short.MaxValue / 2f, short.MaxValue / 6f, .00005f) {
            Init(leftUp);
            mController = new Controller(UserIndex.One);
            if (mController.IsConnected)
                return;

            mController = new Controller(UserIndex.Two);
            if (mController.IsConnected)
                return;

            mController = new Controller(UserIndex.Three);
            if (mController.IsConnected)
                return;

            mController = new Controller(UserIndex.Four);
            if (mController.IsConnected)
                return;

            mController = null;
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

        public void Init(IInputSource source) {
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
