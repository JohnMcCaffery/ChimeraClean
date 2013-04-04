using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay;

namespace Chimera.Kinect.Overlay {
    public class KinectControlWindowState : WindowState {

        public KinectControlWindowState(WindowOverlayManager manager)
            : base(manager) {

            AddFeature(new SkeletonFeature(0f, 1f, 150f / 1080f, 100f, manager.Window.Name));
        }

        protected override void OnActivated() {
            Manager.Opacity = .3;
            Manager.ControlPointer = false;
        }
    }
}
