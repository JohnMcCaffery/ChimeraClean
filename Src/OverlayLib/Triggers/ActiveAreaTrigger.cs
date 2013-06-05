using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Chimera.Interfaces.Overlay;
using OpenMetaverse;
using System.Xml;
using Chimera.Interfaces;
using Chimera.Util;

namespace Chimera.Overlay.Triggers {
    public enum Event {
        Both,
        Exit,
        Enter
    }

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

        public class ActiveAreaTrigger : ConditionTrigger, IDiagramDrawable {
            private OverlayPlugin mManager;
            private List<Vector2> mPoints = new List<Vector2>();
            private Event mType = Event.Both;
            private double mCheckWaitS;
            private bool mInside;

            private bool mUseZ;
            private bool mUseYaw;
            private bool mUsePitch;

            private float mZStart;
            private float mZFinish;
            private double mPitchStart;
            private double mPitchFinish;
            private double mYawStart;
            private double mYawFinish;

            private Vector2 FinalPoint {
                get { return mPoints[mPoints.Count - 1]; }
            }

            private float Cross(Vector2 v, Vector2 w) {
                return (v.X * w.Y) - (v.Y * w.X);
            }

            public bool Inside {
                get {
                    // http://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect
                    Vector3 p3 = mManager.Coordinator.Position;
                    Vector2 p = new Vector2(p3.X, p3.Y);
                    Vector2 r = new Vector2(0f, 100f) - p;

                    Vector2 q = FinalPoint;
                    int c = 0;
                    foreach (Vector2 sAbs in mPoints) {
                        Vector2 s = sAbs - q;
                        //Parallel
                        if (Cross(r, s) != 0f) {
                            float t = Cross(q - p, s / Cross(r, s));
                            float u = Cross(q - p, r / Cross(r, s));

                            if (t >= 0f && t <= 1f &&
                                u >= 0f && u <= 1f)
                                c++;
                        }
                        q = sAbs;
                    }

                    bool insideH = c % 2 != 0;

                    double pitch = mManager.Coordinator.Orientation.Pitch;
                    double yaw = mManager.Coordinator.Orientation.Yaw;
                    bool yawMet = mUseYaw ? yaw >= mYawStart && yaw <= mYawFinish : true;
                    bool pitchMet = mUsePitch ? pitch >= mPitchStart && pitch <= mPitchFinish : true;
                    bool z = mUseZ ? p3.Z >= mZStart && p3.Z <= mZFinish : true;

                    return insideH && yawMet && pitchMet;
                }
            }

            public override bool Condition {
                get {
                    bool wasInside = mInside;
                    mInside = Inside;
                    switch (mType) {
                        case Event.Enter: return mInside;
                        case Event.Exit: return !mInside;
                        default: return wasInside != mInside;
                    }
                }
            }

            public ActiveAreaTrigger(OverlayPlugin manager, XmlNode node)
                : base(manager.Coordinator) {
                mManager = manager;
                mCheckWaitS = GetDouble(node, 2, "CheckWaitS");
                foreach (var child in GetChildrenOfChild(node, "Points")) {
                    float x = GetFloat(child, -1f, "X");
                    float y = GetFloat(child, -1f, "Y");
                    if (x > 0f && y > 0f)
                        mPoints.Add(new Vector2(x, y));
                }

                string t = GetString(node, null, "Type");
                if (t != null) {
                    Event type = mType;
                    if (Enum.TryParse<Event>(t, out type))
                        mType = type;
                }

                double defD = -1000.0;
                float defF = -1000f;

                XmlNode yawNode = node.SelectSingleNode("child::Yaw");
                XmlNode pitchNode = node.SelectSingleNode("child::Pitch");
                XmlNode zNode = node.SelectSingleNode("child::Height");

                if (yawNode != null) {
                    mYawStart = GetDouble(yawNode, defD, "Min");
                    mYawFinish = GetDouble(yawNode, defD, "Max");
                    mUseYaw = mYawStart != defD && mYawFinish != defD;
                }
                if (pitchNode != null) {
                    mPitchStart = GetDouble(pitchNode, defD, "Min");
                    mPitchFinish = GetDouble(pitchNode, defD, "Max");
                    mUsePitch = mPitchStart != defD && mPitchFinish != defD;
                }
                if (zNode != null) {
                    mUseZ = mZStart != defF && mZFinish != defF;
                    mZStart = GetFloat(zNode, defF, "Bottom");
                    mZFinish = GetFloat(zNode, defF, "Top");
                }
            }

            public Event Type {
                get { return mType; }
                set { mType = value; }
            }

            public void Draw(Graphics graphics, Func<Vector3, Point> to2D, Action redraw, Perspective perspective) {
                if (!Active || perspective != Perspective.Map)
                    return;
                PointF final = new PointF(FinalPoint.X, FinalPoint.Y);
                graphics.DrawPolygon(Pens.Red, mPoints.Concat(new Vector2[] { FinalPoint }).Select(p => to2D(new Vector3(p.X, p.Y, 0f))).ToArray());
            }
        }
}
