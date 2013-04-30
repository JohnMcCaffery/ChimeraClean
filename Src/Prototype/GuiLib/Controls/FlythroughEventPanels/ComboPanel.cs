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
using UtilLib;

namespace ChimeraGUILib.Controls.FlythroughEventPanels {
    public partial class ComboPanel : UserControl {
        private ComboEvent mEvent;
        private CameraMaster mMaster;

        public ComboPanel() {
            InitializeComponent();
        }

        public ComboPanel(ComboEvent evt, CameraMaster master)
            : this() {
            mEvent = evt;
            mMaster = master;

            sequence1Panel.Init(evt, true, mMaster);
            sequence2Panel.Init(evt, false, mMaster);
        }

        public void AddEvent(FlythroughEvent evt, UserControl panel, bool sequence1) {
            if (sequence1)
                sequence1Panel.AddEvent(evt, panel);
            else
                sequence2Panel.AddEvent(evt, panel);
        }
    }
}
