using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Interfaces;

namespace KinectLib.GUI {
    public partial class UpdatedScalarPanel : ScalarPanel {
        private IUpdater<float> mScalar;

        public IUpdater<float> Scalar {
            get { return mScalar; }
            set {
                if ((object) mScalar != null)
                    mScalar.Changed -= mScalar_OnChange;
                mScalar = value;
                if ((object) mScalar != null)
                    mScalar.Changed += mScalar_OnChange;
            }
        }

        void mScalar_OnChange(float val) {
                Value = val;
        }
    }
}
