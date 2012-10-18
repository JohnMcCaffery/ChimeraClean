using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CookComputing.XmlRpc;
using System.Runtime.Remoting;
using System.Threading;
using UtilLib;

namespace SlaveProxy {
    public partial class SlaveProxyForm : Form {
        private XMLRPCSlave slaveClient;
        private int packetCount = 0;

        public SlaveProxyForm() {
            InitializeComponent();

            slaveClient = new XMLRPCSlave();
            slaveClient.PacketReceived += (packet, ep) => {
                packetCount++;
                countLabel.Text = packetCount + "";
                return packet;
            };
        }

        private void connectButton_Click(object sender, EventArgs e) {
            slaveClient.Name = nameBox.Text;
            slaveClient.Connect(masterURIBox.Text);

        }

        private void checkTimer_Tick(object sender, EventArgs e) {
            pingedLabel.Text = slaveClient.Pinged ? "Pinged" : "";
            if (slaveClient.Pinged) {
                nameBox.Enabled = false;
                masterURIBox.Enabled = false;
                connectButton.Enabled = false;
            }
        }
    }
}
