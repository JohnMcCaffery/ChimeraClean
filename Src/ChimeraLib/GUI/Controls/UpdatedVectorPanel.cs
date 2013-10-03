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
using Chimera.GUI;
using OpenMetaverse;
using Chimera.Interfaces;

namespace Chimera.GUI {
    public partial class UpdatedVectorPanel : VectorPanel {
        private IUpdater<Vector3> mVector;
        private Action<Vector3> mChangeListener;
        private bool mGuiChanged;
        private bool mExternalChanged;

        public UpdatedVectorPanel()
            : base() {

            ValueChanged += UpdatedVectorPanel_ValueChanged;
            Disposed += new EventHandler(UpdatedVectorPanel_Disposed);
            mChangeListener = new Action<Vector3>(mVector_OnChange);
        }

        void UpdatedVectorPanel_Disposed(object sender, EventArgs e) {
            if (mVector != null)
                mVector.Changed -= mChangeListener;
        }

        public IUpdater<Vector3> Vector {
            get { return mVector; }
            set {
                if (mVector != null)
                    mVector.Changed -= mChangeListener;
                mVector = value;
                if (mVector != null) {
                    Value = value.Value;
                    mVector.Changed += mChangeListener;
                    Text = mVector.Name;
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

        /*
        private void UpdatedVectorPanel_VisibleChanged(object sender, EventArgs e) {
            if (Visible)
                mVector.Changed += mChangeListener;
            else
                mVector.Changed -= mChangeListener;
        }

        private void UpdatedVectorPanel_Load(object sender, EventArgs e) {
            UpdatedVectorPanel_VisibleChanged(sender, e);
        }

        void UpdatedVectorPanel_HandleCreated(object sender, EventArgs e) {
            UpdatedVectorPanel_VisibleChanged(sender, e);
        }
        */
    }
}
