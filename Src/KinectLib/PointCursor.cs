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
using NuiLibDotNet;
using OpenMetaverse;
using Chimera.Kinect.GUI;
using System.Windows.Forms;
using System.Drawing;
using Chimera.Util;
using C = NuiLibDotNet.Condition;
using Chimera.Config;

namespace Chimera.Kinect {
    public class PointCursor : ISystemPlugin {
        public static float SCALE = 1000f;

        private Frame mFrame;
        private Rotation mOrientation;
        private Vector mPlaneTopLeft, mPlaneNormal;
        private Vector mPointDir, mPointStart;
        private Vector mIntersection;
        private Vector mSide, mTop;
        private Scalar mWorldW, mWorldH;
        private Scalar mScreenW, mScreenH;
        private Scalar mX, mY;
        private Condition mIntersects;
        private PointCursorPanel mPanel;
        private PointF mLocation;
        private RectangleF mBounds = new RectangleF(0f, 0f, 1f, 1f);
        private bool mEnabled = true;
        private bool mOnScreen;
        private bool mTest = false;

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
            float x =  mX.Value;
            float y =  mY.Value;

            if (mLocation.X != y || mLocation.Y != y) {
                mLocation = new PointF(x, y);

                if (mBounds.Contains(mLocation) && !mOnScreen) {
                    mOnScreen = true;
                    if (CursorEnter != null && mEnabled)
                        CursorEnter(this);
                } else if (!mBounds.Contains(mLocation) && mOnScreen) {
                    mOnScreen = false;
                    if (CursorLeave != null && mEnabled)
                        CursorLeave(this);
                }

                if (CursorMove != null && mEnabled)
                    CursorMove(this, x, y);
            }
        }

        private void orientation_Changed(object sender, EventArgs e) {
            ConfigureFromWindow();
        }

        private void ConfigureFromWindow() {
            mWorldW.Value = (float) mFrame.Width;
            mWorldH.Value = (float) mFrame.Height;
            mScreenW.Value = mFrame.Monitor.Bounds.Width;
            mScreenH.Value = mFrame.Monitor.Bounds.Height;

            Vector3 topLeft = mFrame.TopLeft;
            //TODO - this should be set as property
            topLeft -= Vector3.Zero;
            topLeft *= mOrientation.Quaternion;

            Vector3 normal = mFrame.Orientation.LookAtVector * mOrientation.Quaternion;

            mPlaneTopLeft.Set(topLeft.Y, topLeft.Z, topLeft.X);
            mPlaneNormal.Set(normal.Y, normal.Z, normal.X);

            Nui.Poll();

            Tick();
        }

        #region IKinectCursor

        public event Action<PointCursor> CursorEnter;

        public event Action<PointCursor> CursorLeave;

        public event Action<PointCursor, float, float> CursorMove;

        public event Action<IPlugin, bool> EnabledChanged;

        public PointF Location {
            get { return mLocation; }
        }

        public UserControl ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new PointCursorPanel(this);
                return mPanel;
            }
        }

        public Frame Frame {
            get { return mFrame; }
        }

        public string State {
            get { return ""; }
        }

        public bool OnScreen {
            get { return mBounds.Contains(Location); }
        }

        public bool Enabled {
            get { return mEnabled; }
            set {
                if (value != mEnabled) {
                    mEnabled = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public void Init(Frame frame) {
            mFrame = frame;
            //TODO - this should be set properly - as a property
            mOrientation = Rotation.Zero;

            mOrientation.Changed += orientation_Changed;
            mFrame.Changed += (win, args) => ConfigureFromWindow();

            mPlaneTopLeft = Vector.Create("PlanePoint", 1f, 1f, 0f);
            mPlaneNormal = Nui.normalize(Vector.Create("PlaneNormal", 0f, 0f, 1f));
            mWorldW = Scalar.Create("WorldW", (float) mFrame.Width);
            mWorldH = Scalar.Create("WorldH", (float) mFrame.Height);
            mScreenW = Scalar.Create("ScreenW", mFrame.Monitor.Bounds.Width);
            mScreenH = Scalar.Create("ScreenH", mFrame.Monitor.Bounds.Height);

            Vector pointEnd = Nui.joint(Nui.Hand_Right) * SCALE;
            if (mTest) {
                mPointStart = Vector.Create("PointStart", 0f, 0f, 5f);
                mPointDir = Vector.Create("PointDir", 0f, 0f, -1f);
            } else {
                mPointStart = Nui.joint(Nui.Elbow_Right) * SCALE;
                mPointDir = Nui.normalize(mPointStart - pointEnd);
            }


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

            Scalar x = (mWorldW - kinectCoordX) / mWorldW;
            Scalar y = (mWorldH - kinectCoordY) / mWorldH;

            mX = Nui.ifScalar(C.And(mIntersects, C.And(x >= 0f, x <= 1f)), x, -1f);
            mY = Nui.ifScalar(C.And(mIntersects, C.And(y >= 0f, y <= 1f)), y, -1f);

            mIntersection.Name = "Intersection";
            mTop.Name = "Top";
            mSide.Name = "Side";
            mX.Name = "X";
            mY.Name = "Y";

            ConfigureFromWindow();

            Nui.Tick += Tick;
        }

        public void SetPosition(Vector3 position) {
            ConfigureFromWindow();
        }

        public void SetOrientation(Rotation orientation) {
            if (mOrientation != null)
                mOrientation.Changed -= orientation_Changed;
            mOrientation = orientation;
            orientation.Changed += orientation_Changed;
        }

        #endregion

        #region ISystemPlugin Members

        public void Init(Core coordinator) {

        }

        #endregion

        #region IPlugin Members


        public string Name {
            get { return "RayTracingCursor"; }
        }

        public ConfigBase Config {
            get { throw new NotImplementedException(); }
        }

        public void Close() {
        }

        public void Draw(Graphics graphics, Func<Vector3, Point> to2D, Action redraw, Perspective perspective) {
        }

        #endregion

        #region ISystemPlugin Members


        public void SetForm(Form form) {
        }

        #endregion
    }
}
