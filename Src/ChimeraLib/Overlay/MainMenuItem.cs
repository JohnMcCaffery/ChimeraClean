using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chimera.Overlay {
    public class MainMenuItem {
        private string mWindowName = "Main Window";
        private IOverlayState mState;
        private ISelectable mOverlay;

        public MainMenuItem(IOverlayState state, ISelectable overlay) {
            mState = state;
            mOverlay = overlay;
        }

        /// <summary>
        /// The overlay area which will trigger this item.
        /// </summary>
        public ISelectable Menu {
            get { return mOverlay; }
        }

        /// <summary>
        /// The state which this menu item launches.
        /// </summary>
        public IOverlayState State {
            get { return mState; }
        }

        public string WindowName {
            get { return mWindowName; }
        }

        /// <summary>
        /// Initialise this menu item, linking it to a main menu and a window.
        /// </summary>
        /// <param name="menu">The main menu this menu item is part of.</param>
        /// <param name="window">The window this menu item is rendered onto.</param>
        public void Init(MainMenu menu, Window window) {
            mState.Init(menu);
            mOverlay.Init(window);
        }
    }
}
