using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Experimental.Plugins;
using Chimera.Util;

namespace Chimera.Experimental.GUI {
    public partial class MovementTrackerControl : UserControl {
        private MovementTracker mPlugin;

        public MovementTrackerControl() {
            InitializeComponent();
        }

        public MovementTrackerControl(MovementTracker movementTracker) : this() {
            mPlugin = movementTracker;
            mPlugin.TimeChanged += new Action(mPlugin_TimeChanged);
            mPlugin.StateChanged += new Action(mPlugin_StateChanged);
        }

        void mPlugin_StateChanged() {
            InvokeExtension.Invoke(this, () => {
                stateLabel.Text = mPlugin.ExperimentState.ToString();
                prepCheck.Checked = mPlugin.Prep;

                timeLabel.Text = mPlugin.ExperimentState == State.Running || mPlugin.ExperimentState == State.Finished ? 
                    string.Format("{0}m {1}s", (int) mPlugin.Time.TotalMinutes, mPlugin.Time.Seconds) :
                    "Ready to start";
            });
        }

        void mPlugin_TimeChanged() {
            InvokeExtension.Invoke(this, () =>
                timeLabel.Text = mPlugin.ExperimentState == State.Running || mPlugin.ExperimentState == State.Finished ? 
                string.Format("{0}m {1}s", (int) mPlugin.Time.TotalMinutes, mPlugin.Time.Seconds) :
                "Ready to start"
            );
        }

        private void prepCheck_CheckedChanged(object sender, EventArgs e) {
            mPlugin.Prep = prepCheck.Checked;
        } 

    }
}
