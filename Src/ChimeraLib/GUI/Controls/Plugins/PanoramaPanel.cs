using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Plugins;
using Chimera.Config;

namespace Chimera.GUI.Controls.Plugins {
    public partial class PanoramaPanel : UserControl {
        private PanoramaPlugin mPlugin;
        private CoreConfig mConfig;

        public PanoramaPanel() {
            InitializeComponent();
        }

        public PanoramaPanel(PanoramaPlugin plugin) {
            InitializeComponent();

            mPlugin = plugin;
            mConfig = plugin.Config as CoreConfig;

            folderAddress.Text = mConfig.ScreenshotFolder;
            folderDialog.RootFolder = Environment.SpecialFolder.MyDocuments;
            folderDialog.SelectedPath = mConfig.ScreenshotFolder;
            captureDelay.Value = mConfig.CaptureDelayMS;
        }

        private void captureButton_Click(object sender, EventArgs e) {
            mPlugin.TakePanorama();
        }

        private void folderButton_Click(object sender, EventArgs e) {
            if (folderDialog.ShowDialog() == DialogResult.OK) {
                folderAddress.Text = folderDialog.SelectedPath;
                mConfig.ScreenshotFolder = folderDialog.SelectedPath;
            }
        }

        private void folderAddress_TextChanged(object sender, EventArgs e) {
            folderDialog.SelectedPath = folderAddress.Text;
            mConfig.ScreenshotFolder = folderDialog.SelectedPath;
        }

        private void captureDelay_ValueChanged(object sender, EventArgs e) {
            mConfig.CaptureDelayMS = Decimal.ToInt32(captureDelay.Value);
        }

        private void setCentreButton_Click(object sender, EventArgs e) {
            mPlugin.SetCentre();
        }

        private void nextImageButton_Click(object sender, EventArgs e) {
            mPlugin.ShowNextImage();
        }

        private void screenshotButton_Click(object sender, EventArgs e) {
            mPlugin.TakeScreenshot();
        }
    }
}
