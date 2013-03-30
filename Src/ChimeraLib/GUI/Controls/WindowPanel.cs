using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chimera.GUI.Controls {
    public partial class WindowPanel : UserControl {
        private Window mWindow;

        public WindowPanel() {
            InitializeComponent();

            topLeftPanel.Text = "Top Left";
            orientationPanel.Text = "Orientation";
        }

        public WindowPanel(Window window)
            : this() {
            
            Init(window);
        }

        public void Init(Window window) {
            mWindow = window;

            widthPanel.Value = (float) window.Width / 10f;
            heightPanel.Value = (float) window.Height / 10f;
            topLeftPanel.Value = window.TopLeft / 10f;
            orientationPanel.Value = window.Orientation;
            controlCursor.Checked = mWindow.OverlayManager.ControlPointer;

            mWindow.OverlayManager.OverlayClosed += new EventHandler(mWindow_OverlayClosed);
            mWindow.OverlayManager.OverlayLaunched += new EventHandler(mWindow_OverlayLaunched);

            foreach (var screen in Screen.AllScreens) {
                monitorPulldown.Items.Add(screen);
                if (screen.DeviceName.Equals(window.Monitor.DeviceName))
                    monitorPulldown.SelectedItem = screen;
            }

            if (window.Output != null) {
                UserControl panel = window.Output.ConfigPanel;
                panel.Dock = DockStyle.Fill;

                TabPage tab = new TabPage();
                tab.Name = "outputTab";
                tab.Text = "Output";
                tab.Controls.Add(panel);

                mainTab.Controls.Add(tab);
            }
            if (mWindow.OverlayManager.Visible) {
                launchOverlayButton.Text =  "Close Overlay";
                mWindow.OverlayManager.Launch();
            }
        }

        void mWindow_OverlayLaunched(object sender, EventArgs e) {
            launchOverlayButton.Text = "Close Overlay";
        }

        void mWindow_OverlayClosed(object sender, EventArgs e) {
            launchOverlayButton.Text = "Launch Overlay";
        }

        private void mainTab_KeyDown(object sender, KeyEventArgs e) {
            if (mWindow != null)
                mWindow.Coordinator.TriggerKeyboard(true, e);
        }

        private void mainTab_KeyUp(object sender, KeyEventArgs e) {
            if (mWindow != null)
                mWindow.Coordinator.TriggerKeyboard(false, e);
        }

        private void monitorPulldown_SelectedIndexChanged(object sender, EventArgs e) {
            if (mWindow != null)
                mWindow.Monitor = (Screen)monitorPulldown.SelectedItem;
        }

        private void launchOverlayButton_Click(object sender, EventArgs e) {
            if (launchOverlayButton.Text == "Launch Overlay") {
                mWindow.OverlayManager.Launch();
                launchOverlayButton.Text = "Close Overlay";
            } else {
                mWindow.OverlayManager.Close();
                launchOverlayButton.Text = "Launch Overlay";
            }
        }

        private void bringToFrontButtin_Click(object sender, EventArgs e) {
            //mManager.Overlay.ForegroundOverlay();
        }

        private void showBordersTextBox_CheckedChanged(object sender, EventArgs e) {
            //mManager.Overlay.Fullscreen = fullscreenCheck.Checked;
        }

        private void topLeftPanel_ValueChanged(object sender, EventArgs e) {
            mWindow.TopLeft = topLeftPanel.Value * 10f;
        }

        private void widthPanel_Changed(float obj) {
            mWindow.Width = widthPanel.Value * 10.0;
        }

        private void heightPanel_Changed(float obj) {
            mWindow.Height = heightPanel.Value * 10.0;
        }

        private void controlCursor_CheckedChanged(object sender, EventArgs e) {
            //mManager.Overlay.ControlPointer = controlCursor.Checked;
        }

        private void restartButton_Click(object sender, EventArgs e) {
            if (mWindow.Output != null)
                mWindow.Output.Restart();
        }

        private void distancePanel_ValueChanged(float obj) {

        }

        private void skewHPanel_ValueChanged(float obj) {

        }

        private void skewVPanel_ValueChanged(float obj) {

        }

        private void aspectRatioValue_ValueChanged(object sender, EventArgs e) {

        }

        private void aspectRatioHValue_ValueChanged(object sender, EventArgs e) {

        }

        private void aspectRatioWValue_ValueChanged(object sender, EventArgs e) {

        }

        private void fovHPanel_ValueChanged(float obj) {

        }

        private void fovVPanel_ValueChanged(float obj) {

        }

        private void ProjectionButton_CheckedChanged(object sender, EventArgs e) {

        }

        private void AnchorButton_CheckedChanged(object sender, EventArgs e) {

        }

        private void centrePanel_ValueChanged(object sender, EventArgs e) {

        }

        private void positionPanel_ValueChanged(object sender, EventArgs e) {

        }
    }
}
