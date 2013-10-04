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
                orientationPanel.Value = mProjector.Orientation;
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
            Invoke(() => {
                mExternalUpdate = true;
                throwRatioPanel.Value = (float)mProjector.ThrowRatio;
                positionPanel.Value = mProjector.Position / 10f;
                wallDistancePanel.Value = (float)(mProjector.D / 10);
                orientationPanel.Value = mProjector.Orientation;
                projectorDrawCheck.Checked = mProjector.DrawDiagram;
                projectorDrawLabelsCheck.Checked = mProjector.DrawLabels;
                projectorAspectPulldown.SelectedItem = mProjector.AspectRatio;
                projectorNativeAspectPulldown.SelectedItem = mProjector.NativeAspectRatio;
                upsideDownCheck.Checked = mProjector.UpsideDown;
                vOffsetPanel.Value = (float)mProjector.VOffset;
                switch (mProjector.Lock) {
                    case LockedVariable.Nothing: noLockButton.Checked = true; break;
                    case LockedVariable.Width: lockWidthButton.Checked = true; break;
                    case LockedVariable.Height: lockHeightButton.Checked = true; break;
                    case LockedVariable.Position: lockPositionButton.Checked = true; break;
                }

                projectorDrawLabelsCheck.Enabled = mProjector.DrawDiagram;

                orientationPanel.Text = "Orientation (cm)";
                positionPanel.Text = "Position (cm)";
                mExternalUpdate = false;
            });
        }

        private void Invoke(Action a) {
            if (InvokeRequired && Created && !IsDisposed && !Disposing)
                BeginInvoke(a);
            else
                a();
        }

        private void projectorDrawCheck_CheckedChanged(object sender, EventArgs e) {
            if (!mExternalUpdate) {
                mProjector.DrawDiagram = projectorDrawCheck.Checked;

                projectorDrawLabelsCheck.Enabled = mProjector.DrawDiagram;
            }
        }

        private void throwRatioPanel_ValueChanged(float obj) {
            if (!mExternalUpdate)
                mProjector.ThrowRatio = throwRatioPanel.Value;
        }

        private void projectorPositionPanel_ValueChanged(object sender, EventArgs e) {
            if (!mExternalUpdate)
                mProjector.Position = positionPanel.Value * 10f;
        }

        private void wallDistancePanel_ValueChanged(float obj) {
            if (!mExternalUpdate)
                mProjector.D = wallDistancePanel.Value * 10f;
        }

        private void projectorDrawLabelsCheck_CheckedChanged(object sender, EventArgs e) {
            if (!mExternalUpdate)
                mProjector.DrawLabels = projectorDrawLabelsCheck.Checked;
        }

        private void projectorNativeAspectPulldown_SelectedIndexChanged(object sender, EventArgs e) {
            if (!mExternalUpdate)
                mProjector.NativeAspectRatio = (AspectRatio)projectorNativeAspectPulldown.SelectedItem;
        }

        private void projectorAspectPulldown_SelectedIndexChanged(object sender, EventArgs e) {
            if (!mExternalUpdate)
                mProjector.AspectRatio = (AspectRatio)projectorAspectPulldown.SelectedItem;
        }

        private void upsideDownCheck_CheckedChanged(object sender, EventArgs e) {
            if (!mExternalUpdate)
                mProjector.UpsideDown = upsideDownCheck.Checked;
        }

        private void vOffsetPanel_ValueChanged(float obj) {
            if (!mExternalUpdate)
                mProjector.VOffset = vOffsetPanel.Value;
        }

        private void lockPositionButton_CheckedChanged(object sender, EventArgs e) {
            if (lockPositionButton.Checked) {
                mProjector.Lock = LockedVariable.Position;
                positionPanel.Enabled = true;
                wallDistancePanel.Enabled = true;
                wallDistanceLabel.Enabled = true;
                
            }
        }

        private void lockWidthButton_CheckedChanged(object sender, EventArgs e) {
            if (lockWidthButton.Checked) {
                mProjector.Lock = LockedVariable.Width;
                positionPanel.Enabled = false;
                wallDistancePanel.Enabled = false;
                wallDistanceLabel.Enabled = false;
            }
        }

        private void lockHeightButton_CheckedChanged(object sender, EventArgs e) {
            if (lockHeightButton.Checked) {
                mProjector.Lock = LockedVariable.Height;
                positionPanel.Enabled = false;
                wallDistancePanel.Enabled = false;
                wallDistanceLabel.Enabled = false;
            }
        }

        private void noLockButton_CheckedChanged(object sender, EventArgs e) {
            if (noLockButton.Checked) {
                mProjector.Lock = LockedVariable.Nothing;
                positionPanel.Enabled = true;
                wallDistancePanel.Enabled = true;
                wallDistanceLabel.Enabled = true;
            }
        }
    }
}
