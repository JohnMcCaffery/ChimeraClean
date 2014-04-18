using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Chimera.Config;
using Chimera.ConfigurationTool.Controls;
using System.Configuration;
using Nini.Config;

namespace Chimera.ConfigurationTool {
    public partial class ConfigurationTool : Form {
        private bool mLoaded = false;

        public ConfigurationTool() {
            InitializeComponent();
        }

        public void LoadFolders() {
            foreach (var folder in Directory.GetDirectories(".").Select(f => f.Substring(2))) {
                TabPage page = new TabPage(folder);

                ConfigurationFolderPanel panel = new ConfigurationFolderPanel(folder);
                panel.Dock = DockStyle.Fill;

                page.Controls.Add(panel);

                FoldersTab.TabPages.Add(page);
            }
        }
        

        private void ConfigurationTool_Load(object sender, EventArgs e) {
            LoadFolders();
        }
    }
}
