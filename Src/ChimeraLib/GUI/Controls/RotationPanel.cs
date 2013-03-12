/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
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
using Chimera.Util;

namespace ProxyTestGUI {
    public partial class RotationPanel : UserControl {
        private Rotation rotation = new Rotation();
        public event EventHandler OnChange;

        public Rotation Value {
            get { return rotation; }
            set {
                rotation = value;
                rotation.OnChange += (src, args) => {
                    vectorPanel.Value = rotation.LookAtVector;
                    pitchValue.Value = Math.Max(pitchValue.Minimum, Math.Min(pitchValue.Maximum, new decimal(rotation.Pitch)));
                    pitchSlider.Value = Math.Max(pitchSlider.Minimum, Math.Min(pitchSlider.Maximum, (int) rotation.Pitch));
                    yawValue.Value = Math.Max(yawValue.Minimum, Math.Min(yawValue.Maximum, new decimal(rotation.Yaw)));
                    yawSlider.Value = Math.Max(yawSlider.Minimum, Math.Min(yawSlider.Maximum, (int) rotation.Yaw));
                    if (OnChange != null)
                        OnChange(this, null);
                };
            }
        }
        public Quaternion Quaternion {
            get { return rotation.Quaternion; }
            set { rotation.Quaternion = value; }
        }
        public Vector3 LookAtVector {
            get { return rotation.LookAtVector; }
            set { rotation.LookAtVector = value; }
        }
        public double Yaw {
            get { return rotation.Yaw; }
            set { rotation.Yaw = value; }
        }
        public double Pitch {
            get { return rotation.Pitch; }
            set { rotation.Pitch = value; }
        }

        public RotationPanel() {
            InitializeComponent();
            Value = new Rotation();
        }


        public string DisplayName {
            get { return nameLabel.Text; }
            set { 
                vectorPanel.DisplayName = value;
                nameLabel.Text = value;
            }
        }

        private void rollSlider_Scroll(object sender, EventArgs e) {
            rollValue.Value = rollSlider.Value;
        }

        private void pitchSlider_Scroll(object sender, EventArgs e) {
            pitchValue.Value = pitchSlider.Value;
        }

        private void yawSlider_Scroll(object sender, EventArgs e) {
            yawValue.Value = yawSlider.Value;
        }

        private void rollValue_ValueChanged(object sender, EventArgs e) {
            //TODO implement Rotation.Roll
            //rotation.Roll = (float) decimal.ToDouble(yawValue.Value);
        }

        private void pitchValue_ValueChanged(object sender, EventArgs e) {
            rotation.Pitch = decimal.ToDouble(pitchValue.Value);
        }

        private void yawValue_ValueChanged(object sender, EventArgs e) {
            rotation.Yaw = decimal.ToDouble(yawValue.Value);
        }

        private void vectorPanel_OnChange(object sender, EventArgs e) {
            rotation.LookAtVector = vectorPanel.Value;
        }

        private void rpyButton_CheckedChanged(object sender, EventArgs e) {
            vectorPanel.Visible = !rpyButton.Checked;
        }

        private void lookAtButton_CheckedChanged(object sender, EventArgs e) {
            vectorPanel.Visible = lookAtButton.Checked;
        }
    }
}
