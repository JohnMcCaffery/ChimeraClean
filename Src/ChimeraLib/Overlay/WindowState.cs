using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;

namespace Chimera.Overlay {
    public abstract class WindowState : DrawableRoot, IWindowState {
        /// <summary>
        /// The overlay form for the window this window state is linked to.
        /// </summary>
        private readonly WindowOverlayManager mManager;
        /// <summary>
        /// Whether the window state is currently active and should be drawn.
        /// </summary>
        private bool mActive;

        /// <param name="manager">The manager which controls this window state.</param>
        public WindowState(WindowOverlayManager manager) {
            mManager = manager;
        }

        /// <summary>
        /// The manager which controls this window state.
        /// </summary>
        public WindowOverlayManager Manager {
            get { return mManager; }
        }

        /// <summary>
        /// Whether or not the window state is currently enabled.
        /// </summary>
        public virtual bool Active {
            get { return mActive; }
            set { 
                mActive = value;
                if (value)
                    OnActivated();
            }
        }

        /// <summary>
        /// Do any actions that need to be set as soon as the state is activated.
        /// Use this to make sure the overlay is set up as expected, e.g. set opacity and cursor control variables.
        /// </summary>
        protected abstract void OnActivated();
    }
}
