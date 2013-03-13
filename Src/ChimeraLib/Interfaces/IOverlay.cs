using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera {
    public interface IOverlay {
        /// <summary>
        /// Triggered whenever the active window changes. First argument is the old window, second the new one.
        /// </summary>
        event System.Action<Chimera.IOverlayState> StateSelected;
        /// <summary>
        /// The current window the overlay is in.
        /// </summary>
        IOverlayState SelectedState {
            get;
        }

        bool Active {
            get;
            set;
        }
        /// <summary>
        /// DrawDynamic the overlay for a specific window.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="clipRectangle">The bounds of the area being drawn on.</param>
        /// <param name="transparentColour">The colour that can be used to make the surface transparent.</param>
        /// <param name="window">The overlay window to draw this window on.</param>
        void Draw(Graphics graphics, Rectangle clipRectangle, Window window);

        /// <summary>
        /// Initialise this overlay with a reference to the coordinator it is part of.
        /// </summary>
        /// <param name="coordinator">The coordinator this overlay is part of.</param>
        void Init(Coordinator coordinator);

        /// <summary>
        /// Set the currently active window.
        /// </summary>
        /// <param name="newState">The new window.</param>
        void SelectState(IOverlayState newState);
    }
}
