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

        public TouchscreenState(string name, bool avatar, StateManager manager)
            : base(name, manager) {

            mPlugin = manager.Coordinator.GetPlugin<TouchscreenPlugin>();
            mPlugin.Enabled = false;
            mAvatar = avatar;
        }

        public override IWindowState CreateWindowState(WindowOverlayManager manager) {
            if (manager.Name.Equals(mPlugin.Manager.Window.Name))
                return new TouchscreenWindow(manager, mPlugin);
            return new WindowState(manager);
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
