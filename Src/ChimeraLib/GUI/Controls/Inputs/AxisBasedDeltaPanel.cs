using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Inputs;
using Chimera.Interfaces;

namespace Chimera.GUI.Controls.Inputs {
    public partial class AxisBasedDeltaPanel : UserControl {
        private static readonly int PADDING = 3;
        private AxisBasedDelta mInput;

        public AxisBasedDeltaPanel() {
            InitializeComponent();
        }

        public AxisBasedDeltaPanel(AxisBasedDelta input)
            : this() {

            mInput = input;
            mInput.AxisAdded += new Action<Interfaces.IAxis>(mInput_AxisAdded);

            scalePanel.Value = input.Scale;

            foreach (var axis in mInput.Axes)
                mInput_AxisAdded(axis);
        }

        void mInput_AxisAdded(IAxis axis) {
            AxisPanel panel = new AxisPanel(axis);
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
            int y = PADDING;
            foreach (Control panel in axesBox.Controls) {
                panel.Location = new Point(x, y);
                y += panel.Height;
            }
        }

        private void scalePanel_ValueChanged(float obj) {
            mInput.Scale = scalePanel.Value;
        }
    }
}
