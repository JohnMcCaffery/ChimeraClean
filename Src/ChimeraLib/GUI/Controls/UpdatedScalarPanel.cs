using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Interfaces;

namespace KinectLib.GUI {
    public partial class UpdatedScalarPanel : ScalarPanel {
        private IUpdater<float> mScalar;
        private bool mGuiChanged;
        private bool mExternalChanged;

        public IUpdater<float> Scalar {
            get { return mScalar; }
            set {
                if ((object) mScalar != null)
                    mScalar.Changed -= mScalar_OnChange;
                mScalar = value;
                if ((object)mScalar != null) {
                    Value = value.Value;
                    mScalar.Changed += mScalar_OnChange;
                }
            }
        }

        private void UpdatedScalarPanel_Changed(float obj) {
            if (!mExternalChanged) {
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