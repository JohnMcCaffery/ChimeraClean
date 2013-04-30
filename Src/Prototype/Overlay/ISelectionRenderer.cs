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

namespace Chimera {
    public interface ISelectionRenderer {
        /// <summary>
        /// The selectable area this renderer is render selections for.
        /// </summary>
        ISelectable Selectable {
            get;
        }

        /// <summary>
        /// DrawDynamic on the overlay to indicate the selectable area is being hovered over.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="clipRectangle">The bounds of the area being drawn on.</param>
        /// <param name="mHoverStart">The time when the hover started.</param>
        /// <param name="selectTime">How long the selection will hover before being selected.</param>
        void DrawHover(Graphics graphics, Rectangle clipRectangle, DateTime mHoverStart, double selectTime);

        /// <summary>
        /// DrawDynamic on the overlay to indicate the selectable area is selected.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="clipRectangle">The bounds of the area being drawn on.</param>
        /// <param name="window">The window the selection is being draw on.</param>
        void DrawSelected(Graphics graphics, Rectangle clipRectangle);

        /// <summary>
        /// Initialise this render, linking it to a selectable area.
        /// </summary>
        /// <param name="area">The area this renderer should highlight as hovering or selected.</param>
        void Init(ISelectable area);
    }
}
