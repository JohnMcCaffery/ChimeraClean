using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuiLibDotNet;
using OpenMetaverse;
using Chimera.Kinect.GUI;
using System.Windows.Forms;
using System.Drawing;
using Chimera.Util;
using C = NuiLibDotNet.Condition;

namespace Chimera.Kinect {
    public class PointCursorFactory : IKinectCursorWindowFactory {
        public IKinectCursorWindow Make() {
            return new PointCursor();
        }
    }
    public class PointCursor : IKinectCursorWindow {
        public static float SCALE = 1000f;

        private Window mWindow;
        private Vector mPlaneTopLeft, mPlaneNormal;
        private Vector mPointDir, mPointStart;
        private Vector mIntersection;
        private Vector mSide, mTop;
        private Scalar mWorldW, mWorldH;
        private Scalar mScreenW, mScreenH;
        private Scalar mX, mY;
        private Condition mIntersects;
        private PointCursorPanel mPanel;
        private Vector3 mPosition;
        private Rotation mOrientation;
        private Point mLocation;
        private bool mEnabled;
        private bool mOnScreen;
        private bool mTest = false;

        public Window Window {
            get { return mWindow; }
        }
        public UserControl Panel {
            get {
                if (mPanel == null)
                    mPanel = new PointCursorPanel(this);
                return mPanel;
            }
        }
        public Scalar X { get { return mX; } }
        public Scalar Y { get { return mY; } }
        public Scalar WorldW { get { return mWorldW; } }
        public Scalar WorldH { get { return mWorldH; } }
        public Scalar ScreenW { get { return mScreenW; } }
        public Scalar ScreenH { get { return mScreenH; } }
        public Vector PointStart { get { return mPointStart; } }
        public Vector PointDir { get { return mPointDir; } }
        public Vector Top { get { return mTop; } }
        public Vector Side { get { return mSide; } }
        public Vector TopLeft { get { return mPlaneTopLeft; } }
        public Vector Normal { get { return mPlaneNormal; } }
        public Vector Intersection { get { return mIntersection; } }

        public PointCursor() { }

        public PointCursor(bool test) { mTest = test; }

        private void Tick() {
            int x = (int) mX.Value;
            int y = (int) mY.Value;

            if (mLocation.X != y || mLocation.Y != y) {
                mLocation = new Point(x, y);

                if (mWindow.Monitor.Bounds.Contains(mLocation) && !mOnScreen) {
                    mOnScreen = true;
                    if (CursorEnter != null)
                        CursorEnter();
                } else if (!mWindow.Monitor.Bounds.Contains(mLocation) && mOnScreen) {
                    mOnScreen = false;
                    if (CursorLeave != null)
                        CursorLeave();
                }

                if (CursorMove != null)
                    CursorMove(x, y);

                mWindow.UpdateCursor(mLocation.X, mLocation.Y);
            }
        }

        private void orientation_Changed(object sender, EventArgs e) {
            ConfigureFromWindow();
        }

        private void ConfigureFromWindow() {
            mWorldW.Value = (float) mWindow.Width;
            mWorldH.Value = (float) mWindow.Height;
            mScreenW.Value = mWindow.Monitor.Bounds.Width;
            mScreenH.Value = mWindow.Monitor.Bounds.Height;

            Vector3 topLeft = mWindow.TopLeft;
            topLeft -= mPosition;
            topLeft *= mOrientation.Quaternion;

            Vector3 normal = mWindow.Orientation.LookAtVector * mOrientation.Quaternion;

            mPlaneTopLeft.Set(topLeft.Y, topLeft.Z, topLeft.X);
            mPlaneNormal.Set(normal.Y, normal.Z, normal.X);

            Nui.Poll();

            Tick();
        }

        #region IKinectCursor

        public event Action CursorEnter;

        public event Action CursorLeave;

        public event Action<int, int> CursorMove;

        public event Action<bool> EnabledChanged;

        public Point Location {
            get { return mLocation; }
        }

