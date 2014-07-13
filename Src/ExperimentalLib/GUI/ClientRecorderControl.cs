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
    public partial class ClientRecorderControl : UserControl {
        private ExperimentalConfig mConfig;
        private ClientRecorderPlugin mPlugin;

        public ClientRecorderControl()
            : base() {
            InitializeComponent();
        }

        public ClientRecorderControl(ClientRecorderPlugin plugin) {

            InitializeComponent();

            mPlugin = plugin;
            mConfig = mPlugin.Config as ExperimentalConfig;

            statsList.Columns.Add("Timestamp");
            foreach (var colName in mConfig.OutputKeys) {
                var col = statsList.Columns.Add(colName);
                col.Width = 30;
            }
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

                foreach (var stat in mPlugin.Stats) {
                    ListViewItem item = new ListViewItem(stat.ToString());
                    foreach (var value in stat.Split(','))
                        item.SubItems.Add(value);

                    statsList.Items.Add(item);
                }
            }
        }

        private void loadLogButton_Click(object sender, EventArgs e) {
            string file = mConfig.GetLogFileName();
            openFileDialog.InitialDirectory = Path.GetDirectoryName(file);
            openFileDialog.FileName = mConfig.GetLogFileName();
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                mPlugin.LoadViewerLog(openFileDialog.FileName);

                statsList.Items.Clear();

                foreach (var stat in mPlugin.Stats) {
                    ListViewItem item = new ListViewItem(stat.ToString());
                    foreach (var value in stat.Split(','))
                        item.SubItems.Add(value);

                    statsList.Items.Add(item);
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
