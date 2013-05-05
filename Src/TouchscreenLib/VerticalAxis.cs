using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Plugins;
using Chimera.Interfaces;
using Chimera;
using System.Drawing;
using Chimera.Overlay;

namespace Touchscreen {
    public class VerticalAxis : ConstrainedAxis, ITickListener {
        private float mW;
        private float mH;
        private float mPaddingH;
        private float mPaddingV;
        private float mStart;
        private bool mDown;
        private WindowOverlayManager mManager;

        public event Action SizeChanged;

        public float W { get { return mW; } set { if (mW != value) { mW = value; Change(); } } }
        public float H { get { return mH; } set { if (mH != value) { mH = value; Change(); } } }
        public float PaddingH { get { return mPaddingH; } set { if (mPaddingH != value) { mPaddingH = value; Change(); } } }
        public float PaddingH { get { return mPaddingV; } set { if (mPaddingV != value) { mPaddingV = value; Change(); } } }
        public float Start { get { return mStart; } set { if (mStart != value) { mStart = value; Change(); } } }
        public RectangleF Bounds {
            get { return new RectangleF(mStart + mPaddingH, mPaddingV, mW, mH); }
        }

        private void Change() {
            if (SizeChanged != null)
                SizeChanged();
        }

        #region ITickListener Members

        public void Init(ITickSource source) {
            source.Tick += new Action(source_Tick);
        }

        void source_Tick() {
            if (mDown && Bounds.Contains(mManager.CursorPosition)) {
                SetRawValue((float)mManager.CursorY - mPaddingV);
            }
        }

        #endregion
    }
}
