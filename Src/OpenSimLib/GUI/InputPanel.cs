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

namespace Chimera.OpenSim.GUI {
    public partial class InputPanel : UserControl {
        private SetFollowCamProperties mProperties;
        private bool mGuiUpdate;
        private bool mExternalUpdate;

        public InputPanel() {
            InitializeComponent();
            focusOffset3DPanel.Text = "Focus Offset";
        }

        internal InputPanel(SetFollowCamProperties properties)
            : this() {

            mProperties = properties;
            properties.ControlCameraChanged += new Action(properties_ControlCameraChanged);

            behindnessAnglePanel.Value = properties.BehindnessAngle;
            behindnessLagPanel.Value = properties.BehindnessLag;
            distancePanel.Value = properties.Distance;
            focusPanel.Value = properties.Focus;
            focusLagPanel.Value = properties.FocusLag;
            focusOffsetPanel.Value = properties.FocusOffset;
            focusOffset3DPanel.Value = properties.FocusOffset3D;
            focusThresholdPanel.Value = properties.FocusThreshold;
            lookAtPanel.Value = properties.LookAt;
            lookAtLagPanel.Value = properties.LookAtLag;
            lookAtThresholdPanel.Value = properties.LookAtThreshold;
            pitchPanel.Value = properties.Pitch;
            activeCheckbox.Checked = properties.ControlCamera;
        }

        void properties_ControlCameraChanged() {
            if (!mGuiUpdate) {
                mExternalUpdate = true;
                Invoke(new Action(() => activeCheckbox.Checked = mProperties.ControlCamera));
                mExternalUpdate = false;
            }

        }

        private void focusOffset3DPanel_ValueChanged(object sender, EventArgs e) {
            if (mProperties != null)
                mProperties.FocusOffset3D = focusOffset3DPanel.Value;
        }

        private void focusOffsetPanel_ValueChanged(float obj) {
            if (mProperties != null)
                mProperties.FocusOffset= focusOffsetPanel.Value;
        }

        private void pitchPanel_ValueChanged(float obj) {
            if (mProperties != null)
                mProperties.Pitch = pitchPanel.Value;
        }

        private void lookAtLagPanel_ValueChanged(float obj) {
            if (mProperties != null)
                mProperties.LookAtLag = lookAtLagPanel.Value;
        }

        private void focusLagPanel_ValueChanged(float obj) {
            if (mProperties != null)
                mProperties.FocusLag = focusLagPanel.Value;
        }

        private void behindnessAnglePanel_ValueChanged(float obj) {
            if (mProperties != null)
                mProperties.BehindnessAngle = behindnessAnglePanel.Value;
        }

        private void behindnessLagPanel_ValueChanged(float obj) {
            if (mProperties != null)
                mProperties.BehindnessLag = behindnessLagPanel.Value;
        }

        private void lookAtThresholdPanel_ValueChanged(float obj) {
            if (mProperties != null)
                mProperties.LookAtThreshold = lookAtThresholdPanel.Value;
        }

        private void focusThresholdPanel_ValueChanged(float obj) {
            if (mProperties != null)
                mProperties.FocusThreshold = focusThresholdPanel.Value;
        }

        private void distancePanel_ValueChanged(float obj) {
            if (mProperties != null)
                mProperties.Distance = distancePanel.Value;
        }

        private void lookAtPanel_ValueChanged(float obj) {
            if (mProperties != null)
                mProperties.LookAt = lookAtPanel.Value;
        }

        private void focusPanel_ValueChanged(float obj) {
            if (mProperties != null)
                mProperties.Focus = focusPanel.Value;
        }

        private void activeCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (!mExternalUpdate) {
                mGuiUpdate = true;
                if (mProperties != null)
                    mProperties.ControlCamera = activeCheckbox.Checked;
                mGuiUpdate = false;
            } 
        }
    }
}
