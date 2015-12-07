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
using Chimera.Plugins;
using Chimera;
using Chimera.Interfaces;
using SlimDX.XInput;
using Chimera.Config;

namespace Joystick {
    public class XBoxControllerPlugin : AxisBasedDelta {
        public XBoxControllerPlugin()
            : base(
                "XBoxController",
                new XBoxMovementConfig(),
                new LeftThumbstickX(),
                new LeftThumbstickY(),
                new TriggerAxis(false),
                new RightThumbstickX(),
                new RightThumbstickY(),
                new DPadX(),
                new DPadY()
            ) { }
    }

    public class XBoxMovementConfig : AxisConfig {
        public XBoxMovementConfig()
            : base("XBoxController") {
        }

        protected override void InitConfig() {
            GetDeadzone("LeftThumbstickX");
            GetDeadzone("LeftThumbstickY");
            GetDeadzone("RightThumbstickX");
            GetDeadzone("RightThumbstickY");
            GetDeadzone("Trigger");
            GetDeadzone("DPadX");
            GetDeadzone("DPadY");

            GetScale("LeftThumbstickX");
            GetScale("LeftThumbstickY");
            GetScale("RightThumbstickX");
            GetScale("RightThumbstickY");
            GetScale("Trigger");
            GetScale("DPadX");
            GetScale("DPadY");

            GetBinding("LeftThumbstickX");
            GetBinding("LeftThumbstickY");
            GetBinding("RightThumbstickX");
            GetBinding("RightThumbstickY");
            GetBinding("Trigger");
            GetBinding("DPadX");
            GetBinding("DPadY");
        }
    }
}
