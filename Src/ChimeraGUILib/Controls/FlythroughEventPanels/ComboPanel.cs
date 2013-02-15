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
    public partial class ComboPanel : UserControl {
        private ComboEvent mEvent;
        private CameraMaster mMaster;

        public ComboPanel() {
            InitializeComponent();
        }

        public ComboPanel(ComboEvent evt, CameraMaster master)
            : this() {
            mEvent = evt;
            mMaster = master;

            sequence1Panel.Init(evt, true, mMaster);
            sequence2Panel.Init(evt, false, mMaster);
        }

        public void AddEvent(FlythroughEvent evt, UserControl panel, bool sequence1) {
            if (sequence1)
                sequence1Panel.AddEvent(evt, panel);
            else
                sequence2Panel.AddEvent(evt, panel);
        }
    }
}
