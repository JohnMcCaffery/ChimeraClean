using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ProxyTestGUI;
using OpenMetaverse;
using Chimera.Interfaces;

namespace KinectLib.GUI {
    public partial class UpdatedVectorPanel : VectorPanel {
        private IUpdater<Vector3> mVector;
        private bool mGuiChanged;
        private bool mExternalChanged;

        public UpdatedVectorPanel()
            : base() {

            ValueChanged += UpdatedVectorPanel_ValueChanged;
            Disposed += new EventHandler(UpdatedScalarPanel_Disposed);
        }

        void UpdatedScalarPanel_Disposed(object sender, EventArgs e) {
            if (mVector != null)
                mVector.Changed -= mVector_OnChange;
        }

        public IUpdater<Vector3> Vector {
            get { return mVector; }
            set {
                if ( mVector != null)
                    mVector.Changed -= mVector_OnChange;
                mVector = value;
                if (mVector != null) {
                    Value = value.Value;
                    mVector.Changed += mVector_OnChange;
                }
            }
        }

        void UpdatedVectorPanel_ValueChanged(object sender, EventArgs e) {
            if (!mExternalChanged && (object) mVector != null) {
                mGuiChanged = true;
                mVector.Value = Value;
                mGuiChanged = false;
            }
        }

        void mVector_OnChange(Vector3 val) {
            if (!mGuiChanged) {
                mExternalChanged = true;
                Value = val;
                mExternalChanged = false;
            }
        }
    }
}
