using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera;
using Chimera.Overlay;

namespace Joystick.Overlay {
    public class JoystickInactiveTrigger : JoystickActivatedTrigger {
        private double mTimeoutS;
        private DateTime mLastTrigger = DateTime.Now;

        public JoystickInactiveTrigger(double timeout, Coordinator coordinator)
            : base(coordinator) {
            mTimeoutS = timeout;
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
