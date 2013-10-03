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
using Chimera.Kinect.Overlay;

namespace Chimera.Kinect.GUI {
    public partial class RaiseArmTriggerPanel : UserControl {
        private RaiseArmTrigger mInput;


        public RaiseArmTriggerPanel() {
            InitializeComponent();
        }

        public RaiseArmTriggerPanel(RaiseArmTrigger input)
            : this() {

            mInput = input;

            ArmR.Vector = new VectorUpdater(mInput.ArmR);
            ArmL.Vector = new VectorUpdater(mInput.ArmL);
            AngleR.Scalar = new ScalarUpdater(mInput.AngleR);
            AngleL.Scalar = new ScalarUpdater(mInput.AngleL);
            TriggerR.Condition = new ConditionUpdater(mInput.TriggerR);
            TriggerL.Condition = new ConditionUpdater(mInput.TriggerL);
            Trigger.Condition = new ConditionUpdater(mInput.Trigger);
            AngleThreshold.Scalar = new ScalarUpdater(mInput.AngleThreshold);
            HeightThreshold.Scalar = new ScalarUpdater(mInput.HeightThreshold);
            DepthThreshold.Scalar = new ScalarUpdater(mInput.DepthThreshold);
            hipCentrePanel.Vector = new VectorUpdater(mInput.Body);
        }

        private void forceTriggerButton_Click(object sender, EventArgs e) {
            mInput.ForceTrigger();
        }
    }
}
