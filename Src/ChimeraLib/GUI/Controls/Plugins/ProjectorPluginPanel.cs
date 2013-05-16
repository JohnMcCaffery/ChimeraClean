using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Plugins;

namespace Chimera.GUI.Controls.Plugins {
    public partial class ProjectorPluginPanel : UserControl {
        private ProjectorPlugin mPlugin;
        private bool mExternalUpdate;

        public ProjectorPluginPanel() {
            InitializeComponent();
        }

        public ProjectorPluginPanel(ProjectorPlugin plugin)
            : this() {
            mPlugin = plugin;
            roomPositionPanel.Text = "Room Position (cm)";
            mPlugin.Change += new Action(mPlugin_Change);
            mPlugin.ProjectorAdded += new Action<Projector>(mPlugin_ProjectorAdded);

            foreach (var projector in mPlugin.Projectors)
                mPlugin_ProjectorAdded(projector);
        }

        void mPlugin_ProjectorAdded(Projector projector) {
            TabPage projectorTab = new TabPage();
            projectorTab.Name = projector.Window.Name;
            projectorTab.Text = projector.Window.Name;
            
            ProjectorPanel panel = new ProjectorPanel(projector);
            panel.Dock = DockStyle.Fill;

            projectorTab.Controls.Add(panel);
            mainTab.Controls.Add(projectorTab);
        }

        void mPlugin_Change() {
            mExternalUpdate = true;
            roomPositionPanel.Value = mPlugin.RoomPosition / 10f;
            projectorDrawRoomCheck.Checked = mPlugin.DrawRoom;
            projectorDrawLabelsCheck.Checked = mPlugin.DrawLabels;
            mExternalUpdate = false;
        }
        private void roomPosition_ValueChanged(object sender, EventArgs e) {
            if (!mExternalUpdate)
                mPlugin.RoomPosition = roomPositionPanel.Value * 10f;
        }

        private void projectorDrawRoomChecked_CheckedChanged(object sender, EventArgs e) {
            if (!mExternalUpdate)
                mPlugin.DrawRoom = projectorDrawRoomCheck.Checked;
        }

        private void projectorDrawLabelsCheck_CheckedChanged(object sender, EventArgs e) {
            if (!mExternalUpdate)
                mPlugin.DrawLabels = projectorDrawLabelsCheck.Checked;
        }   
    }
}
