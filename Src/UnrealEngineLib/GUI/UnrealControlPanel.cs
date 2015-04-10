using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UnrealEngineLib.GUI {
    public partial class UnrealControlPanel : UserControl {
        private UnrealController mController;

        public UnrealControlPanel() {
            InitializeComponent();
        }

        public UnrealControlPanel(UnrealController controller) : this() {
            mController = controller;
            unrealExeBox.Text = mController.Config.UnrealExecutable;
            listenPortUpDown.Value = mController.Config.ListenPort;
        }

        private void unrealLaunchButton_Click(object sender, EventArgs e) {
            mController.Config.UnrealExecutable = unrealExeBox.Text;
            mController.Launch();
        }

        private void startUDPButton_Click(object sender, EventArgs e) {
            mController.StartServer();
        }

        private void sendButton_Click(object sender, EventArgs e) {
            mController.SendString(sendBox.Text);
        }

        private void sendBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == '\r')
                sendButton_Click(this, e);
        }
    }
}
