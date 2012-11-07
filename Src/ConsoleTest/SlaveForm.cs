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
            Text = slave.Name;
            if (slave.ProxyRunning)
                Text += ": " + slave.ProxyPort;
            slave.OnProxyStarted += (source, args) => {
                Invoke(new Action(() => Text = Name + ": " + slave.ProxyPort));
            };
            slave.OnUpdateSentToClient += (position, lookAt) => {
                Invoke(new Action(() => {
                    masterPosition.Value = slave.MasterPosition;
                    masterRotation.LookAtVector = slave.MasterRotation.LookAtVector;
                    finalPosition.Value = position;
                    finalRotation.LookAtVector = lookAt;
                }));
            };
        }

        private void rotationOffsetPanel_OnChange(object sender, EventArgs e) {
            slave.OffsetRotation.Quaternion = rotationOffsetPanel.Rotation;
        }

        private void positionOffset_OnChange(object sender, EventArgs e) {
            slave.OffsetPosition = positionOffsetPanel.Value;
        }

        private void rawRotation_OnChange(object sender, EventArgs e) {
            slave.MasterRotation.Quaternion = masterRotation.Rotation;
        }

        private void rawPosition_OnChange(object sender, EventArgs e) {
            slave.MasterPosition = masterPosition.Value;
        }

        private void SlaveForm_FormClosing(object sender, FormClosingEventArgs e) {
            slave.Stop();
        }
    }
}
