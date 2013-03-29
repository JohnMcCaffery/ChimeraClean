using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay;
using Chimera.Interfaces.Overlay;

namespace Chimera {
    public abstract class WindowTransition : IWindowTransition {
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
        public WindowTransition(StateTransition transition, Window window) {
            mManager = window.OverlayManager;
            mTransition = transition;

            mFrom = transition.From.WindowStates.First(s => s.Manager.Window.Name.Equals(window.Name));
            mTo = transition.To.WindowStates.First(s => s.Manager.Window.Name.Equals(window.Name));
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

        public abstract bool NeedsRedrawn {
            get;
        }

        public abstract void RedrawStatic(System.Drawing.Rectangle clip, System.Drawing.Graphics graphics);

        public abstract void DrawDynamic(System.Drawing.Graphics graphics);
    }
}
