using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Touchscreen.GUI {
    public partial class TouchscreenPluginPanel : UserControl {
        private TouchscreenPlugin mPlugin;

        public TouchscreenPluginPanel() {
            InitializeComponent();
        }

        public TouchscreenPluginPanel(TouchscreenPlugin plugin) {
            mPlugin = plugin;
            axisBasedDeltaPanel.Plugin = plugin;

            foreach (var pos in Enum.GetValues(typeof(SinglePos))) {
                singleAxisBox.Items.Add(pos);
                if ((SinglePos) pos == mPlugin.SinglePos)
                    singleAxisBox.SelectedItem = pos;
            }
        }
    }
}
