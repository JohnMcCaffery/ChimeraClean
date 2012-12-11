/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Routing Project is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/

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
        private Vector3 vector;

        public Vector3 Value {
            get { return vector; }
            set {
                if (vector.X == value.X && vector.Y == value.Y && vector.Z == value.Z)
                    return;
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

                    if (!valueChange) {
                        xValue.Value = float.IsInfinity(value.X) ? xValue.Maximum : new decimal(value.X);
                        yValue.Value = float.IsInfinity(value.Y) ? yValue.Maximum : new decimal(value.Y);
                        zValue.Value = float.IsInfinity(value.Z) ? zValue.Maximum : new decimal(value.Z);
                    }
                    externalSet = false;

                    vector = new Vector3((float)xValue.Value, (float)yValue.Value, (float)zValue.Value);

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
            float v = (float)decimal.ToDouble(xValue.Value);
            int value = Convert.ToInt32(v * trackerScale);
            if (v == vector.X)
                return;

            valueChange = true;

            if (!externalSet)
                Value = new Vector3(value, vector.Y, vector.Z);
            xSlider.Value = Math.Max(xSlider.Minimum, Math.Min(xSlider.Maximum, value));

            valueChange = false;
        }

        private void yValue_ValueChanged(object sender, EventArgs e) {
            float v = (float)decimal.ToDouble(yValue.Value);
            int value = Convert.ToInt32(v * trackerScale);
            if (v == vector.Y)
                return;

            valueChange = true;

            if (!externalSet)
                Value = new Vector3(vector.X, value, vector.Z);
            ySlider.Value = Math.Max(ySlider.Minimum, Math.Min(ySlider.Maximum, value));

            valueChange = false;
        }

        private void zValue_ValueChanged(object sender, EventArgs e) {
            float v = (float)decimal.ToDouble(zValue.Value);
            int value = Convert.ToInt32(v * trackerScale);
            if (v == vector.Z)
                return;

            valueChange = true;

            if (!externalSet)
                Value = new Vector3(vector.X, vector.Y, value);
            zSlider.Value = Math.Max(zSlider.Minimum, Math.Min(zSlider.Maximum, value));

            valueChange = false;
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
