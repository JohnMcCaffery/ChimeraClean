using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera;

namespace Joystick.Overlay {
    public class JoystickInactiveTrigger : ITrigger {
        private bool mActive;
        private double mTimeout;
        private bool mTriggered = false;
        private DateTime mLastTrigger = DateTime.Now;
        private JoystickActivatedTrigger mPressedTrigger;

        public JoystickActivatedTrigger PressedTrigger {
            get { return mPressedTrigger; }
        }

        public JoystickInactiveTrigger(double timeout, Coordinator coordinator) {

            mTimeout = timeout;
            mPressedTrigger = new JoystickActivatedTrigger(coordinator);
            if (mPressedTrigger.Initialised) {
                mPressedTrigger.Coordinator.Tick += new Action(Coordinator_Tick);
                mPressedTrigger.Triggered += new Action(trigger_Triggered);
            }        }

        void trigger_Triggered() {
            mLastTrigger = DateTime.Now;
            mTriggered = false;
        }

        void Coordinator_Tick() {
            if (mActive && !mTriggered && Triggered != null && DateTime.Now.Subtract(mLastTrigger).TotalMilliseconds > mTimeout) {
                mTriggered = true;
                Triggered();
            }
        }

        #region ITrigger Members

        public event Action Triggered;

        public bool Active {
            get { return mActive; }
            set { 
                mActive = value;
                mPressedTrigger.Active = value;
            }
        }

        #endregion
    }
}
