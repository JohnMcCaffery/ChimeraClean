using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Config;

namespace Chimera.ConfigurationTool.Controls {
    public partial class ConfigurationObjectControlPanel : UserControl {
        ConfigBase mConfig;

        public ConfigurationObjectControlPanel() {
            InitializeComponent();
        }

        public ConfigurationObjectControlPanel(ConfigBase config)
            : this() {
            mConfig = config;

            foreach (var parameter in mConfig.Parameters) {
                /*
                DataGridViewRow row = new DataGridViewRow();

                DataGridViewCell keyCell = new DataGridViewCell();
                DataGridViewCell valueCell = new DataGridViewCell();
                DataGridViewCell defaultCell = new DataGridViewCell();
                DataGridViewCell descriptionCell = new DataGridViewCell();

                parametersList.Rows.Add(row);

                /*
                it.SubItems.Add(new ListViewItem.ListViewSubItem(it, parameter.Value));
                it.SubItems.Add(new ListViewItem.ListViewSubItem(it, parameter.Default));
                it.SubItems.Add(new ListViewItem.ListViewSubItem(it, parameter.Description));
                */

                int r = parametersList.Rows.Add(parameter.Key, parameter, parameter.Default, parameter.Description);
                var valueCell = parametersList.Rows[r].Cells[1];
                //var editor = parametersList.EditingControl

                //valueCell.
            }
        }
    }
}
