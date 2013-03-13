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
        /// Draw on the overlay to indicate the selectable area is being hovered over.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="clipRectangle">The bounds of the area being drawn on.</param>
        /// <param name="mHoverStart">The time when the hover started.</param>
        /// <param name="selectTime">How long the selection will hover before being selected.</param>
        void DrawHover(Graphics graphics, Rectangle clipRectangle, DateTime mHoverStart, double selectTime);

        /// <summary>
        /// Draw on the overlay to indicate the selectable area is selected.
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
