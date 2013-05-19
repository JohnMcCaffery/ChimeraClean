using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay.Transitions;
using Chimera.Overlay.States;
using Chimera.Flythrough.Overlay;
using Chimera.Overlay.Triggers;
using Chimera.Kinect.Overlay;
using Chimera.Multimedia;

namespace Chimera.Launcher {
    public class XmlOverlayLauncher : Launcher {
        protected override void InitOverlay() {
            Coordinator.StateManager.LoadXML(
                Config.OverlayFile,
                Config.InterfaceMode,
                new IImageTransitionFactory[] {
                    new BitmapFadeFactory()
                },
                new IDrawableFactory[0],
                new ITriggerFactory[] {
                    new SkeletonLostFactory(),
                    new SkeletonFoundFactory(),
                    new HoverTriggerFactory(),
                    new ClickTriggerFactory(),
                    new ImageHoverTriggerFactory(),
                    new ImageClickTriggerFactory(),
                    new TextHoverTriggerFactory(),
                    new TextClickTriggerFactory()
                },
                new ISelectionRendererFactory[0],
                new ITransitionStyleFactory[] { 
                    new BitmapTransitionFactory(),
                    new OpacityTransitionFactory()
                },
                new IStateFactory[] { 
                    new ImageBGStateFactory(),
                    new FlythroughStateFactory(),
                    new VideoStateFactory(new WMPMediaPlayer()),
                    new SlideshowStateFactory()
                });
        }
    }
}
