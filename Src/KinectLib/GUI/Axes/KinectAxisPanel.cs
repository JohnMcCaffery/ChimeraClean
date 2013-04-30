using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Kinect.Axes;
using NuiLibDotNet;
using Chimera.Plugins;
using Chimera.Kinect.Interfaces;

namespace Chimera.Kinect.GUI.Axes {
    public partial class KinectAxisPanel : UserControl {
        private IKinectAxis mAxis;
        private ChangeDelegate mChangeListener;

        public IKinectAxis Axis {
            get { return mAxis; }
            set {            mAxis = value;

            stateLabel.Text = mAxis.Active.Value ? "Active" : "Disabled";

            constrainedAxisPanel.Axis = mAxis.Axis;
            pushPanel.Scalar = new ScalarUpdater(mAxis.Raw);
            mChangeListener = new ChangeDelegate(Active_OnChange);
            mAxis.Active.OnChange += mChangeListener;
            Disposed += new EventHandler(KinectAxisPanel_Disposed);
            }
        }

        public KinectAxisPanel() {
            InitializeComponent();
        }

        public KinectAxisPanel(IKinectAxis axis)
            : this() {

            Axis = axis;
        }

        void KinectAxisPanel_Disposed(object sender, EventArgs e) {
            mAxis.Active.OnChange -= mChangeListener;
        }

        void Active_OnChange() {
            Invoke(new Action(() => stateLabel.Text = mAxis.Active.Value ? "Active" : "Disabled"));
        }
    }
}
