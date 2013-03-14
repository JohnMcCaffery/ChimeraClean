using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NuiLibDotNet;

namespace KinectLib.GUI {
    public partial class ScalarPanel : UserControl {
        private Scalar mScalar;
        private bool mGuiChanged;
        private bool mExternalChanged;
        private bool mSliderChanged;

        public ScalarPanel() {
            InitializeComponent();
        }

        public Scalar Scalar {
            get { return mScalar; }
            set {
                if ((object) mScalar == null)
                    throw new ArgumentException("Unable to set Scalar. Value cannot be null.");
                if ((object) mScalar != null)
                    mScalar.OnChange -= mScalar_OnChange;
                mScalar = value;
                mScalar.OnChange += mScalar_OnChange;
            }
        }

        private void value_ValueChanged(object sender, EventArgs e) {
            if (!mExternalChanged) {
                mGuiChanged = true;
                mScalar.Value = (float) decimal.ToDouble(value.Value);
                mGuiChanged = false;
            }
            if (!mSliderChanged)
                value.Value = new decimal(Scalar.Value);
        }

        private void valueSlider_Scroll(object sender, EventArgs e) {
            mSliderChanged = true;
            value.Value = new decimal(valueSlider.Value / 100.0);
            mSliderChanged = false;
        }

        void mScalar_OnChange() {
            if (!mGuiChanged) {
                mExternalChanged = true;
                value.Value = new decimal(mScalar.Value);
                mExternalChanged = false;
            }
        }
    }
}
