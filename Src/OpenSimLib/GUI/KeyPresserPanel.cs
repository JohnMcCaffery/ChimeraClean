using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Config;
using Chimera.OpenSim.Plugins;

namespace Chimera.OpenSim.GUI.Controls.Plugins {
    public partial class KeyPresserPanel : UserControl {
        private KeyPresser mPlugin;
        private CoreConfig mConfig;

        public KeyPresserPanel() {
            InitializeComponent();
        }

        public KeyPresserPanel(KeyPresser plugin) {
            InitializeComponent();

            mPlugin = plugin;
            mConfig = plugin.Config as CoreConfig;

            mPlugin.Started += () => {
                if (InvokeRequired)
                    Invoke(new Action(() => startButton.Text = "Stop"));
                else
                    startButton.Text = "Start";
            };

            intervalS.Value = new Decimal(mConfig.IntervalMS / 1000);
            key.Text = mConfig.Key;
        }

        private void startButton_Click(object sender, EventArgs e) {
            mPlugin.Running = !mPlugin.Running;
        }

        private void intervalS_ValueChanged(object sender, EventArgs e) {
            mConfig.IntervalMS = Decimal.ToDouble(intervalS.Value) * 1000.0;
        }

        private void shutdownM_ValueChanged(object sender, EventArgs e) {
            mConfig.ShutdownM = Decimal.ToInt32(shutdownM.Value);
        }
    }
}
