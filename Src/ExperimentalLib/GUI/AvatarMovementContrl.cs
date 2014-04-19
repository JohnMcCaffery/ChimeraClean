using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Experimental.Plugins;

namespace Chimera.Experimental.GUI {
    public partial class AvatarMovementContrl : UserControl {
        private AvatarMovementPlugin mPlugin;

        public AvatarMovementContrl() {
            InitializeComponent();
        }

        public AvatarMovementContrl(AvatarMovementPlugin plugin) : this() {
            mPlugin = plugin;
        }

        private void startButton_Click(object sender, EventArgs e) {
            mPlugin.Start();
        }
    }
}
