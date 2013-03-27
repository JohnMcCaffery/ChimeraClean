using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chimera {
    public interface IState {

        Chimera.IWindowState[] WindowStates {
            get;
            set;
        }

        Chimera.StateTransition[] Transitions {
            get;
            set;
        }
    }
}
