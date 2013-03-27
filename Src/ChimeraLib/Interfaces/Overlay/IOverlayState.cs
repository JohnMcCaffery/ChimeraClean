using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay;

namespace Chimera.Interfaces.Overlay {
    public interface IState {

        IWindowState[] WindowStates {
            get;
            set;
        }

        StateTransition[] Transitions {
            get;
            set;
        }
    }
}
