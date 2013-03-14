using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NuiLibDotNet;

namespace KinectLib.GUI {
    public class ConditionalCheck : CheckBox {
        private Condition mCondition = Condition.Create(false);
        private bool mGuiChanged;
        private bool mExternalChanged;

        public ConditionalCheck()
            : base() {

            Condition = mCondition;
            CheckedChanged += ConditionalCheck_CheckedChanged;
        }

        public Condition Condition {
            get { return mCondition; }
            set {
                if (mCondition == null)
                    throw new ArgumentException("Unable to set Condition. Value cannot be null.");
                if (mCondition != null)
                    mCondition.OnChange -= mCondition_OnChange;
                mCondition = value;
                mCondition.OnChange += mCondition_OnChange;
            }
        }

        void ConditionalCheck_CheckedChanged(object sender, EventArgs e) {
            if (!mExternalChanged) {
                mGuiChanged = true;
                mCondition.Value = Checked;
                mGuiChanged = false;
            }
        }

        void mCondition_OnChange() {
            if (!mGuiChanged) {
                mExternalChanged = true;
                Checked = mCondition.Value;
                mExternalChanged = false;
            }
        }
    }
}
