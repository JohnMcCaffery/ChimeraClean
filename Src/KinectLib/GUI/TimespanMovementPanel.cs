using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;
using Chimera.GUI;

namespace Chimera.Kinect.GUI {
    public partial class TimespanMovementPanel : UserControl {
        private TimespanMovementInput mInput;
        private bool mGuiInput, mExternalInput;

        public TimespanMovementPanel() {
            InitializeComponent();
            valuePanel.Text = "Deltas (Y = yaw)";
        }

        public TimespanMovementPanel(TimespanMovementInput input)
            : this() {
            Init(input);
        }

        public void Init(TimespanMovementInput input) {
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
            constrainedFlyAngleR.Scalar = new ScalarUpdater(mInput.ConstrainedFlyAngleR * (float) (180 / Math.PI));
            constrainedFlyAngleL.Scalar = new ScalarUpdater(mInput.ConstrainedFlyAngleL * (float) (180 / Math.PI));
            flyScale.Scalar = new ScalarUpdater(mInput.FlyScale);
            flyThreshold.Scalar = new ScalarUpdater(mInput.FlyThreshold);
            flyMax.Scalar = new ScalarUpdater(mInput.FlyMax);
            flyTimer.Scalar = new ScalarUpdater(mInput.FlyTimer);
            flyMin.Scalar = new ScalarUpdater(mInput.FlyMin);

            yawLean.Scalar = new ScalarUpdater(mInput.YawLean * (float) (180 / Math.PI));
            yawValue.Scalar = new ScalarUpdater(mInput.Yaw);
            yawScale.Scalar = new ScalarUpdater(mInput.YawScale);
            yawThreshold.Scalar = new ScalarUpdater(mInput.YawThreshold);

            mExternalInput = true;
            enabled.Checked = mInput.Enabled;
            walkEnabled.Checked = mInput.WalkEnabled;
            flyEnabled.Checked = mInput.FlyEnabled;
            yawEnabled.Checked = mInput.YawEnabled;
            mExternalInput = false;

            mInput.Change += source => {
                Vector3 delta = mInput.PositionDelta;
                delta.Y = (float) mInput.OrientationDelta.Yaw;
                valuePanel.Value = delta;
            };

            HandleCreated += new EventHandler(TimespanMovementPanel_HandleCreated);
        }

        void TimespanMovementPanel_HandleCreated(object sender, EventArgs e) {
            enabled.Checked = mInput.Enabled;
            flyEnabled.Checked = mInput.FlyEnabled;
            walkEnabled.Checked = mInput.WalkEnabled;
            yawEnabled.Checked = mInput.YawEnabled;
            mInput.EnabledChanged += (source, value) => {
                BeginInvoke(new Action(() => {
                    if (!mGuiInput) {
                        mExternalInput = true;
                        enabled.Checked = source.Enabled;
                        flyEnabled.Checked = mInput.FlyEnabled;
                        walkEnabled.Checked = mInput.WalkEnabled;
                        yawEnabled.Checked = mInput.YawEnabled;
                        mExternalInput = false;
                    }
                }));
            };
        }

        private void CheckedChanged(object sender, EventArgs e) {
            if (!mExternalInput) {
                mGuiInput = true;
                mInput.Enabled = enabled.Checked;

                mInput.FlyEnabled = flyEnabled.Checked;
                mInput.WalkEnabled = walkEnabled.Checked;
                mInput.YawEnabled = yawEnabled.Checked;
                mGuiInput = false;
            }
        }
    }
}
