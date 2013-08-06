using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chimera.Kinect.GUI {
    public partial class EyeTrackerPluginControl : UserControl {
        private EyeTrackerPlugin mPlugin;

        public EyeTrackerPluginControl() {
            InitializeComponent();
        }

        public EyeTrackerPluginControl(EyeTrackerPlugin plugin)
            : this() {

            mPlugin = plugin;

            positionPanel.Text = "Position";
            orientationPanel.Text = "Orientation";

            positionPanel.Value = mPlugin.Position;
            orientationPanel.Value = mPlugin.Orientation;
        }

        private void positionPanel_ValueChanged(object sender, EventArgs e) {
            mPlugin.Position = positionPanel.Value;
        }
    }
}
