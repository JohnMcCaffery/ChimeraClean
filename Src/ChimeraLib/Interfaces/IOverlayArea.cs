using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera {
    public interface ISelectable {
        /// <summary>
        /// Selected whenever this overlay area is select.
        /// </summary>
        event Action<ISelectable> Selected;

        /// <summary>
        /// Triggered when this overlay state is made visible. May be some time after the 'Show' method is called. Will be triggered after any fade in style effects complete.
        /// </summary>
        event Action<Chimera.ISelectable> Shown;

        /// <summary>
        /// Triggered when this overlay state is hidden. May be some time after the 'Hide' method is called. Will be triggered after any fade out style effects complete.
        /// </summary>
        event Action<Chimera.ISelectable> Hidden;

        /// <summary>
        /// The name of the window area is rendered on. Should be populated within the constructor.
        /// This may be set when Window is still null.
        /// </summary>
        string WindowName {
            get;
        }

        /// <summary>
        /// A multi line string that can be printed to file to store a record of state in the event of a crash.
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
        /// The state this area is rendered as part of.
        /// </summary>
        IOverlayState OverlayState {
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
        /// The bounding box around the area.
        /// </summary>
        Rectangle Bounds {
            get;
        }

        /// <summary>
        /// Initialise the overlay area, giving it a reference to the state it is linked to.
        /// </summary>
        /// <param name="coordinator">The coordinator object area is on.</param>
        /// <param name="state">The state this overlay is part of.</param>
        void Init(IOverlayState state);

        /// <summary>
        /// DrawSelected the overlay area on its parent container's surface. Don't draw any selection hints.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="clipRectangle">The bounds of the area being drawn on.</param>
        /// <param name="transparentColour">The colour that can be used to make the surface transparent.</param>
        void DrawBG(Graphics graphics, Rectangle clipRectangle);

        /// <summary>
        /// Show this overlay window. Will instantly activate the state. Has the same effect as setting Active to true.
        /// </summary>
        void Show();

        /// <summary>
        /// De-activate this overlay selection area. Doesn't guarantee that the area is hidden when it returns. The area is only officially hidden when the Visible property is false and the Hidden event fires.
        /// </summary>
        void Hide();

        /// <summary>
        /// DrawSelected the overlay area on its parent container's surface. Don't draw any selection hints.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="clipRectangle">The bounds of the area being drawn on.</param>
        /// <param name="transparentColour">The colour that can be used to make the surface transparent.</param>
        void Draw(Graphics graphics, Rectangle clipRectangle);
    }
}
