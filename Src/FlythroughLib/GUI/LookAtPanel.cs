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

namespace Chimera.Flythrough.GUI {
    public partial class LookAtPanel : UserControl {
        private LookAtEvent mEvent;
        private Action<FlythroughEvent<Rotation>, int> mTimeChangeListener;

        public LookAtPanel() {
            InitializeComponent();
        }

        public LookAtPanel(LookAtEvent evt)
            : this() {
            mEvent = evt;

            mTimeChangeListener = new Action<FlythroughEvent<Rotation>, int>(TimeChanged);

            if (mEvent.Target == Vector3.Zero) {
                mEvent.Target = targetVectorPanel.Value;
                mEvent.Length = (int)lengthValue.Value;
            } else {
                targetVectorPanel.Value = mEvent.Target;
                lengthValue.Value = mEvent.Length;
            }

            targetVectorPanel.ValueChanged += (source, args) => {
                mEvent.Target = targetVectorPanel.Value;
                mEvent.Container.Coordinator.Update(mEvent.Container.Coordinator.Position, Vector3.Zero, mEvent.Value, Rotation.Zero);
            };
            lengthValue.ValueChanged += (source, args) => mEvent.Length = (int)lengthValue.Value;
            evt.TimeChange += mTimeChangeListener;
            Disposed += new EventHandler(LookAtPanel_Disposed);
        }

        void LookAtPanel_Disposed(object sender, EventArgs e) {
            mEvent.TimeChange -= mTimeChangeListener;
        }

        private void TimeChanged(FlythroughEvent<Rotation> evt, int time) {
            if (!IsDisposed)
                Invoke(new Action(() => {
                    progressBar.Maximum = evt.Length;
                    progressBar.Value = evt.Time;
                }));
        }

        private void moveToTakeCurrentButton_Click(object sender, EventArgs e) {
            mEvent.Target = mEvent.Container.Coordinator.Position;
            targetVectorPanel.Value = mEvent.Target;
        }

        private void goToTargetButton_Click(object sender, EventArgs e) {
            if (mEvent != null)
                mEvent.Container.Coordinator.Update(mEvent.Target, Vector3.Zero, new Rotation(mEvent.Container.Coordinator.Orientation), Rotation.Zero);
        }
    }
}
