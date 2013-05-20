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
using System.Xml;

namespace Chimera.Kinect.Overlay {
    public class InfoStateFactory : IStateFactory {
        public string Name {
            get { return "Info"; }
        }

        #region IFactory<IFeature> Members

        public State Create(OverlayPlugin manager, XmlNode node) {
            return new InfoState(manager, node);
        }

        public State Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }

        #endregion
    }

    public class InfoState : State {
        private readonly List<ActiveArea> mActiveAreas = new List<ActiveArea>();
        private List<OpenSimController> mControllers = new List<OpenSimController>();
        private WindowOverlayManager mMainWindow;
        private OverlayPlugin mPlugin;
        private string mGlowString;
        private string mNoGlowString;
        private int mGlowChannel;

        private class ActiveArea : XmlLoader {
            private IFeature mImage;
            private OverlayPlugin mManager;
            private List<PointF> mPoints = new List<PointF>();

            public IFeature Image {
                get { return mImage; }
            }
            private PointF FinalPoint {
                get { return mPoints[mPoints.Count - 1]; }
            }
            private bool Active {
                get {
                    Vector3 p = mManager.Coordinator.Position;
                    PointF p1 = FinalPoint;
                    int c = 0;
                    foreach (PointF p2 in mPoints) {
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

            public ActiveArea(OverlayPlugin manager, XmlNode node) {
                mManager = manager;
                mImage = manager.GetFeature(node, "help state active area", null);
                foreach (var child in GetChildrenOfChild(node, "Points")) {
                    float x = GetFloat(node, -1f, "X");
                    float y = GetFloat(node, -1f, "Y");
                    if (x > 0f && y > 0f)
                        mPoints.Add(new PointF(x, y));
                }
            }

            public void Test() {
                mImage.Active = Active;
            }

            public void Draw(Graphics g, Func<Vector3, Point> to2D) {
                PointF final = FinalPoint;
                g.DrawPolygon(Pens.Red, mPoints.Concat(new PointF[] { FinalPoint }).Select(p => to2D(new Vector3(p.X, p.Y, 0f))).ToArray());
            }
        }

        public override IWindowState CreateWindowState(WindowOverlayManager manager) {
            return new WindowState(manager);
        }

        public InfoState(string name, OverlayPlugin manager, string mainWindow, string whereWindow)
            : base(name, manager) {

            mPlugin = manager;
            mMainWindow = manager[mainWindow];
        }

        public InfoState(OverlayPlugin manager, XmlNode node)
            : base(GetName(node), manager) {

            mGlowString = GetString(node, "Glow", "GlowMessage");
            mNoGlowString = GetString(node, "NoGlow", "NoGlowMessage");
            mGlowChannel = GetInt(node, -40, "GlowChannel");

            foreach (var child in GetChildrenOfChild(node, "ActiveAreas")) {
                ActiveArea area = new ActiveArea(manager, child);
                AddFeature(area.Image);
                mActiveAreas.Add(area);
            }
        }

        public override void TransitionToStart() {
            TransitionToFinish();
        }
        protected override void TransitionToFinish() {
            Manager.Coordinator.EnableUpdates = false;
            foreach (var manager in mPlugin.OverlayManagers)
                manager.ControlPointer = true;
            foreach (var area in mActiveAreas)
                area.Test();
            Chat(mGlowString);
        }

        protected override void TransitionFromStart() {
            Chat(mNoGlowString);
        }

        public override void TransitionFromFinish() {
            Chat(mNoGlowString);
        }

        private void Chat(string msg) {
            foreach (var input in mControllers)
                input.ProxyController.Chat(msg, mGlowChannel);
        }

        public override void Draw(Graphics graphics, Func<Vector3, Point> to2D, Action redraw, Perspective perspective) {
            if (perspective == Perspective.Map) {
                foreach (var activeArea in mActiveAreas)
                    activeArea.Draw(graphics, to2D);
            }
        }
    }
}
