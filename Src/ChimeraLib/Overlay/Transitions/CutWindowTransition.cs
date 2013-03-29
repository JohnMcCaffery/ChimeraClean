using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay;
using System.Drawing;

namespace Chimera.Overlay.Transitions {
    public class CutWindowTransitionFactory : IWindowTransitionFactory {
        public IWindowTransition Create(StateTransition transition, Window window) {
            throw new NotImplementedException();
        }
    }
    public class CutWindowTransition : IWindowTransition {
        public event Action<IWindowTransition> Finished;

        public Overlay.StateTransition StateTransition {
            get { throw new NotImplementedException(); }
        }

        public IWindowState To {
            get { throw new NotImplementedException(); }
        }

        public IWindowState From {
            get { throw new NotImplementedException(); }
        }

        public WindowOverlayManager Manager {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public void Begin() {
            throw new NotImplementedException();
        }

        public void Cancel() {
            throw new NotImplementedException();
        }

        public bool NeedsRedrawn {
            get { throw new NotImplementedException(); }
        }

        public void RedrawStatic(Rectangle clip, Graphics graphics) {
            throw new NotImplementedException();
        }

        public void DrawDynamic(Graphics graphics) {
            throw new NotImplementedException();
        }
    }
}
