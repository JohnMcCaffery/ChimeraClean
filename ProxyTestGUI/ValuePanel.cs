/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Armadillo Proxy.

Routing Project is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Routing Project is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Routing Project.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/

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
