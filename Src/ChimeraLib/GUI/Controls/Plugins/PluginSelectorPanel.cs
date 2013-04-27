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
    public partial class PluginSelectorPanel : UserControl {
        private PluginSelector mPlugin;

        public PluginSelectorPanel() {
            InitializeComponent();
        }

        public PluginSelectorPanel(PluginSelector input)
            : this() {

            mPlugin = input;
            mPlugin.PluginAdded += new Action<ISystemPlugin>(mPlugin_PluginAdded);

            foreach (var inpt in mPlugin.Plugins)
                mPlugin_PluginAdded(inpt);
        }

        public void mPlugin_PluginAdded(ISystemPlugin input) {
            inputSelectionBox.Items.Add(input);
            if (mPlugin.CurrentPlugin == input)
                inputSelectionBox.SelectedItem = input;
        }

        private void inputSelectionBox_SelectedIndexChanged(object sender, EventArgs e) {
            mPlugin.CurrentPlugin = (ISystemPlugin)inputSelectionBox.SelectedItem;
        }
    }
}
