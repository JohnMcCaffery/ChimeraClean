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

        public KinectInput Input {
            get { return mInput; }
        }
        public Window Window {
            get { return mWindow; }
        }
        public KinectWindowPanel Panel {
            get {
                if (mPanel == null) {
                    mPanel = new KinectWindowPanel();
                    mPanel.TopLeft = new VectorUpdater(mPlaneTopLeft);
                    mPanel.Top = new VectorUpdater(mTop);
                    mPanel.Side = new VectorUpdater(mSide);
                    mPanel.Intersection = new VectorUpdater(mIntersection);
                    mPanel.X = new ScalarUpdater(mX);
                    mPanel.Y = new ScalarUpdater(mY);
                }
                return mPanel;
            }
        }

        public float X {
            get { return mX.Value; }
        }
        public float Y {
            get { return mY.Value; }
        }
        public float W {
            get { return mW.Value; }
        }
        public float H {
            get { return mH.Value; }
        }
        public Vector3 Intersection {
            get { 
                if ((object) mIntersection != null)
                    return new Vector3(mIntersection.X, mIntersection.Y, mIntersection.Z);
                return Vector3.Zero;
            }
        }
        public Vector3 Top {
            get { 
                if ((object) mTop != null)
                    return new Vector3(mTop.X, mTop.Y, mTop.Z);
                return Vector3.Zero;
            }
        }
        public Vector3 Side {
            get { 
                if ((object) mSide != null)
                    return new Vector3(mSide.X, mSide.Y, mSide.Z);
                return Vector3.Zero;
            }
        }
        public Vector3 TopLeft {
            get { 
                if ((object) mPlaneTopLeft != null)
                    return new Vector3(mPlaneTopLeft.X, mPlaneTopLeft.Y, mPlaneTopLeft.Z);
                return Vector3.Zero;
            }
        }
        public Vector3 Normal {
            get { 
                if ((object) mPlaneNormal != null)
                    return new Vector3(mPlaneNormal.X, mPlaneNormal.Y, mPlaneNormal.Z);
                return Vector3.Zero;
            }
        }

        public bool Active {
            get { return X > 0f && X < 1f && Y > 0f && Y < 1f; }
        }

        public WindowInput(KinectInput input, Window window) {
            mWindow = window;
            mInput = input;

            mWindow.Changed += (win, args) => ConfigureFromWindow();
            mInput.PositionChanged += pos => ConfigureFromWindow();
            mInput.OrientationChanged += rot => ConfigureFromWindow();
            mInput.VectorsAssigned += InitVectors;

            InitVectors(mInput.PointStart, mInput.PointDir);
        }

        private void InitVectors(Vector pointStart, Vector pointDir) {
            mPointDir = pointDir;
            mPointStart = pointStart;

            mPlaneTopLeft = Vector.Create("PlanePoint", 1f, 1f, 0f);
            mPlaneNormal = Vector.Create("PlaneNormal", 0f, 0f, 1f);
            mW = Scalar.Create(1f);
            mH = Scalar.Create(1f);

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

            mH.OnChange += () => {
                Console.WriteLine("H changed.");
            };

            mTop.OnChange += () => {
                Console.WriteLine("Top changed.");
            };

            ConfigureFromWindow();

            Nui.Tick += mWindow_Change;
        }

        private void mWindow_Change() {
            if (mOldX != X || mOldY != Y) {
                mOldX = X;
                mOldY = Y;
                mWindow.PositionCursor(X, Y);
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

            /*
            if (mPanel != null) {
                mPanel.TopLeft = new VectorUpdater(mPlaneTopLeft);
                mPanel.Top = new VectorUpdater(mTop);
                mPanel.Side = new VectorUpdater(mSide);
                mPanel.Intersection = new VectorUpdater(mIntersection);
                mPanel.X = new ScalarUpdater(mX);
                mPanel.Y = new ScalarUpdater(mY);
            }
            */

            mWindow_Change();
        }

    }
}
