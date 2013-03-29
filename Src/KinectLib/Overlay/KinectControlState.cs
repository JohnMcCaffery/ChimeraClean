using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay;

namespace Chimera.Kinect.Overlay {
    public class KinectControlState : State {
        private KinectInput mInput;

        public override bool Active {
            get { return base.Active; }
            set {
                base.Active = value;
                mInput.FlyEnabled = value;
                mInput.WalkEnabled = value;
                mInput.YawEnabled = value;
                if (value)
                    Manager.Coordinator.EnableUpdates = true;
            }
        }
        public override IWindowState CreateWindowState(Window window) {
            return new KinectControlWindowState(window.OverlayManager);
        }

        public KinectControlState(string name, StateManager manager)
            : base(name, manager) {

            mInput = manager.Coordinator.GetInput<KinectInput>();
        }
    }
}
