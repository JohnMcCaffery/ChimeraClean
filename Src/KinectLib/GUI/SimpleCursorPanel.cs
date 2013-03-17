using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chimera.Kinect.GUI {
    public partial class SimpleCursorPanel : UserControl {
        private SimpleCursor mInput;

        public SimpleCursorPanel() {
            InitializeComponent();
        }

        public SimpleCursorPanel(SimpleCursor cursor)
            : this() {

            Init(cursor);
        }

        public void Init(SimpleCursor input) {
            mInput = input;

            Anchor.Vector = new VectorUpdater(mInput.Anchor);
            HandR.Vector = new VectorUpdater(mInput.HandR);
            Width.Scalar = new ScalarUpdater(mInput.Width);
            Height.Scalar = new ScalarUpdater(mInput.Height);
            WidthScale.Scalar = new ScalarUpdater(mInput.WidthScale);
            HeightScale.Scalar = new ScalarUpdater(mInput.HeightScale);
            LeftShift.Scalar = new ScalarUpdater(mInput.LeftShift);
            UpShift.Scalar = new ScalarUpdater(mInput.UpShift);
            TopLeftX.Scalar = new ScalarUpdater(mInput.TopLeftX);
            TopLeftY.Scalar = new ScalarUpdater(mInput.TopLeftY);
            X.Scalar = new ScalarUpdater(mInput.X);
            Y.Scalar = new ScalarUpdater(mInput.Y);
            ConstrainedX.Scalar = new ScalarUpdater(mInput.ConstrainedX);
            ConstrainedY.Scalar = new ScalarUpdater(mInput.ConstrainedY);
            RawX.Scalar = new ScalarUpdater(mInput.RawX);
            RawY.Scalar = new ScalarUpdater(mInput.RawY);

            enabledCheck.Checked = mInput.Enabled;

            Anchor.Text = "Anchor";
            HandR.Text = "Hand";
        }

        private void enabledCheck_CheckedChanged(object sender, EventArgs e) {
            mInput.Enabled = enabledCheck.Checked;
        }
    }
}
