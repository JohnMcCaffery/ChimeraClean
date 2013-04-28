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
using Chimera.Interfaces;

namespace Chimera.GUI.Controls.Plugins {
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

            bindingDropdown.SelectedItem = axis.Binding;
            mainGroup.Text = axis.Name;

            UserControl control = mAxis.ControlPanel;

            mExpandedSize = MinimumSize.Height + control.Height;
            Height = editBox.Checked ? mExpandedSize : MinimumSize.Height;

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
