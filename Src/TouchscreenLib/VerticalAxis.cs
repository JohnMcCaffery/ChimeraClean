using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Plugins;
using Chimera.Interfaces;
using Chimera;
using System.Drawing;
using Chimera.Overlay;
using Touchscreen.GUI;
using System.Windows.Forms;

namespace Touchscreen {
    public class VerticalAxis : ConstrainedAxis {
        private float mW;
        private float mH;
        private float mPaddingH;
        private float mPaddingV;
        private float mStartH;
        private bool mDown;
        private bool mWasDown;
        private FrameOverlayManager mManager;
        private VerticalAxisPanel mPanel;

        public event Action SizeChanged;

        public float W { get { return mW; } set { if (mW != value) { mW = value; Change(); } } }
        public float H { get { return mH; } set { if (mH != value) { mH = value; Change(); } } }
        public float PaddingH { get { return mPaddingH; } set { if (mPaddingH != value) { mPaddingH = value; Change(); } } }
        public float PaddingV { get { return mPaddingV; } set { if (mPaddingV != value) { mPaddingV = value; Change(); } } }
        public float StartH { get { return mStartH; } set { if (mStartH != value) { mStartH = value; Change(); } } }
        public FrameOverlayManager Manager { get { return mManager; } }
        public RectangleF Bounds {
            get { return new RectangleF(mStartH + mPaddingH, mPaddingV, mW, mH); }
        }

        public VerticalAxis(FrameOverlayManager manager)
            : base("Single") {
            mManager = manager;
            mManager.OnPress += i => mDown = true;
            mManager.OnRelease += i => mDown = false;

            Deadzone.Changed += d => Change();
        }


        public override UserControl  ControlPanel {
            get { 
                if (mPanel == null)
                    mPanel = new VerticalAxisPanel(this);
                return mPanel;
            }
        }

        public void Change() {
            if (SizeChanged != null)
                SizeChanged();
        }

        protected override float RawValue {
            get {
                return mDown && Bounds.Contains(mManager.CursorPosition) ?
                    GetValue(mPaddingV, mH, mManager.CursorY) :
                    0f;
            }
        }

        public static float GetValue(float start, float w, double pos) {
            return (float) pos - (start + (w/2));
        }
    }
}
