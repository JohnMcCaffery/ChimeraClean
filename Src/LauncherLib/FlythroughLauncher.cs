using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay;
using Chimera.Flythrough.Overlay;

namespace Chimera.Launcher {
    public class FlythroughLauncher : Launcher {
        protected override void InitOverlay() {
            State idleFlythrough = new FlythroughState("Flythrough", Coordinator.StateManager, "../Flythroughs/Cathedral5.xml");

            Coordinator.StateManager.AddState(idleFlythrough);
        }
    }
}
