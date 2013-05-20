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
using System.Drawing;
using Chimera.Overlay.Interfaces;

namespace Chimera {
    public interface IHoverSelectorRenderer : IControllable {
        /// <summary>
        /// Draw the selector hovering.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="bounds">The area which surrounds the selection.</param>
        /// <param name="percentageDone">How long the hovering has being going on. 0, just started, 1, complete.</param>
        void DrawHover(Graphics graphics, Rectangle bounds, double percentageDone);

        /// <summary>
        /// Draw the selection after it has been selected.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="bounds">The area which surrounds the selection.</param>
        void DrawSelected(Graphics graphics, Rectangle bounds);

        /// <summary>
        /// Clear up anything that needs to be cleared up after the selection has completed.
        /// </summary>
        void Clear();
    }
}
