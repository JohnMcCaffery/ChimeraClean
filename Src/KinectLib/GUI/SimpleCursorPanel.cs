/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
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
        private SimpleKinectCursor mInput;

        public SimpleCursorPanel() {
            InitializeComponent();
        }

        public SimpleCursorPanel(SimpleKinectCursor cursor)
            : this() {

            Init(cursor);
        }

        public void Init(SimpleKinectCursor input) {
            mInput = input;

            Anchor.Vector = new VectorUpdater(mInput.Anchor);
            HandR.Vector = new VectorUpdater(mInput.HandR);
            HandL.Vector = new VectorUpdater(mInput.HandL);
            Width.Scalar = new ScalarUpdater(mInput.Width);
            Height.Scalar = new ScalarUpdater(mInput.Height);
            LeftHandShift.Scalar = new ScalarUpdater(mInput.LeftHandShift);
            LeftShift.Scalar = new ScalarUpdater(mInput.LeftShift);
            UpShift.Scalar = new ScalarUpdater(mInput.UpShift);
            TopLeftX.Scalar = new ScalarUpdater(mInput.TopLeftX);
            TopLeftY.Scalar = new ScalarUpdater(mInput.TopLeftY);
            X.Scalar = new ScalarUpdater(mInput.X);
            Y.Scalar = new ScalarUpdater(mInput.Y);
            ConstrainedXRight.Scalar = new ScalarUpdater(mInput.ConstrainedXRight);
            ConstrainedYRight.Scalar = new ScalarUpdater(mInput.ConstrainedYRight);
            ConstrainedXLeft.Scalar = new ScalarUpdater(mInput.ConstrainedXLeft);
            ConstrainedYLeft.Scalar = new ScalarUpdater(mInput.ConstrainedYLeft);
            RawXRight.Scalar = new ScalarUpdater(mInput.RawXRight);
            RawYRight.Scalar = new ScalarUpdater(mInput.RawYRight);
            RawXLeft.Scalar = new ScalarUpdater(mInput.RawXLeft);
            RawYLeft.Scalar = new ScalarUpdater(mInput.RawYLeft);
            SmoothingFactor.Scalar = new ScalarUpdater(mInput.SmoothingFactor);

            enabledCheck.Checked = mInput.Enabled;

            Anchor.Text = "Anchor";
            HandR.Text = "Hand";
        }

        private void enabledCheck_CheckedChanged(object sender, EventArgs e) {
            mInput.Enabled = enabledCheck.Checked;
        }
    }
}
