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
            return new CutWindowTransition(transition, window);
        }
    }
    public class CutWindowTransition : WindowTransition {
        public override event Action<IWindowTransition> Finished;

        public CutWindowTransition(StateTransition transition, Window window)
            : base(transition, window) {
        }

        public override void Begin() {
            base.Begin();
            if (Finished != null)
                Finished(this);
        }

        public override void Cancel() { }

        public override bool NeedsRedrawn {
            get { return false; }
        }

        public override void DrawStatic(Graphics graphics) { }

        public override void DrawDynamic(Graphics graphics) { }
    }
}
