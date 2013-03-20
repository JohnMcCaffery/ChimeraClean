using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Interfaces;

namespace Chimera.GUI {
    public partial class UpdatedScalarPanel : ScalarPanel {
        private IUpdater<float> mScalar;
        private bool mGuiChanged;
        private bool mExternalChanged;

        public UpdatedScalarPanel()
            : base() {

            ValueChanged += UpdatedScalarPanel_ValueChanged;
            Disposed += new EventHandler(UpdatedScalarPanel_Disposed);
        }

        void UpdatedScalarPanel_Disposed(object sender, EventArgs e) {
            if (mScalar != null)
                mScalar.Changed -= mScalar_OnChange;
        }

        public IUpdater<float> Scalar {
            get { return mScalar; }
            set {
                if ( mScalar != null)
                    mScalar.Changed -= mScalar_OnChange;
                mScalar = value;
                if (mScalar != null) {
                    Value = value.Value;
                    mScalar.Changed += mScalar_OnChange;
                }
            }
        }

        private void UpdatedScalarPanel_ValueChanged(float obj) {
            if (!mExternalChanged && (object) mScalar != null) {
                mGuiChanged = true;
                mScalar.Value = Value;
                mGuiChanged = false;
            }
        }

        void mScalar_OnChange(float val) {
            if (!mGuiChanged) {
                mExternalChanged = true;
                Value = val;
                mExternalChanged = false;
            }
        }
    }
}