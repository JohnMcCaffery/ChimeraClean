﻿using System;
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

            if (mEvent.PitchTarget == 0f && mEvent.YawTarget == 0f) {
                mEvent.PitchTarget = (float)pitchValue.Value;
                mEvent.YawTarget = (float)yawValue.Value;
                mEvent.Length = (int)lengthValue.Value;
            } else {
                pitchValue.Value = new decimal(mEvent.PitchTarget);
                yawValue.Value = new decimal(mEvent.YawTarget);
                lengthValue.Value = mEvent.Length;
            }

            pitchValue.ValueChanged += (source, args) => mEvent.PitchTarget = (float)pitchValue.Value;
            yawValue.ValueChanged += (source, args) => mEvent.YawTarget = (float)yawValue.Value;
            lengthValue.ValueChanged += (source, args) => mEvent.Length = (int)lengthValue.Value;
            evt.OnStep += (source, args) => {
                Invoke(new Action(() => {
                    progressBar.Maximum = evt.TotalSteps;
                    progressBar.Value = Math.Min(evt.CurrentStep, progressBar.Maximum);
                }));
            };
        }

        private void rotateToTakeCurrentButton_Click(object sender, EventArgs e) {
            Rotation rot = new Rotation(mMaster.LookAt);
            mEvent.PitchTarget = rot.Pitch;
            mEvent.YawTarget = rot.Yaw;
            pitchValue.Value = new decimal(mEvent.PitchTarget);
            yawValue.Value = new decimal(mEvent.YawTarget);
        }
    }
}
