using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay.States {
    public class SeeThroughMenuState : State {
        public SeeThroughMenuState(string name, StateManager manager)
            : base(name, manager) {
        }

        public override IWindowState CreateWindowState(Window window) {
            throw new NotImplementedException();
        }

        public override void TransitionToStart() {
            throw new NotImplementedException();
        }

        protected override void TransitionToFinish() {
            throw new NotImplementedException();
        }

        protected override void TransitionFromStart() {
            throw new NotImplementedException();
        }

        public override void TransitionFromFinish() {
            throw new NotImplementedException();
        }
    }
}
