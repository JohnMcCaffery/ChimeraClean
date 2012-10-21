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
        private float trackerScale = 100.0f;
        private bool externalSet = false;
        private bool valueChange = false;

        public Vector3 Value {
            get { return new Vector3((float)xValue.Value, (float)yValue.Value, (float)zValue.Value); }
            set {
                Action change = new Action(() => {
                    externalSet = true;
                    if (!float.IsInfinity(value.X) && value.X > decimal.ToDouble(xValue.Maximum))
                        xValue.Maximum = new decimal(value.X);
                    if (value.X < decimal.ToDouble(xValue.Minimum))
                        xValue.Minimum = new decimal(value.X);

                    if (!float.IsInfinity(value.Y) && value.Y > decimal.ToDouble(yValue.Maximum))
                        yValue.Maximum = new decimal(value.Y);
                    if (value.Y < decimal.ToDouble(yValue.Minimum))
                        yValue.Minimum = new decimal(value.Y);

                    if (!float.IsInfinity(value.Z) && value.Z > decimal.ToDouble(zValue.Maximum))
                        zValue.Maximum = new decimal(value.Z);
                    if (value.Z < decimal.ToDouble(zValue.Minimum))
                        zValue.Minimum = new decimal(value.Z);

                    xValue.Value = float.IsInfinity(value.X) ? xValue.Maximum : new decimal(value.X);
                    yValue.Value = float.IsInfinity(value.Y) ? yValue.Maximum : new decimal(value.Y);
                    zValue.Value = float.IsInfinity(value.Z) ? zValue.Maximum : new decimal(value.Z);
                    externalSet = false;

                    if (OnChange != null)
                        OnChange(this, null);
                });
                if (InvokeRequired)
                    Invoke(change);
                else
                    change();
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

        private void xValue_ValueChanged(object sender, EventArgs e) {
            float value = Value.X * trackerScale;
            valueChange = true;
            if (value > (float)xSlider.Maximum) xSlider.Value = xSlider.Maximum;
            else if (value < (float)xSlider.Minimum) xSlider.Value = xSlider.Minimum;
            else xSlider.Value = Convert.ToInt32(value);
            valueChange = false;

            if (!externalSet && OnChange != null)
                OnChange(this, new EventArgs());
        }

        private void yValue_ValueChanged(object sender, EventArgs e) {
            valueChange = true;
            float value = Value.Y * trackerScale;
            if (value > (float)ySlider.Maximum) ySlider.Value = ySlider.Maximum;
            else if (value < (float)ySlider.Minimum) ySlider.Value = ySlider.Minimum;
            else ySlider.Value = Convert.ToInt32(value);
            valueChange = false;

            if (!externalSet && OnChange != null)
                OnChange(this, new EventArgs());
        }

        private void zValue_ValueChanged(object sender, EventArgs e) {
            valueChange = true;
            float value = Value.Z * trackerScale;
            if (value > (float)zSlider.Maximum) zSlider.Value = zSlider.Maximum;
            else if (value < (float)zSlider.Minimum) zSlider.Value = zSlider.Minimum;
            else zSlider.Value = Convert.ToInt32(value);
            valueChange = false;

            if (!externalSet && OnChange != null)
                OnChange(this, new EventArgs());
        }

        private void xSlider_Scroll(object sender, EventArgs e) {
            if (!valueChange)
                xValue.Value = new decimal(xSlider.Value / trackerScale);
        }

        private void ySlider_Scroll(object sender, EventArgs e) {
            if (!valueChange)
                yValue.Value = new decimal(ySlider.Value / trackerScale);

        }

        private void zSlider_Scroll(object sender, EventArgs e) {
            if (!valueChange)
                zValue.Value = new decimal(zSlider.Value / trackerScale);
        }

        private void VectorPanel_EnabledChanged(object sender, EventArgs e) {
            xValue.Enabled = Enabled;
            yValue.Enabled = Enabled;
            zValue.Enabled = Enabled;
            xSlider.Enabled = Enabled;
            ySlider.Enabled = Enabled;
            zSlider.Enabled = Enabled;
        }
    }
}
