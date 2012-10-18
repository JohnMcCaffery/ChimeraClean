using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;

namespace ProxyTestGUI {
    public partial class VectorPanel : UserControl {
        public event EventHandler OnChange;
        private double trackerScale = 100.0;

        public Vector3 Value {
            get { return new Vector3((float) xValue.Value, (float) yValue.Value, (float) zValue.Value); }
            set {
                xValue.Value = new decimal(value.X);
                yValue.Value = new decimal(value.Y);
                zValue.Value = new decimal(value.Z);
                //if (OnChange != null)
                    //OnChange(this, new EventArgs());
            }
        }

        public float X {
            get { return (float)xValue.Value; }
            set { xValue.Value = new decimal(value); }
        }
        public float Y {
            get { return (float)yValue.Value; }
            set { yValue.Value = new decimal(value); }
        }
        public float Z {
            get { return (float)zValue.Value; }
            set { zValue.Value = new decimal(value); }
        }

        public string DisplayName {
            get { return nameLabel.Text; }
            set { nameLabel.Text = value; }
        }
        public double Min {
            get { return decimal.ToDouble(xValue.Minimum); }
            set {
                xValue.Minimum = new decimal(value);
                yValue.Minimum = new decimal(value);
                zValue.Minimum = new decimal(value);

                xSlider.Minimum = (int)(value * trackerScale);
                ySlider.Minimum = (int)(value * trackerScale);
                zSlider.Minimum = (int)(value * trackerScale);
            }

        }
        public double Max {
            get { return decimal.ToDouble(xValue.Maximum); }
            set {
                xValue.Maximum = new decimal(value);
                yValue.Maximum = new decimal(value);
                zValue.Maximum = new decimal(value);

                xSlider.Maximum = (int)(value * trackerScale);
                ySlider.Maximum = (int)(value * trackerScale);
                zSlider.Maximum = (int)(value * trackerScale);
            }

        }

        public VectorPanel() {
            InitializeComponent();
        }

        private void value_ValueChanged(object sender, EventArgs e) {
            xSlider.Value = (int) (decimal.ToDouble(xValue.Value) * trackerScale);
            ySlider.Value = (int) (decimal.ToDouble(yValue.Value) * trackerScale);
            zSlider.Value = (int) (decimal.ToDouble(zValue.Value) * trackerScale);
            if (OnChange != null)
                OnChange(this, new EventArgs());
        }

        private void slider_Scroll(object sender, EventArgs e) {
            xValue.Value = new decimal(xSlider.Value / trackerScale);
            yValue.Value = new decimal(ySlider.Value / trackerScale);
            zValue.Value = new decimal(zSlider.Value / trackerScale);
            if (OnChange != null)
                OnChange(this, new EventArgs());
        }
    }
}
