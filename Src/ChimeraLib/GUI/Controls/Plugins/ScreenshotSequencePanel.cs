using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Config;
using Chimera.Plugins;

namespace Chimera.GUI.Controls.Plugins {
    public partial class ScreenshotSequencePanel : UserControl {
        private ScreenshotSequencePlugin mPlugin;
        private CoreConfig mConfig;

        public ScreenshotSequencePanel() {
            InitializeComponent();
        }

        public ScreenshotSequencePanel(ScreenshotSequencePlugin plugin) {
            InitializeComponent();

            mPlugin = plugin;
            mConfig = plugin.Config as CoreConfig;

            mPlugin.Started += () => {
                if (InvokeRequired)
                    Invoke(new Action(() => startButton.Text = "Stop"));
                else
                    startButton.Text = "Stop";
            };

            mPlugin.Stopped += () => {
                if (InvokeRequired)
                    Invoke(new Action(() => startButton.Text = "Start"));
                else
                    startButton.Text = "Start";
            };

            intervalS.Value = new Decimal(mConfig.IntervalMS / 1000);
            stopM.Value = new Decimal(mConfig.StopM);
            shutdownBox.Checked = mConfig.AutoShutdown;
            file.Text = mConfig.ScreenshotFile;
            folderBox.Text = mConfig.ScreenshotFolder;

            file.Text = mConfig.Key;
        }

        private void startButton_Click(object sender, EventArgs e) {
            mPlugin.Running = !mPlugin.Running;
        }

        private void intervalS_ValueChanged(object sender, EventArgs e) {
            mConfig.IntervalMS = Decimal.ToDouble(intervalS.Value) * 1000.0;
        }

        private void shutdownM_ValueChanged(object sender, EventArgs e) {
            mConfig.StopM = Decimal.ToDouble(stopM.Value);
        }

        private void shutdownBox_CheckedChanged(object sender, EventArgs e) {
            mConfig.AutoShutdown = shutdownBox.Checked;
        }

        private void folderBox_TextChanged(object sender, EventArgs e) {
            mConfig.ScreenshotFolder = folderBox.Text;
        }

        private void file_TextChanged(object sender, EventArgs e) {
            mConfig.ScreenshotFile = file.Text;
        }
    }
}
