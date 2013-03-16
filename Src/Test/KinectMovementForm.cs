using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Kinect;
using NuiLibDotNet;

namespace Test {
    public partial class KinectMovementForm : Form {
        public KinectMovementForm() {
            InitializeComponent();

            DolphinMovementInput input = new DolphinMovementInput();
            dolphinMovementPanel1.Init(input);
        }
    }
}
