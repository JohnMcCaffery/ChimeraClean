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
using Chimera.Util;

namespace Chimera.Kinect.GUI {
    public partial class KinectPanel : UserControl {
        private bool mGuiUpdate;
        private bool mExternalUpdate;
        private bool mStarted;

        public event Action<Vector3> PositionChanged;

        public event Action Started;

        public bool HasStarted {
            get { return mStarted; }
            set {
                mStarted = value;
                startButton.Enabled = !value;
            }
        }

        public Vector3 Position {
            get { return positionPanel.Value * 10f; }
            set {
                if (!mGuiUpdate) {
                    mExternalUpdate = true;
                    positionPanel.Value = value / 10f;
                    mExternalUpdate = true;
                }
            }
        }

        public Rotation Orientation {
            get { return orientationPanel.Value; }
            set { orientationPanel.Value = value; }
        }

        public IUpdater<Vector3> PointStart {
            get { return pointStartPanel.Vector; }
            set { pointStartPanel.Vector = value; }
        }

        public IUpdater<Vector3> PointDir {
            get { return pointDirPanel.Vector; }
            set { pointDirPanel.Vector = value; }
        }

        public KinectPanel() {
            InitializeComponent();
        }

        private void positionPanel_ValueChanged(object sender, EventArgs e) {
            if (!mExternalUpdate && PositionChanged != null) {
                mGuiUpdate = true;
                PositionChanged(positionPanel.Value);
                mGuiUpdate = false;
            }
        }

        private void startButton_Click(object sender, EventArgs e) {
            if (Started != null)
                Started();
            mStarted = true;
            startButton.Enabled = !mStarted;
        }

        public void AddWindow(KinectWindowPanel windowPanel, string name) {
            // 
            // windowTab
            // 
            TabPage windowTab = new TabPage();
            windowTab.Location = new System.Drawing.Point(4, 22);
            windowTab.Name = name + " Tab";
            windowTab.Padding = new System.Windows.Forms.Padding(3);
            windowTab.Size = new System.Drawing.Size(701, 494);
            windowTab.TabIndex = 2;
            windowTab.Text = name;
            windowTab.UseVisualStyleBackColor = true;
            windowTab.AutoScroll = true;
            // 
            // kinectWindowPanel
            // 
            windowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            windowPanel.Location = new System.Drawing.Point(3, 3);
            windowPanel.Name = name + "WindowPanel";
            windowPanel.Size = new System.Drawing.Size(695, 488);
            windowPanel.TabIndex = 0;

            windowTab.Controls.Add(windowPanel);
            mainTab.Controls.Add(windowTab);
        }
    }
}
