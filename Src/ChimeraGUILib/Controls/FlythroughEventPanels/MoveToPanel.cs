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

            mEvent.Target = targetVectorPanel.Value;
            mEvent.Length = (int) lengthValue.Value;

            targetVectorPanel.OnChange += (source, args) => mEvent.Target = targetVectorPanel.Value;
            lengthValue.ValueChanged += (source, args) => mEvent.Length = (int)lengthValue.Value;
        }

        private void moveToTakeCurrentButton_Click(object sender, EventArgs e) {
            mEvent.Target = mMaster.Position;
            targetVectorPanel.Value = mEvent.Target;
        }


    }
}
