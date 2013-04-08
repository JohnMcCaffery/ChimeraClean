using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay;

namespace Chimera.Kinect.Overlay {
    public class KinectControlState : State {
        private KinectInput mInput;

        public override IWindowState CreateWindowState(Window window) {
            return new KinectControlWindowState(window.OverlayManager);
        }

        public KinectControlState(string name, StateManager manager)
            : base(name, manager) {

            mInput = manager.Coordinator.GetInput<KinectInput>();
        }

        protected override void TransitionToFinish() {
            mInput.FlyEnabled = true;
            mInput.WalkEnabled = true;
            mInput.YawEnabled = true;
            Manager.Coordinator.EnableUpdates = true;
        }

        protected override void TransitionFromStart() { 
            mInput.FlyEnabled = false;
            mInput.WalkEnabled = false;
            mInput.YawEnabled = false;       
        }

        public override void TransitionToStart() {
            throw new NotImplementedException();
        }

        public override void TransitionFromFinish() {
            throw new NotImplementedException();
        }
    }
}
