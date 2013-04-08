using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.Drawing;
using Chimera.Util;

namespace Chimera.Core {
    internal class Projector {
        private readonly List<Vector3[]> mWalls = new List<Vector3[]>();
        private readonly HashSet<Action> mRedraws = new HashSet<Action>();

        private static float mW = 4780f;
        private static float mH = 2300f;
        private static float mD = 3710f;

        private Window mWindow;
        private Vector3 mRoomPosition = new Vector3(mD / -2f, mW / -2f, mH / 2f);
        private Vector3 mPosition = new Vector3(200f, mW / 2f, -251f);
        private Rotation mOrientation = new Rotation(5.0, 0.0);
        private float mThrowRatio = 1.7f;
        private float mAspectRatio = 16f / 9f;
        private bool mDraw = true;
        private bool mDrawRoom = true;
        private bool mDrawLabels = true;
        private bool mAutoUpdate = false;
        private float mWallDistance = 2000f;

        public Vector3 Position {
            get { return mPosition; }
            set { 
                mPosition = value;
                Redraw();
            }
        }

        public Vector3 RoomPosition {
            get { return mRoomPosition; }
            set { 
                mRoomPosition = value;
                Redraw();
            }
        }

        public Rotation Orientation {
            get { return mOrientation; }
        }

        public bool AutoUpdate {
            get { return mAutoUpdate; }
            set { mAutoUpdate = value; }
        }

        public bool DrawDiagram {
            get { return mDraw; }
            set {
                mDraw = value;
                Redraw();
            }
        }

        public bool DrawRoom {
            get { return mDrawRoom; }
            set {
                mDrawRoom = value;
                Redraw();
            }
        }

        public bool DrawLabels {
            get { return mDrawLabels; }
            set {
                mDrawLabels = value;
                Redraw();
            }
        }

        public float AspectRatio {
            get { return mAspectRatio; }
            set {
                mAspectRatio = value;
                Redraw();
            }
        }

        public float ThrowRatio {
            get { return mAspectRatio; }
            set {
                mThrowRatio = value;
                Redraw();
            }
        }

        public float WallDistance {
            get { return mWallDistance; }
            set {
                mWallDistance = value;
                Redraw();
            }
        }

        public float Clearance {
            get {
                Vector3 bottomCentre = GetCorner(0f, -1f);
                Vector3 projector = mRoomPosition + mPosition;
                Vector3 planeDir = bottomCentre - projector;
                Vector3 planeNormal = Vector3.Cross(planeDir, Vector3.UnitY * new Rotation(0.0, mOrientation.Yaw).Quaternion);

                //Calculate the intersection of the plane defined by the point mPlaneTopLeft and the normal mPlaneNormal and the line defined by the point mPointStart and the direction mPointDir.
                float numerator = Vector3.Dot(Vector3.Zero - projector, planeNormal);
                float denominator = Vector3.Dot(planeNormal, Vector3.UnitZ);
                bool mIntersects = denominator != 0f;
                if (!mIntersects)
                    return -1f;
                return (-numerator / denominator) + (mH - mRoomPosition.Z);
            }
        }

        internal Projector(Window window) {
            mWindow = window;
            mOrientation.Changed += new EventHandler(mOrientation_Changed);
            mWallDistance = mD - mPosition.X;
        }

        void mOrientation_Changed(object sender, EventArgs e) {
            Redraw();
        }

        internal void Redraw() {
            if (mAutoUpdate && mWindow != null)
                ConfigureWindow();
            foreach (var redraw in mRedraws)
                redraw();
        }

        private void AddWall(Vector3 topLeft, Vector3 topRight, Vector3 bottomLeft, Vector3 bottomRight) {
            mWalls.Add(new Vector3[] { topLeft, topRight, bottomRight, bottomLeft });
        }

