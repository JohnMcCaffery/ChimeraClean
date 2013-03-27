using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera {
    public interface ISelectable {
        /// <summary>
        /// Triggered whenever this overlay area is selected.
        /// </summary>
        event Action<ISelectable> Selected;
        /// <summary>
        /// Triggered whenever the static portion of what this selectable renders changes.
        /// </summary>
        event Action<ISelectable> StaticChanged;

        /// <summary>
        /// Triggered when this overlay window is made visible. May be some time after the 'Show' method is called. Will be triggered after any fade in style effects complete.
        /// </summary>
        event Action<Chimera.ISelectable> Shown;

        /// <summary>
        /// Triggered when this overlay window is hidden. May be some time after the 'Hide' method is called. Will be triggered after any fade out style effects complete.
        /// </summary>
        event Action<Chimera.ISelectable> Hidden;

        /// <summary>
        /// A multi line string that can be printed to file to store a record of window in the event of a crash.
        /// </summary>
        string DebugState {
            get;
        }

        /// <summary>
        /// Whether this overlay area has been selected.
        /// </summary>
        bool CurrentlySelected {
            get;
        }

        /// <summary>
        /// Whether the mouse is currently hovering over the area.
        /// </summary>
        bool CurrentlyHovering {
            get;
        }

        /// <summary>
        /// Whether this area is currently visible. Setting this will instantly hide or show the area, without running through any fade in/out routine/animations.
        /// </summary>
        bool Visible {
            get;
            set;
        }

        /// <summary>
        /// The renderer used to render when this is hovering or selected.
        /// </summary>
        ISelectionRenderer SelectionRenderer {
            get;
        }

        /// <summary>
        /// The bounding box around the area, specified as percentages.
        /// </summary>
        RectangleF Bounds {
            get;
        }

        /// <summary>
        /// The bounding box around the area, in pixels.
        /// </summary>
        Rectangle ScaledBounds {
            get;
        }

        /// <summary>
        /// True if the selectable area can currently be selected.
        /// </summary>
        bool Active {
            get;
            set;
        }

        /// <summary>
        /// Initialise the overlay area, giving it a reference to the window it is linked to.
        /// </summary>
        /// <param name="input">The input object area is on.</param>
        /// <param name="window">The window this overlay is part of.</param>
        void Init(Window window);

        /// <summary>
        /// DrawDynamic the static elements of the overlay area using the supplied graphics object.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="clipRectangle">The bounds of the area being drawn on.</param>
        /// <param name="transparentColour">The colour that can be used to make the surface transparent.</param>
        void DrawStatic(Graphics graphics, Rectangle clipRectangle);

        /// <summary>
        /// Show this overlay window. Will instantly activate the window. Has the same effect as setting Active to true.
        /// </summary>
        void Show();

        /// <summary>
        /// De-activate this overlay selection area. Doesn't guarantee that the area is hidden when it returns. The area is only officially hidden when the Visible property is false and the Hidden event fires.
        /// </summary>
        void Hide();

        /// <summary>
        /// DrawDynamic the dynamic elemnts of the overlay area using the supplied graphics object..
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="clipRectangle">The bounds of the area being drawn on.</param>
        /// <param name="transparentColour">The colour that can be used to make the surface transparent.</param>
        void DrawDynamic(Graphics graphics, Rectangle clipRectangle);
    }
}
