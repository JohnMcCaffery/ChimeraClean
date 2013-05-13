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

        public JoystickInactiveTrigger(double timeout, JoystickActivatedTrigger pressedTrigger) {
            mTimeout = timeout;
            mPressedTrigger = pressedTrigger;
            if (pressedTrigger.Initialised) {
                pressedTrigger.Coordinator.Tick += new Action(Coordinator_Tick);
                pressedTrigger.Triggered += new Action(trigger_Triggered);
            }
        }

        public JoystickInactiveTrigger(double timeout, Coordinator coordinator)
            : this(timeout, new JoystickActivatedTrigger(coordinator)) {
        }

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
            set { mActive = value; }
        }

        #endregion
    }
}
