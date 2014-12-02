using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Plugins;
using Chimera.Config;

namespace Chimera.GUI.Controls.Plugins {
    public partial class PanoramaPanel : UserControl {
        private PanoramaPlugin mPlugin;
        private CoreConfig mConfig;

        public PanoramaPanel() {
            InitializeComponent();
        }

        public PanoramaPanel(PanoramaPlugin plugin) {
            InitializeComponent();

            mPlugin = plugin;
            mConfig = plugin.Config as CoreConfig;
        }

        private void button1_Click(object sender, EventArgs e) {
            mPlugin.TakePanorama();
        }

    }
}
