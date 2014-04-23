using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.OpenSim.GUI;
using Chimera.OpenSim;

namespace Chimera.Experimental.GUI {
    public partial class RecorderControl : OpensimBotPanel {
        private ExperimentalConfig mConfig;
        private RecorderPlugin mPlugin;

        public RecorderControl()
            : base() {
            InitializeComponent();
        }

        public RecorderControl(RecorderPlugin plugin)
            : base(plugin) {

            InitializeComponent();

            mPlugin = plugin;
            mConfig = mPlugin.Config as ExperimentalConfig;
        }

        private void updateTimer_Tick(object sender, EventArgs e) {
            /*
            if (mPlugin.LastStat != null && mPlugin.LastStat != null) {

            }
            */
        }

        private void updateFreq_ValueChanged(object sender, EventArgs e) {
            updateTimer.Interval = decimal.ToInt32(updateFreq.Value);
        }
    }
}
