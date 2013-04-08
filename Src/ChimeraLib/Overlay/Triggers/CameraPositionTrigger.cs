using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;

namespace Chimera.Overlay.Triggers {
    public class CameraPositionTrigger : ITrigger {
        private bool mActive;
        private bool mInArea;
        private Rectangle mActiveArea;

        public event Action Triggered;
        public event Action Left;

        public CameraPositionTrigger(Coordinator coordinator) {
            coordinator.CameraUpdated += new Action<Coordinator,CameraUpdateEventArgs>(coordinator_CameraUpdated);
        }

        private void coordinator_CameraUpdated(Coordinator coordinator, CameraUpdateEventArgs args) {
            if (mActive) {
                if (mActiveArea.Contains(new Point((int)args.position.X, (int)args.position.Y))) {
                    mInArea = true;
                    if (Triggered != null)
                        Triggered();
                } else if (mInArea) {
                    mInArea = false;
                    if (Left != null)
                        Left();
                }
            }
        }

        public bool Active {
            get { return mActive; }
            set { 
                mActive = value;
                if (!value)
                    mInArea = false;
            }
        }
    }
}
