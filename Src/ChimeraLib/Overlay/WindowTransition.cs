using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay;
using Chimera.Interfaces.Overlay;
using System.Drawing;

namespace Chimera {
    public abstract class WindowTransition : DrawableRoot, IWindowTransition {
        private StateTransition mTransition;
        private WindowOverlayManager mManager;
        private IWindowState mFrom;
        private IWindowState mTo;
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="window"></param>
        /// <exception cref="InvalidArgumentException">Thrown if there is no window state for the To or From state.</exception>
        public WindowTransition(StateTransition transition, Window window)
            : base(window.Name) {
            mManager = window.OverlayManager;
            mTransition = transition;

            mFrom = transition.From[window.Name];
            mTo = transition.To[window.Name];
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

        public WindowOverlayManager Manager {
            get { return mManager; }
        }

        public abstract event Action<IWindowTransition> Finished;

        public abstract void Begin();

        public abstract void Cancel();
    }
}
