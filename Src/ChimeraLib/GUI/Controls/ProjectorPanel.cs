using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using Chimera.Core;
using System.Windows.Forms;

namespace Chimera.GUI.Controls {
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

        void ProjectorPanel_HandleCreated(object sender, EventArgs e) {
            if (mProjector != null)
                mProjector.Change += mChangeListener;
        }

        void ProjectorPanel_Disposed(object sender, EventArgs e) {
            if (mProjector != null)
                mProjector.Change -= mChangeListener;
        }

        public ProjectorPanel(Projector projector)
            : this() {

            Projector = projector;
        }

        void mProjector_Change() {
            mExternalUpdate = true;
            projectorThrowRatioPanel.Value = mProjector.ThrowRatio;
            projectorRoomPositionPanel.Value = mProjector.RoomPosition / 10f;
            projectorPositionPanel.Value = mProjector.Position / 10f;
            projectorWallDistancePanel.Value = mProjector.ScreenDistance / 10f;
            projectorOrientationPanel.Value = mProjector.Orientation;
            projectorDrawCheck.Checked = mProjector.DrawDiagram;
            projectorDrawRoomCheck.Checked = mProjector.DrawRoom;
            projectorDrawLabelsCheck.Checked = mProjector.DrawLabels;
            projectorAutoUpdateCheck.Checked = mProjector.AutoUpdate;
            projectorAspectPulldown.SelectedItem = mProjector.AspectRatio;
            projectorNativeAspectPulldown.SelectedItem = mProjector.NativeAspectRatio;
            configureProjectorButton.Checked = mProjector.ConfigureFromProjector;
            configureWindowButton.Checked = !mProjector.ConfigureFromProjector;
            upsideDownCheck.Checked = mProjector.UpsideDown;
            vOffsetPanel.Value = mProjector.VOffset;

            projectorDrawRoomCheck.Enabled = mProjector.DrawDiagram;
            projectorDrawLabelsCheck.Enabled = mProjector.DrawDiagram;
            projectorAutoUpdateCheck.Enabled = mProjector.DrawDiagram;

            projectorOrientationPanel.Text = "Orientation (cm)";
            projectorPositionPanel.Text = "Position (cm)";
            projectorRoomPositionPanel.Text = "Room Position (cm)";
            mExternalUpdate = false;
        }

        private void projectorConfigureutton_Click(object sender, EventArgs e) {
            mProjector.Configure();
        }

        private void projectorDrawCheck_CheckedChanged(object sender, EventArgs e) {
            if (!mExternalUpdate) {
                mProjector.DrawDiagram = projectorDrawCheck.Checked;

                projectorDrawRoomCheck.Enabled = mProjector.DrawDiagram;
                projectorDrawLabelsCheck.Enabled = mProjector.DrawDiagram;
                projectorAutoUpdateCheck.Enabled = mProjector.DrawDiagram;
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

        private void projectorEyePosition_ValueChanged(object sender, EventArgs e) {
            if (!mExternalUpdate)
                mProjector.RoomPosition = projectorRoomPositionPanel.Value * 10f;
        }

        private void projectorDrawRoomChecked_CheckedChanged(object sender, EventArgs e) {
            if (!mExternalUpdate)
                mProjector.DrawRoom = projectorDrawRoomCheck.Checked;
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
    }
}
