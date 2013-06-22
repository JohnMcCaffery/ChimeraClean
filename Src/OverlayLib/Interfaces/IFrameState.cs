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
    public interface IFrameState : IFeature {

        /// <summary>
        /// All features which are to be drawn onto the window state.
        /// </summary>
        IFeature[] Features {
            get;
        }

        /// <summary>
        /// Whether or not the window state is currently enabled.
        /// </summary>
        bool Active {
            get;
            set;
        }

        /// <summary>
        /// The manager which controls the window state.
        /// </summary>
        FrameOverlayManager Manager {
            get;
        }

        /// <summary>
        /// Add a drawable feature to the state. Any features added will be drawn on top of content drawn as part of the state itself.
        /// </summary>
        /// <param name="feature">The feature to add.</param>
        void AddFeature(IFeature feature);
    }
}
