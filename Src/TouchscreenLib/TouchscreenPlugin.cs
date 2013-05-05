using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera;
using Chimera.Plugins;

namespace Touchscreen {
    public enum BinPos {
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
        private BinPos mBinPos;

        public BinPos BinPos {
            get { return mBinPos; }
            set { mBinPos = value; }
        }

        public event Action ConfigChanged;

        public float LeftPad {
            get { return mLeftPad; }
            set {
                if (mLeftPad != value) {
                    mLeftPad = value; Change();
                }
            }
        }
        public float LeftW {
            get { return mLeftW; }
            set {
                if (mLeftW != value) {
                    mLeftW = value; Change();
                }
            }
        }
        public float LeftBreak {
            get { return mLeftBreak; }
            set {
                if (mLeftBreak != value) {
                    mLeftBreak = value; Change();
                }
            }
        }
        public float MidW {
            get { return mMidW; }
            set {
                if (mMidW != value) {
                    mMidW = value; Change();
                }
            }
        }
        public float RightBreak {
            get { return mRightBreak; }
            set {
                if (mRightBreak != value) {
                    mRightBreak = value; Change();
                }
            }
        }
        public float RightW {
            get { return mRightW; }
            set {
                if (mRightW != value) {
                    mRightW = value; Change();
                }
            }
        }
        public float FlyBreak {
            get { return mFlyBreak; }
            set {
                if (mFlyBreak != value) {
                    mFlyBreak = value; Change();
                }
            }
        }

        private void Change() {
            if (ConfigChanged != null)
                ConfigChanged();
        }
    }
}
