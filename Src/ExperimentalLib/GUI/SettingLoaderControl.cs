using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Experimental.Plugins;

namespace Chimera.Experimental.GUI {
    public partial class SettingLoaderControl : UserControl {
        private SettingLoaderPlugin mPlugin;
        private ExperimentalConfig mConfig;
        private float mValue;

        public SettingLoaderControl() {
            InitializeComponent();
        }

        public SettingLoaderControl(SettingLoaderPlugin plugin) : this() {
            mPlugin = plugin;
            mConfig = plugin.Config as ExperimentalConfig;
            mValue = mConfig.Value;

            statusLabel.Text = mConfig.Setting + " will be set to " + mConfig.Value + " on login. Increment: " + mConfig.Increment + ".";

            mPlugin.Set += () =>
                Invoke(new Action(() => {
                    statusLabel.Text = mConfig.Setting + " set to " + mValue + ". Next iteration: " + mConfig.Value + " (increment " + mConfig.Increment + ").";
                    mValue = mConfig.Value;
                }));
        }
    }
}
