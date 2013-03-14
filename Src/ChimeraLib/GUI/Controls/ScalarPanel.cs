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
            get { return (float)decimal.ToDouble(value.Value); }
            set {
                if (!mGuiChanged) {
                    mExternalChanged = true;
                    this.value.Value = new decimal(value);
                    if (Changed != null)
                        Changed(value);
                    mExternalChanged = false;
                }
            }
        }

        public int Max {
            get { return valueSlider.Maximum / 100; }
            set {
                valueSlider.Maximum = value * 100;
                this.value.Maximum = new Decimal(value);
            }
        }

        public int Min {
            get { return valueSlider.Minimum / 100; }
            set {
                valueSlider.Minimum = value * 100;
                this.value.Minimum = new Decimal(value);
            }
        }

        public ScalarPanel() {
            InitializeComponent();
        }

        private void value_ValueChanged(object sender, EventArgs e) {
            if (!mExternalChanged) {
                mGuiChanged = true;
                Value = (float) decimal.ToDouble(value.Value);
                mGuiChanged = false;
            }
            if (!mSliderChanged)
                value.Value = new decimal(Value * 100.0);
        }

        private void valueSlider_Scroll(object sender, EventArgs e) {
            mSliderChanged = true;
            value.Value = new decimal(valueSlider.Value / 100.0);
            mSliderChanged = false;
        }

        void mScalar_OnChange(float val) {
            if (!mGuiChanged) {
                mExternalChanged = true;
                value.Value = new decimal(val);
                mExternalChanged = false;
            }
        }
    }
}
