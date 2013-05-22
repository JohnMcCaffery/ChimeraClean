using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using Chimera.Plugins;
using System.Windows.Forms;

namespace Chimera.GUI.Controls.Plugins {
    public partial class ProjectorPanel : UserControl {
        private bool mExternalUpdate;
        private bool mGuiUpdate;
        private Projector mProjector;
        private Action mChangeListener;

        public Projector Projector {
            get { return mProjector; }
            set {
                if (mProjector != null)
                    mProjector.Change -= mChangeListener;
                mProjector = value;
                projectorOrientationPanel.Value = mProjector.Orientation;
                if (Created)
                    mProjector.Change += mChangeListener;
                mProjector_Change();
            }
        }

        public ProjectorPanel() {
            mChangeListener = new Action(mProjector_Change);
            InitializeComponent();
            foreach (var ar in Enum.GetValues(typeof(AspectRatio))) {
                projectorAspectPulldown.Items.Add(ar);
                projectorNativeAspectPulldown.Items.Add(ar);
            }

            HandleCreated += new EventHandler(ProjectorPanel_HandleCreated);
            Disposed += new EventHandler(ProjectorPanel_Disposed);
        }

        public ProjectorPanel(Projector projector)
            : this() {

            Projector = projector;
        }

        void ProjectorPanel_HandleCreated(object sender, EventArgs e) {
            if (mProjector != null)
                mProjector.Change += mChangeListener;
        }

        void ProjectorPanel_Disposed(object sender, EventArgs e) {
            if (mProjector != null)
                mProjector.Change -= mChangeListener;
        }

        void mProjector_Change() {
            mExternalUpdate = true;
            projectorThrowRatioPanel.Value = (float) mProjector.ThrowRatio;
            projectorPositionPanel.Value = mProjector.Position / 10f;
            projectorWallDistancePanel.Value = (float) (mProjector.ScreenDistance / 10);
            projectorOrientationPanel.Value = mProjector.Orientation;
            projectorDrawCheck.Checked = mProjector.DrawDiagram;
            projectorDrawLabelsCheck.Checked = mProjector.DrawLabels;
            projectorAutoUpdateCheck.Checked = mProjector.AutoUpdate;
            projectorAspectPulldown.SelectedItem = mProjector.AspectRatio;
            projectorNativeAspectPulldown.SelectedItem = mProjector.NativeAspectRatio;
            configureProjectorButton.Checked = mProjector.ConfigureFromProjector;
            configureWindowButton.Checked = !mProjector.ConfigureFromProjector;
            upsideDownCheck.Checked = mProjector.UpsideDown;
            lockHeightCheck.Checked = mProjector.LockHeight;
            vOffsetPanel.Value = (float) mProjector.VOffset;

            projectorDrawLabelsCheck.Enabled = mProjector.DrawDiagram;

            projectorOrientationPanel.Text = "Orientation (cm)";
            projectorPositionPanel.Text = "Position (cm)";
            mExternalUpdate = false;
        }

        private void projectorConfigureutton_Click(object sender, EventArgs e) {
            mProjector.Configure();
        }

        private void projectorDrawCheck_CheckedChanged(object sender, EventArgs e) {
            if (!mExternalUpdate) {
                mProjector.DrawDiagram = projectorDrawCheck.Checked;

                projectorDrawLabelsCheck.Enabled = mProjector.DrawDiagram;
            }
        }

        private void throwRatioPanel_ValueChanged(float obj) {
            if (!mExternalUpdate)
                mProjector.ThrowRatio = projectorThrowRatioPanel.Value;
        }

        private void projectorPositionPanel_ValueChanged(object sender, EventArgs e) {
            if (!mExternalUpdate)
                mProjector.Position = projectorPositionPanel.Value * 10f;
        }

        private void wallDistancePanel_ValueChanged(float obj) {
            if (!mExternalUpdate)
                mProjector.ScreenDistance = projectorWallDistancePanel.Value * 10f;
        }

        private void projectorDrawLabelsCheck_CheckedChanged(object sender, EventArgs e) {
            if (!mExternalUpdate)
                mProjector.DrawLabels = projectorDrawLabelsCheck.Checked;
        }

        private void projectorAutoUpdate_CheckedChanged(object sender, EventArgs e) {
            if (!mExternalUpdate)
                mProjector.AutoUpdate = projectorAutoUpdateCheck.Checked;
        }

        private void projectorNativeAspectPulldown_SelectedIndexChanged(object sender, EventArgs e) {
            if (!mExternalUpdate)
                mProjector.NativeAspectRatio = (AspectRatio)projectorNativeAspectPulldown.SelectedItem;
        }

        private void projectorAspectPulldown_SelectedIndexChanged(object sender, EventArgs e) {
            if (!mExternalUpdate)
                mProjector.AspectRatio = (AspectRatio)projectorAspectPulldown.SelectedItem;
        }

        private void configureWindowButton_CheckedChanged(object sender, EventArgs e) {
            if (!mExternalUpdate && configureWindowButton.Checked)
                mProjector.ConfigureFromProjector = false;
        }

        private void configureProjectorButton_CheckedChanged(object sender, EventArgs e) {
            if (!mExternalUpdate && configureProjectorButton.Checked)
                mProjector.ConfigureFromProjector = true;
        }

        private void upsideDownCheck_CheckedChanged(object sender, EventArgs e) {
            if (!mExternalUpdate)
                mProjector.UpsideDown = upsideDownCheck.Checked;
        }

        private void vOffsetPanel_ValueChanged(float obj) {
            if (!mExternalUpdate)
                mProjector.VOffset = vOffsetPanel.Value;
        }

        private void lockHeightCheck_CheckedChanged(object sender, EventArgs e) {
            if (!mExternalUpdate)
                mProjector.LockHeight = lockHeightCheck.Checked;
        }
    }
}
