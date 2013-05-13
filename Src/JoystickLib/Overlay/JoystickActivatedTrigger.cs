using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera;
using OpenMetaverse;

namespace Joystick.Overlay {
    public class JoystickActivatedTrigger : ITrigger {
        private static readonly double TIMEOUT = 10000.0;
        private bool mActive;
        private Coordinator mCoordinator;
        private XBoxControllerPlugin mPlugin;
        private DateTime mLastTriggered = DateTime.Now;
        private bool mInitialised;

        public Coordinator Coordinator {
            get { return mCoordinator; }
        }

        public bool Initialised {
            get { return mInitialised; }
        }

        public JoystickActivatedTrigger(Coordinator coordinator) {
            mCoordinator = coordinator;
            if (mCoordinator.HasPlugin<XBoxControllerPlugin>()) {
                mPlugin = mCoordinator.GetPlugin<XBoxControllerPlugin>();
                mCoordinator.Tick += new Action(coordinator_Tick);
                mInitialised = true;
            }
        }

        void coordinator_Tick() {
            if (mActive && Triggered != null && DateTime.Now.Subtract(mLastTriggered).TotalMilliseconds > TIMEOUT &&
                (mPlugin.PositionDelta != Vector3.Zero || mPlugin.OrientationDelta.Pitch != 0.0 || mPlugin.OrientationDelta.Yaw != 0.0))
                Triggered();
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
