using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay;

namespace Chimera.Flythrough.Overlay {
    public class FlythroughWindowState : WindowState {
        public FlythroughWindowState(WindowOverlayManager manager)
            : base(manager) {
        }

        protected override void OnActivated() { }
    }
}
