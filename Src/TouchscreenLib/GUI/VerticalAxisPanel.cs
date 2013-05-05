using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Touchscreen.GUI {
    public partial class VerticalAxisPanel : UserControl {
        private VerticalAxis mAxis;

        public VerticalAxisPanel() {
            InitializeComponent();
        }

        public VerticalAxisPanel(VerticalAxis axis)
            : this() {

            mAxis = axis;
            wPanel.Value = axis.W;
            hPanel.Value = axis.H;
            paddingHPanel.Value = axis.PaddingH;
            paddingVPanel.Value = axis.PaddingV;
            constrainedAxisPanel.Axis = axis;
        }
    }
}
