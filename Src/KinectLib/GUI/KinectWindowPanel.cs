using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Interfaces;
using OpenMetaverse;

namespace Chimera.Kinect.GUI {
    public partial class KinectWindowPanel : UserControl {
        private WindowInput mInput;

        public KinectWindowPanel() {
            InitializeComponent();
        }

        public KinectWindowPanel(WindowInput input)
            : this() {
            mInput = input;
            mInput.VectorsRecalculated += Init;
            Init();
        }

        public void Init() {
            topLeftPanel.Vector = new VectorUpdater(mInput.TopLeft);
            topPanel.Vector = new VectorUpdater(mInput.Top);
            sidePanel.Vector = new VectorUpdater(mInput.Side);
            intersectionPanel.Vector = new VectorUpdater(mInput.Intersection);
            normalPanel.Vector = new VectorUpdater(mInput.Normal);
            xPanel.Scalar = new ScalarUpdater(mInput.X);
            yPanel.Scalar = new ScalarUpdater(mInput.Y);
            wPanel.Scalar = new ScalarUpdater(mInput.W);
            hPanel.Scalar = new ScalarUpdater(mInput.H);
        }
    }
}
