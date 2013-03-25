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
        private Vector mHandL;
        private Vector mAnchor;
        private Scalar mWidth;
        private Scalar mHeight;
        private Scalar mLeftHandShift;
        private Scalar mLeftShift;
        private Scalar mUpShift;
        private Scalar mTopLeftX;
        private Scalar mTopLeftY;
        private Scalar mX;
        private Scalar mY;
        private Scalar mRawXRight;
        private Scalar mRawYRight;
        private Scalar mRawXLeft;
        private Scalar mRawYLeft;
        private Scalar mConstrainedXRight;
        private Scalar mConstrainedYRight;
        private Scalar mConstrainedXLeft;
        private Scalar mConstrainedYLeft;
        private Condition mOnScreenConditionRight;
        private Condition mOnScreenConditionLeft;
        private Window mWindow;
        private PointF mLocation = new PointF(-1f, -1f);
        private RectangleF mBounds = new RectangleF(0f, 0f, 1f, 1f);
        private bool mOnScreen;
        private bool mEnabled;
        private bool mListening;

        public Vector Anchor { get { return mAnchor; } }
        public Vector HandR { get { return mHandR; } }
        public Vector HandL { get { return mHandL; } }
        public Scalar Width { get { return mWidth; } }
        public Scalar Height { get { return mHeight; } }
        public Scalar LeftHandShift { get { return mLeftHandShift; } }
        public Scalar LeftShift { get { return mLeftShift; } }
        public Scalar UpShift { get { return mUpShift; } }
        public Scalar TopLeftX { get { return mTopLeftX; } }
        public Scalar TopLeftY { get { return mTopLeftY; } }
        public Scalar RawXRight { get { return mRawXRight; } }
        public Scalar RawYRight { get { return mRawYRight; } }
        public Scalar RawXLeft { get { return mRawXLeft; } }
        public Scalar RawYLeft { get { return mRawYLeft; } }
        public Scalar ConstrainedXRight { get { return mConstrainedXRight; } }
        public Scalar ConstrainedYRight { get { return mConstrainedYRight; } }
        public Scalar ConstrainedXLeft { get { return mConstrainedXLeft; } }
        public Scalar ConstrainedYLeft { get { return mConstrainedYLeft; } }
        public Scalar X { get { return mX; } }
        public Scalar Y { get { return mY; } }

        public SimpleCursor() : this (false) { }
        public SimpleCursor(bool test) {
            mHandR = test ? Vector.Create("HandR", 0f, 0f, 0f) : Nui.joint(Nui.Hand_Right);
            mHandL = test ? Vector.Create("HandL", 0f, 0f, 0f) : Nui.joint(Nui.Hand_Left);
            mAnchor = test ? Vector.Create("Anchor", 0f, 0f, 0f) : Nui.joint(Nui.Hip_Centre);
            Vector head = test ? Vector.Create("Head", -1f, 0f, 0f) : Nui.joint(Nui.Head);
            Scalar headShoulderDiff = Nui.magnitude(mAnchor - head);

            mLeftShift = Scalar.Create("Left Shift", .0f);
            mUpShift = Scalar.Create("Up Shift", .0f);

            mWidth = Scalar.Create("Width", .5f);
            mHeight = Scalar.Create("Height", .5f);
            //mWidth = headShoulderDiff * mWidthScale;
            //mHeight = headShoulderDiff * mHeightScale;

            mTopLeftX = Nui.x(mAnchor) - (mWidth * mLeftShift);
            mTopLeftY = Nui.y(mAnchor) + (mHeight * mUpShift);

            mRawXRight = Nui.x(mHandR) - mTopLeftX;
            mRawYRight = Nui.y(mHandR) - mTopLeftY;
            Condition xActiveRight = C.And(mRawXRight >= 0f, mRawXRight <= mWidth);
            Condition yActiveRight = C.And(mRawYRight >= 0f, mRawYRight <= mHeight);
            mConstrainedXRight = Nui.constrain(mRawXRight, .01f, mWidth, .10f, false);
            mConstrainedYRight = Nui.constrain(mRawYRight, .01f, mHeight, .10f, false);
            mOnScreenConditionRight = C.And(xActiveRight, yActiveRight);

            mLeftHandShift = Scalar.Create("LeftHandShift", .5f);
            mRawXLeft = (Nui.x(mHandL) + mLeftHandShift) - mTopLeftX;
            mRawYLeft = Nui.y(mHandL) - mTopLeftY;
            Condition xActiveLeft = C.And(mRawXLeft >= 0f, mRawXLeft <= mWidth);
            Condition yActiveLeft = C.And(mRawYLeft >= 0f, mRawYLeft <= mHeight);
            mConstrainedXLeft = Nui.constrain(mRawXLeft, .01f, mWidth, .10f, false);
            mConstrainedYLeft = Nui.constrain(mRawYLeft, .01f, mHeight, .10f, false);
            mOnScreenConditionLeft = C.And(xActiveLeft, yActiveLeft);
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
            mX = Nui.ifScalar(C.And(mOnScreenConditionLeft, !mOnScreenConditionRight), mConstrainedXLeft, mConstrainedXRight);
            mY = 1f - Nui.ifScalar(C.And(mOnScreenConditionLeft, !mOnScreenConditionRight), mConstrainedYLeft, mConstrainedYRight);
            //mY = 1f - mConstrainedYRight;

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
            get { return Nui.HasSkeleton && mOnScreenConditionRight.Value; }
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
