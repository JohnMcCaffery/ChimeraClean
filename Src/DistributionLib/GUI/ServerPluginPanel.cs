using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DistributionLib.Config;
using DistributionLib.Plugins;

namespace DistributionLib.GUI {
    public partial class ServerPluginPanel : UserControl {
        private ServerPlugin mPlugin;
        private DistributedConfig mConfig;

        public ServerPluginPanel() {
            InitializeComponent();
        }

        public ServerPluginPanel(ServerPlugin plugin) : this() {
            mPlugin = plugin;
            mConfig = plugin.Config as DistributedConfig;

            portUpDown.Value = mConfig.Port;
            if (mPlugin.Started)
                startButton.Enabled = false;
            else
                stopButton.Enabled = false;

            mPlugin.ClientConnected += (name, source) => Invoke(new Action(() => eventsBox.Text += name + " connected from " + source.Address));
            mPlugin.ClientDisconnected += name => Invoke(new Action(() => eventsBox.Text += name + " disconnected"));
        }

        private void portUpDown_ValueChanged(object sender, EventArgs e) {
            mConfig.Port = Decimal.ToInt32(portUpDown.Value);
        }

        private void startButton_Click(object sender, EventArgs e) {
            mPlugin.Start();
            startButton.Enabled = false;
            stopButton.Enabled = true;
        }

        private void stopButton_Click(object sender, EventArgs e) {
            mPlugin.Stop();
            startButton.Enabled = true;
            stopButton.Enabled = false;
        }
    }
}
