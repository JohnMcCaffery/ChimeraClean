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
        private bool mMassUpdated;
        private double mScale = 10.0;
        private Window mWindow;

        public WindowPanel() {
            InitializeComponent();

            topLeftPanel.Text = "Top Left";
            centrePanel.Text = "Centre";
            orientationPanel.Text = "Orientation";
        }

        public WindowPanel(Window window)
            : this() {
            
            Init(window);
        }

        public void Init(Window window) {
            mWindow = window;

            controlCursor.Checked = mWindow.OverlayManager.ControlPointer;

            mWindow.OverlayManager.OverlayClosed += new EventHandler(mWindow_OverlayClosed);
            mWindow.OverlayManager.OverlayLaunched += new EventHandler(mWindow_OverlayLaunched);
            mWindow.Changed += new Action<Window,EventArgs>(mWindow_Changed);

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

            mWindow_Changed(window, null);
        }

        void mWindow_Changed(Window source, EventArgs args) {
            Action a = () => {
                mMassUpdated = true;

                topLeftPanel.Value = mWindow.TopLeft / (float) mScale;
                centrePanel.Value = mWindow.Centre / (float) mScale;
                orientationPanel.Value = mWindow.Orientation;
                distancePanel.Value = (float) (mWindow.ScreenDistance / mScale);
                skewHPanel.Value = (float)(mWindow.HSkew / mScale);
                vSkewPanel.Value = (float)(mWindow.VSkew / mScale);
                widthPanel.Value = (float) (mWindow.Width / mScale);
                heightPanel.Value = (float) (mWindow.Height / mScale);
                aspectRatioWValue.Value = new decimal(mWindow.Width);
                aspectRatioHValue.Value = new decimal(mWindow.Height);
                aspectRatioValue.Value = new decimal(mWindow.AspectRatio);
                diagonalPanel.Value = (float) (mWindow.Diagonal / mScale);
                fovHPanel.Value = (float)(mWindow.HFieldOfView * (180.0 / Math.PI));
                fovVPanel.Value = (float)(mWindow.VFieldOfView * (180.0 / Math.PI));

                switch (mWindow.Projection) {
                    case ProjectionStyle.Simple: simpleProjectionButton.Checked = true; break;
                    case ProjectionStyle.Skewed: skewedProjectionButton.Checked = true; break;
                    case ProjectionStyle.Calculated: calculatedProjectionButton.Checked = true; break;
                }

                if (mWindow.Anchor == WindowAnchor.Centre)
                    centreAnchorButton.Checked = true;
                else
                    topLeftAnchorButton.Checked = true;

                linkFOVCheck.Checked = mWindow.LinkFoVs;

                mMassUpdated = false;
            };

            if (InvokeRequired)
                Invoke(a);
            else
                a();
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

        private void controlCursor_CheckedChanged(object sender, EventArgs e) {
            mWindow.OverlayManager.ControlPointer = controlCursor.Checked;
        }

        private void restartButton_Click(object sender, EventArgs e) {
            if (mWindow.Output != null)
                mWindow.Output.Restart();
        }

        private void distancePanel_ValueChanged(float obj) {
            if (!mMassUpdated)
                mWindow.ScreenDistance = distancePanel.Value * mScale;
        }

        private void skewHPanel_ValueChanged(float obj) {
            if (!mMassUpdated)
                mWindow.HSkew = skewHPanel.Value * mScale;
        }

        private void skewVPanel_ValueChanged(float obj) {
            if (!mMassUpdated)
                mWindow.VSkew = vSkewPanel.Value * mScale;
        }

        private void topLeftPanel_ValueChanged(object sender, EventArgs e) {
            if (!mMassUpdated)
                mWindow.TopLeft = topLeftPanel.Value * (float) mScale;
        }

        private void centrePanel_ValueChanged(object sender, EventArgs e) {
            if (!mMassUpdated)
                mWindow.Centre = centrePanel.Value * (float) mScale;
        }

        private void widthPanel_Changed(float obj) {
            if (!mMassUpdated)
                mWindow.Width = widthPanel.Value * mScale;
        }

        private void heightPanel_Changed(float obj) {
            if (!mMassUpdated)
                mWindow.Height = heightPanel.Value * mScale;
        }

        private void diagonalPanel_ValueChanged(float obj) {
            if (!mMassUpdated)
                mWindow.Diagonal = (float)diagonalPanel.Value * mScale;
        }

        private void aspectRatioValue_ValueChanged(object sender, EventArgs e) {
            if (!mMassUpdated)
                mWindow.AspectRatio = (float)decimal.ToDouble(aspectRatioValue.Value);
        }

        private void aspectRatioHValue_ValueChanged(object sender, EventArgs e) {
            if (!mMassUpdated)
                aspectRatioValue.Value = aspectRatioWValue.Value / aspectRatioHValue.Value;
        }

        private void aspectRatioWValue_ValueChanged(object sender, EventArgs e) {
            if (!mMassUpdated)
                aspectRatioValue.Value = aspectRatioWValue.Value / aspectRatioHValue.Value;
        }

        private void fovHPanel_ValueChanged(float obj) {
            if (!mMassUpdated)
                mWindow.HFieldOfView = fovHPanel.Value * (Math.PI / 180.0);
        }

        private void fovVPanel_ValueChanged(float obj) {
            if (!mMassUpdated)
                mWindow.VFieldOfView = fovVPanel.Value * (Math.PI / 180.0);
        }

        private void ProjectionButton_CheckedChanged(object sender, EventArgs e) {
            if (mMassUpdated)
                return;
            if (simpleProjectionButton.Checked)
                mWindow.Projection = ProjectionStyle.Simple;
            else if (skewedProjectionButton.Checked)
                mWindow.Projection = ProjectionStyle.Skewed;
            else if (calculatedProjectionButton.Checked)
                mWindow.Projection = ProjectionStyle.Calculated;
        }

        private void AnchorButton_CheckedChanged(object sender, EventArgs e) {
            if (!mMassUpdated)
                mWindow.Anchor = topLeftAnchorButton.Checked ? WindowAnchor.TopLeft : WindowAnchor.Centre;
        }

        private void linkFOVCheck_CheckedChanged(object sender, EventArgs e) {
            if (!mMassUpdated)
                mWindow.LinkFoVs = linkFOVCheck.Checked;
        }
    }
}
