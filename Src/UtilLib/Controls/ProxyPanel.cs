/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Armadillo SlaveProxy.

Routing Project is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Routing Project is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Routing Project.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GridProxy;
using System.Diagnostics;
using System.Runtime.InteropServices;
using OpenMetaverse;

namespace UtilLib {
    public partial class ProxyPanel : UserControl {
        private ProxyManager proxy;

        public ProxyManager Proxy { 
            get { return proxy; }
            set { 
                proxy = value;
                if (proxy == null)
                    return;

                proxy.OnClientLoggedIn += (source, args) => {
                    Invoke(new Action(() => proxyStatusLabel.Text = "Started + Client Logged In"));
                };
                proxy.OnProxyStarted += (source, args) => {
                    Invoke(new Action(() => proxyStatusLabel.Text = "Started"));
                };

                Action init = () => {
                    portBox.Text = proxy.ProxyConfig.ProxyPort.ToString();
                    loginURIBox.Text = proxy.ProxyConfig.ProxyLoginURI;
                    targetBox.Text = proxy.ProxyConfig.ClientExecutable;
                    firstNameBox.Text = proxy.ProxyConfig.LoginFirstName;
                    lastNameBox.Text = proxy.ProxyConfig.LoginLastName;
                    passwordBox.Text = proxy.ProxyConfig.LoginPassword;
                    gridBox.Text = proxy.ProxyConfig.LoginGrid;
                    gridBox.Enabled = proxy.ProxyConfig.UseGrid;
                    gridCheck.Checked = proxy.ProxyConfig.UseGrid;

                    if (proxy.ProxyRunning) {
                        proxyStartButton.Text = "Disconnect SlaveProxy";
                        proxyStatusLabel.Text = "Started";

                        portBox.Enabled = false;
                        loginURIBox.Enabled = false;
                    }

                    if (proxy.ClientLoggedIn) {
                        clientStatusLabel.Text = "Started";
                        clientStartButton.Text = "Disconnect Client";
                        proxyStatusLabel.Text = "Started + Client Logged In";
                        firstNameBox.Enabled = false;
                        lastNameBox.Enabled = false;
                        passwordBox.Enabled = false;
                        targetBox.Enabled = false;
                    }
                };
                if (InvokeRequired)
                    Invoke(init);
                else
                    init();
            }
        }
        public bool HasStarted { get { return proxy != null; } }

        public string Port {
            get { return portBox.Text; }
            set { portBox.Text = value; }
        }
        public string LoginURI {
            get { return loginURIBox.Text; }
            set { loginURIBox.Text = value; }
        }

        public ProxyPanel() {
            InitializeComponent();
        }

        public ProxyPanel(ProxyManager proxy)
            : this() {
            Proxy = proxy;
        }

        private void connectButton_Click(object sender, EventArgs e) {
            if (proxy == null)
                return;
            if (proxyStartButton.Text.Equals("Start Proxy")) {
                if (proxy.ProxyRunning)
                    proxy.StopProxy();
                string file = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

                for (int i = 0; i < 5 && !proxy.StartProxy(); i++, proxy.ProxyConfig.ProxyPort++) {
                    int port = proxy.ProxyConfig.ProxyPort;
                    Logger.Log("Unable to start proxy on port " + (port - 1) + ", trying " + port + ".", Helpers.LogLevel.Info);
                }
                if (proxy.ProxyRunning) {
                    proxyStartButton.Text = "Stop Proxy";
                    proxyStatusLabel.Text = "Started";

                    portBox.Enabled = false;
                    loginURIBox.Enabled = false;
                } else {
                    Logger.Log("Unable to start proxy.", Helpers.LogLevel.Info);
                    proxyStatusLabel.Text = "Unable to start";
                }
            } else if (proxy != null) {
                proxy.StopProxy();

                proxyStartButton.Text = "Start Proxy";
                proxyStatusLabel.Text = "Stopped";

                portBox.Enabled = true;
                loginURIBox.Enabled = true;
            }
        }

        private void clientStartButton_Click(object sender, EventArgs e) {
            if (clientStartButton.Text.Equals("Launch Client")) {
                if (proxy != null && !proxy.ProxyRunning)
                    proxyStartButton.PerformClick();

                proxy.StartClient();

                clientStatusLabel.Text = "Started";
                clientStartButton.Text = "Disconnect Client";
                firstNameBox.Enabled = false;
                lastNameBox.Enabled = false;
                passwordBox.Enabled = false;
                targetBox.Enabled = false;
                gridCheck.Enabled = false;
                gridBox.Enabled = false;
            } else {
                clientStatusLabel.Text = "Stopped";
                clientStartButton.Text = "Launch Client";
                firstNameBox.Enabled = true;
                lastNameBox.Enabled = true;
                passwordBox.Enabled = true;
                targetBox.Enabled = true;
                gridCheck.Enabled = true;
                gridBox.Enabled = true;
                //SendMEssage(proxyAddress.Id, 
            }
        }

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMEssage(int hWnd, int Msg, int wParam, int lParam);

        public string FirstName {
            get { return firstNameBox.Text; }
            set { firstNameBox.Text = value; }
        }


        public string LastName {
            get { return lastNameBox.Text; }
            set { lastNameBox.Text = value; }
        }

        public string Password {
            get { return passwordBox.Text; }
            set { passwordBox.Text = value; }
        }

        private void portBox_TextChanged(object sender, EventArgs e) {
            if (gridBox.Text.Equals("")) {
                gridBox.Text = portBox.Text;
                proxy.ProxyConfig.LoginGrid = portBox.Text;
            }
            proxy.ProxyConfig.ProxyPort = Int32.Parse(portBox.Text);
        }

        private void gridCheck_CheckedChanged(object sender, EventArgs e) {
            gridBox.Enabled = gridCheck.Checked;
            if (gridCheck.Checked && gridBox.Text.Equals(""))
                gridBox.Text = portBox.Text;
            else if (gridCheck.Checked)
                proxy.ProxyConfig.LoginGrid = gridBox.Text;
        }

        private void loginURIBox_TextChanged(object sender, EventArgs e) {
            proxy.ProxyConfig.ProxyLoginURI = loginURIBox.Text;
        }

        private void firstNameBox_TextChanged(object sender, EventArgs e) {
            proxy.ProxyConfig.LoginFirstName = firstNameBox.Text;
        }

        private void lastNameBox_TextChanged(object sender, EventArgs e) {
            proxy.ProxyConfig.LoginLastName = lastNameBox.Text;
        }

        private void passwordBox_TextChanged(object sender, EventArgs e) {
            proxy.ProxyConfig.LoginPassword = passwordBox.Text;
        }

        private void targetBox_TextChanged(object sender, EventArgs e) {
            proxy.ProxyConfig.ClientExecutable = targetBox.Text;
        }

        private void gridBox_TextChanged(object sender, EventArgs e) {
            proxy.ProxyConfig.LoginGrid = gridBox.Text;
        }

    }
}
