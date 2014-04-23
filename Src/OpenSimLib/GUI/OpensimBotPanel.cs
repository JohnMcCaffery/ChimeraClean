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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.OpenSim.Interfaces;

namespace Chimera.OpenSim.GUI {
    public partial class OpensimBotPanel : UserControl {
        private OpensimBotPlugin mInput;
        private IOpensimBotConfig mConfig;

        public OpensimBotPanel() {
            InitializeComponent();
        }

        public OpensimBotPanel(OpensimBotPlugin input)
            : this() {
            Init(input);
        }

        public void Init(OpensimBotPlugin input) {
            mInput = input;
            mConfig = (IOpensimBotConfig) mInput.Config;
            loginErrorLabel.Text = mInput.LoginFailMessage != null ? mInput.LoginFailMessage : "";
            mInput_LoggedInChanged(mInput.LoggedIn);

            firstNameBox.Text = mConfig.FirstName;
            lastNameBox.Text = mConfig.LastName;
            passwordBox.Text = mConfig.Password;
            regionBox.Text = mConfig.StartIsland;
            startPositionPanel.Value = mConfig.StartLocation;

            mInput.LoggedInChanged += mInput_LoggedInChanged;
            mInput.LoginFailed += mInput_LoginFailed;
            Disposed += new EventHandler(HeightmapInputPanel_Disposed);
        }

        private void HeightmapInputPanel_Disposed(object sender, EventArgs e) {
            mInput.LoggedInChanged -= mInput_LoggedInChanged;
            mInput.LoginFailed -= mInput_LoginFailed;
        }

        private void mInput_LoginFailed() {
            loginErrorLabel.Text = mInput.LoginFailMessage;
        }

        private void mInput_LoggedInChanged(bool status) {
            Action a = () => {
                if (status) {
                    loginButton.Text = "Logout";
                    firstNameBox.Enabled = false;
                    lastNameBox.Enabled = false;
                    passwordBox.Enabled = false;
                    teleportButton.Enabled = true;
                } else {
                    loginButton.Text = "Login";
                    firstNameBox.Enabled = true;
                    lastNameBox.Enabled = true;
                    passwordBox.Enabled = true;
                    teleportButton.Enabled = false;
                }
            };
            if (InvokeRequired)
                BeginInvoke(a);
            else
                a();
        }

        private void regionBox_TextChanged(object sender, EventArgs e) {
            mConfig.StartIsland = regionBox.Text;
        }

        private void firstNameBox_TextChanged(object sender, EventArgs e) {
            mConfig.FirstName = firstNameBox.Text;
        }

        private void lastNameBox_TextChanged(object sender, EventArgs e) {
            mConfig.LastName = lastNameBox.Text;
        }

        private void passwordBox_TextChanged(object sender, EventArgs e) {
            mConfig.Password = passwordBox.Text;
        }

        private void startPositionPanel_ValueChanged(object sender, EventArgs e) {
            mConfig.StartLocation = startPositionPanel.Value;
        }

        private void loginButton_Click(object sender, EventArgs e) {
            if (loginButton.Text == "Login")
                mInput.Login();
            else
                mInput.Logout();
        }

        private void teleportButton_Click(object sender, EventArgs e) {

        }
    }
}
