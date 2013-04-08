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
        private readonly Dictionary<Rectangle, OverlayImage> mActiveAreas = new Dictionary<Rectangle, OverlayImage>();
        private KinectInput mInput;
        private string mHelpImages = "../Images/HelpSidebar.png";
        private string mWhereAmIImage = "../WhereAmIButton.png";
        private string mWhereWindow;
        private ImageHoverTrigger mWhereButton;
        private ImageHoverTrigger mCloseWhereButton;
        private readonly HashSet<OverlayImage> mInfoImages = new HashSet<OverlayImage>();

        public override IWindowState CreateWindowState(Window window) {
            return new KinectControlWindowState(window.OverlayManager);
        }

        public KinectControlState(string name, StateManager manager)
            : base(name, manager) {

            mInput = manager.Coordinator.GetInput<KinectInput>();
        }

        void mCloseWhereButton_Triggered() {
            mCloseWhereButton.Active = false;
        }

        void mWhereButton_Triggered() {
            mCloseWhereButton.Image = mInfoImages.First();
            mCloseWhereButton.Active = true;
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
            mInfoImages.Clear();
        }

        public override void TransitionFromFinish() { }

        public void AddActiveArea(Rectangle area, Bitmap infoImage) {
            OverlayImage info = new OverlayImage(infoImage, .2f, .2f, mWhereWindow);
            CameraPositionTrigger trigger = new CameraPositionTrigger(Manager.Coordinator);
            trigger.Triggered += () => {
                mInfoImages.Add(info);
                mWhereButton.Active = true;
            };
            trigger.Left += () => {
                mInfoImages.Remove(info);
                mWhereButton.Active = mInfoImages.Count > 0;
            };
        }
    }
}
