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
using Chimera.Util;
using OpenMetaverse;

namespace Chimera.Flythrough.GUI {
    public partial class RotateToPanel : UserControl {
        private RotateToEvent mEvent;

        public RotateToPanel() {
            InitializeComponent();
        }

        public RotateToPanel(RotateToEvent evt)
            : this() {
            mEvent = evt;

            rotationPanel.Value = mEvent.Target;
            lengthValue.Value = mEvent.Length;

            lengthValue.ValueChanged += (source, args) => mEvent.Length = (int)lengthValue.Value;
            rotationPanel.OnChange += (sender, args) => {
                mEvent.Target = rotationPanel.Value;
                mEvent.Container.Coordinator.Update(mEvent.Container.Coordinator.Position, Vector3.Zero, rotationPanel.Value, Rotation.Zero);
            };
            evt.TimeChange += (source, args) => {
                Invoke(new Action(() => {
                    progressBar.Maximum = evt.Length;
                    progressBar.Value = evt.Time;
                }));
            };
        }

        private void rotateToTakeCurrentButton_Click(object sender, EventArgs e) {
            rotationPanel.Value = new Rotation(mEvent.Container.Coordinator.Orientation);
        }
    }
}
