using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Chimera.Interfaces.Overlay;
using OpenMetaverse;
using System.Xml;
using Chimera.Interfaces;

namespace Chimera.Overlay.Triggers {
    public class ActiveAreaTriggerFactory : ITriggerFactory {
        #region ITriggerFactory Members

        public ITrigger Create(OverlayPlugin manager, XmlNode node) {
            return new ActiveAreaTrigger(manager, node);
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }

        #endregion

        #region IFactory Members

        public string Name {
            get { return "ActiveArea"; }
        }

        #endregion

        #region ITriggerFactory Members

        public SpecialTrigger Special {
            get { return SpecialTrigger.None; }
        }

        public string Mode {
            get { return null; }
        }

        #endregion
    }

        public class ActiveAreaTrigger : XmlLoader, ITrigger, IDiagramDrawable {
            private OverlayPlugin mManager;
            private List<PointF> mPoints = new List<PointF>();
            private DateTime mLastCheck;
            private DateTime mEntered;
            private Action mTickListener;
            private double mCheckWaitS;
            private bool mActive;
            private bool mInside;
            private bool mTriggered;

            private PointF FinalPoint {
                get { return mPoints[mPoints.Count - 1]; }
            }

            public bool Inside {
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

            public ActiveAreaTrigger(OverlayPlugin manager, XmlNode node) {
                mTickListener = new Action(Coordinator_Tick);
                mManager = manager;
                mCheckWaitS = GetDouble(node, 2, "CheckWaitS");
                foreach (var child in node.ChildNodes.OfType<XmlElement>()) {
                    float x = GetFloat(child, -1f, "X");
                    float y = GetFloat(child, -1f, "Y");
                    if (x > 0f && y > 0f)
                        mPoints.Add(new PointF(x, y));
                }
            }

            public void Draw(Graphics graphics, Func<Vector3, Point> to2D, Action redraw, Perspective perspective) {
                if (!mActive || perspective != Perspective.Map)
                    return;
                PointF final = FinalPoint;
                graphics.DrawPolygon(Pens.Red, mPoints.Concat(new PointF[] { FinalPoint }).Select(p => to2D(new Vector3(p.X, p.Y, 0f))).ToArray());
            }

            #region ITrigger Members

            public event Action Triggered;

            public bool Active {
                get { return mActive; }
                set {
                    if (value != mActive) {
                        mActive = value;
                        if (value)
                            mManager.Coordinator.Tick += mTickListener;
                        else
                            mManager.Coordinator.Tick -= mTickListener;
                    }
                }
            }

            void Coordinator_Tick() {
                if (Triggered == null)
                    return;
                if (Inside) {
                    if (!mInside) {
                        mInside = true;
                        Triggered();
                    }
                } else if (mInside) {
                    mInside = false;
                    mTriggered = false;
                }
            }

            #endregion
        }
}
