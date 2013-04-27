using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Interfaces;

namespace Chimera.GUI.Controls.Inputs {
    public partial class AxisPanel : UserControl {
        private IAxis mAxis;
        private int mExpandedSize;

        public AxisPanel() {
            InitializeComponent();

            foreach (var binding in Enum.GetValues(typeof(AxisBinding)))
                bindingDropdown.Items.Add(binding);
        }

        public AxisPanel(IAxis axis)
            : this() {

            mAxis = axis;

            UserControl control = mAxis.ControlPanel;

            mExpandedSize = MinimumSize.Height + control.Height;
            if (editBox.Checked)
                Height = mExpandedSize;

            control.Visible = editBox.Checked;
            control.Width = configPanel.Width;
            control.Dock = DockStyle.Fill;
            //control.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            configPanel.Controls.Add(control);
        }

        private void editBox_CheckedChanged(object sender, EventArgs e) {
            Height = editBox.Checked ? mExpandedSize : MinimumSize.Height;
            mAxis.ControlPanel.Visible = editBox.Checked;
        }

        private void bindingDropdown_SelectedIndexChanged(object sender, EventArgs e) {
            mAxis.Binding = (AxisBinding)bindingDropdown.SelectedItem;
        }
    }
}
