using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuiLibDotNet;
using OpenMetaverse;
using Chimera.Kinect.GUI;

namespace Chimera.Kinect {
    public class WindowInput {
        public static float SCALE = 1000f;

        private Window mWindow;
        private KinectInput mInput;
        private Vector mPlaneTopLeft, mPlaneNormal;
        private Vector mPointDir, mPointStart;
        private Vector mIntersection;
        private Scalar mW, mH;
        private Vector mSide, mTop;
        private Scalar mX, mY;
        private KinectWindowPanel mPanel;

        private double mOldX, mOldY;

        public event Action VectorsRecalculated;

        public KinectInput Input {
            get { return mInput; }
        }
        public Window Window {
            get { return mWindow; }
        }
        public KinectWindowPanel Panel {
            get {
                if (mPanel == null)
                    mPanel = new KinectWindowPanel(this);
                return mPanel;
            }
        }

        public Scalar X {
            get { return mX; }
        }
        public Scalar Y {
            get { return mY; }
        }
        public Scalar W {
            get { return mW; }
        }
        public Scalar H {
            get { return mH; }
        }
        public Vector Intersection {
            get { return mIntersection; }
        }
        public Vector Top {
            get { return mTop; }
        }
        public Vector Side {
            get { return mSide; }
        }
        public Vector TopLeft {
            get { return mPlaneTopLeft; }
        }
        public Vector Normal {
            get { return mPlaneNormal; }
        }

        public WindowInput(KinectInput input, Window window) {
            mWindow = window;
            mInput = input;

            mWindow.Changed += (win, args) => ConfigureFromWindow();
            mInput.PositionChanged += pos => ConfigureFromWindow();
            mInput.OrientationChanged += rot => ConfigureFromWindow();
            mInput.VectorsAssigned += InitVectors;

            mPlaneTopLeft = Vector.Create("PlanePoint", 1f, 1f, 0f);
            mPlaneNormal = Vector.Create("PlaneNormal", 0f, 0f, 1f);
            mW = Scalar.Create("W", 1f);
            mH = Scalar.Create("H", 1f);

            InitVectors(mInput.PointStart, mInput.PointDir);
        }

        private void InitVectors(Vector pointStart, Vector pointDir) {
            mPointDir = pointDir;
            mPointStart = pointStart;

            Vector vertical = Vector.Create(0f, 1f, 0f); // Vertical
            //Calculate the intersection of the plane defined by the point mPlaneTopLeft and the normal mPlaneNormal and the line defined by the point mPointStart and the direction mPointDir.
            mIntersection = Nui.intersect(mPlaneTopLeft, Nui.normalize(mPlaneNormal), mPointStart, Nui.normalize(mPointDir)); 
            //Calculate a vector that represents the orientation of the top of the coordinator.
            mTop = Nui.scale(Nui.cross(vertical, mPlaneNormal), mW);
            //Calculate a vector that represents the orientation of the side of the coordinator.
            mSide = Nui.scale(Nui.cross(mPlaneNormal, mTop), mH);

            //Calculate the vector (running along the plane) between the top left corner and the point of intersection.
            Vector diff = mIntersection - mPlaneTopLeft;

            //Project the diff line onto the top and side vectors to get x and y values.
            mX = Nui.project(diff, mTop);
            mY = Nui.project(diff, mSide);

            mIntersection.Name = "Intersection";
            mTop.Name = "Top";
            mSide.Name = "Side";
            mX.Name = "X";
            mY.Name = "Y";

            if (VectorsRecalculated != null)
                VectorsRecalculated();

            ConfigureFromWindow();

            Nui.Tick += mWindow_Change;
        }

        private void mWindow_Change() {
            if (mOldX != mX.Value || mOldY != mY.Value) {
                mOldX = mX.Value;
                mOldY = mY.Value;
                mWindow.UpdateCursorCm(mWindow.Width - (mOldX * SCALE), mOldY * SCALE);
            }
        }

        private void ConfigureFromWindow() {
            mW.Value = (float) mWindow.Width / SCALE;
            mH.Value = (float) mWindow.Height / SCALE;

            Vector3 topLeft = mWindow.TopLeft;
            topLeft -= mInput.Position;
            topLeft *= mInput.Orientation.Quaternion;

            Vector3 normal = mWindow.Orientation.LookAtVector * mInput.Orientation.Quaternion;

            mPlaneTopLeft.Set(topLeft.Y / SCALE, topLeft.Z / SCALE, topLeft.X / SCALE);
            mPlaneNormal.Set(normal.Y, normal.Z, normal.X);

            Nui.Poll();

            mWindow_Change();
        }

    }
}
