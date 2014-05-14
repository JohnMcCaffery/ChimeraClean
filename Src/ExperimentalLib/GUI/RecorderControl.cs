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
using System.IO;

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

            mPlugin.LoggedInChanged += new Action<bool>(mPlugin_LoggedInChanged);
        }

        void mPlugin_LoggedInChanged(bool loggedIn) {
            Invoke(new Action(() => updateTimer.Enabled = mConfig.UpdateStatsGUI && loggedIn));
        }

        private void updateTimer_Tick(object sender, EventArgs e) {
            if (!mPlugin.Recording)
                return;

            ListViewItem item = new ListViewItem(mPlugin.LastStat.ToString());
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
            Clipboard.SetText("-" + mConfig.Timestamp.ToString(mConfig.TimestampFormat) + "-MainWindow.log");
            clipboardLabel.Text = "'" + Clipboard.GetText() + "' in the clipboard.";
        }

        private void loadCSVButton_Click(object sender, EventArgs e) {            openFileDialog.FileName = mPlugin.GetCSVName();
            openFileDialog.InitialDirectory = Path.GetDirectoryName(openFileDialog.FileName);
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                mConfig.Timestamp = mPlugin.LoadCSV(openFileDialog.FileName);

                statsList.Items.Clear();

                foreach (var stat in mPlugin.StatsList) {
                    ListViewItem item = new ListViewItem(stat.ToString());
                    foreach (var key in mConfig.OutputKeys)
                        item.SubItems.Add(stat.Get(key));

                    statsList.Items.Add(item);
                }
            }

        }

        private void loadLogButton_Click(object sender, EventArgs e) {
            mPlugin.Logout();
            string file = mConfig.GetLogFileName();
            openFileDialog.InitialDirectory = Path.GetDirectoryName(file);
            openFileDialog.FileName = mConfig.GetLogFileName();
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                mConfig.Timestamp = mPlugin.LoadViewerLog(openFileDialog.FileName);

                int cfps = GetKeyIndex("CFPS");
                int polygons = GetKeyIndex("Polygons");

                foreach (var it in statsList.Items) {
                    ListViewItem item = it as ListViewItem;
                    if (mPlugin.HasStat(item.Text, "CFPS"))
                        item.SubItems[cfps].Text = mPlugin[item.Text, "CFPS"];
                    if (mPlugin.HasStat(item.Text, "Polygons"))
                        item.SubItems[polygons].Text = mPlugin[item.Text, "Polygons"];
                }
            }
        }

        private int GetKeyIndex(string key) {
            for (int i = 0; i < mConfig.OutputKeys.Length; i++)
                if (mConfig.OutputKeys[i] == key)
                    return i + 1;
            return 0;
        }

        private void saveCSVButton_Click(object sender, EventArgs e) {
            saveFileDialog.FileName = mPlugin.GetCSVName();
            saveFileDialog.InitialDirectory = Path.GetDirectoryName(saveFileDialog.FileName);
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                mPlugin.WriteCSV(saveFileDialog.FileName);
            }
        }

        private void recordFPS_Click(object sender, EventArgs e) {
            mPlugin.StartLogging();
        }
    }
}
