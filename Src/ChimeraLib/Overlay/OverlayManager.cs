using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay {
    public class StateManager {
        public IState[] States {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        public Chimera.Coordinator Coordinator {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        public Chimera.Interfaces.Overlay.IState CurrentState {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        public StateTransition CurrentTransition {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        /// <param name="coordinator">The coordinator this manager manages states for.</param>
        public void Init(Coordinator coordinator) {
            throw new System.NotImplementedException();
        }
    }
}