        public UserControl ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new PointCursorPanel(this);
                return mPanel;
            }
        }

        public string State {
            get { return ""; }
        }

        public bool OnScreen {
            get { return mWindow.Monitor.Bounds.Contains(Location); }
        }

        public bool Enabled {
            get { return mEnabled; }
        }

        public void Init(Window window, Vector3 position, Rotation orientation) {
            mWindow = window;
            mOrientation = orientation;
            mPosition = position;

            orientation.Changed += orientation_Changed;
            mWindow.Changed += (win, args) => ConfigureFromWindow();

            mPlaneTopLeft = Vector.Create("PlanePoint", 1f, 1f, 0f);
            mPlaneNormal = Nui.normalize(Vector.Create("PlaneNormal", 0f, 0f, 1f));
            mWorldW = Scalar.Create("WorldW", (float) mWindow.Width);
            mWorldH = Scalar.Create("WorldH", (float) mWindow.Height);
            mScreenW = Scalar.Create("ScreenW", mWindow.Monitor.Bounds.Width);
            mScreenH = Scalar.Create("ScreenH", mWindow.Monitor.Bounds.Height);

            Vector pointEnd = Nui.joint(Nui.Hand_Right) * SCALE;
            if (mTest) {
                mPointStart = Vector.Create("PointStart", 0f, 0f, 5f);
                mPointDir = Vector.Create("PointDir", 0f, 0f, -1f);
            } else {
                mPointStart = Nui.joint(Nui.Elbow_Right) * SCALE;
                mPointDir = Nui.normalize(mPointStart - pointEnd);
            }

            //mIntersection = Nui.intersect(mPlaneTopLeft, Nui.normalize(mPlaneNormal), mPointStart, Nui.normalize(mPointDir));


            //Calculate the intersection of the plane defined by the point mPlaneTopLeft and the normal mPlaneNormal and the line defined by the point mPointStart and the direction mPointDir.
            Scalar numerator = Nui.dot(mPointStart - mPlaneTopLeft, mPlaneNormal);
            Scalar denominator = Nui.dot(mPlaneNormal, mPointDir);
            mIntersects = denominator != 0f;
            mIntersection = mPointStart + Nui.scale(mPointDir, Nui.ifScalar(mIntersects, numerator / denominator, 0f));

            Vector vertical = Vector.Create(0f, 1f, 0f); // Vertical
            //Calculate a vector that represents the orientation of the top of the input.
            mTop = Nui.scale(Nui.cross(vertical, mPlaneNormal), mWorldW);
            //Calculate a vector that represents the orientation of the side of the input.
            mSide = Nui.scale(Nui.cross(mPlaneNormal, mTop), mWorldH);

            //Calculate the vector (running along the plane) between the top left corner and the point of intersection.
            Vector diff = mIntersection - mPlaneTopLeft;

            //Project the diff line onto the top and side vectors to get x and y values.
            Scalar kinectCoordX = Nui.project(diff, mTop);
            Scalar kinectCoordY = Nui.project(diff, mSide);

            Scalar x = ((mWorldW - kinectCoordX) / mWorldW) * mScreenW;
            Scalar y = (kinectCoordY / mWorldH) * mScreenH;

            mX = Nui.ifScalar(C.And(mIntersects, C.And(x >= 0f, x <= mScreenW)), x, -1f);
            mY = Nui.ifScalar(C.And(mIntersects, C.And(y >= 0f, y <= mScreenH)), y, -1f);

            mIntersection.Name = "Intersection";
            mTop.Name = "Top";
            mSide.Name = "Side";
            mX.Name = "X";
            mY.Name = "Y";

            ConfigureFromWindow();

            Nui.Tick += Tick;
        }

        public void SetPosition(Vector3 position) {
            mPosition = position;
            ConfigureFromWindow();
        }

        public void SetOrientation(Rotation orientation) {
            if (mOrientation != null)
                mOrientation.Changed += orientation_Changed;
            mOrientation = orientation;
            orientation.Changed += orientation_Changed;
        }

        #endregion
    }
}
