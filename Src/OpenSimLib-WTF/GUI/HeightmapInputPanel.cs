using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chimera.OpenSim.GUI {
    public partial class HeightmapInputPanel : UserControl {
        private HeightmapInput mInput;
        private HeightmapConfig mConfig;

        public HeightmapInputPanel() {
            InitializeComponent();
        }

        public HeightmapInputPanel(HeightmapInput input)
            : this() {
            Init(input);
        }

        public void Init(HeightmapInput input) {
            mInput = input;
            mConfig = (HeightmapConfig) mInput.Config;
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
                Invoke(a);
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
