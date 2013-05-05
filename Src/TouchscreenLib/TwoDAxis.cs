using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Plugins;
using Chimera.GUI.Controls.Plugins;
using System.Windows.Forms;
using Chimera.Interfaces;

namespace Touchscreen {
    public class TwoDAxis : ConstrainedAxis, ITickListener {
        private VerticalAxis mWrappedAxis;
        private bool mX;
        private ConstrainedAxisPanel mPanel;

        public TwoDAxis(VerticalAxis axis, bool x, bool right)
            : base((right ? "Right" : "Left") + (x ? "X" : "Y")) {

            mX = x;
            mWrappedAxis = axis;
        }

        public override UserControl ControlPanel {
            get { return mX ? mWrappedAxis.ControlPanel : base.ControlPanel; }
        }

        #region ITickListener Members

        public void Init(Chimera.ITickSource source) {
            if (!mX)
                source.Tick += new Action(source_Tick);
        }

        void source_Tick() {
            if (mWrappedAxis.Bounds.Contains(mWrappedAxis.Manager.CursorPosition))
                SetRawValue(VerticalAxis.GetValue(mWrappedAxis.StartH + mWrappedAxis.PaddingH, mWrappedAxis.W, mWrappedAxis.Manager.CursorX));
        }

        #endregion
    }
}
