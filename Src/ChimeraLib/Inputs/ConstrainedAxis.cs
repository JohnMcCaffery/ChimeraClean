using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces;
using System.Windows.Forms;
using Chimera.GUI.Controls.Plugins;

namespace Chimera.Plugins {
    public abstract class ConstrainedAxis : IAxis {
        private AxisBinding mBinding = AxisBinding.None;
        private ConstrainedAxisPanel mPanel;
        private readonly string mName;

        private float mRaw = 0f;
        private float mDelta = 0f;
        private float mDeadzone = .1f;
        private float mRange = .5f;
        private float mGrace = .1f;
        private float mScale = 1f;

        public float Deadzone {
            get { return mDeadzone; }
            set { 
                mDeadzone = value;
                Recalculate();
            }
        }

        public float Range {
            get { return mRange; }
            set { 
                mRange = value;
                Recalculate();
            }
        }

        public float Grace {
            get { return mGrace; }
            set { 
                mGrace = value;
                Recalculate();
            }
        }

        public float Scale {
            get { return mScale; }
            set { 
                mScale = value;
                Recalculate();
            }
        }

        protected ConstrainedAxis(string name) {
            mName = name;
        }

        protected ConstrainedAxis(string name, float deadzone, float range, float grace, float scale)
            : this(name) {
            mDeadzone = deadzone;
            mRange = range;
            mGrace = grace;
            mScale = scale;
        }

        protected void SetRawValue(float value) {
            mRaw = value;
            Recalculate();
        }

        private void Recalculate() {
            float old = mDelta;
            if (mRaw < mDeadzone || mRaw > mDeadzone + mRange + mGrace)
                mDelta = 0f;
            if (mRaw <= mDeadzone + mRange)
                mDelta = ((mRaw - mDeadzone) / mRange) * mScale;
            else
                mDelta = mScale;

            if (Changed != null)
                Changed();
        }

        #region IAxis Members

        public event Action Changed;

        public virtual UserControl ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new ConstrainedAxisPanel(this);
                return mPanel;
            }
        }

        public float Delta {
            get { return mDelta; }
        }

        public AxisBinding Binding {
            get { return mBinding; }
            set {
                if (mBinding != value) {
                    mBinding = value;
                    if (Changed != null)
                        Changed();
                }
            }
        }

        public string Name {
            get { return mName; }
        }

        #endregion
    }
}
