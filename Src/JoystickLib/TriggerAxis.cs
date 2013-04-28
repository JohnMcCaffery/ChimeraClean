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
        private bool mLeftUp;

        
        public TriggerAxis(bool leftUp, AxisBinding binding)
            : base("Trigger", 0, .0005f, binding) {

            mLeftUp = leftUp;
        }
       
        public TriggerAxis(bool leftUp)
            : this(leftUp, AxisBinding.None) {
        }

        public void Init(ITickSource source) {
            GamepadManager.Init(source);
            source.Tick += new Action(source_Tick);
        }

        void source_Tick() {
            if (!GamepadManager.Initialised)
                return;

            Gamepad g = GamepadManager.Gamepad;

            if (g.RightTrigger > 0)
                SetRawValue(g.RightTrigger * (mLeftUp ? -1f : 1f));
            else if (g.LeftTrigger > 0)
               SetRawValue(g.LeftTrigger * (mLeftUp ? 1f : -1f));
            else
                SetRawValue(0f);
        }
    }
}
