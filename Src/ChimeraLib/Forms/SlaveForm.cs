/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
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
        private bool updating;

        public SlaveForm() : this (new CameraSlave()) { }

        public SlaveForm(CameraSlave slave) {
            this.slave = slave;
            InitializeComponent();

            proxyPanel.Proxy = slave;
            screenWindowPanel.Window = slave.Window;

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
                    updating = true;
                    positionPanel.Value = slave.Position;
                    rotationPanel.LookAtVector = slave.Rotation.LookAtVector;
                    receivedLabel.Text = slave.PacketsReceived.ToString();
                    injectedLabel.Text = slave.PacketsInjected.ToString();
                    updating = false;
                }));
            };
        }

        private void rotation_OnChange(object sender, EventArgs e) {
            if (!updating) {
                slave.Rotation.Pitch = rotationPanel.Pitch;
                slave.Rotation.Yaw = rotationPanel.Yaw;
            }
        }

        private void position_OnChange(object sender, EventArgs e) {
            if (!updating)
                slave.Position = positionPanel.Value;
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
