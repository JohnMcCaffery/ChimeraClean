using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay;
using Chimera.Overlay.Drawables;
using System.Drawing;

namespace Chimera.Kinect.Overlay {
    public class KinectControlState : State {
        private readonly Dictionary<Rectangle, OverlayImage> mActiveAreas = new Dictionary<Rectangle, OverlayImage>();
        private KinectInput mInput;
        private string mHelpImages;
        private string mWhereAmIImage;

        public override IWindowState CreateWindowState(Window window) {
            return new KinectControlWindowState(window.OverlayManager);
        }

        public KinectControlState(string name, StateManager manager, string mainWindow, string whereWindow)
            : base(name, manager) {

            mInput = manager.Coordinator.GetInput<KinectInput>();
            
            AddFeature(new SkeletonFeature(0f, 1f, 150f / 1080f, 100f, mainWindow));
            AddFeature(new OverlayImage(new Bitmap(mHelpImages), .05f, .2f, mainWindow));
            AddFeature(new OverlayImage(new Bitmap(mWhereAmIImage), .85f, .2f, mainWindow));
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

        public override void TransitionToStart() { }

        public override void TransitionFromFinish() { }

        public void AddActiveArea(Rectangle area, OverlayImage infoImage) {
        }
    }
}
