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
        private float mScale = 100f;

        public event Action<float> ValueChanged;

        public float Value {
            get { return (float)decimal.ToDouble(spinner.Value); }
            set {
                if (!mGuiChanged) {
                    mExternalChanged = true;
                    Invoke(() => spinner.Value = checkValue(value));
                    mExternalChanged = false;
                }
                if (ValueChanged != null)
                    ValueChanged(value);
            }
        }

        public float Max {
            get { return valueSlider.Maximum / mScale; }
            set { Invoke(() => SetMax(Convert(value))); }
        }

        public float Min {
            get { return valueSlider.Minimum / mScale; }
            set { Invoke(() => SetMin(Convert(value))); }
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
                int sliderValue = (int)(Value * mScale);
                Invoke(() => {
                    valueSlider.Minimum = Math.Min(sliderValue, valueSlider.Minimum);
                    valueSlider.Maximum = Math.Max(sliderValue, valueSlider.Maximum);
                    valueSlider.Value = sliderValue;
                });
            }
        }

        private void valueSlider_Scroll(object sender, EventArgs e) {
            mSliderChanged = true;
            spinner.Value = Convert(valueSlider.Value / mScale);
            mSliderChanged = false;
        }

        private void Invoke(Action a) {
            if (InvokeRequired && !IsDisposed && Created)
                base.Invoke(a);
            else
                a();
        }

        private void SetMin(decimal value) {
            spinner.Minimum = value;
            SetSlider();
        }
        private void SetMax(decimal value) {
            spinner.Maximum = value;
            SetSlider();
        }

        private void SetSlider() {
            if (spinner.Maximum - spinner.Minimum > 10) {
                spinner.DecimalPlaces = 2;
                mScale = 100;
            } else if (spinner.Maximum- spinner.Maximum < 10) {
                spinner.DecimalPlaces = 4;
                mScale = 1000;
            }
            int val = (int)(Value * mScale);

            valueSlider.Minimum = (int)Math.Min(val, (decimal.ToDouble(spinner.Minimum) * mScale));
            valueSlider.Maximum = (int)Math.Max(val, (decimal.ToDouble(spinner.Maximum) * mScale));

            valueSlider.Value = val;
        }

        private decimal Convert(float value) {
            value = Math.Min(value, (float)decimal.ToDouble(decimal.MaxValue));
            value = Math.Max(value, (float)decimal.ToDouble(decimal.MinValue));
            return new decimal(value);
        }

        private decimal checkValue(float val) {
            decimal max = spinner.Maximum;
            decimal min = spinner.Minimum;
            decimal value =
                float.IsInfinity(val) ? decimal.MaxValue :
                float.IsNaN(val) ? decimal.Zero :
                float.IsNegativeInfinity(val) ? decimal.MinValue :
                Convert(val);

            //TODO - slider can get out of synch so invalid values are passed in
            if (value < min)
                Min = (float) decimal.ToDouble(value);
            if (value > max)
                Max = (float) decimal.ToDouble(value);

            return value;
        }
    }
}
