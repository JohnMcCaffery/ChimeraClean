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
        private float mScale = 1f;

        public float Deadzone {
            get { return mDeadzone; }
            set { 
                mDeadzone = value;
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
        protected ConstrainedAxis(string name, AxisBinding binding)
            : this(name) {
            mBinding = binding;
        }

        protected ConstrainedAxis(string name, float deadzone, float scale)
            : this(name) {
            mDeadzone = deadzone;
            mScale = scale;
        }

        protected ConstrainedAxis(string name, float deadzone, float scale, AxisBinding binding)
            : this(name, deadzone, scale) {
            mBinding = binding;
        }

        protected void SetRawValue(float value) {
            mRaw = value;
            Recalculate();
        }

        private void Recalculate() {
            mDelta = mRaw < mDeadzone ? 0f : (mRaw - mDeadzone) * mScale;

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