        public void ConfigureWindow() {
            mWindow.Width = Vector3.Mag(new Vector3(TopLeft.X, TopLeft.Y, 0f) - new Vector3(TopRight.X, TopRight.Y, 0f));
            mWindow.Height = TopLeft.Z - BottomLeft.Z;
            mWindow.Orientation.Quaternion = new Rotation(0.0, mOrientation.Yaw).Quaternion;
            mWindow.TopLeft = TopLeft;
        }

        private float WallScale {
            get { return (new Vector3(mWallDistance, 0f, 0f) * new Rotation(0.0, mOrientation.Yaw).Quaternion).X; }
        }

        public float Drop {
            get { return Math.Abs(mPosition.Z); }
        }

        private Vector3 TopLeft {
            get { return GetCorner(-1f, 1f); }
        }
        private Vector3 TopRight {
            get { return GetCorner(1f, 1f); }
        }
        private Vector3 BottomLeft {
            get { return GetCorner(-1f, -1f); }
        }
        private Vector3 BottomRight {
            get { return GetCorner(1f, -1f); }
        }

        private Vector3 GetCorner(float left, float up) {
            Vector3 originalVector = new Vector3(mThrowRatio, .5f * left, (.5f * up) / mAspectRatio);
            Vector3 corner = originalVector * new Rotation(mOrientation.Pitch, 0.0).Quaternion;
            corner *= mWallDistance / corner.X;
            corner *= new Rotation(0.0, mOrientation.Yaw).Quaternion;
            corner += mPosition + mRoomPosition;
            return corner;
        }

