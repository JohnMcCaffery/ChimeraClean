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
using Chimera.Plugins;
using Chimera.Interfaces;

namespace Chimera.GUI.Controls.Plugins {
    public partial class AxisBasedDeltaPanel : UserControl {
        private static readonly int PADDING = 3;
        private AxisBasedDelta mPlugin;

        public AxisBasedDelta Plugin {
            get { return mPlugin; }
            set {
                mPlugin = value;
                mPlugin.AxisAdded += new Action<Interfaces.IAxis>(mInput_AxisAdded);

                scalePanel.Value = mPlugin.Scale;
                rotXMovePanel.Value = mPlugin.RotXMove;

                foreach (var axis in mPlugin.Axes)
                    mInput_AxisAdded(axis);
            }
        }

        public AxisBasedDeltaPanel() {
            InitializeComponent();
        }

        public AxisBasedDeltaPanel(AxisBasedDelta plugin)
            : this() {

            Plugin = plugin;
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

        private void scalePanel_ValueChanged(float obj) {
            mPlugin.Scale = scalePanel.Value;
        }

        private void rotXMove_ValueChanged(float obj) {
            mPlugin.RotXMove = rotXMovePanel.Value;
        }
    }
}
