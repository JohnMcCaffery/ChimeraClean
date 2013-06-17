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
using Chimera.Interfaces.Overlay;
using System.Drawing;

namespace Chimera.Overlay {
    public class FrameState : DrawableRoot, IWindowState {
        /// <summary>
        /// The overlay form for the window this window state is linked to.
        /// </summary>
        private readonly FrameOverlayManager mManager;

        /// <param name="manager">The manager which controls this window state.</param>
        public FrameState(FrameOverlayManager manager)
            : base(manager.Frame.Name) {
            mManager = manager;
        }

        /// <summary>
        /// The manager which controls this window state.
        /// </summary>
        public FrameOverlayManager Manager {
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
