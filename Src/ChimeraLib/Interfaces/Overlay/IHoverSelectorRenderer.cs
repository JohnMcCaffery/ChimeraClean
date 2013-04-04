using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera {
    public interface IHoverSelectorRenderer {
        /// <summary>
        /// Draw the selector hovering.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="bounds">The area which surrounds the selection.</param>
        /// <param name="hoverStart">When hovering started.</param>
        /// <param name="hoverLength">How long the selector will hover for.</param>
        void DrawHover(Graphics graphics, Rectangle bounds, double hoverLength);

        /// <summary>
        /// Draw the selection after it has been selected.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="bounds">The area which surrounds the selection.</param>
        void DrawSelected(Graphics graphics, Rectangle bounds);

        /// <summary>
        /// Clear up anything that needs to be cleared up after the selection has completed.
        /// </summary>
        void Completed();
    }
}
