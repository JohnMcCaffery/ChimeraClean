using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera {
    public interface IOverlayArea {
        IMode Mode {
            get;
            set;
        }

        Window Window {
            get;
            set;
        }

        /// <summary>
        /// Initialise the overlay area, giving it a reference to the window it is linked to.
        /// </summary>
        /// <param name="window">The window object area is on.</param>
        void Init(Window window);

        /// <summary>
        /// DrawH the overlay area on its parent container's surface.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="clipRectangle">The bounds of the area being drawn on.</param>
        /// <param name="transparentColour">The colour that can be used to make the surface transparent.</param>
        void Draw(Graphics graphics, ResolveEventArgs clipRectangle, Color transparentColour);
    }
}
