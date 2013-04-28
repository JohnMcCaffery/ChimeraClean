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
using OpenMetaverse;
using Chimera.GUI;
using NuiLibDotNet;

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

            ArmR.Vector = new VectorUpdater(mInput.ArmR);
            ArmL.Vector = new VectorUpdater(mInput.ArmL);
            flyVal.Scalar = new ScalarUpdater(mInput.FlyVal);
            flyAngleR.Scalar = new ScalarUpdater(Nui.acos(mInput.FlyAngleR) * (float) (180 / Math.PI));
            flyAngleL.Scalar = new ScalarUpdater(Nui.acos(mInput.FlyAngleL) * (float) (180 / Math.PI));
            constrainedFlyAngleR.Scalar = new ScalarUpdater(mInput.ConstrainedFlyAngleR);
            constrainedFlyAngleL.Scalar = new ScalarUpdater(mInput.ConstrainedFlyAngleL);
            flyScale.Scalar = new ScalarUpdater(mInput.FlyScale);
            flyThreshold.Scalar = new ScalarUpdater(mInput.FlyThreshold);
            flyMax.Scalar = new ScalarUpdater(mInput.FlyMax);
            flyTimer.Scalar = new ScalarUpdater(mInput.FlyTimer);
            flyMin.Scalar = new ScalarUpdater(mInput.FlyMin);

            yawLean.Scalar = new ScalarUpdater(Nui.acos(mInput.YawLean) * (float) (180 / Math.PI));
            yawTwist.Scalar = new ScalarUpdater(mInput.YawTwist);
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
