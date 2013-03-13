using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera {
    public interface IOverlayState {
        /// <summary>
        /// Triggered when this overlay state is activated.
        /// </summary>
        event System.Action<IOverlayState> Activated;

        /// <summary>
        /// Triggered when this overlay state is de-activated. May be some time after the 'Deactivate' method is called.
        /// </summary>
        event Action<IOverlayState> Deactivated;

        /// <summary>
        /// A multi line string that can be printed to file to store a record of state in the event of a crash.
        /// </summary>
        string State {
            get;
        }

        /// <summary>
        /// The unique name by which the state is known.
        /// </summary>
        string Name {
            get;
            set;
        }

        /// <summary>
        /// The type of mode this is.
        /// </summary>
        string Type {
            get;
        }

        /// <summary>
        /// Whether this state is currently active. Setting this to false will instantly de-activate the state, without running through any de-activation routine/animation that the Deactivate method would have triggered.
        /// </summary>
        bool Active {
            get;
            set;
        }

        Coordinator Coordinator {
            get;
        }

        /// <summary>
        /// Initialise the mode, linking it with an overlay area which can trigger it.
        /// </summary>
        /// <param name="coordinator">The coordinator this state is tied to.</param>
        void Init(Coordinator coordinator);

        /// <summary>
        /// Draw any features of the state which refresh every tick.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="clipRectangle">The bounds of the area being drawn on.</param>
        /// <param name="transparentColour">The colour that can be used to make the surface transparent.</param>
        /// <param name="window">The overlay window to draw this state on.</param>
        void Draw(Graphics graphics, Rectangle clipRectangle, Window window);

        /// <summary>
        /// De-activate this overlay window. Doesn't guarantee that the state is de-activated when it returns. The state is only officially de-activated when the Active property is false and the Hidden event fires. Use this method to implement any fade out style effects.
        /// </summary>
        void Deactivate();

        /// <summary>
        /// Show this overlay window. Will instantly activate the state. Has the same effect as setting Active to true.
        /// </summary>
        void Activate();

        /// <summary>
        /// Draw any features of the state which only change when the window resizes.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="clipRectangle">The bounds of the area being drawn on.</param>
        /// <param name="transparentColour">The colour that can be used to make the surface transparent.</param>
        void DrawBG(Graphics graphics, Rectangle clipRectangle);
    }
}
