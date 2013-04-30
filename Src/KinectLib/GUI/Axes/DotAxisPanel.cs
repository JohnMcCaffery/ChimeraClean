using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Kinect.Axes;

namespace Chimera.Kinect.GUI.Axes {
    public partial class DotAxisPanel : UserControl {
        public DotAxisPanel() {
            InitializeComponent();
        }

        public DotAxisPanel(DotAxis axis)
            : this() {

            kinectAxisPanel.Axis = axis;
            aPanel.Vector = new VectorUpdater(axis.A);
            bPanel.Vector = new VectorUpdater(axis.B);
        }
    }
}
