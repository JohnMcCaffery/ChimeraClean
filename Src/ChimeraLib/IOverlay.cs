using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera {
    public interface IOverlay {
        /// <summary>
        /// DrawSelected the overlay area on its parent container's surface.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="clipRectangle">The bounds of the area being drawn on.</param>
        /// <param name="transparentColour">The colour that can be used to make the surface transparent.</param>
        /// <param name="window">The overlay window to draw this state on.</param>
        void Draw(Graphics graphics, Rectangle clipRectangle, Color transparentColour, Window window);
    }
}
