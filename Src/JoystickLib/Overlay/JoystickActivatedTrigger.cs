using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera;
using OpenMetaverse;
using Chimera.Overlay;
using Chimera.Overlay.Triggers;

namespace Joystick.Overlay {
    public class JoystickActivatedTrigger : ConditionTrigger {
        private Coordinator mCoordinator;
        private XBoxControllerPlugin mPlugin;
        private bool mInitialised;

        public JoystickActivatedTrigger(Coordinator coordinator)
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
