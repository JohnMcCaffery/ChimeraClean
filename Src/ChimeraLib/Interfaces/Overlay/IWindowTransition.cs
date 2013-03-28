using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay;

namespace Chimera.Interfaces.Overlay {
    public interface IWindowTransition : IDrawable {
        /// <summary>
        /// Triggered when the transition has finished.
        /// </summary>
        event Action<IWindowTransition> Finished;

        /// <summary>
        /// The transition for the states as a whole.
        /// </summary>
        StateTransition StateTransition {
            get;
        }

        /// <summary>
        /// The window state the window is transitioning to.
        /// </summary>
        IWindowState To {
            get;
        }

        /// <summary>
        /// The window state the window is transitioning from.
        /// </summary>
        IWindowState From {
            get;
        }

        Chimera.Overlay.WindowOverlayManager Manager {
            get;
            set;
        }

        /// <summary>
        /// Begin the transition.
        /// </summary>
        void Begin();
    }
}
