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
using System.Linq;
using System.Text;
using Chimera.Interfaces;
using System.Windows.Forms;
using Chimera.GUI.Controls.Plugins;
using Chimera.Util;

namespace Chimera.Plugins {
    public abstract class ConstrainedAxis : IAxis {
        private AxisBinding mBinding = AxisBinding.None;
        private ConstrainedAxisPanel mPanel;
        private readonly string mName;

        protected bool mMirror = true;

        private float mRaw = 0f;
        private float mDelta = 0f;
        private IUpdater<float> mDeadzone = new Updater<float>("Deadzone", .1f);
        private IUpdater<float> mScale  = new Updater<float>("Deadzone", 1f);

        public IUpdater<float> Deadzone {
            get { return mDeadzone; }
            set { 
                mDeadzone = value;
                Recalculate();
            }
        }

        public IUpdater<float> Scale {
            get { return mScale; }
            set { 
                mScale = value;
                Recalculate();
            }
        }

        protected ConstrainedAxis(string name) {
            mName = name;
        }
        protected ConstrainedAxis(string name, AxisBinding binding)
            : this(name) {
            mBinding = binding;
        }

        protected ConstrainedAxis(string name, IUpdater<float> deadzone, IUpdater<float> scale)
            : this(name) {
            mDeadzone = deadzone;
            mScale = scale;
        }

        protected ConstrainedAxis(string name, IUpdater<float> deadzone, float scale) :
            this(name, deadzone, new Updater<float>("Scale", scale)) {
        }

        protected ConstrainedAxis(string name, float deadzone, IUpdater<float> scale) :
            this(name, new Updater<float>("Deadzone", deadzone), scale) {
        }

        protected ConstrainedAxis(string name, float deadzone, float scale) :
            this(name, new Updater<float>("Deadzone", deadzone), new Updater<float>("Scale", scale)) {
        }

        protected ConstrainedAxis(string name, float deadzone, float scale, AxisBinding binding)
            : this(name, deadzone, scale) {
            mBinding = binding;
        }

        protected void SetRawValue(float value) {
            mRaw = value;
            Recalculate();
        }

        private void Recalculate() {
            float sign = mRaw < 0f ? -1f : 1f;
            if(mMirror)
                mRaw = Math.Abs(mRaw);
            mDelta = mRaw < mDeadzone.Value ? 0f : (mRaw - mDeadzone.Value) * mScale.Value;
            if (mMirror)
                mDelta *= sign;
        }

        #region IAxis Members

        public virtual UserControl ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new ConstrainedAxisPanel(this);
                return mPanel;
            }
        }

        public float Delta {
            get { return mDelta; }
        }

        public AxisBinding Binding {
            get { return mBinding; }
            set {
                if (mBinding != value)
                    mBinding = value;
            }
        }

        public string Name {
            get { return mName; }
        }

        #endregion
    }
}
