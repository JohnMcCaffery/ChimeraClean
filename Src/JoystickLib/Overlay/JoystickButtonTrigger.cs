using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay.Triggers;
using SlimDX.XInput;
using System.Xml;
using Chimera;
using Chimera.Interfaces.Overlay;
using System.Drawing;
using log4net;
using Chimera.Overlay;

namespace Joystick.Overlay {
    public class JoystickButtonTriggerFactory : ITriggerFactory {
        public SpecialTrigger Special {
            get { return SpecialTrigger.None; }
        }

        public string Mode {
            get { return ""; }
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node) {
            return new JoystickButtonTrigger(manager.Core, node);
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }

        public string Name {
            get { return "JoystickButton"; }
        }
    }

    public class JoystickButtonTrigger : ConditionTrigger {
        private GamepadButtonFlags mButton = GamepadButtonFlags.A;
        private readonly ILog Logger = LogManager.GetLogger("OpenSim");

        public override bool Condition {
            get {
                return (GamepadManager.Gamepad.Buttons & mButton) == mButton;
            }
        }

        public JoystickButtonTrigger(Core coordinator, XmlNode node)
            : base(coordinator, GetName(node, "Joystick Button Trigger")) {

            
            GamepadButtonFlags b = GamepadButtonFlags.A;
            if (Enum.TryParse<GamepadButtonFlags>(GetString(node, mButton.ToString(), "Button"), out b))
                mButton = b;
        }
    }
}
