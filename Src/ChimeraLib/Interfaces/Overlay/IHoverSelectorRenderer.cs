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
<<<<<<< HEAD
        /// <param name="percentageDone">How long the hovering has being going on. 0, just started, 1, complete.</param>
        void DrawHover(Graphics graphics, Rectangle bounds, double percentageDone);
=======
        /// <param name="hoverStart">When hovering started.</param>
        /// <param name="hoverLength">How long the selector will hover for.</param>
        void DrawHover(Graphics graphics, Rectangle bounds, double hoverLength);
>>>>>>> Implemented CursorSelectionRenderer.

        /// <summary>
        /// Draw the selection after it has been selected.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="bounds">The area which surrounds the selection.</param>
        void DrawSelected(Graphics graphics, Rectangle bounds);

        /// <summary>
        /// Clear up anything that needs to be cleared up after the selection has completed.
        /// </summary>
<<<<<<< HEAD
        void Clear();
=======
        void Completed();
>>>>>>> Implemented CursorSelectionRenderer.
    }
}
