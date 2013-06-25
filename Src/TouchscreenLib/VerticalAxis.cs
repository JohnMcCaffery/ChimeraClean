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
using Touchscreen.Interfaces;

namespace Touchscreen {
    public class VerticalAxis : ConstrainedAxis {
        private float mW;
        private float mH;
        private float mPaddingH;
        private float mPaddingV;
        private float mStartH;
        private bool mDown;
        private bool mWasDown;
        private ITouchSource[] mSources;
        private VerticalAxisPanel mPanel;

        private ITouchSource mCurrentlyDown;

        public event Action SizeChanged;

        public float W { get { return mW; } set { if (mW != value) { mW = value; Change(); } } }
        public float H { get { return mH; } set { if (mH != value) { mH = value; Change(); } } }
        public float PaddingH { get { return mPaddingH; } set { if (mPaddingH != value) { mPaddingH = value; Change(); } } }
        public float PaddingV { get { return mPaddingV; } set { if (mPaddingV != value) { mPaddingV = value; Change(); } } }
        public float StartH { get { return mStartH; } set { if (mStartH != value) { mStartH = value; Change(); } } }
        internal ITouchSource[] Sources { get { return mSources; } }
        public RectangleF Bounds {
            get { return new RectangleF(mStartH + mPaddingH, mPaddingV, mW, mH); }
        }

        internal VerticalAxis(params ITouchSource[] sources)
            : base("Single") {

            mSources = sources;

            foreach (var source in mSources) {
                source.OnPress += i => {
                    mDown = true;
                    mCurrentlyDown = source;
                };
                source.OnRelease += i => {
                    mDown = false;
                    mCurrentlyDown = null;
                };
            }

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
                return mDown && Bounds.Contains(mCurrentlyDown.Position) ?
                    GetValue(mPaddingV, mH, mCurrentlyDown.Position.Y) :
                    0f;
            }
        }

        public static float GetValue(float start, float w, double pos) {
            return (float) pos - (start + (w/2));
        }
    }
}
