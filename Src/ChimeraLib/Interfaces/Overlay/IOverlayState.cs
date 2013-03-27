using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay;

namespace Chimera.Interfaces.Overlay {
    public interface IState {

        /// <summary>
        /// The states for the individual windows.
        /// </summary>
        IWindowState[] WindowStates {
            get;
            set;
        }

        /// <summary>
        /// All the transitions from this state to other states.
        /// </summary>
        StateTransition[] Transitions {
            get;
            set;
        }

        /// <summary>
        /// The unique name for the state.
        /// </summary>
        string Name {
            get;
            set;
        }
    }
}
