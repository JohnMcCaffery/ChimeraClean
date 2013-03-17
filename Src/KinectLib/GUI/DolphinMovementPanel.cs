using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;
using KinectLib.GUI;

namespace Chimera.Kinect.GUI {
    public partial class DolphinMovementPanel : UserControl {
        private DolphinMovementInput mInput;

        public DolphinMovementPanel() {
            InitializeComponent();
            valuePanel.Text = "Deltas (Y = yaw)";
        }

        public DolphinMovementPanel(DolphinMovementInput input)
            : this() {
            Init(input);
        }

        public void Init(DolphinMovementInput input) {
            mInput = input;

            walkVal.Scalar = new ScalarUpdater(mInput.WalkVal);
            walkDiffL.Scalar = new ScalarUpdater(mInput.WalkDiffL);
            walkDiffR.Scalar = new ScalarUpdater(mInput.WalkDiffR);
            walkValL.Scalar = new ScalarUpdater(mInput.WalkValL);
            walkValR.Scalar = new ScalarUpdater(mInput.WalkValR);
            walkScale.Scalar = new ScalarUpdater(mInput.WalkScale);
            walkThreshold.Scalar = new ScalarUpdater(mInput.WalkThreshold);

            flyVal.Scalar = new ScalarUpdater(mInput.FlyVal);
            flyAngleR.Scalar = new ScalarUpdater(mInput.FlyAngleR * (float) (180 / Math.PI));
            flyAngleL.Scalar = new ScalarUpdater(mInput.FlyAngleL * (float) (180 / Math.PI));
            flyScale.Scalar = new ScalarUpdater(mInput.FlyScale);
            flyThreshold.Scalar = new ScalarUpdater(mInput.FlyThreshold);
            flyMax.Scalar = new ScalarUpdater(mInput.FlyMax);

            yawLean.Scalar = new ScalarUpdater(mInput.YawLean * (float) (180 / Math.PI));
            yawValue.Scalar = new ScalarUpdater(mInput.Yaw);
            yawScale.Scalar = new ScalarUpdater(mInput.YawScale);
            yawThreshold.Scalar = new ScalarUpdater(mInput.YawThreshold);

            enabled.Checked = mInput.Enabled;
            walkEnabled.Checked = mInput.WalkEnabled;
            flyEnabled.Checked = mInput.FlyEnabled;
            yawEnabled.Checked = mInput.YawEnabled;

            mInput.Change += () => {
                Vector3 delta = mInput.PositionDelta;
                delta.Y = (float) mInput.OrientationDelta.Yaw;
                valuePanel.Value = delta;
            };
        }

        private void CheckedChanged(object sender, EventArgs e) {
            mInput.Enabled = enabled.Checked;

            mInput.FlyEnabled = flyEnabled.Checked;
            mInput.WalkEnabled = walkEnabled.Checked;
            mInput.YawEnabled = yawEnabled.Checked;
        }
    }
}
