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
using Chimera.Kinect.Interfaces;

namespace Chimera.Kinect {
    public class SimpleCursorFactory : IKinectCursorFactory {
        public IKinectCursor Make() { return new SimpleCursor(); }
        public string Name { get { return "Simple X/Y Cursor"; } }
    }
    public class SimpleCursor : IKinectCursor {
        private SimpleCursorPanel mPanel;
        private Vector mHandR;
        private Vector mAnchor;
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
        private PointF mLocation = new PointF(-1f, -1f);
        private RectangleF mBounds = new RectangleF(0f, 0f, 1f, 1f);
        private bool mOnScreen;
        private bool mEnabled;
        private bool mListening;

        public Vector Anchor { get { return mAnchor; } }
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
            mAnchor = test ? Vector.Create("Anchor", 0f, 0f, 0f) : Nui.joint(Nui.Hip_Centre);
            Vector head = test ? Vector.Create("Head", -1f, 0f, 0f) : Nui.joint(Nui.Head);
            Scalar headShoulderDiff = Nui.magnitude(mAnchor - head);

            mWidthScale = Scalar.Create("Width Scale", 1.5f);
            mHeightScale = Scalar.Create("Height Scale", 1.5f);

            mLeftShift = Scalar.Create("Left Shift", .0f);
            mUpShift = Scalar.Create("Up Shift", .0f);

            mWidth = Scalar.Create("Width", .5f);
            mHeight = Scalar.Create("Height", .5f);
            //mWidth = headShoulderDiff * mWidthScale;
            //mHeight = headShoulderDiff * mHeightScale;

            mTopLeftX = Nui.x(mAnchor) - (mWidth * mLeftShift);
            mTopLeftY = Nui.y(mAnchor) + (mHeight * mUpShift);

            mRawX = Nui.x(mHandR) - mTopLeftX;
            mRawY = Nui.y(mHandR) - mTopLeftY;


            Condition xActive = C.And(mRawX >= 0f, mRawX <= mWidth);
            Condition yActive = C.And(mRawY >= 0f, mRawY <= mHeight);

            mConstrainedX = Nui.constrain(mRawX, .01f, mWidth, .10f, false);
            mConstrainedY = Nui.constrain(mRawY, .01f, mHeight, .10f, false);

            mOnScreenCondition = C.And(xActive, yActive);
        }

        void Nui_Tick() {
            float x = mX.Value;
            float y = mY.Value;

            if (mLocation.X != y || mLocation.Y != y) {
                mLocation = new PointF(x, y);

                if (mBounds.Contains(mLocation) && !OnScreen) {
                    mOnScreen = true;
                    if (CursorEnter != null && mEnabled)
                        CursorEnter(this);
                } else if (!mBounds.Contains(mLocation) && OnScreen) {
                    mOnScreen = false;
                    if (CursorLeave != null && mEnabled)
                        CursorLeave(this);
                }

                if (CursorMove != null && mEnabled)
                    CursorMove(this, x, y);
            }
        }

        private void Init() {
            //mX = mConstrainedX * (float)mWindow.Monitor.Bounds.Width;
            //mY = (float) mWindow.Monitor.Bounds.Height - (mConstrainedY * (float)mWindow.Monitor.Bounds.Height);
            mX = mConstrainedX;
            mY = 1f - mConstrainedY;

            if (!mListening) {
                mListening = true;
                Nui.Tick += new ChangeDelegate(Nui_Tick);
            }
        }

        #region IKinectCursorWindow Members

        public event Action<IKinectCursor> CursorEnter;

        public event Action<IKinectCursor> CursorLeave;

        public event Action<IKinectCursor, float, float> CursorMove;

        public event Action<bool> EnabledChanged;

        public PointF Location {
            get { return mLocation; }
        }

        public UserControl ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new SimpleCursorPanel(this);
                return mPanel; 
            }
        }

        public Window Window {
            get { return mWindow; }
        }

        public string State {
            get { return ""; }
        }

        public bool OnScreen {
            get { return Nui.HasSkeleton && mOnScreenCondition.Value; }
        }

        public bool Enabled {
            get { return mEnabled; }
            set {
                if (value != mEnabled) {
                    mEnabled = value;
                    if (EnabledChanged != null)
                        EnabledChanged(value);
                }
            }
        }

        public void Init(IKinectController controller, Window window) {
            mWindow = window;
            mWindow.MonitorChanged += (win, monitor) => Init();
            Init();
        }

        #endregion
    }
}
