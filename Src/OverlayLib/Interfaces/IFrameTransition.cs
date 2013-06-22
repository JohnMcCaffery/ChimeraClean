/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay;

namespace Chimera.Interfaces.Overlay {
    public interface IWindowTransition : IFeature {
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
        IFrameState To {
            get;
        }

        /// <summary>
        /// The window state the window is transitioning from.
        /// </summary>
        IFrameState From {
            get;
        }

        /// <summary>
        /// The manager for the overlay on the window this state transition is rendered on.
        /// </summary>
        FrameOverlayManager Manager {
            get;
        }

        /// <summary>
        /// Begin the transition.
        /// </summary>
        void Begin();

        /// <summary>
        /// Cancel the transition before it has completed.
        /// </summary>
        void Cancel();

        bool Selected { get; set; }
    }
}
