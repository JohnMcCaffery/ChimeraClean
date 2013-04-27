using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Inputs;

namespace Chimera.GUI.Controls.Inputs {
    public partial class AxisBasedDeltaPanel : UserControl {
        private AxisBasedDelta mInput;

        public AxisBasedDeltaPanel() {
            InitializeComponent();
        }

        public AxisBasedDeltaPanel(AxisBasedDelta input)
            : this() {

            mInput = input;
        }
    }
}
