/**************************************************************************
Chimera (c) 2012 John McCaffery 

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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GridProxy;
using System.Diagnostics;
using System.Runtime.InteropServices;
using OpenMetaverse;

namespace Chimera.OpenSim.GUI {
    public partial class OutputPanel : UserControl {
        private OpenSimController mController;
        private ViewerConfig mConfig;

        public OpenSimController Controller { 
            get { return mController; }
            set { 
                mController = value;
                if (mController == null)
                    return;

                mController.ProxyController.ProxyStarted += ProxyStarted;
                mController.ProxyController.OnClientLoggedIn += (source, args) => ClientLoggedIn();
                mController.ViewerController.Exited += () => ViewerExit();

                mConfig = mController.Config as ViewerConfig;

                Invoke(() => {
                    portBox.Text = mConfig.ProxyPort.ToString();
                    loginURIBox.Text = mConfig.ProxyLoginURI;
                    viewerExeBox.Text = mConfig.ViewerExecutable;
                    workingDirectoryBox.Text = mConfig.ViewerWorkingDirectory;
                    firstNameBox.Text = mConfig.LoginFirstName;
                    lastNameBox.Text = mConfig.LoginLastName;
                    passwordBox.Text = mConfig.LoginPassword;
                    gridBox.Text = mConfig.LoginGrid;
                    gridBox.Enabled = mConfig.UseGrid;
                    gridCheck.Checked = mConfig.UseGrid;
                    autoRestartBox.Checked = mController.AutoRestart;
                    fullscreenCheck.Checked = mController.Fullscreen;
                    controlCameraCheck.Checked = mController.ControlCamera;
                    controlFrustumCheck.Checked = mController.ControlFrustum;
                    backwardsCompatibleLabel.Text = mConfig.BackwardsCompatible ? "Backwards Compatible" : "";
                });

                if (mController.ProxyController.Started)
                    ProxyStarted();

                if (mController.ViewerController.Started)
                    ClientLoggedIn();
            }
        }

        private void ProxyStarted() {
            Invoke(() => {
                proxyStartButton.Text = "Disconnect SlaveProxy";
                proxyStatusLabel.Text = "Started";

                portBox.Enabled = false;
                loginURIBox.Enabled = false;
            });
        }

        private void ClientLoggedIn() {
            Invoke(() => {
                clientStatusLabel.Text = "Started";
                viewerLaunchButton.Text = "Stop Viewer";
                proxyStatusLabel.Text = "Started + Client Logged In";
                firstNameBox.Enabled = false;
                lastNameBox.Enabled = false;
                passwordBox.Enabled = false;
                viewerExeBox.Enabled = false;
                workingDirectoryBox.Enabled = false;
                gridBox.Enabled = false;
                gridCheck.Enabled = false;
            });
        }

        private void ViewerExit() {
            Invoke(() => {
                clientStatusLabel.Text = "Exited";
                viewerLaunchButton.Text = "Launch Viewer";
                proxyStatusLabel.Text = "Started";
                firstNameBox.Enabled = true;
                lastNameBox.Enabled = true;
                passwordBox.Enabled = true;
                viewerExeBox.Enabled = true;
                workingDirectoryBox.Enabled = true;
                gridBox.Enabled = true;
                gridCheck.Enabled = true;
            });
        }

        public void Invoke(Action a) {
            if (InvokeRequired)
                base.BeginInvoke(a);
            else
                a();
        }

        public bool HasStarted { get { return mController != null; } }

        public string Port {
            get { return portBox.Text; }
            set { portBox.Text = value; }
        }
        public string LoginURI {
            get { return loginURIBox.Text; }
            set { loginURIBox.Text = value; }
        }

        public OutputPanel() {
            InitializeComponent();
        }

        public OutputPanel(OpenSimController proxy)
            : this() {
            Controller = proxy;
        }

        private void connectButton_Click(object sender, EventArgs e) {
            if (mController == null)
                return;
            if (proxyStartButton.Text.Equals("Begin Proxy")) {
                if (mController.ProxyController.Started)
                    mController.Stop();
                string file = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

                for (int i = 0; i < 5 && !mController.StartProxy(); i++, mConfig.ProxyPort++) {
                    int port = mConfig.ProxyPort;
                    Logger.Log("Unable to start proxy on port " + (port - 1) + ", trying " + port + ".", Helpers.LogLevel.Info);
                }
                if (!mController.ProxyController.Started) {
                    Logger.Log("Unable to start proxy.", Helpers.LogLevel.Info);
                    proxyStatusLabel.Text = "Unable to start";
                }
            } else if (mController != null) {
                mController.Stop();

                proxyStartButton.Text = "Begin Proxy";
                proxyStatusLabel.Text = "Stopped";

                portBox.Enabled = true;
                loginURIBox.Enabled = true;
            }
        }

        private void viewerLaunchButton_Click(object sender, EventArgs e) {
            if (viewerLaunchButton.Text.Equals("Launch Viewer")) {
                if (mController != null && !mController.ProxyController.Started)
                    mController.StartProxy();

                mController.Launch();
            } else {
                /*
                clientStatusLabel.Text = "Stopped";
                viewerLaunchButton.Text = "Launch Viewer";
                firstNameBox.Active = true;
                lastNameBox.Active = true;
                passwordBox.Active = true;
                viewerExeBox.Active = true;
                workingDirectoryBox.Active = true;
                gridCheck.Active = true;
                gridBox.Active = proxy.ProxyConfig.UseGrid;
                */
                if (mController != null)
                    mController.ProxyController.Stop();
                //SendMEssage(proxyAddress.Id, 
            }
        }

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
                if (mController != null)
                    mConfig.LoginGrid = portBox.Text;
            }
            if (mController != null)
                mConfig.ProxyPort = Int32.Parse(portBox.Text);
        }

        private void gridCheck_CheckedChanged(object sender, EventArgs e) {
            gridBox.Enabled = gridCheck.Checked;
            mConfig.UseGrid = gridCheck.Checked;
            if (gridCheck.Checked && gridBox.Text.Equals(""))
                gridBox.Text = portBox.Text;
            else if (gridCheck.Checked && mController != null)
                mConfig.LoginGrid = gridBox.Text;
        }

        private void loginURIBox_TextChanged(object sender, EventArgs e) {
            if (mController != null)
                mConfig.ProxyLoginURI = loginURIBox.Text;
        }

        private void firstNameBox_TextChanged(object sender, EventArgs e) {
            if (mController != null)
                mConfig.LoginFirstName = firstNameBox.Text;
        }

        private void lastNameBox_TextChanged(object sender, EventArgs e) {
            if (mController != null)
                mConfig.LoginLastName = lastNameBox.Text;
        }

        private void passwordBox_TextChanged(object sender, EventArgs e) {
            if (mController != null)
                mConfig.LoginPassword = passwordBox.Text;
        }

        private void targetBox_TextChanged(object sender, EventArgs e) {
            if (mController != null)
                mConfig.ViewerExecutable = viewerExeBox.Text;
        }

        private void gridBox_TextChanged(object sender, EventArgs e) {
            if (mController != null)
                mConfig.LoginGrid = gridBox.Text;
        }

        private void workingDirectory_TextChanged(object sender, EventArgs e) {
            if (mController != null)
                mConfig.ViewerWorkingDirectory = workingDirectoryBox.Text;
        }

        private void controlCamera_CheckedChanged(object sender, EventArgs e) {
            if (mController != null)
                mController.ControlCamera = controlCameraCheck.Checked;
        }

        private void autoRestartBox_CheckedChanged(object sender, EventArgs e) {
            if (mController != null)
                mController.AutoRestart = autoRestartBox.Checked;
        }

        private void hudButton_Click(object sender, EventArgs e) {
            if (mController != null)
                mController.ViewerController.ToggleHUD();
        }

        private void borderCheck_CheckedChanged(object sender, EventArgs e) {
            if (mController != null)
                mController.Fullscreen = fullscreenCheck.Checked;
        }

        private void controlFrustumCheck_CheckedChanged(object sender, EventArgs e) {
            if (mController != null)
                mController.ControlFrustum = controlFrustumCheck.Checked;
        }
    }
}
