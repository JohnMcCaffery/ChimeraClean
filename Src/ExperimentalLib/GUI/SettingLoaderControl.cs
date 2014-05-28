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

            statusLabel.Text = mConfig.SettingsLoaderEnabled ? "Loaded settings from: " + mConfig.RunInfo : "Settings loading disabled.";
        }
    }
}
