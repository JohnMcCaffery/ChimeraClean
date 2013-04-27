using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Plugins;
using Chimera;
using Chimera.Interfaces;
using SlimDX.XInput;

namespace Joystick {
    public class XBoxControllerDelta : AxisBasedDelta {
        private static Controller C() {
            return JoystickInput.GetController();
        }
        public XBoxControllerDelta()
            : base(
                "XBox Controller",
                new ThumbstickAxis(C(), false, false),
                new ThumbstickAxis(C(), false, true),
                new TriggerAxis(C(), false),
                new ThumbstickAxis(C(), true, true),
                new ThumbstickAxis(C(), true, false)
            ) { }
    }

    public class XBoxControllerInput : DeltaBasedInput {
        public XBoxControllerInput()
            : base(new XBoxControllerDelta()) { }
    }
}
