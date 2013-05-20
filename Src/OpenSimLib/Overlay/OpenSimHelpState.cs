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
using Chimera.OpenSim;
using OpenMetaverse;

namespace Chimera.Kinect.Overlay {
    public class KinectHelpState : State {
        private readonly List<ActiveArea> mActiveAreas = new List<ActiveArea>();
        private readonly HashSet<OverlayImage> mInfoImages = new HashSet<OverlayImage>();
        private List<OpenSimController> mControllers = new List<OpenSimController>();
        private readonly CursorTrigger mClickTrigger;

        private ImageHoverTrigger mWhereButton;
        private ImageHoverTrigger mCloseWhereButton;
        private WindowOverlayManager mMainWindow;
        private OverlayPlugin mPlugin;

        private string mHelpImages = "../Images/Caen/Misc/HelpSidebar.png";
        private string mWhereAmIImage = "../Images/Caen/Buttons/WhereAmIButton.png";
        private string mWhereWindow;

        private class ActiveArea {
            private OverlayImage mImage;
            private WindowOverlayManager mManager;
            private List<Vector2> mPoints;

            private Vector2 FinalPoint {
                get { return mPoints[mPoints.Count - 1]; }
            }
            private bool Active {
                get {
                    Vector3 p = mManager.Manager.Coordinator.Position;
                    Vector2 p1 = FinalPoint;
                    int c = 0;
                    foreach (Vector2 p2 in mPoints) {
                        float delta = p1.X * p2.Y - p1.Y * p2.X;
                        if (delta == 0)
                            continue;

                        float x = (p2.Y * p.X - p2.X * p.Y) / delta;
                        float y = (p1.X * p.Y - p1.Y * p.X) / delta;
                        if (Math.Min(p1.X, p2.X) < p.X &&
                            Math.Min(p1.Y, p2.Y) < p.Y &&
                            Math.Max(p1.X, p2.X) > p.X &&
                            Math.Max(p1.Y, p2.Y) > p.Y)
                            c++;
                        p1 = p2;
                    }
                    return c % 2 != 0;
                }
            }

            public void Draw(Graphics g, Func<Vector3, Point> to2D) {
                Vector2 final = FinalPoint;
                g.DrawPolygon(Pens.Red, mPoints.Concat(new Vector2[] { FinalPoint }).Select(p => to2D(new Vector3(p, 0f))).ToArray());
            }
        }

        public override IWindowState CreateWindowState(WindowOverlayManager manager) {
            return new WindowState(manager);
        }

        public KinectHelpState(string name, OverlayPlugin manager, string mainWindow, string whereWindow)
            : base(name, manager) {

            mPlugin = manager;
            mMainWindow = manager[mainWindow];

            mWhereWindow = whereWindow;
            mWhereButton = new ImageHoverTrigger(mMainWindow, manager.Renderers[0], new OverlayImage(new Bitmap(mWhereAmIImage), .65f, .25f, mainWindow));
            mWhereButton.Triggered += new Action(mWhereButton_Triggered);

            mCloseWhereButton = new ImageHoverTrigger(Manager[whereWindow], manager.Renderers[0], mWhereButton.Image);
            mCloseWhereButton.Triggered += new Action(mCloseWhereButton_Triggered);

            mClickTrigger = new CursorTrigger(new CircleRenderer(100), mMainWindow);

            //SkeletonFeature helpSkeleton = new SkeletonFeature(.065f, 0f, .13f, 125f, mainWindow);
            //AddFeature(helpSkeleton);
            AddFeature(new OverlayImage(new Bitmap(mHelpImages), .05f, .1f, mainWindow));
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
            mMainWindow.ControlPointer = true;
            mClickTrigger.Active = true;
            Chat("Glow");
        }

        protected override void TransitionFromStart() { 
            mClickTrigger.Active = false;
            Chat("NoGlow");
        }

        public override void TransitionToStart() {
            Chat("Glow");
            mInfoImages.Clear();
        }

        public override void TransitionFromFinish() {
            mClickTrigger.Active = false;
            Chat("NoGlow");
        }

        private void Chat(string msg) {
            foreach (var input in mControllers)
                input.ProxyController.Chat(msg, -40);
        }

        public override void Draw(Graphics graphics, Func<Vector3, Point> to2D, Perspective perspective) {
            if (perspective == Perspective.Map) {
                foreach (var activeArea in mActiveAreas)
                    activeArea.Draw(graphics, to2D);
            }
        }
    }
}
