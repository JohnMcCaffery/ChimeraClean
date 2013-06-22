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
using Chimera.Plugins;

namespace Chimera.GUI.Controls {
    public partial class FramePanel : UserControl {
        private bool mMassUpdated;
        private double mScale = 10.0;
        private Frame mFrame;

        public FramePanel() {
            InitializeComponent();

            topLeftPanel.Text = "Top Left";
            centrePanel.Text = "Centre";
            orientationPanel.Text = "Orientation";
        }

        public FramePanel(Frame frame)
            : this() {
            
            Init(frame);
        }

        public void Init(Frame frame) {
            mFrame = frame;

            mFrame.Changed += new Action<Frame, EventArgs>(mWindow_Changed);

            foreach (var screen in Screen.AllScreens) {
                monitorPulldown.Items.Add(screen);
                if (screen.DeviceName.Equals(frame.Monitor.DeviceName))
                    monitorPulldown.SelectedItem = screen;
            }

            if (frame.Output != null) {
                Control panel = frame.Output.ControlPanel;
                panel.Dock = DockStyle.Fill;

                TabPage tab = new TabPage();
                tab.Name = "outputTab";
                tab.Text = "Output";
                tab.Controls.Add(panel);

                mainTab.Controls.Add(tab);
            }

            mWindow_Changed(frame, null);
        }

        void mWindow_Changed(Frame source, EventArgs args) {
            Action a = () => {
                mMassUpdated = true;

                topLeftPanel.Value = mFrame.TopLeft / (float) mScale;
                centrePanel.Value = mFrame.Centre / (float) mScale;
                orientationPanel.Value = mFrame.Orientation;
                distancePanel.Value = (float) (mFrame.ScreenDistance / mScale);
                skewHPanel.Value = (float)(mFrame.HSkew / mScale);
                vSkewPanel.Value = (float)(mFrame.VSkew / mScale);
                widthPanel.Value = (float) (mFrame.Width / mScale);
                heightPanel.Value = (float) (mFrame.Height / mScale);
                aspectRatioWValue.Value = new decimal(Math.Max(1, mFrame.Width));
                aspectRatioHValue.Value = new decimal(Math.Max(1, mFrame.Height));
                aspectRatioValue.Value = new decimal(mFrame.AspectRatio);
                diagonalPanel.Value = (float) (mFrame.Diagonal / mScale);
                fovHPanel.Value = (float)(mFrame.HFieldOfView * (180.0 / Math.PI));
                fovVPanel.Value = (float)(mFrame.VFieldOfView * (180.0 / Math.PI));
                drawCheck.Checked = mFrame.DrawWindow;
                drawEyeCheck.Checked = mFrame.DrawEye;

                switch (mFrame.Projection) {
                    case ProjectionStyle.Simple: simpleProjectionButton.Checked = true; break;
                    case ProjectionStyle.Calculated: calculatedProjectionButton.Checked = true; break;
                }

                if (mFrame.Anchor == WindowAnchor.Centre)
                    centreAnchorButton.Checked = true;
                else
                    topLeftAnchorButton.Checked = true;

                linkFOVCheck.Checked = mFrame.LinkFoVs;

                mMassUpdated = false;
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a();
        }

        private void mainTab_KeyDown(object sender, KeyEventArgs e) {
            if (mFrame != null)
                mFrame.Core.TriggerKeyboard(true, e);
        }

        private void mainTab_KeyUp(object sender, KeyEventArgs e) {
            if (mFrame != null)
                mFrame.Core.TriggerKeyboard(false, e);
        }

        private void monitorPulldown_SelectedIndexChanged(object sender, EventArgs e) {
            if (mFrame != null)
                mFrame.Monitor = (Screen)monitorPulldown.SelectedItem;
        }

        private void restartButton_Click(object sender, EventArgs e) {
            mFrame.Restart();
        }

        private void distancePanel_ValueChanged(float obj) {
            if (!mMassUpdated)
                mFrame.ScreenDistance = distancePanel.Value * mScale;
        }

        private void skewHPanel_ValueChanged(float obj) {
            if (!mMassUpdated)
                mFrame.HSkew = skewHPanel.Value * mScale;
        }

        private void skewVPanel_ValueChanged(float obj) {
            if (!mMassUpdated)
                mFrame.VSkew = vSkewPanel.Value * mScale;
        }

        private void topLeftPanel_ValueChanged(object sender, EventArgs e) {
            if (!mMassUpdated)
                mFrame.TopLeft = topLeftPanel.Value * (float) mScale;
        }

        private void centrePanel_ValueChanged(object sender, EventArgs e) {
            if (!mMassUpdated)
                mFrame.Centre = centrePanel.Value * (float) mScale;
        }

        private void widthPanel_Changed(float obj) {
            if (!mMassUpdated)
                mFrame.Width = widthPanel.Value * mScale;
        }

        private void heightPanel_Changed(float obj) {
            if (!mMassUpdated)
                mFrame.Height = heightPanel.Value * mScale;
        }

        private void diagonalPanel_ValueChanged(float obj) {
            if (!mMassUpdated)
                mFrame.Diagonal = (float)diagonalPanel.Value * mScale;
        }

        private void aspectRatioValue_ValueChanged(object sender, EventArgs e) {
            if (!mMassUpdated)
                mFrame.AspectRatio = (float)decimal.ToDouble(aspectRatioValue.Value);
        }

        private void fovHPanel_ValueChanged(float obj) {
            if (!mMassUpdated)
                mFrame.HFieldOfView = fovHPanel.Value * (Math.PI / 180.0);
        }

        private void fovVPanel_ValueChanged(float obj) {
            if (!mMassUpdated)
                mFrame.VFieldOfView = fovVPanel.Value * (Math.PI / 180.0);
        }

        private void ProjectionButton_CheckedChanged(object sender, EventArgs e) {
            if (mMassUpdated)
                return;
            if (simpleProjectionButton.Checked)
                mFrame.Projection = ProjectionStyle.Simple;
            else if (calculatedProjectionButton.Checked)
                mFrame.Projection = ProjectionStyle.Calculated;
        }

        private void AnchorButton_CheckedChanged(object sender, EventArgs e) {
            if (!mMassUpdated)
                mFrame.Anchor = topLeftAnchorButton.Checked ? WindowAnchor.TopLeft : WindowAnchor.Centre;
        }

        private void linkFOVCheck_CheckedChanged(object sender, EventArgs e) {
            if (!mMassUpdated)
                mFrame.LinkFoVs = linkFOVCheck.Checked;
        }

        private void drawEyeCheck_CheckedChanged(object sender, EventArgs e) {
            mFrame.DrawEye = drawEyeCheck.Checked;
        }

        private void aspectRatioButton_Click(object sender, EventArgs e) {
            mFrame.AspectRatio = decimal.ToDouble(aspectRatioHValue.Value / aspectRatioWValue.Value);
        }

        private void drawCheck_CheckedChanged(object sender, EventArgs e) {
            mFrame.DrawWindow = drawCheck.Checked;
        }
    }
}
