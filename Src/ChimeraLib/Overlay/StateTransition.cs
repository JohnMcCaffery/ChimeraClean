using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay {
    public interface IStateTransition {
        IState From {
            get;
            set;
        }

        IState To {
            get;
            set;
        }

        IWindowTransition[] WindowTransitions {
            get;
            set;
        }

        ITrigger Trigger {
            get;
            set;
        }
    }

    public class StateTransition {
        public IWindowTransition[] WindowTransitions {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        public IState From {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        public IState To {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }
    }
}
