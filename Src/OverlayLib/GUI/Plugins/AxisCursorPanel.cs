using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Joystick.Plugins;
using Chimera.Interfaces;
using Chimera.GUI.Controls.Plugins;

namespace Chimera.Kinect.GUI {
    public partial class AxisCursorPanel : UserControl {
        private static readonly int PADDING = 3;
        private AxisCursorPlugin mPlugin;

        public AxisCursorPanel() {
            InitializeComponent();
        }

        public AxisCursorPanel(AxisCursorPlugin plugin)
            : this() {
            mPlugin = plugin;
        }

        void mInput_AxisAdded(IAxis axis) {
            AxisPanel panel = new AxisPanel(axis, AxisBinding.MouseX, AxisBinding.MouseY);
            panel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            panel.Width = axesBox.Width - PADDING * 2;
            axesBox.Controls.Add(panel);
            panel.SizeChanged += new EventHandler(panel_SizeChanged);

            RepositionPanels();
        }

        void panel_SizeChanged(object sender, EventArgs e) {
            RepositionPanels();
        }

        private void RepositionPanels() {
            int x = PADDING;
            int y = PADDING * 5;
            foreach (Control panel in axesBox.Controls) {
                panel.Location = new Point(x, y);
                y += panel.Height;
            }

            //axesBox.Height = y + PADDING;
        }
    }
}
