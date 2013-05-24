using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera;
using Chimera.Overlay;
using System.Xml;

namespace Joystick.Overlay {    public class JoystickInactivatedTriggerFactory : ITriggerFactory {
        public SpecialTrigger Special {
            get { return SpecialTrigger.None; }
        }

        public string Mode {
            get { return ""; }
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node) {
            return new JoystickInactiveTrigger(manager.Coordinator, node);
        }

        public ITrigger Create(OverlayPlugin manager, System.Xml.XmlNode node, System.Drawing.Rectangle clip) {
            return Create(manager, node);
        }

        public string Name {
            get { return "JostickActivated"; }
        }
    }

    public class JoystickInactiveTrigger : JoystickActivatedTrigger {
        private double mTimeoutS;
        private DateTime mLastTrigger = DateTime.Now;

        public JoystickInactiveTrigger(double timeout, Coordinator coordinator)
            : base(coordinator) {
            mTimeoutS = timeout;
        }

        public JoystickInactiveTrigger(Coordinator coordinator, XmlNode node)
            : base(coordinator) {
            mTimeoutS = GetDouble(node, 30, "TimeoutS");
        }

        public override bool Condition {
            get {
                if (base.Condition) {
                    mLastTrigger = DateTime.Now;
                    return false;
                }
                return DateTime.Now.Subtract(mLastTrigger).TotalSeconds > mTimeoutS;
            }
        }
    }
}
