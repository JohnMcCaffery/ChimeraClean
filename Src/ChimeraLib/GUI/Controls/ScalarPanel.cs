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
    public partial class ScalarPanel : UserControl {
        private bool mGuiChanged;
        private bool mExternalChanged;
        private bool mSliderChanged;

        public event Action<float> Changed;

        public float Value {
            get { return (float)decimal.ToDouble(spinner.Value); }
            set {
                if (!mGuiChanged) {
                    mExternalChanged = true;
                    decimal val = 
                        float.IsInfinity(value) ? spinner.Maximum : 
                        float.IsNaN(value) ? decimal.Zero : 
                        float.IsNegativeInfinity(value) ? spinner.Minimum : 
                        new decimal(value);

                    spinner.Maximum = Math.Max(val, spinner.Maximum);
                    spinner.Minimum = Math.Min(val, spinner.Minimum);
                    spinner.Value = val;

                    mExternalChanged = false;
                }
                if (Changed != null)
                    Changed(value);
            }
        }

        public int Max {
            get { return valueSlider.Maximum / 100; }
            set {
                valueSlider.Maximum = value * 100;
                this.spinner.Maximum = new Decimal(value);
            }
        }

        public int Min {
            get { return valueSlider.Minimum / 100; }
            set {
                valueSlider.Minimum = value * 100;
                this.spinner.Minimum = new Decimal(value);
            }
        }

        public ScalarPanel() {
            InitializeComponent();
        }

        private void value_ValueChanged(object sender, EventArgs e) {
            if (!mExternalChanged) {
                mGuiChanged = true;
                Value = (float) decimal.ToDouble(spinner.Value);
                mGuiChanged = false;
            }
            if (!mSliderChanged) {
                int val = (int)(Value * 100.0);
                valueSlider.Maximum = Math.Max(val, valueSlider.Maximum);
                valueSlider.Minimum = Math.Min(val, valueSlider.Minimum);
                valueSlider.Value = val;
            }
        }

        private void valueSlider_Scroll(object sender, EventArgs e) {
            mSliderChanged = true;
            spinner.Value = new decimal(valueSlider.Value / 100.0);
            mSliderChanged = false;
        }

        void mScalar_OnChange(float val) {
            if (!mGuiChanged) {
                mExternalChanged = true;
                spinner.Value = new decimal(val);
                mExternalChanged = false;
            }
        }
    }
}
