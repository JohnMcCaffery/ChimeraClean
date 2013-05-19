using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay.States {
    public class ExploreState : State {
        public ExploreState(string name, OverlayPlugin manager)
            : base(name, manager) {
        }

        public override IWindowState CreateWindowState(WindowOverlayManager manager) {
            return new WindowState(manager);
        }

        public override void TransitionToStart() {
            Manager.Coordinator.EnableUpdates = true;
        }

        protected override void TransitionToFinish() { }

        protected override void TransitionFromStart() { }

        public override void TransitionFromFinish() { }
    }
}
