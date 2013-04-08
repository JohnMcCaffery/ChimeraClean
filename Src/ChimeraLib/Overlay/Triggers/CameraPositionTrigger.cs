using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;

namespace Chimera.Overlay.Triggers {
    public class CameraPositionTrigger : ITrigger {
        private bool mActive;
        private Rectangle mActiveArea;

        public event Action Triggered;

        public CameraPositionTrigger(Coordinator coordinator) {
            coordinator.CameraUpdated += new Action<Coordinator,CameraUpdateEventArgs>(coordinator_CameraUpdated);
        }

        private void coordinator_CameraUpdated(Coordinator coordinator, CameraUpdateEventArgs args) {
            if (mActive && Triggered != null && mActiveArea.Contains(new Point((int) args.position.X, (int) args.position.Y))) {
                Triggered();
            }
        }

        public bool Active {
            get { return mActive; }
            set { mActive = value; }
        }
    }
}
