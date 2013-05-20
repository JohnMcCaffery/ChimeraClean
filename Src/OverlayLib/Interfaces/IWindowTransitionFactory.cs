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
using System.Xml;
using Chimera.Overlay.Interfaces;

namespace Chimera.Interfaces.Overlay {
    public interface IWindowTransitionFactory : IControllable {
        /// <summary>
        /// CreateWindowState a state transition for the specified window.
        /// </summary>
        /// <param name="transition">The transition which the new window transition will be part of.</param>
        /// <param name="manager">The window for the factory to create the transition for.</param>
        IWindowTransition Create(StateTransition transition, WindowOverlayManager manager);
    }
}
