using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FlythroughLib;
using OpenMetaverse;
using UtilLib;

namespace ChimeraGUILib.Controls.FlythroughEventPanels {
    public partial class MoveToPanel : UserControl {
        private MoveToEvent mEvent = new MoveToEvent(null, 0, Vector3.Zero);
        private CameraMaster mMaster;

        public MoveToPanel() {
            InitializeComponent();
        }

        public MoveToPanel(MoveToEvent evt, CameraMaster master)
            : this() {
            mEvent = evt;
            mMaster = master;

            if (mEvent.Target == Vector3.Zero) {
                mEvent.Target = targetVectorPanel.Value;
                mEvent.Length = (int)lengthValue.Value;
            } else {
                targetVectorPanel.Value = mEvent.Target;
                lengthValue.Value = mEvent.Length;
            }

            targetVectorPanel.ValueChanged += (source, args) => mEvent.Target = targetVectorPanel.Value;
            lengthValue.ValueChanged += (source, args) => mEvent.Length = (int)lengthValue.Value;
            evt.OnStep += (source, args) => {
                if (!IsDisposed)
                    Invoke(new Action(() => {
                        progressBar.Maximum = evt.TotalSteps;
                        progressBar.Value = Math.Min(evt.CurrentStep, progressBar.Maximum);
                    }));
            };
        }

        private void moveToTakeCurrentButton_Click(object sender, EventArgs e) {
            mEvent.Target = mMaster.Position;
            targetVectorPanel.Value = mEvent.Target;
        }


    }
}
