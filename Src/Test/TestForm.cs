using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NuiLibDotNet;
using Chimera.Kinect.GUI;

namespace Test {
    public partial class TestForm : Form {
        public TestForm() {
            InitializeComponent();
        }

        public TestForm(Vector topLeft, Vector normal, Vector top, Vector side, Vector pointStart, Vector pointDir, Vector intersection, Scalar w, Scalar h, Scalar x, Scalar y)
            : this() {

            topLeftPanel.Vector = new VectorUpdater(topLeft);
            normalPanel.Vector = new VectorUpdater(normal);
            topPanel.Vector = new VectorUpdater(top);
            sidePanel.Vector = new VectorUpdater(side);
            pointStartPanel.Vector = new VectorUpdater(pointStart);
            pointDirPanel.Vector = new VectorUpdater(pointDir);
            intersectionPanel.Vector = new VectorUpdater(intersection);
            wPanel.Scalar = new ScalarUpdater(w);
            hPanel.Scalar = new ScalarUpdater(h);
            xPanel.Scalar = new ScalarUpdater(x);
            yPanel.Scalar = new ScalarUpdater(y);
        }
    }
}
