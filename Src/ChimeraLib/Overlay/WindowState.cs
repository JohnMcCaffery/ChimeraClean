using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;

namespace Chimera.Overlay {
    public class WindowState : DrawableRoot, IWindowState {
        /// <summary>
        /// The overlay form for the window this window state is linked to.
        /// </summary>
        private readonly WindowOverlayManager mManager;

        /// <param name="manager">The manager which controls this window state.</param>
        public WindowState(WindowOverlayManager manager)
            : base(manager.Window.Name) {
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
        public override bool Active {
            get { return base.Active; }
            set {
                base.Active = value;
                if (value)
                    OnActivated();
            }
        }

        /// <summary>
        /// Do any actions that need to be set as soon as the state is activated.
        /// Use this to make sure the overlay is set up as expected, e.g. set opacity and cursor control variables.
        /// </summary>
        protected virtual void OnActivated() { }
    }
}
