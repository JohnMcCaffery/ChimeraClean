using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilLib;

namespace ConsoleTest {
    public partial class SlaveForm : Form {
        private CameraSlave slave;

        public SlaveForm() : this (new CameraSlave()) { }

        public SlaveForm(CameraSlave slave) {
            this.slave = slave;
            InitializeComponent();
            Text = "Slave: " + slave.ProxyPort;
            slave.OnProxyStarted += (source, args) => {
                Invoke(new Action(() => Text = "Slave: " + slave.ProxyPort));
            };
        }

        private void rotationOffsetPanel_OnChange(object sender, EventArgs e) {
            slave.OffsetRotation.Rot = rotationOffsetPanel.Rotation;
        }

        private void positionOffset_OnChange(object sender, EventArgs e) {
            slave.OffsetPosition = positionOffsetPanel.Value;
        }

        private void SlaveForm_FormClosing(object sender, FormClosingEventArgs e) {
            slave.Stop();
        }
    }
}
