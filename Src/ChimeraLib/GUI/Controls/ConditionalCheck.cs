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
                base.BeginInvoke(a);
            else
                a();
        }
    }
}
