using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProxyTestGUI {
    public partial class ValuePanel : UserControl {
        private decimal sliderMultiplier;

        public decimal SliderMultiplier {
            get { return sliderMultiplier; }
            set {
                valueSlider.Minimum = (int)(sliderMultiplier * value);
                valueSlider.Maximum = (int)(sliderMultiplier * value);
                valueSlider.Value = (int)(sliderMultiplier * value);
            }
        }
        public decimal Min {
            get { return valueUpDown.Minimum;  }
            set {
                valueUpDown.Minimum = value;
                valueSlider.Minimum = (int)(sliderMultiplier * value);
            }
        }
        public decimal Max {
            get { return valueUpDown.Maximum;  }
            set {
                valueUpDown.Maximum = value;
                valueSlider.Maximum = (int)(sliderMultiplier * value);
            }
        }
        public decimal Value {
            get { return valueUpDown.Value;  }
            set {
                valueUpDown.Value = value;
                valueSlider.Value = (int)(sliderMultiplier * value);
            }
        }
        public decimal Increment {
            get { return valueUpDown.Increment; }
            set {
                valueUpDown.Increment = value;
                valueSlider.TickFrequency = (int)(sliderMultiplier * value);
            }
        }
        public string DisplayText {
            get { return nameLabel.Text; }
            set { nameLabel.Text = value; }
        }


        public ValuePanel() {
            InitializeComponent();
        }

        private void valueSlider_Scroll(object sender, EventArgs e) {
            valueUpDown.Value = valueSlider.Value / sliderMultiplier;
        }

        private void valueUpDown_ValueChanged(object sender, EventArgs e) {
            valueSlider.Value = (int) (valueUpDown.Value * sliderMultiplier);
        }
    }
}
