using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Plugins;
using Chimera.Interfaces;

namespace Chimera.GUI.Controls.Plugins {
    public partial class SplitAxisPanel : UserControl {
        private SplitAxis mAxis;

        public SplitAxisPanel() {
            InitializeComponent();
        }

        public SplitAxisPanel(SplitAxis axis)
            : this() {

            mAxis = axis;
            mAxis.AxisAdded += new Action<Interfaces.IAxis>(mAxis_AxisAdded);

            foreach (var subAx in axis.Axes)
                mAxis_AxisAdded(subAx);
        }

        private void mAxis_AxisAdded(IAxis axis) {
            positivePulldown.Items.Add(axis);
            negativePulldown.Items.Add(axis);

            if (axis == mAxis.Positive)
                positivePulldown.SelectedItem = axis;
            if (axis == mAxis.Negative)
                negativePulldown.SelectedItem = axis;
        }

        private void positivePulldown_SelectedIndexChanged(object sender, EventArgs e) {
            mAxis.Positive = (IAxis) positivePulldown.SelectedItem;
            mAxis.Positive.ControlPanel.Dock = DockStyle.Fill;

            positivePanel.Controls.Clear();
            positivePanel.Controls.Add(mAxis.Positive.ControlPanel);

            Height = Math.Max(mAxis.Negative.ControlPanel.Height, mAxis.Positive.ControlPanel.Height) + MinimumSize.Height;
        }

        private void negativePulldown_SelectedIndexChanged(object sender, EventArgs e) {
            mAxis.Negative = (IAxis) negativePulldown.SelectedItem;
            mAxis.Negative.ControlPanel.Dock = DockStyle.Fill;

            negativePanel.Controls.Clear();
            negativePanel.Controls.Add(mAxis.Negative.ControlPanel);

            Height = Math.Max(mAxis.Negative.ControlPanel.Height, mAxis.Positive.ControlPanel.Height) + MinimumSize.Height;
        }
    }
}
