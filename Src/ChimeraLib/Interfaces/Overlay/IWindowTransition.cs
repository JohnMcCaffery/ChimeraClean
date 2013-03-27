using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chimera.Interfaces.Overlay {
    public interface IWindowTransition : IDrawable {
        /// <summary>
        /// Triggered when the transition has finished.
        /// </summary>
        event System.Action Finished;

        /// <summary>
        /// The transition for the state as a whole.
        /// </summary>
        Chimera.Overlay.StateTransition StateTransition {
            get;
            set;
        }

        /// <summary>
        /// Begin the transition.
        /// </summary>
        void Begin();
    }
}
