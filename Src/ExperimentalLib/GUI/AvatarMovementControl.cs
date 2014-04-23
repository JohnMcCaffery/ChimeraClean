using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Experimental.Plugins;
using OpenMetaverse;

namespace Chimera.Experimental.GUI {
    public partial class AvatarMovementControl : UserControl {
        private AvatarMovementPlugin mPlugin;

        public AvatarMovementControl() {
            InitializeComponent();
        }

        public AvatarMovementControl(AvatarMovementPlugin plugin) : this() {
            mPlugin = plugin;
            mPlugin.TargetChanged += new Action<string, Vector3>(mPlugin_TargetChanged);

            turnRatePanel.Value = (float) (mPlugin.Config as ExperimentalConfig).TurnRate;
            moveRatePanel.Value = (mPlugin.Config as ExperimentalConfig).MoveRate;
            distanceThresholdPanel.Value = (mPlugin.Config as ExperimentalConfig).DistanceThreshold;
            heightOffsetPanel.Value = (mPlugin.Config as ExperimentalConfig).HeightOffset;
        }

        private void mPlugin_TargetChanged(string name, Vector3 position) {
            Action a = () => statusLabel.Text = "Aiming for '" + name + "' at " + position + ".";
            if (InvokeRequired)
                Invoke(a);
            else
                a();
        }

        private void startButton_Click(object sender, EventArgs e) {
            mPlugin.Start();
        }

        private void turnRatePanel_ValueChanged(float obj) {
            (mPlugin.Config as ExperimentalConfig).TurnRate = turnRatePanel.Value;
        }

        private void movePanel_ValueChanged(float obj) {
            (mPlugin.Config as ExperimentalConfig).MoveRate = moveRatePanel.Value;
        }

        private void targetThresholdPanel_ValueChanged(float obj) {
            (mPlugin.Config as ExperimentalConfig).DistanceThreshold = distanceThresholdPanel.Value;
        }

        private void heightOffsetPanel_ValueChanged(float obj) {
            (mPlugin.Config as ExperimentalConfig).HeightOffset = heightOffsetPanel.Value;
        }

        private void stopButton_Click(object sender, EventArgs e) {
            mPlugin.Stop();
        }
    }
}
