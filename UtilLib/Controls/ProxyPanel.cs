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

namespace UtilLib {
    public partial class ProxyPanel : UserControl {
        private Proxy proxy;
        private Process client;
        private bool loggedIn;

        public Proxy Proxy { get { return proxy; } }
        public bool HasStarted { get { return proxy != null; } }

        public string Port {
            get { return portBox.Text; }
            set { portBox.Text = value; }
        }
        public string ListenIP {
            get { return listenIPBox.Text; }
            set { listenIPBox.Text = value; }
        }
        public string LoginURI {
            get { return loginURIBox.Text; }
            set { loginURIBox.Text = value; }
        }

        public event EventHandler LoggedIn;
        public event EventHandler OnStarted;

        public ProxyPanel() {
            InitializeComponent();
        }

        private void connectButton_Click(object sender, EventArgs e) {
            if (proxyStartButton.Text.Equals("Bind SlaveProxy")) {
                if (proxy != null)
                    proxy.Stop();
                string file = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

                string portArg = "--proxy-login-masterPort=" + portBox.Text;
                string listenIPArg = "--proxy-proxyAddress-facing-masterAddress=" + listenIPBox.Text;
                string loginURIArg = "--proxy-remote-login-uri=" + loginURIBox.Text;
                string[] args = { portArg, listenIPArg, loginURIArg };
                ProxyConfig config = new ProxyConfig("Routing God", "jm726@st-andrews.ac.uk", args);
                proxy = new Proxy(config);
                proxy.AddLoginResponseDelegate(response => {
                    loggedIn = true;
                    if (LoggedIn != null)
                        LoggedIn(proxy, null);
                    return response;
                });

                proxy.Start();

                if (OnStarted != null)
                    OnStarted(proxy, null);

                proxyStartButton.Text = "Disconnect SlaveProxy";
                proxyStatusLabel.Text = "Started";

                portBox.Enabled = false;
                listenIPBox.Enabled = false;
                loginURIBox.Enabled = false;
            } else if (proxy != null) {
                proxy.Stop();
                proxy = null;

                proxyStartButton.Text = "Bind SlaveProxy";
                proxyStatusLabel.Text = "Stopped";

                portBox.Enabled = true;
                listenIPBox.Enabled = true;
                loginURIBox.Enabled = true;
            }
        }

        private void updateTimer_Tick(object sender, EventArgs e) {
            if (loggedIn) {
                proxyStatusLabel.Text = "Started + Client Logged In";
            } 
        }

        private void clientStartButton_Click(object sender, EventArgs e) {
            if (clientStartButton.Text.Equals("Bind Client")) {
                if (proxy == null)
                    proxyStartButton.PerformClick();
                client = new Process();
                client.StartInfo.FileName = targetBox.Text;
                if (useLoginURICheck.Checked)
                    client.StartInfo.Arguments = "--proxyLoginURI http://" + listenIPBox.Text + ":" + portBox.Text;
                else
                    client.StartInfo.Arguments = " --grid " + portBox.Text;
                client.StartInfo.Arguments += " --login " + firstNameBox.Text + " " + lastNameBox.Text + " " + passwordBox.Text;
                client.Start();

                clientStatusLabel.Text = "Started";
                clientStartButton.Text = "Disconnect Client";
                firstNameBox.Enabled = false;
                lastNameBox.Enabled = false;
                passwordBox.Enabled = false;
                targetBox.Enabled = false;
            } else {
                clientStatusLabel.Text = "Stopped";
                clientStartButton.Text = "Bind Client";
                firstNameBox.Enabled = true;
                lastNameBox.Enabled = true;
                passwordBox.Enabled = true;
                targetBox.Enabled = true;
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

    }
}
