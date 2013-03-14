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
        public IUpdater<float> X {
            get { return xPanel.Scalar; }
            set { xPanel.Scalar = value; }
        }
        public IUpdater<float> Y {
            get { return yPanel.Scalar; }
            set { yPanel.Scalar = value; }
        }

        public IUpdater<Vector3> TopLeft {
            get { return topLeftPanel.Vector; }
            set { topLeftPanel.Vector = value; }
        }

        public IUpdater<Vector3> Top {
            get { return topPanel.Vector; }
            set { topPanel.Vector = value; }
        }

        public IUpdater<Vector3> Side {
            get { return sidePanel.Vector; }
            set { sidePanel.Vector = value; }
        }

        public IUpdater<Vector3> Intersection {
            get { return intersectionPanel.Vector; }
            set { intersectionPanel.Vector = value; }
        }


        public KinectWindowPanel() {
            InitializeComponent();
        }
    }
}
