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

            updateFreq.Value = updateTimer.Interval;

            statsList.Columns.Add("Timestamp");
            foreach (var colName in mConfig.OutputKeys) {
                var col = statsList.Columns.Add(colName);
                col.Width = 30;
            }

            if (mConfig.UpdateStatsGUI)
                updateTimer.Enabled = true;
        }

        private void updateTimer_Tick(object sender, EventArgs e) {
            if (!mPlugin.Recording)
                return;

            ListViewItem item = new ListViewItem(mPlugin.LastStat.TimeStamp.ToString(mConfig.TimestampFormat));
            foreach (var key in mConfig.OutputKeys)
                item.SubItems.Add(mPlugin.LastStat.Get(key));

            statsList.Items.Insert(0, item);
        }

        private void updateFreq_ValueChanged(object sender, EventArgs e) {
            updateTimer.Interval = decimal.ToInt32(updateFreq.Value);
        }

        private void pingButton_Click(object sender, EventArgs e) {
            mPlugin.LoadPingTime();
        }

        private void timestampButton_Click(object sender, EventArgs e) {
            mConfig.Timestamp = DateTime.Now;
        }
    }
}
