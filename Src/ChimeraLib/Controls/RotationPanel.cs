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
using UtilLib;

namespace ProxyTestGUI {
    public partial class RotationPanel : UserControl {
        private readonly Rotation rotation = new Rotation();
        public event EventHandler OnChange;

        public Quaternion Rotation {
            get { return rotation.Quaternion; }
            set { rotation.Quaternion = value; }
        }
        public Vector3 LookAtVector {
            get { return rotation.LookAtVector; }
            set { rotation.LookAtVector = value; }
        }
        public float Yaw {
            get { return rotation.Yaw; }
            set { rotation.Yaw = value; }
        }
        public float Pitch {
            get { return rotation.Pitch; }
            set { rotation.Pitch = value; }
        }

        public RotationPanel() {
            InitializeComponent();

            rotation.OnChange += (src, args) => {
                vectorPanel.Value = rotation.LookAtVector;
                pitchValue.Value = new decimal (rotation.Pitch);
                pitchSlider.Value = (int)rotation.Pitch;
                yawValue.Value = new decimal (rotation.Yaw);
                yawSlider.Value = (int)rotation.Yaw;
                if (OnChange != null)
                    OnChange(this, null);
            };
        }


        public string DisplayName {
            get { return vectorPanel.DisplayName; }
            set { vectorPanel.DisplayName = value; }
        }

        private void yawSlider_Scroll(object sender, EventArgs e) {
            yawValue.Value = yawSlider.Value;
        }

        private void pitchSlider_Scroll(object sender, EventArgs e) {
            pitchValue.Value = pitchSlider.Value;
        }

        private void yawValue_ValueChanged(object sender, EventArgs e) {
            rotation.Yaw = (float) decimal.ToDouble(yawValue.Value);
        }

        private void pitchValue_ValueChanged(object sender, EventArgs e) {
            rotation.Pitch = (float) decimal.ToDouble(pitchValue.Value);
        }

        private void vectorPanel_OnChange(object sender, EventArgs e) {
            rotation.LookAtVector = vectorPanel.Value;
        }
    }
}
