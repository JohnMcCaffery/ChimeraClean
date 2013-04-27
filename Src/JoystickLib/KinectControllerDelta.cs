using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Inputs;
using Chimera;
using Chimera.Interfaces;

namespace Joystick {
    public class XBoxControllerDelta : AxisBasedDelta {
        public XBoxControllerDelta(IInputSource source)
            : base(
                "XBox Controller",
                new ThumbstickAxis(false, false, source),
                new ThumbstickAxis(false, true, source),
                new TriggerAxis(false, source),
                new ThumbstickAxis(true, true, source),
                new ThumbstickAxis(true, false, source)
            ) { }
    }

    public class XBoxControllerInput : DeltaBasedInput {
        public XBoxControllerInput(IInputSource source)
            : base(new XBoxControllerDelta(source)) { }
    }
}
