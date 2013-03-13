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
        /// Triggered when this overlay state is made visible
        /// </summary>
        event System.Action<Chimera.ISelectable> Shown;

        /// <summary>
        /// Triggered when this overlay state is hidden. May be some time after the 'Hide' method is called. Use this method to implement any fade out style effects.
        /// </summary>
        event System.Action<Chimera.ISelectable> Hidden;

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
        /// Whether this area is currently visible. Setting this to false will instantly hide the area, without running through any hide routine/animation that the Hide method would have triggered.
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
            set;
        }

        /// <summary>
        /// Initialise the overlay area, giving it a reference to the state it is linked to.
        /// </summary>
        /// <param name="coordinator">The coordinator object area is on.</param>
        /// <param name="window">The window this overlay should render on.</param>
        /// <param name="state">The state this overlay is part of.</param>
        void Init(Window window, IOverlayState state);

        /// <summary>
        /// DrawSelected the overlay area on its parent container's surface.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="clipRectangle">The bounds of the area being drawn on.</param>
        /// <param name="transparentColour">The colour that can be used to make the surface transparent.</param>
        void Draw(Graphics graphics, Rectangle clipRectangle, Color transparentColour);

        /// <summary>
        /// Show this overlay window. Will instantly activate the state. Has the same effect as setting Active to true.
        /// </summary>
        void Show();

        /// <summary>
        /// De-activate this overlay selection area. Doesn't guarantee that the area is hidden when it returns. The area is only officially hidden when the Visible property is false and the Hidden event fires.
        /// </summary>
        void Hide();
    }
}
