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

            rotationPanel.Rotation = mEvent.Target;
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
            mEvent.Target.Pitch = rot.Pitch;
            mEvent.Target.Yaw = rot.Yaw;
        }
    }
}
