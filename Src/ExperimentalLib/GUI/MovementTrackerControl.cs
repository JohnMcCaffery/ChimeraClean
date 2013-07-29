using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Experimental.Plugins;

namespace Chimera.Experimental.GUI {
    public partial class MovementTrackerControl : UserControl {
        private MovementTracker mPlugin;

        public MovementTrackerControl() {
            InitializeComponent();
        }

        public MovementTrackerControl(MovementTracker movementTracker) : this() {
            mPlugin = movementTracker;
        }
    }
}
