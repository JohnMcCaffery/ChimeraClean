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
        }

        public WindowPanel(Window window)
            : this() {
            
            Init(window);
        }

        public void Init(Window window) {
            mWindow = window;

            mWindow.OverlayClosed += new EventHandler(mWindow_OverlayClosed);
            mWindow.OverlayLaunched += new EventHandler(mWindow_OverlayLaunched);

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

            mouseControlCheck.Checked = mWindow.MouseControl;
            if (mWindow.OverlayVisible) {
                launchOverlayButton.Text =  "Close Overlay";
                mWindow.LaunchOverlay();
            }

            orientationPanel.Value = window.Orientation;
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

        private void screenPulldown_SelectedIndexChanged(object sender, EventArgs e) {
            if (mWindow != null)
                mWindow.Monitor = (Screen)monitorPulldown.SelectedItem;
        }

        private void launchOverlayButton_Click(object sender, EventArgs e) {
            mWindow.LaunchOverlay();
            launchOverlayButton.Text = "Close Overlay";
        }

        private void bringToFrontButtin_Click(object sender, EventArgs e) {
            mWindow.ForegroundOverlay();
        }

        private void showBordersTextBox_CheckedChanged(object sender, EventArgs e) {
            mWindow.OverlayFullscreen = fullscreenCheck.Checked;
        }

        private void mouseControlCheck_CheckedChanged(object sender, EventArgs e) {
            mWindow.MouseControl = mouseControlCheck.Checked;
        }

        private void positionPanel_ValueChanged(object sender, EventArgs e) {
            mWindow.TopLeft = positionPanel.Value * 10f;
        }

        private void widthPanel_Changed(float obj) {
            mWindow.Width = widthPanel.Value * 10.0;
        }

        private void heighPanel_Changed(float obj) {
            mWindow.Height = heighPanel.Value * 10.0;
        }
    }
}
