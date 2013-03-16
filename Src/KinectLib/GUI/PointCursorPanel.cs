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
    public partial class PointCursorPanel : UserControl {
        private PointCursor mInput;

        public PointCursorPanel() {
            InitializeComponent();
        }

        public PointCursorPanel(PointCursor input)
            : this() {
            Init(input);
        }

        public void Init(PointCursor input) {
            mInput = input;

            pointStartPanel.Vector = new VectorUpdater(mInput.PointStart);
            pointDirPanel.Vector = new VectorUpdater(mInput.PointDir);
            topLeftPanel.Vector = new VectorUpdater(mInput.TopLeft);
            topPanel.Vector = new VectorUpdater(mInput.Top);
            sidePanel.Vector = new VectorUpdater(mInput.Side);
            intersectionPanel.Vector = new VectorUpdater(mInput.Intersection);
            normalPanel.Vector = new VectorUpdater(mInput.Normal);
            xPanel.Scalar = new ScalarUpdater(mInput.X);
            yPanel.Scalar = new ScalarUpdater(mInput.Y);
            worldWPanel.Scalar = new ScalarUpdater(mInput.WorldW);
            worldHPanel.Scalar = new ScalarUpdater(mInput.WorldH);
            screenWPanel.Scalar = new ScalarUpdater(mInput.ScreenW);
            screenHPanel.Scalar = new ScalarUpdater(mInput.ScreenH);

            xPanel.Max = mInput.ScreenW.Value;
            yPanel.Max = mInput.ScreenH.Value;

            pointStartPanel.Text = "Point Start";
            pointDirPanel.Text = "Point Dir";
            topLeftPanel.Text = "Top Left";
            normalPanel.Text = "Normal";
            topPanel.Text = "Top";
            sidePanel.Text = "Side";
            intersectionPanel.Text = "Intersection";
            
        }
    }
}
