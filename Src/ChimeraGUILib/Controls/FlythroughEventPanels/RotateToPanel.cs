using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FlythroughLib;
using UtilLib;

namespace ChimeraGUILib.Controls.FlythroughEventPanels {
    public partial class RotateToPanel : UserControl {
        private RotateToEvent mEvent;
        private CameraMaster mMaster;

        public RotateToPanel() {
            InitializeComponent();
        }

        public RotateToPanel(RotateToEvent evt, CameraMaster master)
            : this() {
            mEvent = evt;
            mMaster = master;

            mEvent.PitchTarget = (float) pitchValue.Value;
            mEvent.YawTarget = (float) yawValue.Value;
            mEvent.Length = (int) lengthValue.Value;

            pitchValue.ValueChanged += (source, args) => mEvent.PitchTarget = (float)pitchValue.Value;
            yawValue.ValueChanged += (source, args) => mEvent.YawTarget = (float)yawValue.Value;
            lengthValue.ValueChanged += (source, args) => mEvent.Length = (int)lengthValue.Value;
        }

        private void rotateToTakeCurrentButton_Click(object sender, EventArgs e) {
            mEvent.PitchTarget = mMaster.Rotation.Pitch;
            mEvent.YawTarget = mMaster.Rotation.Yaw;
            pitchValue.Value = new decimal(mEvent.PitchTarget);
            yawValue.Value = new decimal(mEvent.YawTarget);
        }
    }
}
