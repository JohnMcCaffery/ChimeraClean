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
    public class KinectHelpState : State {
        private readonly List<ITrigger> mActiveAreas = new List<ITrigger>();
        private KinectInput mInput;
        private string mHelpImages = "../Images/HelpSidebar.png";
        private string mWhereAmIImage = "../Images/WhereAmIButton.png";
        private string mWhereWindow;
        private ImageHoverTrigger mWhereButton;
        private ImageHoverTrigger mCloseWhereButton;
        private readonly HashSet<OverlayImage> mInfoImages = new HashSet<OverlayImage>();
        private Window mMainWindow;

        public override IWindowState CreateWindowState(Window window) {
            return new WindowState(window.OverlayManager);
        }

        public KinectHelpState(string name, StateManager manager, string mainWindow, string whereWindow)
            : base(name, manager) {

            mInput = manager.Coordinator.GetInput<KinectInput>();

            mMainWindow = manager.Coordinator[mainWindow];

            mWhereWindow = whereWindow;
            mWhereButton = new ImageHoverTrigger(mMainWindow.OverlayManager, new DialCursorRenderer(), new OverlayImage(new Bitmap(mWhereAmIImage), .65f, .25f, mainWindow));
            mWhereButton.Triggered += new Action(mWhereButton_Triggered);

            mCloseWhereButton = new ImageHoverTrigger(Manager.Coordinator[whereWindow].OverlayManager, new DialCursorRenderer(), mWhereButton.Image);
            mCloseWhereButton.Triggered += new Action(mCloseWhereButton_Triggered);
            
            AddFeature(new SkeletonFeature(0f, 1f, 150f / 1080f, 100f, mainWindow));
            AddFeature(new OverlayImage(new Bitmap(mHelpImages), .05f, .1f, mainWindow));
            AddFeature(mWhereButton);

            mWhereButton.Active = false;
            mCloseWhereButton.Active = false;
        }

        void mCloseWhereButton_Triggered() {
            mCloseWhereButton.Active = false;
        }

        void mWhereButton_Triggered() {
            mCloseWhereButton.Image = mInfoImages.First();
            mCloseWhereButton.Active = true;
        }

        protected override void TransitionToFinish() {
            Manager.Coordinator.EnableUpdates = false;
            mMainWindow.OverlayManager.ControlPointer = true;
        }

        protected override void TransitionFromStart() { 
            foreach (var trigger in mActiveAreas)
                trigger.Active = false;
        }

        public override void TransitionToStart() {
            mInfoImages.Clear();
            foreach (var trigger in mActiveAreas)
                trigger.Active = true;
        }

        public override void TransitionFromFinish() { }

        public void AddActiveArea(Rectangle area, Bitmap infoImage) {
            OverlayImage info = new OverlayImage(infoImage, .2f, .2f, mWhereWindow);
            CameraPositionTrigger trigger = new CameraPositionTrigger(Manager.Coordinator);
            mActiveAreas.Add(trigger);
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
