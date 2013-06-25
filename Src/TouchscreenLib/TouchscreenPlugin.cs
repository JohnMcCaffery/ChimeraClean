using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera;
using Chimera.Plugins;
using Chimera.Overlay;
using Chimera.GUI.Forms;
using Touchscreen.GUI;
using System.Windows.Forms;
using log4net;
using Touchscreen.Interfaces;
using Touchscreen.Overlay;
using System.Drawing;

namespace Touchscreen {
    public enum SinglePos {
        Left,
        Middle,
        Right
    }

    public class TouchscreenPlugin : AxisBasedDelta, ITouchSource {
        private static readonly ILog Logger = LogManager.GetLogger("Touchscreen");
        private SinglePos mSinglePos = SinglePos.Right;

        private TwoDAxis mLeftX;
        private TwoDAxis mLeftY;
        private TwoDAxis mRightX;
        private TwoDAxis mRightY;
        private VerticalAxis mSingle;
        private VerticalAxis mL;
        private VerticalAxis mR;

        private OverlayPlugin mOverlayPlugin;
        private Frame mFrame;
        private TouchscreenForm mWindow;
        private Form mForm;

        private PointF mMousePosition;
        private TouchscreenConfig mConfig = new TouchscreenConfig();

        public SinglePos SinglePos {
            get { return mSinglePos; }
            set { mSinglePos = value; }
        }

        public override bool Enabled {
            get { return base.Enabled; }
            set {
                if (value != base.Enabled) {
                    base.Enabled = value;
                    if (value && mFrame != null) {
                        CreateWindow();
                    } else if (mWindow != null) {
                        if (mForm == null)
                            mWindow.Close();
                        else
                            mForm.Invoke(new Action(() => mWindow.Close()));
                        mWindow = null;
                    }
                }
            }
        }

        public Frame Frame {
            get { return mFrame; }
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

        public TouchscreenPlugin()
            : base("Touchscreen") {
        }

        public override void Init(Core input) {
            base.Init(input);

            if (input.HasPlugin<OverlayPlugin>()) {
                mOverlayPlugin = input.GetPlugin<OverlayPlugin>();
            }

            if (input.Frames.Count() == 0)
                input.FrameAdded += new Action<Frame, EventArgs>(input_FrameAdded);
            else {
                mFrame = input.Frames.First();
                if (mConfig.Frame != null)
                    mFrame = input[mConfig.Frame];
                input_FrameAdded(mFrame, null);
            }

        }

        void input_FrameAdded(Frame rame, EventArgs args) {
            if (mConfig.Frame == null || rame.Name.Equals(mConfig.Frame)) {
                mFrame = rame;

                List<ITouchSource> sources = new List<ITouchSource>();
                sources.Add(this);
                if (mOverlayPlugin != null) 
                    sources.Add(new OverlayTouchSource(mOverlayPlugin[rame.Name]));

                mL = new VerticalAxis(sources.ToArray());
                mR = new VerticalAxis(sources.ToArray());
                mLeftX = new TwoDAxis(mL, true, false);
                mLeftY = new TwoDAxis(mL, false, false);
                mRightX = new TwoDAxis(mR, true, true);
                mRightY = new TwoDAxis(mR, false, true);
                mSingle = new VerticalAxis(sources.ToArray());

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

                Core input = rame.Core;

                if (Enabled)
                    Enabled = true;
            }
        }

        void mWindow_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
            if (OnPress != null)
                OnPress(0);
        }

        void mWindow_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
            if (OnRelease != null)
                OnRelease(0);
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

        public override void SetForm(Form form) {
            mForm = form;
        }

        public event Action<int> OnPress;

        public event Action<int> OnRelease;

        public PointF Position {
            get {
                if (mWindow == null)
                    return new PointF(0f, 0f);

                return mMousePosition;
            }
        }

        void mWindow_MouseLeave(object sender, EventArgs e) {
            mMousePosition = new PointF(-1f, -1f);
            if (OnRelease != null)
                OnRelease(0);
        }

        void mWindow_MouseMove(object sender, MouseEventArgs e) {
            if (mWindow != null) {
                float x = (float)e.X / (float)mWindow.Width;
                float y = (float)e.Y / (float)mWindow.Width;
                mMousePosition = new PointF(x, y);
            } else
                mMousePosition = new PointF(-1f, -1f);
        }
        private void CreateWindow() {
            mWindow = new TouchscreenForm(this);
            mWindow.Opacity = mConfig.Opacity;
            mWindow.Bounds = mFrame.Monitor.Bounds;
            mWindow.MouseDown += new System.Windows.Forms.MouseEventHandler(mWindow_MouseDown);
            mWindow.MouseUp += new System.Windows.Forms.MouseEventHandler(mWindow_MouseUp); mWindow.Show();
            mWindow.MouseMove += new MouseEventHandler(mWindow_MouseMove);
            mWindow.MouseLeave += new EventHandler(mWindow_MouseLeave);

            if (mForm == null)
                mWindow.Show();
            else
                mWindow.Show(mForm);
        }
    }
}
