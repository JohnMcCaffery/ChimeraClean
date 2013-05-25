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

namespace Chimera.Kinect.GUI.Axes {
    public partial class KinectAxisPanel : UserControl {
        private KinectAxis mAxis;
        private ChangeDelegate mChangeListener;

        public KinectAxis Axis {
            get { return mAxis; }
            set {
                mAxis = value;

                stateLabel.Text = mAxis.Active.Value ? "Active" : "Disabled";

                constrainedAxisPanel.Axis = mAxis.Axis;
                mChangeListener = new ChangeDelegate(Active_OnChange);

                mAxis.Active.OnChange += mChangeListener;
                Disposed += new EventHandler(KinectAxisPanel_Disposed);
            }
        }

        public KinectAxisPanel() {
            InitializeComponent();
        }

        public KinectAxisPanel(KinectAxis axis)
            : this() {

            Axis = axis;
        }

        void KinectAxisPanel_Disposed(object sender, EventArgs e) {
            mAxis.Active.OnChange -= mChangeListener;
        }

        void Active_OnChange() {
            if (Created)
                Invoke(new Action(() => stateLabel.Text = mAxis.Active.Value ? "Active" : "Disabled"));
        }
    }
}
