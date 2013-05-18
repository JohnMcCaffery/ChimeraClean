using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay.Transitions;
using Chimera.Overlay.States;
using Chimera.Flythrough.Overlay;
using Chimera.Overlay.Triggers;

namespace Chimera.Launcher {
    public class XmlOverlayLauncher : Launcher {
        protected override void InitOverlay() {
            Coordinator.StateManager.LoadXML(
                Config.OverlayFile,
                Config.InterfaceMode,
                new IDrawableFactory[0],
                new ITriggerFactory[] {
                    new HoverTriggerFactory()
                },
                new ISelectionRendererFactory[0],
                new ITransitionStyleFactory[] { 
                    new OpacityTransitionFactory()
                },
                new IStateFactory[] { 
                    new ImageBGStateFactory(),
                    new FlythroughStateFactory()
                });
        }
    }
}
