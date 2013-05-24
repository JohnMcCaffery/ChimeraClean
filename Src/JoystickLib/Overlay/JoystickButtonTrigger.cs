using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay.Triggers;
using SlimDX.XInput;
using System.Xml;
using Chimera;

namespace Joystick.Overlay {
    public class JoystickButtonTrigger : ConditionTrigger {
        private GamepadButtonFlags mButton = GamepadButtonFlags.A;

        public override bool Condition {
            get { return GamepadManager.Gamepad.Buttons == mButton; }
        }

        public JoystickButtonTrigger(Coordinator coordinator, XmlNode node)
            : base(coordinator) {
            
            GamepadButtonFlags b = GamepadButtonFlags.A;
            if (Enum.TryParse<GamepadButtonFlags>(GetString(node, mButton.ToString(), "Button"), out b))
                mButton = b;
        }
    }
}
