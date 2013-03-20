using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Interfaces;

namespace Chimera.GUI {
    public class ConditionalCheck : CheckBox {
        private IUpdater<bool> mCondition;
        private bool mGuiChanged;
        private bool mExternalChanged;

        public ConditionalCheck()
            : base() {

            CheckedChanged += ConditionalCheck_CheckedChanged;
            Disposed += new EventHandler(ConditionalCheck_Disposed);
        }

        void ConditionalCheck_Disposed(object sender, EventArgs e) {
            if (mCondition != null)
                mCondition.Changed -= mCondition_OnChange;
        }

        public IUpdater<bool> Condition {
            get { return mCondition; }
            set {
                if (mCondition != null)
                    mCondition.Changed -= mCondition_OnChange;
                mCondition = value;
                if (mCondition != null)
                    mCondition.Changed += mCondition_OnChange;
            }
        }

        void ConditionalCheck_CheckedChanged(object sender, EventArgs e) {
            if (!mExternalChanged) {
                mGuiChanged = true;
                mCondition.Value = Checked;
                mGuiChanged = false;
            }
        }

        void mCondition_OnChange(bool val) {
            if (!mGuiChanged) {
                mExternalChanged = true;
                Invoke(() => Checked = val);
                mExternalChanged = false;
            }
        }

        private void Invoke(Action a) {
            if (InvokeRequired && !IsDisposed && Created)
                base.Invoke(a);
            else
                a();
        }
    }
}
