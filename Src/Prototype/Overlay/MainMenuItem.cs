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

namespace Chimera.Overlay {
    public class MainMenuItem {
        private string mWindowName = "Main Window";
        private IState mState;
        private ISelectable mOverlay;

        public MainMenuItem(IState state, ISelectable overlay) {
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
        public IState State {
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
