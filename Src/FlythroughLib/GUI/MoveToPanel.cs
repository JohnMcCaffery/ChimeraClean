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
    public partial class MoveToPanel : UserControl {
        private MoveToEvent mEvent;

        public MoveToPanel() {
            InitializeComponent();
            Disposed += new EventHandler(MoveToPanel_Disposed);
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

            targetVectorPanel.ValueChanged += (source, args) => {
                mEvent.Target = targetVectorPanel.Value;
                //mEvent.Container.Time = evt.GlobalFinishTime;
                mEvent.Container.Coordinator.Update(mEvent.Target, Vector3.Zero, mEvent.Container.Coordinator.Orientation, Rotation.Zero);
            };
            lengthValue.ValueChanged += (source, args) => mEvent.Length = (int)lengthValue.Value;
            mChangeListener = new Action<FlythroughEvent<Vector3>,int>(evt_TimeChange);
            mEvent.TimeChange += mChangeListener;
        }

        private Action<FlythroughEvent<Vector3>, int> mChangeListener;

        void MoveToPanel_Disposed(object sender, EventArgs e) {
            //mEvent.TimeChange -= evt_TimeChange;
            mEvent.TimeChange -= mChangeListener;
        }

        private void evt_TimeChange(FlythroughEvent<Vector3> evt, int time) {
            if (!IsDisposed && !Disposing && Created)
                Invoke(new Action(() => {
                    progressBar.Maximum = evt.Length;
                    progressBar.Value = evt.Time;
                }));
        }

        private void moveToTakeCurrentButton_Click(object sender, EventArgs e) {
            mEvent.Target = mEvent.Container.Coordinator.Position;
            targetVectorPanel.Value = mEvent.Target;
        }
    }
}
