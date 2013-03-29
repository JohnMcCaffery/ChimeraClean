using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using NuiLibDotNet;

namespace Chimera.Kinect.Overlay {
    public class SkeletonFoundTrigger : ITrigger {
        private bool mActive;

        public event Action Triggered;

        public bool Active {
            get { return mActive; }
            set { mActive = value; }
        }

        public SkeletonFoundTrigger() {
            Nui.SkeletonFound += new SkeletonTrackDelegate(Nui_SkeletonFound);
        }

        void Nui_SkeletonFound() {
            if (mActive && Triggered != null)
                Triggered();
        }
    }
}
