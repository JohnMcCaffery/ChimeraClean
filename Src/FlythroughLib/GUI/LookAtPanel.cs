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
    public partial class LookAtPanel : UserControl {
        private LookAtEvent mEvent;
        private Action<FlythroughEvent<Rotation>, int> mTimeChangeListener;

        public LookAtPanel() {
            InitializeComponent();
        }

        public LookAtPanel(LookAtEvent evt)
            : this() {
            mEvent = evt;

            mTimeChangeListener = new Action<FlythroughEvent<Rotation>, int>(TimeChanged);

            /*
            if (mEvent.Target == Vector3.Zero) {
                mEvent.Target = targetVectorPanel.Value;
                mEvent.Length = (int)lengthValue.Value;
            } else {
                */
                targetVectorPanel.Value = mEvent.Target;
                lengthValue.Value = mEvent.Length;
            //}

            targetVectorPanel.ValueChanged += (source, args) => {
                mEvent.Target = targetVectorPanel.Value;
                mEvent.Container.Core.Update(mEvent.Container.Core.Position, Vector3.Zero, mEvent.Value, Rotation.Zero);
            };
            lengthValue.ValueChanged += (source, args) => mEvent.Length = (int)lengthValue.Value;
            evt.TimeChange += mTimeChangeListener;
            Disposed += new EventHandler(LookAtPanel_Disposed);
        }

        void LookAtPanel_FormClosing(object sender, FormClosingEventArgs e) { mEvent.TimeChange -= mTimeChangeListener; }

        void LookAtPanel_Disposed(object sender, EventArgs e) { mEvent.TimeChange -= mTimeChangeListener; }

        private void TimeChanged(FlythroughEvent<Rotation> evt, int time) {
            if (!IsDisposed && Created && !Disposing)
                BeginInvoke(new Action(() => {
                    progressBar.Maximum = evt.Length;
                    progressBar.Value = evt.Time;
                }));
        }

        private void moveToTakeCurrentButton_Click(object sender, EventArgs e) {
            mEvent.Target = mEvent.Container.Core.Position;
            targetVectorPanel.Value = mEvent.Target;
        }

        private void goToTargetButton_Click(object sender, EventArgs e) {
            if (mEvent != null)
                mEvent.Container.Core.Update(mEvent.Target, Vector3.Zero, new Rotation(mEvent.Container.Core.Orientation), Rotation.Zero);
        }

        private void LookAtPanel_VisibleChanged(object sender, EventArgs e) {
            if (Visible) {
                mEvent.TimeChange += mTimeChangeListener;
                progressBar.Maximum = mEvent.Length;
                progressBar.Value = mEvent.Time;
            } else
                mEvent.TimeChange -= mTimeChangeListener;
        }
    }
}
