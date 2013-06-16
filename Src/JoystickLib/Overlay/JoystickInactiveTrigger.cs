using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera;
using Chimera.Overlay;
using System.Xml;

namespace Joystick.Overlay {
    public class JoystickInactivatedTriggerFactory : ITriggerFactory {
        public SpecialTrigger Special {
            get { return SpecialTrigger.None; }
        }

        public string Mode {
            get { return ""; }
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node) {
            return new JoystickInactiveTrigger(manager.Core, node);
        }

        public ITrigger Create(OverlayPlugin manager, System.Xml.XmlNode node, System.Drawing.Rectangle clip) {
            return Create(manager, node);
        }

        public string Name {
            get { return "JoystickInactive"; }
        }
    }

    public class JoystickInactiveTrigger : JoystickActivatedTrigger {
        private double mTimeoutS;
        private DateTime mLastTrigger = DateTime.Now;

        public JoystickInactiveTrigger(double timeout, Core coordinator)
            : base(coordinator) {
            mTimeoutS = timeout;
        }

        public JoystickInactiveTrigger(Core coordinator, XmlNode node)
            : base(coordinator) {
            mTimeoutS = GetDouble(node, 30, "TimeoutS");
        }


        public override bool Active {
            get { return base.Active; }
            set {
                if (value)
                    mLastTrigger = DateTime.Now;
                base.Active = value;
            }
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
