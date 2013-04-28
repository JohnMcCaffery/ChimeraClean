/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera {
    public interface IOverlayState {
        /// <summary>
        /// Triggered when this overlay window is activated.
        /// </summary>
        event System.Action<IState> Activated;

        /// <summary>
        /// Triggered when this overlay window is de-activated. May be some time after the 'Deactivate' method is called.
        /// </summary>
        event Action<IState> Deactivated;

        /// <summary>
        /// A multi line string that can be printed to file to store a record of window in the event of a crash.
        /// </summary>
        string State {
            get;
        }

        /// <summary>
        /// The unique name by which the window is known.
        /// </summary>
        string Name {
            get;
        }

        /// <summary>
        /// The type of mode this is.
        /// </summary>
        string Type {
            get;
        }

        /// <summary>
        /// Whether this window is currently active. Setting this to false will instantly de-activate the window, without running through any de-activation routine/animation that the Deactivate method would have triggered.
        /// </summary>
        bool Active {
            get;
            set;
        }

        /// <summary>
        /// Initialise the state, linking it with an overlay which it can render on.
        /// </summary>
        /// <param name="overlay">The The overlay this mode renders on.</param>
        void Init(IOverlay overlay);

        /// <summary>
        /// DrawDynamic any features of the window which refresh every tick.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="clipRectangle">The bounds of the area being drawn on.</param>
        /// <param name="transparentColour">The colour that can be used to make the surface transparent.</param>
        /// <param name="window">The overlay window to draw this window on.</param>
        void DrawDynamic(Graphics graphics, Rectangle clipRectangle, Window window);

        /// <summary>
        /// De-activate this overlay window. Doesn't guarantee that the window is de-activated when it returns. The window is only officially de-activated when the Active property is false and the Hidden event fires. Use this method to implement any fade out style effects.
        /// </summary>
        void Deactivate();

        /// <summary>
        /// Show this overlay window. Will instantly activate the window. Has the same effect as setting Active to true.
        /// </summary>
        void Activate();

        /// <summary>
        /// DrawDynamic any features of the window which only change when the window resizes.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="clipRectangle">The bounds of the area being drawn on.</param>
        /// <param name="window">The overlay window to draw this window on.</param>
        /// <param name="transparentColour">The colour that can be used to make the surface transparent.</param>
        void DrawStatic(Graphics graphics, Rectangle clipRectangle, Chimera.Window window);
    }
}
