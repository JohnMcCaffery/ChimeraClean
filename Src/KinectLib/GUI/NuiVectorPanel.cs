using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ProxyTestGUI;
using NuiLibDotNet;
using OpenMetaverse;

namespace KinectLib.GUI {
    public partial class NuiVectorPanel : VectorPanel {
        private Vector mVector = Vector.Create(0f, 0f, 0f);
        private bool mGuiChanged;
        private bool mExternalChanged;

        public NuiVectorPanel()
            : base() {

            Vector = mVector;
            ValueChanged += NuiVectorPanel_ValueChanged;
        }

        public Vector Vector {
            get { return mVector; }
            set {
                if (mVector == null)
                    throw new ArgumentException("Unable to set Vector. Value cannot be null.");
                if ((object) mVector != null)
                    mVector.OnChange -= mVector_OnChange;
                mVector = value;
                mVector.OnChange += mVector_OnChange;
            }
        }

        void NuiVectorPanel_ValueChanged(object sender, EventArgs e) {
            if (!mExternalChanged) {
                mGuiChanged = true;
                mVector.Set(X, Y, Z);
                mGuiChanged = false;
            }
        }

        void mVector_OnChange() {
            if (!mGuiChanged) {
                mExternalChanged = true;
                Value = new Vector3(mVector.X, mVector.Y, mVector.Z);
                mExternalChanged = false;
            }
        }
    }
}
