using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilLib;
using System.Threading;

namespace ConsoleTest {
    public partial class SlaveForm : Form {
        private CameraSlave slave;

        public SlaveForm() : this (new CameraSlave()) { }

        public SlaveForm(CameraSlave slave) {
            this.slave = slave;
            InitializeComponent();
            proxyPanel.Proxy = slave;
            Text = slave.Name;
            debugPanel.Source = slave.Name;
            if (slave.ProxyRunning)
                Text += ": " + slave.ProxyConfig.ProxyPort;

            addressBox.Text = slave.ProxyConfig.MasterAddress;
            portBox.Text = slave.ProxyConfig.MasterPort.ToString();
            nameBox.Text = slave.Name;

            if (slave.ConnectedToMaster) {
                addressBox.Text = slave.ProxyConfig.MasterAddress;
                addressBox.Enabled = false;

                portBox.Text = slave.ProxyConfig.MasterPort.ToString();
                portBox.Enabled = false;

                nameBox.Text = slave.Name;
                nameBox.Enabled = false;

                connectButton.Text = "Disconnect from Master";
                statusLabel.Text = "Connected to " + slave.ProxyConfig.MasterAddress + ":" + slave.ProxyConfig.MasterPort + " as " + slave.Name;
            } else 
                statusLabel.Text = "Not Connected";

            slave.OnProxyStarted += (source, args) => {
                Invoke(new Action(() => Text = slave.Name + ": " + slave.ProxyConfig.ProxyPort));
            };

            slave.OnConnectedToMaster += (source, args) => {
                Action a = () => {
                    addressBox.Text = slave.ProxyConfig.MasterAddress;
                    addressBox.Enabled = false;

                    portBox.Text = slave.ProxyConfig.MasterPort.ToString();
                    portBox.Enabled = false;

                    nameBox.Text = slave.Name;
                    nameBox.Enabled = false;

                    connectButton.Text = "Disconnect from Master";
                    statusLabel.Text = "Connected to " + slave.MasterAddress + ":" + slave.ProxyConfig.MasterPort + " as " + slave.Name;
                };
                if (InvokeRequired)
                    Invoke(a);
                else
                    a();
            };

            slave.OnUnableToConnectToMaster += (source, args) => {
                if (InvokeRequired)
                    Invoke(new Action(() => statusLabel.Text = source.ToString()));
                else
                    statusLabel.Text = source.ToString();
            };

            slave.OnProxyStarted += (source, args) => {
                Invoke(new Action(() => Text = slave.Name + ": " + slave.ProxyConfig.ProxyPort));
            };


            slave.OnDisconnectedFromMaster += () => {
                Action a = () => {
                    addressBox.Enabled = true;
                    portBox.Enabled = true;
                    nameBox.Enabled = true;

                    connectButton.Text = "Connect To Master";
                    statusLabel.Text = "Not Connected";
                };
                if (InvokeRequired)
                    Invoke(a);
                else
                    a();
            };
            slave.OnUpdateSentToClient += (position, lookAt) => {
                BeginInvoke(new Action(() => {
                    masterPosition.Value = slave.Position;
                    masterRotation.LookAtVector = slave.Rotation.LookAtVector;
                    receivedLabel.Text = slave.PacketsReceived.ToString();
                    injectedLabel.Text = slave.PacketsInjected.ToString();
                }));
            };
        }

        private void rotation_OnChange(object sender, EventArgs e) {
            slave.Rotation.Quaternion = masterRotation.Rotation;
        }

        private void position_OnChange(object sender, EventArgs e) {
            slave.Position = masterPosition.Value;
        }

        private void SlaveForm_FormClosing(object sender, FormClosingEventArgs e) {
            slave.Stop();
        }

        private void controlCamera_CheckedChanged(object sender, EventArgs e) {
            slave.ControlCamera = controlCamera.Checked;
        }

        private void masterBox_TextChanged(object sender, EventArgs e) {
            slave.ProxyConfig.MasterAddress = addressBox.Text;
        }

        private void portBox_TextChanged(object sender, EventArgs e) {
            slave.ProxyConfig.MasterPort = Int32.Parse(portBox.Text);
        }

        private void connectButton_Click(object sender, EventArgs e) {
            if (connectButton.Text.Equals("Connect To Master")) {
                slave.Name = nameBox.Text;
                debugPanel.Source = slave.Name;
                new Thread(() => slave.Connect()).Start();
            } else {
                slave.Disconnect();
                addressBox.Enabled = true;
                portBox.Enabled = true;
                nameBox.Enabled = true;
                connectButton.Text = "Connect To Master";
                statusLabel.Text = "Not Connected";
            }
        }
    }
}
