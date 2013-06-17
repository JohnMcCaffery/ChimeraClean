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
using Chimera.Overlay;
using Chimera.Interfaces.Overlay;
using System.Drawing;

namespace Chimera {
    public abstract class FrameTransition : DrawableRoot, IWindowTransition {
        private StateTransition mTransition;
        private FrameOverlayManager mManager;
        private IWindowState mFrom;
        private IWindowState mTo;

        public override bool Active {
            get { return base.Active; }
            set {
                if (!mSelected)
                    base.Active = value;
            }
        }

        public override Rectangle Clip {
            get { return base.Clip; }
            set {
                base.Clip = value;
                mTo.Clip = value;
            }
        }

        public StateTransition Transition {
            get { return mTransition; }
        }
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="window"></param>
        /// <exception cref="InvalidArgumentException">Thrown if there is no window state for the To or From state.</exception>
        public FrameTransition(StateTransition transition, FrameOverlayManager manager)
            : base(manager.Name) {
            mManager = manager;
            mTransition = transition;

            mFrom = transition.From[manager.Name];
            mTo = transition.To[manager.Name];

            Finished += trans => mSelected = false;
        }

        public StateTransition StateTransition {
            get { return mTransition; }
        }

        public IWindowState To {
            get { return mTo; }
        }

        public IWindowState From {
            get { return mFrom; }
        }

        public FrameOverlayManager Manager {
            get { return mManager; }
        }

        public abstract event Action<IWindowTransition> Finished;

        public virtual void Begin() {
            mTo.Clip = Clip;
        }

        public abstract void Cancel();

        public bool Selected {
            get { return mSelected; }
            set { 
                mSelected = value;
                if (!value)
                    base.Active = false;
            }
        }

        private bool mSelected;
    }
}
