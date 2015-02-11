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
    public partial class ClientPluginPanel : UserControl {
        private ClientPlugin mPlugin;
        private DistributedConfig mConfig;

        public ClientPluginPanel() {
            InitializeComponent();
        }

        public ClientPluginPanel(ClientPlugin plugin) : this() {
            mPlugin = plugin;
            mConfig = plugin.Config as DistributedConfig;

            portUpDown.Value = mConfig.Port;
            addressBox.Text = mConfig.Address;
            nameBox.Text = mConfig.ClientName;
            if (mPlugin.Connected)
                connectButton.Enabled = false;
            else
                disconnectButton.Enabled = false;
        }

        private void portUpDown_ValueChanged(object sender, EventArgs e) {
            mConfig.Port = Decimal.ToInt32(portUpDown.Value);
        }

        private void addressBox_TextChanged(object sender, EventArgs e) {
            mConfig.Address = addressBox.Text;
        }

        private void startButton_Click(object sender, EventArgs e) {
            mPlugin.Connect();
            connectButton.Enabled = false;
            disconnectButton.Enabled = true;
        }

        private void disconnectButton_Click(object sender, EventArgs e) {
            mPlugin.Disconnect();
            connectButton.Enabled = true;
            disconnectButton.Enabled = false;
        }

        private void nameBox_TextChanged(object sender, EventArgs e) {
            mConfig.ClientName = nameBox.Text;
        }
    }
}
