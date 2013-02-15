using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FlythroughLib;
using UtilLib;

namespace ChimeraGUILib.Controls.FlythroughEventPanels {
    public partial class LookAtPanel : UserControl {
        private LookAtEvent mEvent;
        private CameraMaster mMaster;

        public LookAtPanel() {
            InitializeComponent();
        }

        public LookAtPanel(LookAtEvent evt, CameraMaster master)
            : this() {
            mEvent = evt;
            mMaster = master;

            mEvent.Target = targetVectorPanel.Value;
            mEvent.Length = (int) lengthValue.Value;

            mEvent.Target = targetVectorPanel.Value;
            targetVectorPanel.OnChange += (source, args) => mEvent.Target = targetVectorPanel.Value;
            lengthValue.ValueChanged += (source, args) => mEvent.Length = (int)lengthValue.Value;
        }

        private void lookAtTakeCurrentButton_Click(object sender, EventArgs e) {
            mEvent.Target = mMaster.Position;
            targetVectorPanel.Value = mEvent.Target;
        }
    }
}