        public void Draw(Graphics g, Func<Vector3, Point> to2D, Action redraw) {
            if (!mRedraws.Contains(redraw))
                mRedraws.Add(redraw);
            
            if (mDraw) {
                Point pos = to2D(mPosition + mRoomPosition);
                g.DrawLine(Pens.Black, pos, to2D(TopLeft));
                g.DrawLine(Pens.Black, pos, to2D(TopRight));
                g.DrawLine(Pens.Black, pos, to2D(BottomLeft));
                g.DrawLine(Pens.Black, pos, to2D(BottomRight));

                if (mDrawRoom) {
                    Vector3 backTopRightCorner = mRoomPosition + new Vector3(0f, mW, 0f);
                    Vector3 frontTopRightCorner = mRoomPosition + new Vector3(mD, mW, 0f);
                    Vector3 frontTopLeftCorner = mRoomPosition + new Vector3(mD, 0f, 0f);

                    Vector3 backBottomLeftCorner = mRoomPosition + new Vector3(0f, 0f, -mH);
                    Vector3 backBottomRightCorner = backBottomLeftCorner + new Vector3(0f, mW, 0f);
                    Vector3 frontBottomRightCorner = backBottomLeftCorner + new Vector3(mD, mW, 0f);
                    Vector3 frontBottomLeftCorner = backBottomLeftCorner + new Vector3(mD, 0f, 0f);


                    Vector3 ventBackTopLeft = mRoomPosition + new Vector3(420f, 0f, 0f);
                    Vector3 ventBackTopRight = mRoomPosition + new Vector3(420f, mW, 0f);
                    Vector3 ventBackBottomLeft = mRoomPosition + new Vector3(420f, 0f, -230f);
                    Vector3 ventBackBottomRight = mRoomPosition + new Vector3(420f, mW, -230f);

                    Vector3 ventFrontTopLeft = mRoomPosition + new Vector3(1020f, 0f, 0f);
                    Vector3 ventFrontTopRight = mRoomPosition + new Vector3(1020f, mW, 0f);
                    Vector3 ventFrontBottomLeft = mRoomPosition + new Vector3(1020f, 0f, -170f);
                    Vector3 ventFrontBottomRight = mRoomPosition + new Vector3(1020f, mW, -170f);

                    using (Brush b = new SolidBrush(Color.FromArgb(128, Color.Black))) {
                        //Draw the vent
                        //Z
                        g.FillPolygon(b, new Point[] {
                            to2D(ventBackTopLeft),
                            to2D(ventBackTopRight),
                            to2D(ventFrontTopRight),
                            to2D(ventFrontTopLeft)
                        });
                        //Y
                        g.FillPolygon(b, new Point[] {
                            to2D(ventBackTopLeft),
                            to2D(ventBackBottomLeft),
                            to2D(ventFrontBottomLeft),
                            to2D(ventFrontTopLeft)
                        });
                        //X
                        g.FillPolygon(b, new Point[] {
                            to2D(ventBackTopLeft),
                            to2D(ventBackTopRight),
                            to2D(ventBackBottomRight),
                            to2D(ventBackBottomLeft)
                        });
                    }

                    using (Pen p = new Pen(Color.FromArgb(230, Color.Black))) {
                        g.DrawPolygon(p, new Point[] {
                            to2D(mRoomPosition),
                            to2D(backTopRightCorner),
                            to2D(frontTopRightCorner),
                            to2D(frontTopLeftCorner)
                        });
                        //Y
                        g.DrawPolygon(p, new Point[] {
                            to2D(mRoomPosition),
                            to2D(backBottomLeftCorner),
                            to2D(frontBottomLeftCorner),
                            to2D(frontTopLeftCorner)
                        });
                        //X
                        g.DrawPolygon(p, new Point[] {
                            to2D(mRoomPosition),
                            to2D(backTopRightCorner),
                            to2D(backBottomRightCorner),
                            to2D(backBottomLeftCorner)
                        });
                    }

                    if (mDrawLabels) {
                        Vector3 toCeiling = new Vector3(0f, 0f, Drop);
                        Vector3 toWall = new Vector3(mWallDistance, 0f, 0f) * new Rotation(0.0, mOrientation.Yaw).Quaternion;
                        Vector3 floor = new Vector3(0f, 0f, mRoomPosition.Z - mH);
                        Vector3 clearance = new Vector3(0f, 0f, Clearance);
                        Vector3 toClearance = floor + clearance;

                        using (Pen p2 = new Pen(Color.Black, 2f)) {
                            g.DrawLine(p2, pos, to2D(mRoomPosition + mPosition + toCeiling));
                            g.DrawLine(p2, pos, to2D(mRoomPosition + mPosition + toWall));
                            g.DrawLine(p2, to2D(floor), to2D(toClearance));
                        }

                        g.DrawString(String.Format("Drop: {0:.#}cm", Drop / 10f), SystemFonts.DefaultFont, Brushes.Black, to2D(mRoomPosition + mPosition + (toCeiling / 2f)));
                        g.DrawString(String.Format("To Wall: {0:.#}cm", WallDistance / 10f), SystemFonts.DefaultFont, Brushes.Black, to2D(mRoomPosition + mPosition + (toWall / 2f)));
                        g.DrawString(String.Format("Clearance: {0:.#}cm", Clearance / 10f), SystemFonts.DefaultFont, Brushes.Black, to2D(floor + (clearance / 2f)));

                        Vector3 up = new Vector3(0f, 0f, mH / 2f);
                        Vector3 middleY = new Vector3(mD / 2, 0f, 0f);
                        Vector3 down = new Vector3(0f, 0f, mH / -2f);
                        g.DrawString(String.Format("Width: {0:.#}cm", mW / 10f), SystemFonts.DefaultFont, Brushes.Black, to2D(backBottomLeftCorner + middleY));
                        g.DrawString(String.Format("Height: {0:.#}cm", mH / 10f), SystemFonts.DefaultFont, Brushes.Black, to2D(frontBottomRightCorner + up));
                        g.DrawString(String.Format("Depth: {0:.#}cm", mD / 10f), SystemFonts.DefaultFont, Brushes.Black, to2D(mRoomPosition + down));
                    }
                }
            }
        }

        private bool Contains(Vector3 p, Vector3[] wall) {
            return
                p.X >= wall[0].X && p.X <= wall[1].X &&
                p.Y >= wall[0].Y && p.Y <= wall[1].Y &&
                p.Z >= wall[0].Z && p.Z <= wall[2].Z;
        }
    }
}
