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
using Chimera.Overlay;
using Chimera.Config;
using log4net;
using Chimera.Plugins;
using Chimera.Interfaces;

namespace Chimera.Kinect.Axes
{
    public class KinectCursorAxisLeftX : KinectCursorAxis
    {
        public KinectCursorAxisLeftX() : base(true, true, AxisBinding.NotSet) {}
    }

    public class KinectCursorAxisLeftY : KinectCursorAxis
    {
        public KinectCursorAxisLeftY() : base(true, false, AxisBinding.NotSet) { }
    }

    public class KinectCursorAxisRightX : KinectCursorAxis
    {
        public KinectCursorAxisRightX() : base(false, true, AxisBinding.NotSet) { }
    }

    public class KinectCursorAxisRightY : KinectCursorAxis
    {
        public KinectCursorAxisRightY() : base(false, false, AxisBinding.NotSet) { }
    }

    public abstract class KinectCursorAxis : IAxis
    {
        private bool mLeft;
        private bool mX;

        private ILog Logger = LogManager.GetLogger("KinectCursor");

        private Vector mHand;
        private Vector mAnchor;
        private Scalar mWidth;
        private Scalar mHeight;
        private Scalar mLeftHandShift;
        private Scalar mLeftShift;
        private Scalar mUpShift;
        private Scalar mTopLeftX;
        private Scalar mTopLeftY;
        private Scalar mNUIRaw;
        private Scalar mConstrained;
        private Scalar mSmoothingFactor;
        private static Condition mOnScreenConditionRight;
        private static Condition mOnScreenConditionLeft;
        private PointF mLocation = new PointF(-1f, -1f);
        private RectangleF mBounds = new RectangleF(0f, 0f, 1f, 1f);
        private bool mEnabled;
        private bool mDisabled = false;
        //TODO add init of this to the config
        private string mWindow = "MainWindow";

        private FrameOverlayManager mManager;
        private OverlayPlugin mOverlayPlugin;

        public Vector Anchor { get { return mAnchor; } }
        public Vector Hand { get { return mHand; } }
        public Scalar Width { get { return mWidth; } }
        public Scalar Height { get { return mHeight; } }
        public Scalar LeftHandShift { get { return mLeftHandShift; } }
        public Scalar LeftShift { get { return mLeftShift; } }
        public Scalar UpShift { get { return mUpShift; } }
        public Scalar TopLeftX { get { return mTopLeftX; } }
        public Scalar TopLeftY { get { return mTopLeftY; } }
        public Scalar Raw { get { return mNUIRaw; } }
        public Scalar ConstrainedX { get { return mConstrained; } }
        public Scalar SmoothingFactor { get { return mSmoothingFactor; } }

        private static readonly int HAND_SMOOTHING_FRAMES = 5;
        private static readonly int ANCHOR_SMOOTHING_FRAMES = 15;
        private static readonly int ANCHOR = Nui.Hip_Centre;

        private static Condition xActiveLeft;
        private static Condition yActiveLeft;
        private static Condition xActiveRight;
        private static Condition yActiveRight;
        private string mName;
        private AxisBinding mBinding = AxisBinding.NotSet;
        private UserControl mPanel;

        public KinectCursorAxis(bool left, bool x, AxisBinding binding)
        {
            mName = MakeName(left, x);
            mBinding = binding;
            mLeft = left;
            mX = x;
            mPanel = new UserControl();

            mSmoothingFactor = Scalar.Create(HAND_SMOOTHING_FRAMES);

            if(mLeft)
                mHand = Nui.smooth(Nui.joint(Nui.Hand_Right), mSmoothingFactor);
            else
                mHand = Nui.smooth(Nui.joint(Nui.Hand_Left), mSmoothingFactor);

            mAnchor = Nui.smooth(Nui.joint(Nui.Hip_Centre), ANCHOR_SMOOTHING_FRAMES);

            mLeftShift = Scalar.Create("Left Shift", .0f);
            mUpShift = Scalar.Create("Up Shift", .0f);

            mWidth = Scalar.Create("Width", .5f);
            mHeight = Scalar.Create("Height", .5f);

            mTopLeftX = Nui.x(mAnchor) - (mWidth * mLeftShift);
            mTopLeftY = Nui.y(mAnchor) + (mHeight * mUpShift);

            if (mLeft)
            {
                if (mX)
                {
                    mLeftHandShift = Scalar.Create("LeftHandShift", .5f);
                    mNUIRaw = (Nui.x(mHand) + mLeftHandShift) - mTopLeftX;
                    xActiveLeft = C.And(mNUIRaw >= 0f, mNUIRaw <= mWidth);
                    mConstrained = Nui.constrain(mNUIRaw, .01f, mWidth, .10f, false);
                }
                else
                {
                    mNUIRaw = Nui.y(mHand) - mTopLeftY;
                    yActiveLeft = C.And(mNUIRaw >= 0f, mNUIRaw <= mHeight);
                    mConstrained = Nui.constrain(mNUIRaw, .01f, mHeight, .10f, false);
                }

                if(xActiveLeft != null && yActiveLeft != null)
                    mOnScreenConditionLeft = C.And(xActiveLeft, yActiveLeft);
            }
            else
            {
                if (mX)
                {
                    mNUIRaw = Nui.x(mHand) - mTopLeftX;
                    xActiveRight = C.And(mNUIRaw >= 0f, mNUIRaw <= mWidth);
                    mConstrained = Nui.constrain(mNUIRaw, .01f, mWidth, .10f, false);
                }
                else
                {
                    mNUIRaw = Nui.y(mHand) - mTopLeftY;
                    yActiveRight = C.And(mNUIRaw >= 0f, mNUIRaw <= mHeight);
                    mConstrained = Nui.constrain(mNUIRaw, .01f, mHeight, .10f, false);
                }
                if (xActiveRight != null && yActiveRight != null) 
                    mOnScreenConditionRight = C.And(xActiveRight, yActiveRight);
                
            }

            
        }

        public bool Disabled
        {
            get { return mDisabled; }
            set { mDisabled = value; }
        }

        private static string MakeName(bool left, bool x)
        {
            return "KinectCursorAxis" + (left ? "Left" : "Right") + (x ? "X" : "Y");
        }

        public UserControl ControlPanel
        {
            get { return mPanel; }
        }

        public float Delta
        {
            get
            {
                if (!mDisabled && ((mLeft && mOnScreenConditionLeft.Value) || (!mLeft && mOnScreenConditionRight.Value)))
                {
                    float raw = mConstrained.Value;
                    Console.WriteLine(raw);
                    return raw;
                }
                else return 0;
            }
        }

        public AxisBinding Binding
        {
            get
            {
                return mBinding;
            }
            set
            {
                if (mBinding != value)
                    mBinding = value;
            }
        }

        public string Name
        {
            get { return mName; }
        }

        public virtual bool Enabled
        {
            get { return mEnabled; }
            set { mEnabled = value; }
        }
    }
}
