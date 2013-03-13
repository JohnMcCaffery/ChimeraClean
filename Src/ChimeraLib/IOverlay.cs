using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera {
    public interface IOverlay {
        /// <summary>
        /// Triggered whenever the active state changes. First argument is the old state, second the new one.
        /// </summary>
        event Action<IOverlayState, IOverlayState> ActiveStateChange;
        /// <summary>
        /// The current state the overlay is in.
        /// </summary>
        IOverlayState ActiveState {
            get;
        }
        /// <summary>
        /// DrawSelected the overlay area on its parent container's surface.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="clipRectangle">The bounds of the area being drawn on.</param>
        /// <param name="transparentColour">The colour that can be used to make the surface transparent.</param>
        /// <param name="window">The overlay window to draw this state on.</param>
        void Draw(Graphics graphics, Rectangle clipRectangle, Window window);

        /// <summary>
        /// Initialise this overlay with a reference to the coordinator it is part of.
        /// </summary>
        /// <param name="coordinator">The coordinator this overlay is part of.</param>
        void Init(Coordinator coordinator);

        /// <summary>
        /// Set the currently active state.
        /// </summary>
        /// <param name="newState">The new state.</param>
        void SetActiveSet(IOverlayState newState);
    }
}
