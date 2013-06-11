using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera;
using Chimera.Plugins;
using Chimera.Overlay;
using Chimera.GUI.Forms;
using Touchscreen.GUI;

namespace Touchscreen {
    public enum SinglePos {
        Left,
        Middle,
        Right
    }
    public class TouchscreenPlugin : AxisBasedDelta {
        private SinglePos mSinglePos = SinglePos.Right;

        private TwoDAxis mLeftX;
        private TwoDAxis mLeftY;
        private TwoDAxis mRightX;
        private TwoDAxis mRightY;
        private VerticalAxis mSingle;
        private VerticalAxis mL;
        private VerticalAxis mR;

        private OverlayPlugin mStateManager;
        private WindowOverlayManager mManager;
        private TouchscreenForm mWindow;

        private TouchscreenConfig mConfig = new TouchscreenConfig();

        public SinglePos SinglePos {
            get { return mSinglePos; }
            set { mSinglePos = value; }
        }

        public override bool Enabled {
            get { return base.Enabled; }
            set {
                base.Enabled = value;
                if (value && mManager != null) {
                    mWindow = new TouchscreenForm(this);
                    mWindow.Opacity = mConfig.Opacity;
                    mWindow.Bounds = mManager.Frame.Monitor.Bounds;
                    mWindow.MouseDown += new System.Windows.Forms.MouseEventHandler(mWindow_MouseDown);
                    mWindow.MouseUp += new System.Windows.Forms.MouseEventHandler(mWindow_MouseUp); mWindow.Show();
                    mWindow.Show();
                } else if (mWindow != null) {
                    mWindow.Close();
                    mWindow = null;
                }
            }
        }

        public WindowOverlayManager Manager {
            get { return mManager; }
        }

        public VerticalAxis Left { get { return mL; } }
        public TwoDAxis LeftX { get { return mLeftX; } }
        public TwoDAxis LeftY { get { return mLeftY; } }
        public VerticalAxis Right { get { return mR; } }
        public TwoDAxis RightX { get { return mRightX; } }
        public TwoDAxis RightY { get { return mRightY; } }
        public VerticalAxis Single { get { return mSingle; } }

        protected override AxisBasedDelta.AxisConfig AxConfig {
            get { return mConfig; }
        }

        public TouchscreenPlugin(OverlayPlugin manager)
            : base("Touchscreen") {
            mStateManager = manager;
        }

        public override void Init(Core input) {
            base.Init(input);

            if (input.Frames.Count() == 0)
                input.FrameAdded += new Action<Frame, EventArgs>(input_FrameAdded);
            else {
                Frame f = input.Frames.First();
                if (mConfig.Window != null)
                    f = input[mConfig.Window];
                input_FrameAdded(f, null);
            }

        }

        void input_FrameAdded(Frame f, EventArgs args) {
            if (mConfig.Window == null || f.Name.Equals(mConfig.Window)) {
                mManager = mStateManager[f.Name];

                mL = new VerticalAxis(mManager);
                mR = new VerticalAxis(mManager);
                mLeftX = new TwoDAxis(mL, true, false);
                mLeftY = new TwoDAxis(mL, false, false);
                mRightX = new TwoDAxis(mR, true, true);
                mRightY = new TwoDAxis(mR, false, true);
                mSingle = new VerticalAxis(mManager);

                mL.W = mConfig.LeftW;
                mL.H = mConfig.LeftH;
                mL.PaddingH = mConfig.LeftPaddingH;
                mL.PaddingV = mConfig.LeftPaddingV;

                mR.W = mConfig.RightW;
                mR.H = mConfig.RightH;
                mR.PaddingH = mConfig.RightPaddingH;
                mR.PaddingV = mConfig.RightPaddingV;

                mSingle.W = mConfig.SingleW;
                mSingle.H = mConfig.SingleH;
                mSingle.PaddingH = mConfig.SinglePaddingH;
                mSingle.PaddingV = mConfig.SinglePaddingV;

                mSinglePos = mConfig.SinglePos;

                mL.SizeChanged += OnChange;
                mR.SizeChanged += OnChange;
                mSingle.SizeChanged += OnChange;
                OnChange();

                AddAxis(mLeftX);
                AddAxis(mLeftY);
                AddAxis(mRightX);
                AddAxis(mRightY);
                AddAxis(mSingle);

                Core input = f.Coordinator;

                if (Enabled)
                    Enabled = true;
            }
        }

        void mWindow_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
            mManager.Release(0);
        }

        void mWindow_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
            mManager.Press(0);
        }

        public void OnChange() {
            mR.StartH = mL.PaddingH + mL.W;

            float mSingleW = mSingle.PaddingH + mSingle.W;
            switch (mSinglePos) {
                case SinglePos.Left:
                    mL.StartH += mSingleW;
                    mR.StartH += mSingleW;
                    break;
                case SinglePos.Middle:
                    mSingle.StartH = mL.PaddingH + mL.W;
                    mR.StartH += mSingleW;
                    break;
                case SinglePos.Right:
                    mSingle.StartH = mR.StartH + mR.PaddingH + mR.W;
                    break;
            }
        }
    }
}
