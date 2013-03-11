using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;

namespace Chimera.FlythroughLib.GUI {
    public partial class MoveToPanel : UserControl {
        private MoveToEvent mEvent;

        public MoveToPanel() {
            InitializeComponent();
        }

        public MoveToPanel(MoveToEvent evt)
            : this() {
            mEvent = evt;

            if (mEvent.Target == Vector3.Zero) {
                mEvent.Target = targetVectorPanel.Value;
                mEvent.Length = (int)lengthValue.Value;
            } else {
                targetVectorPanel.Value = mEvent.Target;
                lengthValue.Value = mEvent.Length;
            }

            targetVectorPanel.OnChange += (source, args) => mEvent.Target = targetVectorPanel.Value;
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
