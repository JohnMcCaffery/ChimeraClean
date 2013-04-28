using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Plugins;

namespace Chimera.GUI.Controls.Plugins {
    public partial class ConstrainedAxisPanel : UserControl {
        private ConstrainedAxis mConstrainedAxis;

        public ConstrainedAxisPanel() {
            InitializeComponent();
        }

        public ConstrainedAxisPanel(Chimera.Plugins.ConstrainedAxis constrainedAxis)
            : this() {
            mConstrainedAxis = constrainedAxis;

            deadzonePanel.Max = mConstrainedAxis.Deadzone * 3;
            scalePanel.Max = mConstrainedAxis.Scale * 3;

            deadzonePanel.Value = mConstrainedAxis.Deadzone;
            scalePanel.Value = mConstrainedAxis.Scale;
        }
    }
}
