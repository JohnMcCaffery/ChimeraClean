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
using Chimera.Flythrough;

namespace ChimeraGUILib.Controls.FlythroughEventPanels {
    public partial class BlankPanel : UserControl {
        private BlankEvent mEvent;

        public BlankPanel() {
            InitializeComponent();
        }

        public BlankPanel(BlankEvent evt)
            : this() {
            mEvent = evt;

            if (mEvent.Length == 0f)
                mEvent.Length = (int)lengthValue.Value;
            else
                lengthValue.Value = mEvent.Length;

            lengthValue.ValueChanged += (source, args) => mEvent.Length = (int)lengthValue.Value;
            evt.OnStep += (source, args) => {
                Invoke(new Action(() => {
                    progressBar.Maximum = evt.TotalSteps;
                    progressBar.Value = Math.Min(evt.CurrentStep, progressBar.Maximum);
                }));
            };
        }
    }
}
