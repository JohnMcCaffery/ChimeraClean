using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Util;
using OpenMetaverse;

namespace Chimera.Flythrough.GUI {
    public partial class RotateToPanel : UserControl {
        private RotateToEvent mEvent;

        public RotateToPanel() {
            InitializeComponent();
        }

        public RotateToPanel(RotateToEvent evt)
            : this() {
            mEvent = evt;

            rotationPanel.Value = mEvent.Target;
            lengthValue.Value = mEvent.Length;

            lengthValue.ValueChanged += (source, args) => mEvent.Length = (int)lengthValue.Value;
            rotationPanel.OnChange += (sender, args) => {
                mEvent.Target = rotationPanel.Value;
                mEvent.Container.Coordinator.Update(mEvent.Container.Coordinator.Position, Vector3.Zero, rotationPanel.Value, Rotation.Zero);
            };
            evt.TimeChange += (source, args) => {
                Invoke(new Action(() => {
                    progressBar.Maximum = evt.Length;
                    progressBar.Value = evt.Time;
                }));
            };
        }

        private void rotateToTakeCurrentButton_Click(object sender, EventArgs e) {
            rotationPanel.Value = new Rotation(mEvent.Container.Coordinator.Orientation);
        }
    }
}
