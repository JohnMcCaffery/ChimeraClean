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

        public InputPanel() {
            InitializeComponent();
        }

        internal InputPanel(SetFollowCamProperties properties)
            : this() {

            mProperties = properties;
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
            if (mProperties != null)
                mProperties.SendPackets = activeCheckbox.Checked;
        }
    }
}
