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
    public partial class ConstrainedAxisPanel : UserControl {
        private ConstrainedAxis mConstrainedAxis;

        public ConstrainedAxisPanel() {
            InitializeComponent();
        }

        public ConstrainedAxisPanel(Chimera.Inputs.ConstrainedAxis constrainedAxis)
            : this() {
            mConstrainedAxis = constrainedAxis;
        }
    }
}
