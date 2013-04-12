using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay;
using Chimera.Overlay.Drawables;
using System.Drawing;
using Chimera.Overlay.Triggers;
using Chimera.Util;
using OpenMetaverse;

namespace Chimera.Kinect.Overlay {
    public class KinectControlState : State {
        private KinectInput mInput;
        private bool mAvatar;
        private List<CursorTrigger> mClickTriggers = new List<CursorTrigger>();
        private Rotation mStartOrientation;
        private Vector3 mStartPosition;

        public override IWindowState CreateWindowState(Window window) {
            return new KinectControlWindowState(window.OverlayManager);
        }

        public KinectControlState(string name, StateManager manager, bool avatar)
            : base(name, manager) {

            mInput = manager.Coordinator.GetInput<KinectInput>();
            mAvatar = avatar;

            mStartOrientation = new Rotation(manager.Coordinator.Orientation);
            mStartPosition = manager.Coordinator.Position;
            mInput.FlyEnabled = false;
            mInput.WalkEnabled = false;
            mInput.YawEnabled = false;
        }

        protected override void TransitionToFinish() {
            mInput.FlyEnabled = true;
            mInput.WalkEnabled = true;
            mInput.YawEnabled = true;
            mInput.Enabled = true;
        }

        protected override void TransitionFromStart() { 
            mInput.FlyEnabled = false;
            mInput.WalkEnabled = false;
            mInput.YawEnabled = false;       
        }

        public override void TransitionToStart() {
            Manager.Coordinator.ControlMode = mAvatar ? ControlMode.Delta : ControlMode.Absolute;
            if (!mAvatar) {
                Manager.Coordinator.EnableUpdates = true;
                Manager.Coordinator.Update(mStartPosition, Vector3.Zero, mStartOrientation, Rotation.Zero);
                Manager.Coordinator.EnableUpdates = false;
            }
            Manager.Coordinator.EnableUpdates = true; foreach (var window in Manager.Coordinator.Windows)
                window.OverlayManager.ControlPointer = false;
        }

        public override void TransitionFromFinish() {
            Manager.Coordinator.EnableUpdates = true; foreach (var window in Manager.Coordinator.Windows)
                window.OverlayManager.ControlPointer = true;
        }
    }
}
