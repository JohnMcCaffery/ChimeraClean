using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chimera.Kinect.GUI {
    public partial class RaiseArmHelpTriggerPanel : UserControl {
        private RaiseArmHelpTrigger mInput;


        public RaiseArmHelpTriggerPanel() {
            InitializeComponent();
        }

        public RaiseArmHelpTriggerPanel(RaiseArmHelpTrigger input)
            : this() {


        ArmR.Vector = new VectorUpdater(mInput.ArmR);
        ArmL.Vector = new VectorUpdater(mInput.ArmL);
        AngleR.Scalar = new ScalarUpdater(mInput.AngleR);
        AngleL.Scalar = new ScalarUpdater(mInput.AngleL);
        TriggerR.Condition = new ConditionUpdater(mInput.TriggerR);
        TriggerL.Condition = new ConditionUpdater(mInput.TriggerL);
        Trigger.Condition = new ConditionUpdater(mInput.Trigger);
        AngleThreshold.Scalar = new ScalarUpdater(mInput.AngleThreshold);
        HeightThreshold.Scalar = new ScalarUpdater(mInput.HeightThreshold);
        }
    }
}
