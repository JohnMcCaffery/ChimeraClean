using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay;

namespace Chimera.Interfaces.Overlay {
    public interface IWindowTransitionFactory {
        /// <summary>
        /// CreateWindowState a state transition for the specified window.
        /// </summary>
        /// <param name="transition">The transition which the new window transition will be part of.</param>
        /// <param name="window">The window for the factory to create the transition for.</param>
        IWindowTransition Create(StateTransition transition, Window window);
    }
}
