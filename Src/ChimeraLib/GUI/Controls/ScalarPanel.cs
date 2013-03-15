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
        private int mScale = 100;

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

                    Invoke(() => {
                        spinner.Maximum = Math.Max(val, spinner.Maximum);
                        spinner.Minimum = Math.Min(val, spinner.Minimum);
                        spinner.Value = val;
                    });

                    mExternalChanged = false;
                }
                if (Changed != null)
                    Changed(value);
            }
        }

        public double Max {
            get { return valueSlider.Maximum / mScale; }
            set {
                Invoke(() => {
                    if (value > 10) {
                        spinner.DecimalPlaces = 2;
                        mScale = 100;
                    }
                    valueSlider.Maximum = (int) (value * mScale);
                    spinner.Maximum = new Decimal(value);
                });
            }
        }

        public double Min {
            get { return valueSlider.Minimum / mScale; }
            set {
                Invoke(() => {
                    if (value <= 1) {
                        spinner.DecimalPlaces = 4;
                        mScale = 1000;
                    }
                    valueSlider.Minimum = (int) (value * mScale);
                    spinner.Minimum = new Decimal(value);
                });
            }
        }

        public ScalarPanel() {
            InitializeComponent();
        }

        private void value_ValueChanged(object sender, EventArgs e) {
            if (!mExternalChanged) {
                mGuiChanged = true;
                Invoke(() => Value = (float)decimal.ToDouble(spinner.Value));
                mGuiChanged = false;
            }
            if (!mSliderChanged) {
                int val = (int)(Value * mScale);
                Invoke(() => {
                    valueSlider.Maximum = Math.Max(val, valueSlider.Maximum);
                    valueSlider.Minimum = Math.Min(val, valueSlider.Minimum);
                    valueSlider.Value = val;
                });
            }
        }

        private void valueSlider_Scroll(object sender, EventArgs e) {
            mSliderChanged = true;
            spinner.Value = new decimal(valueSlider.Value / mScale);
            mSliderChanged = false;
        }

        void mScalar_OnChange(float val) {
            if (!mGuiChanged) {
                mExternalChanged = true;
                spinner.Value = new decimal(val);
                mExternalChanged = false;
            }
        }

        private void Invoke(Action a) {
            if (InvokeRequired)
                base.Invoke(a);
            else
                a();
        }
    }
}
