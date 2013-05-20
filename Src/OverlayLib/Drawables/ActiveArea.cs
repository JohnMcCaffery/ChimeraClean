using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Chimera.Interfaces.Overlay;
using OpenMetaverse;
using System.Xml;
using Chimera.Interfaces;

namespace Chimera.Overlay.Drawables {
        public class ActiveArea : XmlLoader, IFeature, IDiagramDrawable {
            private IFeature mImage;
            private OverlayPlugin mManager;
            private List<PointF> mPoints = new List<PointF>();
            private DateTime mLastCheck;
            private double mCheckWaitS;
            private bool mActive;

            public IFeature Image {
                get { return mImage; }
            }

            private PointF FinalPoint {
                get { return mPoints[mPoints.Count - 1]; }
            }

            public bool Active {
                get {
                    if (!mActive)
                        return false;
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
                    mImage.Active = c % 2 != 0;
                    return mImage.Active;
                }
                set { mActive = value; }
            }

            public ActiveArea(OverlayPlugin manager, XmlNode node) {
                mManager = manager;
                mImage = manager.GetFeature(node, "help state active area", null);
                mCheckWaitS = GetDouble(node, 2, "CheckWaitS");
                foreach (var child in node.ChildNodes.OfType<XmlElement>()) {
                    float x = GetFloat(node, -1f, "X");
                    float y = GetFloat(node, -1f, "Y");
                    if (x > 0f && y > 0f)
                        mPoints.Add(new PointF(x, y));
                }
            }

            public void Draw(Graphics graphics, Func<Vector3, Point> to2D, Action redraw, Perspective perspective) {
                if (perspective != Perspective.Map)
                    return;
                PointF final = FinalPoint;
                graphics.DrawPolygon(Pens.Red, mPoints.Concat(new PointF[] { FinalPoint }).Select(p => to2D(new Vector3(p.X, p.Y, 0f))).ToArray());
            }

            #region IFeature Members

            public Rectangle Clip {
                get { return mImage.Clip; }
                set { mImage.Clip = value; }
            }

            private bool mNeedsRedraw;

            public bool NeedsRedrawn {
                get {
                    if (DateTime.Now.Subtract(mLastCheck).TotalSeconds > mCheckWaitS) {
                        bool val = mNeedsRedraw;
                        bool ret = Active;
                        if (val != ret)
                            mManager[mImage.Window].ForceRedrawStatic();
                        mNeedsRedraw = ret;
                        mLastCheck = DateTime.Now;
                    }
                    return mNeedsRedraw;
                }
            }

            public string Window {
                get { return mImage.Window; }
            }

            public void DrawStatic(Graphics graphics) {
                mImage.DrawStatic(graphics);
            }

            public void DrawDynamic(Graphics graphics) {
                mImage.DrawDynamic(graphics);
            }

            #endregion
        }
}
