using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;
using Chimera.Util;

namespace Chimera.FlythroughLib.GUI {
    public partial class LookAtPanel : UserControl {
        private LookAtEvent mEvent;

        public LookAtPanel() {
            InitializeComponent();
        }

        public LookAtPanel(LookAtEvent evt)
            : this() {
            mEvent = evt;

            if (mEvent.Target == Vector3.Zero) {
                mEvent.Target = targetVectorPanel.Value;
                mEvent.Length = (int)lengthValue.Value;
            } else {
                targetVectorPanel.Value = mEvent.Target;
                lengthValue.Value = mEvent.Length;
            }

            targetVectorPanel.OnChange += (source, args) => {
                mEvent.Target = targetVectorPanel.Value;
                mEvent.Container.Coordinator.Update(mEvent.Container.Coordinator.Position, Vector3.Zero, mEvent.Value, new Rotation());
            };
            lengthValue.ValueChanged += (source, args) => mEvent.Length = (int)lengthValue.Value;
            evt.TimeChange += (source, args) => {
                if (!IsDisposed)
                    Invoke(new Action(() => {
                        progressBar.Maximum = evt.Length;
                        progressBar.Value = evt.Time;
                    }));
            };
        }

        private void moveToTakeCurrentButton_Click(object sender, EventArgs e) {
            mEvent.Target = mEvent.Container.Coordinator.Position;
            targetVectorPanel.Value = mEvent.Target;
        }
    }
}
