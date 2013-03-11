using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Util;

namespace Chimera.FlythroughLib.GUI {
    public partial class RotateToPanel : UserControl {
        private RotateToEvent mEvent;

        public RotateToPanel() {
            InitializeComponent();
        }

        public RotateToPanel(RotateToEvent evt)
            : this() {
            mEvent = evt;

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
            evt.TimeChange += (source, args) => {
                Invoke(new Action(() => {
                    progressBar.Maximum = evt.Length;
                    progressBar.Value = evt.Time;
                }));
            };
        }

        private void rotateToTakeCurrentButton_Click(object sender, EventArgs e) {
            Rotation rot = new Rotation(mEvent.Container.Coordinator.Orientation);
            mEvent.PitchTarget = rot.Pitch;
            mEvent.YawTarget = rot.Yaw;
            pitchValue.Value = new decimal(mEvent.PitchTarget);
            yawValue.Value = new decimal(mEvent.YawTarget);
        }
    }
}
