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
