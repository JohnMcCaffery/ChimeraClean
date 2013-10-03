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
using Chimera.Overlay.Triggers;

namespace Chimera.Overlay.GUI.Triggers {
    public partial class TriggerPanel : UserControl {
        private TriggerBase mTrigger;


        public TriggerPanel() {
            InitializeComponent();
        }

        public TriggerPanel(TriggerBase trigger)
            : this() {

            mTrigger = trigger;
        }

        private void forceTriggerButton_Click(object sender, EventArgs e) {
            mTrigger.Trigger();
        }
    }
}
