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
        private SimpleCursor mCursor;

        public SimpleCursorPanel() {
            InitializeComponent();
        }

        public SimpleCursorPanel(SimpleCursor cursor)
            : this() {

            Init(cursor);
        }

        public void Init(SimpleCursor cursor) {
            mCursor = cursor;

            Anchor.Text = "Anchor";
            HandR.Text = "Hand";

            Anchor.Vector = new VectorUpdater(mCursor.Anchor);
            HandR.Vector = new VectorUpdater(mCursor.HandR);
            Width.Scalar = new ScalarUpdater(mCursor.Width);
            Height.Scalar = new ScalarUpdater(mCursor.Height);
            WidthScale.Scalar = new ScalarUpdater(mCursor.WidthScale);
            HeightScale.Scalar = new ScalarUpdater(mCursor.HeightScale);
            LeftShift.Scalar = new ScalarUpdater(mCursor.LeftShift);
            UpShift.Scalar = new ScalarUpdater(mCursor.UpShift);
            TopLeftX.Scalar = new ScalarUpdater(mCursor.TopLeftX);
            TopLeftY.Scalar = new ScalarUpdater(mCursor.TopLeftY);
            X.Scalar = new ScalarUpdater(mCursor.X);
            Y.Scalar = new ScalarUpdater(mCursor.Y);
            ConstrainedX.Scalar = new ScalarUpdater(mCursor.ConstrainedX);
            ConstrainedY.Scalar = new ScalarUpdater(mCursor.ConstrainedY);
            RawX.Scalar = new ScalarUpdater(mCursor.RawX);
            RawY.Scalar = new ScalarUpdater(mCursor.RawY);
        }
    }
}
