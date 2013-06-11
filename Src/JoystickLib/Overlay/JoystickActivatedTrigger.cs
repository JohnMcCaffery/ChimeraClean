using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera;
using OpenMetaverse;
using Chimera.Overlay;
using Chimera.Overlay.Triggers;
using System.Xml;

namespace Joystick.Overlay {
    public class JoystickActivatedTriggerFactory : ITriggerFactory {
        public SpecialTrigger Special {
            get { return SpecialTrigger.None; }
        }

        public string Mode {
            get { return ""; }
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node) {
            return new JoystickActivatedTrigger(manager.Coordinator);
        }

        public ITrigger Create(OverlayPlugin manager, System.Xml.XmlNode node, System.Drawing.Rectangle clip) {
            return Create(manager, node);
        }

        public string Name {
            get { return "JoystickActivated"; }
        }
    }


    public class JoystickActivatedTrigger : ConditionTrigger {
        private Core mCoordinator;
        private XBoxControllerPlugin mPlugin;
        private bool mInitialised;

        public JoystickActivatedTrigger(Core coordinator)
            : base(coordinator) {
            mCoordinator = coordinator;
            if (mCoordinator.HasPlugin<XBoxControllerPlugin>()) {
                mPlugin = mCoordinator.GetPlugin<XBoxControllerPlugin>();
                mInitialised = true;
            }
        }

        public override bool Condition {
            get {
                return mInitialised &&
                (mPlugin.PositionDelta != Vector3.Zero || mPlugin.OrientationDelta.Pitch != 0.0 || mPlugin.OrientationDelta.Yaw != 0.0);
            }
        }
    }
}
