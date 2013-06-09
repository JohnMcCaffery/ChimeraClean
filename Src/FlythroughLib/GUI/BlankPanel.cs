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

namespace Chimera.Flythrough.GUI {
    public partial class BlankPanel<T> : UserControl {
        private Action<FlythroughEvent<T>, int> mTimeChangeListener;
        private BlankEvent<T> mEvent;
        private bool mExternalUpdate;
        private bool mGuiUpdate;

        public BlankPanel() {
            InitializeComponent();
        }

        public BlankPanel(BlankEvent<T> evt)
            : this() {
            mEvent = evt;

            if (mEvent.Length == 0f)
                mEvent.Length = (int)lengthValue.Value;
            else
                lengthValue.Value = mEvent.Length;

            mEvent.LengthChange += new EventHandler<LengthChangeEventArgs<T>>(mEvent_LengthChange);
            mTimeChangeListener = (e, time) => {
                Invoke(new Action(() => {
                    progressBar.Maximum = evt.Length;
                    progressBar.Value = evt.Time;
                }));
            };
        }

        private void BlankPanel_VisibleChanged(object sender, EventArgs e) {
            if (Visible) {
                mEvent.TimeChange += mTimeChangeListener;
                progressBar.Maximum = mEvent.Length;
                progressBar.Value = mEvent.Time;
            } else
                mEvent.TimeChange -= mTimeChangeListener;
        }

        void mEvent_LengthChange(object source, LengthChangeEventArgs<T> args) {
            if (!mGuiUpdate) {
                mExternalUpdate = true;
                lengthValue.Value = mEvent.Length;
                mExternalUpdate = false;
            }
        }

        private void lengthValue_ValueChanged(object sender, EventArgs e) {
            if (!mExternalUpdate) {
                mGuiUpdate = true;
                mEvent.Length = (int)lengthValue.Value;
                mGuiUpdate = false;
            }
        }
    }
}
