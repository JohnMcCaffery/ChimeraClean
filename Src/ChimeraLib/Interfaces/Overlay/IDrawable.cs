using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera.Interfaces.Overlay {
    public interface IDrawable {
        /// <summary>
        /// Whether the dynmic part of the drawable needs to be redrawn.
        /// </summary>
        bool NeedsRedrawn { get; }
        /// <summary>
        /// The name of the window this drawable is to be rendered on.
        /// </summary>
        string Window { get; }
        /// <summary>
        /// Notify the drawable that the area on which it is to draw has changed. Should draw any elements which only change when the area is resized to the supplied graphics object.
        /// </summary>
        /// <param name="clip">The area in which this drawable will be drawn.</param>
        /// <param name="graphics">The object with which to to draw any elements which only change when the area is resized.</param>
        void RedrawStatic(Rectangle clip, Graphics graphics);

        /// <summary>
        /// Draw the elements of the drawable that change more frequently than when the drawing area is resized.
        /// </summary>
        /// <param name="graphics">The object with which to draw the elements.</param>
        void DrawDynamic(Graphics graphics);
    }
}
