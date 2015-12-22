using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chimera.Flythrough.GUI {
    public partial class VideospherePanel : UserControl {
        private VideospherePlugin mPlugin;

        public VideospherePanel() {
            InitializeComponent();
        }

        public VideospherePanel(VideospherePlugin plugin) : this() {
            mPlugin = plugin;
        }

        private void captureButton_Click(object sender, EventArgs e) {
            mPlugin.Record();
        }

        private void stopButton_Click(object sender, EventArgs e) {
            mPlugin.Stop();
        }
    }
}
