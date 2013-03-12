using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera {
    public interface IOverlayState {

        /// <summary>
        /// A multi line string that can be printed to file to store a record of state in the event of a crash.
        /// </summary>
        string State {
            get;
        }

        /// <summary>
        /// The unique name by which the mode is known.
        /// </summary>
        string Name {
            get;
            set;
        }

        /// <summary>
        /// The type of mode this is.
        /// </summary>
        string Type {
            get;
        }

        /// <summary>
        /// Whether this state is currently active.
        /// </summary>
        bool Active {
            get;
            set;
        }

        /// <summary>
        /// The overlay areas this state renders.
        /// </summary>
        IOverlayArea[] OverlayAreas {
            get;
        }

        /// <summary>
        /// Initialise the mode, linking it with an overlay area which can trigger it.
        /// </summary>
        /// <param name="coordinator">The coordinator this state is tied to.</param>
        void Init(Coordinator coordinator);

        /// <summary>
        /// DrawH the overlay area on its parent container's surface.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="clipRectangle">The bounds of the area being drawn on.</param>
        /// <param name="transparentColour">The colour that can be used to make the surface transparent.</param>
        /// <param name="window">The overlay window to draw this state on.</param>
        void Draw(Graphics graphics, Rectangle clipRectangle, Color transparentColour, Window window);
    }
}
