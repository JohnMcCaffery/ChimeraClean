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
        private float mLeftPad;
        private float mLeftW;
        private float mLeftBreak;
        private float mMidW;
        private float mRightBreak;
        private float mRightW;
        private float mFlyBreak;
        private SinglePos mSinglePos = SinglePos.Right;

        private TwoDAxis mLeftX;
        private TwoDAxis mLeftY;
        private TwoDAxis mRightX;
        private TwoDAxis mRightY;
        private VerticalAxis mSingle;
        private VerticalAxis mL;
        private VerticalAxis mR;

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
                if (value)
                    mWindow.Show();
                else
                    mWindow.Close();
            }
        }

        protected override AxisBasedDelta.AxisConfig AxConfig {
            get { return mConfig; }
        }

        public TouchscreenPlugin()
            : base("Touchscreen") { }

        public override void Init(Coordinator input) {
            base.Init(input);

            Window w = mConfig.Window == null ? input.Windows.First() : input[mConfig.Window];
            mManager = w.OverlayManager;

            mL = new VerticalAxis(mManager);
            mR = new VerticalAxis(mManager);
            mLeftX = new TwoDAxis(mL, true, false);
            mLeftY = new TwoDAxis(mL, false, false);
            mRightX = new TwoDAxis(mL, true, true);
            mRightY = new TwoDAxis(mL, false, true);
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

            mWindow.Opacity = .5;
            mWindow.MouseDown += new System.Windows.Forms.MouseEventHandler(mWindow_MouseDown);
            mWindow.MouseUp += new System.Windows.Forms.MouseEventHandler(mWindow_MouseUp);
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
