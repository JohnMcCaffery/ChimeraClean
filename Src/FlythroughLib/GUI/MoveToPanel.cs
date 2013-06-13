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
using OpenMetaverse;
using Chimera.Util;

namespace Chimera.Flythrough.GUI {
    public partial class MoveToPanel : UserControl {
        private MoveToEvent mEvent;
        private Action<FlythroughEvent<Vector3>, int> mTimeChangeListener;
        private bool mExternalUpdate;
        private bool mGuiUpdate;

        public MoveToPanel() {
            InitializeComponent();
            Disposed += new EventHandler(MoveToPanel_Disposed);
        }

        public MoveToPanel(MoveToEvent evt)
            : this() {
            mEvent = evt;

            mEvent.LengthChange += new EventHandler<LengthChangeEventArgs<Vector3>>(mEvent_LengthChange);
            if (mEvent.Target == Vector3.Zero) {
                mEvent.Target = targetVectorPanel.Value;
                mEvent.Length = (int)lengthValue.Value;
            } else {
                targetVectorPanel.Value = mEvent.Target;
                lengthValue.Value = mEvent.Length;
            }

            targetVectorPanel.ValueChanged += (source, args) => {
                mEvent.Target = targetVectorPanel.Value;
                //mEvent.Container.Time = evt.GlobalFinishTime;
                mEvent.Container.Core.Update(mEvent.Target, Vector3.Zero, mEvent.Container.Core.Orientation, Rotation.Zero);
            };

            mTimeChangeListener = new Action<FlythroughEvent<Vector3>,int>(evt_TimeChange);
        }

        void MoveToPanel_Disposed(object sender, EventArgs e) {
            //mEvent.TimeChange -= evt_TimeChange;
            mEvent.TimeChange -= mTimeChangeListener;
        }

        private void evt_TimeChange(FlythroughEvent<Vector3> evt, int time) {
            if (!IsDisposed && !Disposing && Created)
                BeginInvoke(new Action(() => {
                    progressBar.Maximum = evt.Length;
                    progressBar.Value = evt.Time;
                }));
        }

        private void moveToTakeCurrentButton_Click(object sender, EventArgs e) {
            mEvent.Target = mEvent.Container.Core.Position;
            targetVectorPanel.Value = mEvent.Target;
        }

        private void MoveToPanel_VisibleChanged(object sender, EventArgs e) {
            if (Visible) {
                mEvent.TimeChange += mTimeChangeListener;
                progressBar.Maximum = mEvent.Length;
                progressBar.Value = mEvent.Time;
            } else
                mEvent.TimeChange -= mTimeChangeListener;
        }

        void mEvent_LengthChange(object source, LengthChangeEventArgs<Vector3> args) {
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
