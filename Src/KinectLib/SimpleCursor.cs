using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuiLibDotNet;
using C = NuiLibDotNet.Condition;
using System.Drawing;
using System.Windows.Forms;
using Chimera.Kinect.GUI;
using Chimera.Util;
using OpenMetaverse;

namespace Chimera.Kinect {
    public class SimpleCursor : IKinectCursorWindow {
        private SimpleCursorPanel mPanel;
        private Vector mHandR;
        private Vector mShoulderR;
        private Scalar mWidth;
        private Scalar mHeight;
        private Scalar mWidthScale;
        private Scalar mHeightScale;
        private Scalar mLeftShift;
        private Scalar mUpShift;
        private Scalar mTopLeftX;
        private Scalar mTopLeftY;
        private Scalar mX;
        private Scalar mY;
        private Scalar mRawX;
        private Scalar mRawY;
        private Scalar mConstrainedX;
        private Scalar mConstrainedY;
        private Condition mOnScreenCondition;
        private Window mWindow;
        private Point mLocation = new Point(-1, -1);
        private bool mOnScreen;
        private bool mEnabled;
        private bool mListening;

        public Vector ShoulderR { get { return mShoulderR; } }
        public Vector HandR { get { return mHandR; } }
        public Scalar Width { get { return mWidth; } }
        public Scalar Height { get { return mHeight; } }
        public Scalar WidthScale { get { return mWidthScale; } }
        public Scalar HeightScale { get { return mHeightScale; } }
        public Scalar LeftShift { get { return mLeftShift; } }
        public Scalar UpShift { get { return mUpShift; } }
        public Scalar TopLeftX { get { return mTopLeftX; } }
        public Scalar TopLeftY { get { return mTopLeftY; } }
        public Scalar RawX { get { return mRawX; } }
        public Scalar RawY { get { return mRawY; } }
        public Scalar ConstrainedX { get { return mConstrainedX; } }
        public Scalar ConstrainedY { get { return mConstrainedY; } }
        public Scalar X { get { return mX; } }
        public Scalar Y { get { return mY; } }

        public SimpleCursor() : this (false) { }
        public SimpleCursor(bool test) {
            mHandR = test ? Vector.Create("HandR", 0f, 0f, 0f) : Nui.joint(Nui.Hand_Right);
            mShoulderR = test ? Vector.Create("ShoulderR", 0f, 0f, 0f) : Nui.joint(Nui.Shoulder_Right);
            Vector shoulderL = test ? Vector.Create("ShoulderL", -1f, 0f, 0f) : Nui.joint(Nui.Shoulder_Left);
            Scalar shoulderW = Nui.magnitude(shoulderL - mShoulderR);

            mWidthScale = Scalar.Create("Width Scale", 1.5f);
            mHeightScale = Scalar.Create("Height Scale", 1.5f);

            mLeftShift = Scalar.Create("Left Shift", .25f);
            mUpShift = Scalar.Create("Up Shift", .75f);

            mWidth = shoulderW * mWidthScale;
            mHeight = shoulderW * mHeightScale;

            mTopLeftX = Nui.x(mShoulderR) - (mWidth * mLeftShift);
            mTopLeftY = Nui.y(mShoulderR) - (mHeight * mUpShift);

            mRawX = Nui.x(mHandR) - mTopLeftX;
            mRawY = Nui.y(mHandR) - mTopLeftY;


            Condition xActive = C.And(mRawX >= 0f, mRawX <= mWidth);
            Condition yActive = C.And(mRawY >= 0f, mRawY <= mHeight);

            mConstrainedX = Nui.constrain(mRawX, .01f, mWidth, .10f, false);
            mConstrainedY = Nui.constrain(mRawY, .01f, mHeight, .10f, false);

            mOnScreenCondition = C.And(xActive, yActive);
        }

        void Nui_Tick() {
            int x = (int) mX.Value;
            int y = (int) mY.Value;

            if (mLocation.X != y || mLocation.Y != y) {
                mLocation = new Point(x, y);

                if (mWindow.Monitor.Bounds.Contains(mLocation) && !OnScreen) {
                    mOnScreen = true;
                    if (CursorEnter != null)
                        CursorEnter();
                } else if (!mWindow.Monitor.Bounds.Contains(mLocation) && OnScreen) {
                    mOnScreen = false;
                    if (CursorLeave != null)
                        CursorLeave();
                }

                if (CursorMove != null)
                    CursorMove(x, y);

                if (OnScreen)
                    mWindow.UpdateCursor(mLocation.X, mLocation.Y);
            }
        }

        private void Init() {
            mX = mConstrainedX * (float)mWindow.Monitor.Bounds.Width;
            mY = (float) mWindow.Monitor.Bounds.Height - (mConstrainedY * (float)mWindow.Monitor.Bounds.Height);

            if (!mListening) {
                mListening = true;
                Nui.Tick += new ChangeDelegate(Nui_Tick);
            }
        }

        #region IKinectCursorWindow Members

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
                    mPanel = new SimpleCursorPanel(this);
                return mPanel; 
            }
        }

        public string State {
            get { return ""; }
        }

        public bool OnScreen {
            get { return mOnScreenCondition.Value; }
        }

        public bool Enabled {
            get { return mEnabled; }
        }

        public void Init(Window window, Vector3 position, Rotation orientation) {
            mWindow = window;
            mWindow.MonitorChanged += (win, monitor) => Init();
            Init();
        }

        public void SetPosition(Vector3 position) { }

        public void SetOrientation(Rotation orientation) { }

        #endregion
    }
}
