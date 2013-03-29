using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay;

namespace Chimera.Interfaces.Overlay {
    public interface IWindowState : IDrawable {

        /// <summary>
        /// All features which are to be drawn onto the window state.
        /// </summary>
        IDrawable[] Features {
            get;
        }

        /// <summary>
        /// Whether or not the window state is currently enabled.
        /// </summary>
        bool Active {
            get;
            set;
        }

        /// <summary>
        /// The manager which controls the window state.
        /// </summary>
        WindowOverlayManager Manager {
            get;
        }

        /// <summary>
        /// Add a drawable feature to the state. Any features added will be drawn on top of content drawn as part of the state itself.
        /// </summary>
        /// <param name="feature">The feature to add.</param>
        void AddFeature(IDrawable feature);
    }
}
