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
        /// <summary>
        /// Triggered when the transition has finished.
        /// </summary>
        public event Action Finished;
        /// <summary>
        /// Transitions for each window.
        /// </summary>
        public IWindowTransition[] WindowTransitions {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        /// <summary>
        /// The state the transition goes from.
        /// </summary>
        public IState From {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        /// <summary>
        /// The state the transition goes to.
        /// </summary>
        public IState To {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        /// <summary>
        /// CustomTrigger which will start the transition.
        /// </summary>
        public Chimera.Interfaces.Overlay.ITrigger Trigger {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        public StateManager StateManager {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        /// <summary>
        /// Start the transition.
        /// </summary>
        public void Begin() {
            throw new System.NotImplementedException();
        }
    }
}
