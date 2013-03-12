using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera {
    public interface IOverlayArea {
        /// <summary>
        /// Selected whenever this overlay area is select.
        /// </summary>
        event Action<IOverlayArea> Selected;

        /// <summary>
        /// A multi line string that can be printed to file to store a record of state in the event of a crash.
        /// </summary>
        string DebugState {
            get;
        }

        /// <summary>
        /// Whether this overlay area is currently active and should be drawn / selectable.
        /// </summary>
        bool CurrentlySelected {
            get;
        }

        IOverlayState OverlayState {
            get;
        }

        /// <summary>
        /// Initialise the overlay area, giving it a reference to the coordinator it is linked to.
        /// </summary>
        /// <param name="coordinator">The coordinator object area is on.</param>
        /// <param name="window">The window this overlay should render on.</param>
        /// <param name="state">The state this overlay is part of.</param>
        void Init(Window window, IOverlayState state);

        /// <summary>
        /// DrawH the overlay area on its parent container's surface.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="clipRectangle">The bounds of the area being drawn on.</param>
        /// <param name="transparentColour">The colour that can be used to make the surface transparent.</param>
        void Draw(Graphics graphics, Rectangle clipRectangle, Color transparentColour);
    }
}
