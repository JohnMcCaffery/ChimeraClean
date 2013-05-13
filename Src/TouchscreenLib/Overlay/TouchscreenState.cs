using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay;
using Chimera.Interfaces.Overlay;
using Chimera;

namespace Touchscreen.Overlay {
    public class TouchscreenState : State {
        private TouchscreenPlugin mPlugin;
        private bool mAvatar;

        public TouchscreenState(string name, bool avatar, Coordinator coordinator)
            : base(name, coordinator.StateManager) {

            mPlugin = coordinator.GetPlugin<TouchscreenPlugin>();
            mPlugin.Enabled = false;
            mAvatar = avatar;
        }

        public override IWindowState CreateWindowState(Chimera.Window window) {
            if (window.Name.Equals(mPlugin.Manager.Window.Name))
                return new TouchscreenWindow(window.OverlayManager, mPlugin);
            return new WindowState(window.OverlayManager);
        }

        public override void TransitionToStart() {
            Manager.Coordinator.ControlMode = mAvatar ? ControlMode.Delta : ControlMode.Absolute;
            mPlugin.Enabled = true;
        }

        protected override void TransitionToFinish() {
            Manager.Coordinator.EnableUpdates = true;
        }

        protected override void TransitionFromStart() {
            Manager.Coordinator.EnableUpdates = false;
            mPlugin.Enabled = false;
        }

        public override void TransitionFromFinish() { }
    }
}
