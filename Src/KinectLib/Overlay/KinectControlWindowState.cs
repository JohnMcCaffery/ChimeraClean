using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay;

namespace Chimera.Kinect.Overlay {
    public class KinectControlWindowState : WindowState {
        public override bool Active {
            get { return base.Active; }
            set {
                base.Active = value;
                Manager.Opacity = value ? .3 : 1.0;
                Manager.ControlPointer = !value;
            }
        }
        public KinectControlWindowState(WindowOverlayManager manager)
            : base(manager) {

            AddFeature(new SkeletonFeature(0f, 1f, 150f / 1080f, 100f));
        }
    }
}
