using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;

namespace Chimera.Kinect.Overlay {
    public class SkeletonFoundTrigger : ITrigger {
        public event Action Triggered;

        public bool Active {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
    }
}
