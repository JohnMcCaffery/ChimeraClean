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
    public partial class PushAxisPanel : UserControl {

        public PushAxisPanel() {
            InitializeComponent();
        }

        public PushAxisPanel(PushAxis.PushSingleAxis axis)
            : this() {

            constrainedAxisPanel.Axis = axis;
            pushPanel.Scalar = new ScalarUpdater(axis.Diff);
        }


    }
}
