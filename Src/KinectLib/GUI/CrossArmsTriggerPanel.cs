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
    public partial class CrossArmsTriggerPanel : UserControl {
        private CrossArmsTrigger mInput;


        public CrossArmsTriggerPanel() {
            InitializeComponent();
        }

        public CrossArmsTriggerPanel(CrossArmsTrigger input)
            : this() {

            mInput = input;

            ForeArmR.Vector = new VectorUpdater(mInput.ForeArmR);
            ForeArmL.Vector = new VectorUpdater(mInput.ForeArmL);
            AngleR.Scalar = new ScalarUpdater(mInput.AngleArms);
            AngleL.Scalar = new ScalarUpdater(mInput.AngleL);
            TriggerR.Condition = new ConditionUpdater(mInput.TriggerHeight);
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
