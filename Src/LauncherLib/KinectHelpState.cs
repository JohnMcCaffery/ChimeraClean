/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
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
        private readonly HashSet<OverlayImage> mInfoImages = new HashSet<OverlayImage>();
        private readonly CursorTrigger mClickTrigger;

        private KinectMovementPlugin mInput;
        private ImageHoverTrigger mWhereButton;
        private ImageHoverTrigger mCloseWhereButton;
        private Window mMainWindow;

        private string mHelpAvatarImages = "../Images/Caen/Misc/HelpSidebarAvatar.png";
        private string mHelpFlycamImages = "../Images/Caen/Misc/HelpSidebarFlycam.png";
        private string mWhereAmIImage = "../Images/Caen/Buttons/WhereAmIButton.png";
        private string mWhereWindow;

        public override IWindowState CreateWindowState(Window window) {
            return new WindowState(window.OverlayManager);
        }

        public KinectHelpState(string name, StateManager manager, string mainWindow, string whereWindow, bool avatar)
            : base(name, manager) {

            mInput = manager.Coordinator.GetPlugin<KinectMovementPlugin>();

            mMainWindow = manager.Coordinator[mainWindow];

            mWhereWindow = whereWindow;
            mWhereButton = new ImageHoverTrigger(mMainWindow.OverlayManager, new DialCursorRenderer(), new OverlayImage(new Bitmap(mWhereAmIImage), .65f, .25f, mainWindow));
            mWhereButton.Triggered += new Action(mWhereButton_Triggered);

            mCloseWhereButton = new ImageHoverTrigger(Manager.Coordinator[whereWindow].OverlayManager, new DialCursorRenderer(), mWhereButton.Image);
            mCloseWhereButton.Triggered += new Action(mCloseWhereButton_Triggered);

            mClickTrigger = new CursorTrigger(new CircleRenderer(100), mMainWindow);

            SkeletonFeature helpSkeleton = new SkeletonFeature(.065f, 0f, avatar ? .23f : .13f, 125f, mainWindow);
            AddFeature(helpSkeleton);
            AddFeature(new OverlayImage(new Bitmap(avatar ? mHelpAvatarImages : mHelpFlycamImages), .05f, avatar ? .2f : .1f, mainWindow));
            AddFeature(mClickTrigger);
            //AddFeature(mWhereButton);

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
            mClickTrigger.Active = true;
            SendText("Glow", -40);
        }

        protected override void TransitionFromStart() { 
            foreach (var trigger in mActiveAreas)
                trigger.Active = false;
            mClickTrigger.Active = false;
            SendText("NoGlow", -40);
        }

        public override void TransitionToStart() {
            SendText("Glow", -40);
            mInfoImages.Clear();
            foreach (var trigger in mActiveAreas)
                trigger.Active = true;
        }

        private void SendText(string msg, int channel) {
            foreach (var window in Manager.Coordinator.Windows) {
                if (window is OpenSimController)
                    (window as OpenSimController).ProxyController.Chat(msg, channel);
            }
        }

        public override void TransitionFromFinish() {
            mClickTrigger.Active = false;
            SendText("NoGlow", -40);
        }

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
