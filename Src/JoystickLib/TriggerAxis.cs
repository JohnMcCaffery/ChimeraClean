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

        
        public TriggerAxis(Controller controller, bool leftUp, AxisBinding binding) 
            : base("Trigger", 0, 255f, 0, .0005f, binding) {

            mLeftUp = leftUp;
            mController = controller;
        }
       
        public TriggerAxis(Controller controller, bool leftUp)
            : this(controller, leftUp, AxisBinding.None) {
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
