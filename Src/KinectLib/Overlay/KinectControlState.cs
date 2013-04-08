using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay;
using Chimera.Overlay.Drawables;
using System.Drawing;
using Chimera.Overlay.Triggers;

namespace Chimera.Kinect.Overlay {
    public class KinectControlState : State {
        private KinectInput mInput;
        private bool mAvatar;

        public override IWindowState CreateWindowState(Window window) {
            return new KinectControlWindowState(window.OverlayManager);
        }

        public KinectControlState(string name, StateManager manager, bool avatar)
            : base(name, manager) {

            mInput = manager.Coordinator.GetInput<KinectInput>();
            mAvatar = avatar;
        }

        protected override void TransitionToFinish() {
            mInput.FlyEnabled = true;
            mInput.WalkEnabled = true;
            mInput.YawEnabled = true;
            mInput.Enabled = true;
            Manager.Coordinator.EnableUpdates = true;
        }

        protected override void TransitionFromStart() { 
            mInput.FlyEnabled = false;
            mInput.WalkEnabled = false;
            mInput.YawEnabled = false;       
        }

        public override void TransitionToStart() {
            Manager.Coordinator.ControlMode = mAvatar ? ControlMode.Delta : ControlMode.Absolute;
        }

        public override void TransitionFromFinish() { }
    }
}
