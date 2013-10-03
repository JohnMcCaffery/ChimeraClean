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
    public partial class ComboPanel : UserControl {
        private ComboEvent mEvent;
        private UserControl mCurrentPositionPanel;
        private UserControl mCurrentOrientationPanel;
        private Action<FlythroughEvent<Vector3>, FlythroughEvent<Vector3>> mPositionEvtChanged;
        private Action<FlythroughEvent<Rotation>, FlythroughEvent<Rotation>> mOrientationEvtChanged;
        private bool mGuiUpdate;

        private FlythroughEvent<Vector3> mDraggedPosition = null;
        private FlythroughEvent<Rotation> mDraggedOrientation = null;
        private int mH = -1;

        public ComboPanel() {
            InitializeComponent();
        }

        public ComboPanel(ComboEvent evt)
            : this() {
            mEvent = evt;

            //mPositionTimeChanged = new Action<FlythroughEvent<Vector3>, int>(positionEvt_TimeChanged);
            //mOrientationTimeChanged = new Action<FlythroughEvent<Rotation>, int>(orientationEvt_TimeChanged);
            mPositionEvtChanged += new Action<FlythroughEvent<Vector3>,FlythroughEvent<Vector3>>(Positions_CurrentEventChange);
            mOrientationEvtChanged += new Action<FlythroughEvent<Rotation>,FlythroughEvent<Rotation>>(Orientations_CurrentEventChange);

            evt.PositionOrderChanged += new Action<FlythroughEvent<Vector3>>(evt_PositionOrderChanged);
            evt.OrientationOrderChanged += new Action<FlythroughEvent<Rotation>>(evt_OrientationOrderChanged);

            foreach (var e in mEvent.Positions) {
                AddEvent(e, positionsList, positionPanel);
            } foreach (var e in mEvent.Orientations) {
                AddEvent(e, orientationsList, orientationPanel);
            }
        }

        void evt_PositionOrderChanged(FlythroughEvent<Vector3> evt) {
            MoveUpInList(evt, positionsList);
        }

        void evt_OrientationOrderChanged(FlythroughEvent<Rotation> evt) {
            MoveUpInList(evt, orientationsList);
        }

        private void MoveUpInList<T>(FlythroughEvent<T> evt, ListBox list) {
            int index = list.Items.IndexOf(evt);
            list.Items[index] = list.Items[index -1];
            list.Items[index - 1] = evt;
            list.SelectedItem = evt;
        }

        /*
        private void positionEvt_TimeChanged(FlythroughEvent<Vector3> evt, int time) {
            TimeChanged(evt, positionsList);
        }

        private void orientationEvt_TimeChanged(FlythroughEvent<Rotation> evt, int time) {
            TimeChanged(evt, orientationsList);
        }

        private void TimeChanged<T>(FlythroughEvent<T> evt, ListBox list) {
            if (!mGuiUpdate && !IsDisposed && Created)
                Invoke(new Action(() => { if (list.SelectedItem != evt) list.SelectedItem = evt; }));
        }
        */

        private void AddEvent(FlythroughEvent<Vector3> evt) {
            //evt.TimeChange += mPositionTimeChanged;
            mEvent.AddEvent(evt);
            AddEvent(evt, positionsList, positionPanel);
        }

        private void AddEvent(FlythroughEvent<Rotation> evt) {
            //evt.TimeChange += mOrientationTimeChanged;
            mEvent.AddEvent(evt);
            AddEvent(evt, orientationsList, orientationPanel);
        }

        private void AddEvent<T>(FlythroughEvent<T> evt, ListBox list, Panel panel) {
            evt.ControlPanel.Dock = DockStyle.Fill;
            panel.Controls.Add(evt.ControlPanel);
            list.BeginUpdate();
            list.Items.Add(evt);
            list.SelectedIndex = list.Items.Count - 1;
            list.EndUpdate();
        }

        private void RemoveEvent(FlythroughEvent<Vector3> evt) {
            //evt.TimeChange -= mPositionTimeChanged;
            mEvent.RemoveEvent(evt);
            RemoveEvent(evt, positionsList, positionPanel);
        }

        private void RemoveEvent(FlythroughEvent<Rotation> evt) {
            //evt.TimeChange -= mOrientationTimeChanged;
            mEvent.RemoveEvent(evt);
            RemoveEvent(evt, orientationsList, orientationPanel);
        }

        private void RemoveEvent<T>(FlythroughEvent<T> evt, ListBox list, Panel panel) {
            list.Items.Remove(evt);
            panel.Controls.Remove(evt.ControlPanel);
        }

        private void MoveUp(FlythroughEvent<Vector3> evt) {
            MoveUp(evt, positionsList);
        }

        private void MoveUp(FlythroughEvent<Rotation> evt) {
            MoveUp(evt, positionsList);
        }

        private void MoveUp<T>(FlythroughEvent<T> evt, ListBox list) {
            int index = list.Items.IndexOf(evt);
            if (index > 0) {
                mEvent.MoveUp(evt);
                //list.Items[index] = list.Items[index - 1];
                //list.Items[index - 1] = evt;
                //list.SelectedIndex = index - 1;
            }
        }

        private void Drag<T>(FlythroughEvent<T> evt, MouseEventArgs e, ListBox list) {
            if (evt != null) {
                int h = e.Y - (e.Y % list.ItemHeight);
                if (h != mH) {
                    if (h < mH)
                        MoveUp(evt, list);
                    else {
                        FlythroughEvent<T> next = mEvent.Next(evt);
                        if (next != null) {
                            MoveUp(next, list);
                            list.SelectedItem = evt;
                        }
                    }
                    mH = h;
                }
            }
        }

        // ----- Positions -----

        private void Positions_CurrentEventChange(FlythroughEvent<Vector3> o, FlythroughEvent<Vector3> n) {
            BeginInvoke(new Action(() => positionsList.SelectedItem = n));
        }

        private void moveToEventToolStripMenuItem_Click(object sender, EventArgs e) {
            AddEvent(new MoveToEvent(mEvent.Container, mEvent.Container.DefaultLength, new Vector3(128f, 128f, 60f)));
        }

        private void blankPositionEventToolStripMenuItem_Click(object sender, EventArgs e) {
            AddEvent(new BlankEvent<Vector3>(mEvent.Container, mEvent.Container.DefaultLength));
        }

        private void removePositionToolStripMenuItem_Click(object sender, EventArgs e) {
            if (positionsList.SelectedItem != null)
                RemoveEvent((FlythroughEvent<Vector3>) positionsList.SelectedItem);
        }

        private void moveUpPositionToolStripMenuItem_Click(object sender, EventArgs e) {
            if (positionsList.SelectedItem != null)
                MoveUp((FlythroughEvent<Vector3>)positionsList.SelectedItem);
        }

        private void positionsList_SelectedValueChanged(object sender, EventArgs e) {
            if (positionsList.SelectedItem != null) {
                mGuiUpdate = true;
                UserControl panel = ((FlythroughEvent<Vector3>)positionsList.SelectedItem).ControlPanel;
                if (mCurrentPositionPanel != panel) {
                    if (mCurrentPositionPanel != null)
                        mCurrentPositionPanel.Visible = false;
                    mCurrentPositionPanel = panel;
                    mCurrentPositionPanel.Visible = true;
                }
                mGuiUpdate = false;
            }
        }
        private void positionStartButton_Click(object sender, EventArgs e) {
            if (positionsList.SelectedItem != null)
                mEvent.Container.Time = ((FlythroughEvent<Vector3>)positionsList.SelectedItem).GlobalStartTime;
        }

        private void positionEndButton_Click(object sender, EventArgs e) {
            if (positionsList.SelectedItem != null)
                mEvent.Container.Time = ((FlythroughEvent<Vector3>)positionsList.SelectedItem).GlobalFinishTime;
        }

        private void copyCurrentPositionButton_Click(object sender, EventArgs e) {
            AddEvent(new MoveToEvent(mEvent.Container, mEvent.Container.DefaultLength, mEvent.Container.Core.Position));
        }

        private void positionsList_MouseDown(object sender, MouseEventArgs e) {
            mDraggedPosition = (FlythroughEvent<Vector3>) positionsList.SelectedItem;
            mH = e.Y - (e.Y % positionsList.ItemHeight);
        }

        private void positionsList_MouseMove(object sender, MouseEventArgs e) {
            Drag(mDraggedPosition, e, positionsList);
        }

        private void positionsList_MouseUp(object sender, MouseEventArgs e) {
            mDraggedPosition = null;
            mH = -1;
        }

        private void positionsList_MouseLeave(object sender, EventArgs e) {
            mDraggedPosition = null;
            mH = -1;
        }

        private void positionsList_KeyUp(object sender, KeyEventArgs e) {
            if ((e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back) && positionsList.SelectedItem != null)
                removePositionToolStripMenuItem_Click(sender, e);
        }

        // ----- Orientations -----

        private void Orientations_CurrentEventChange(FlythroughEvent<Rotation> o, FlythroughEvent<Rotation> n) {
            BeginInvoke(new Action(() => orientationsList.SelectedItem = n));
        }

        private void rotateToEventToolStripMenuItem_Click(object sender, EventArgs e) {
            AddEvent(new RotateToEvent(mEvent.Container, mEvent.Container.DefaultLength));
        }

        private void lookAtEventToolStripMenuItem_Click(object sender, EventArgs e) {
            AddEvent(new LookAtEvent(mEvent.Container, mEvent.Container.DefaultLength));
        }

        private void blankOrientationEventToolStripItem_Click(object sender, EventArgs e) {
            AddEvent(new BlankEvent<Rotation>(mEvent.Container, mEvent.Container.DefaultLength));
        }

        private void removeOrientationToolStripItem_Click(object sender, EventArgs e) {
            if (orientationsList.SelectedItem != null)
                RemoveEvent((FlythroughEvent<Rotation>)orientationsList.SelectedItem);
        }

        private void moveUpOrientationToolStripItem_Click(object sender, EventArgs e) {
            if (orientationsList.SelectedItem != null)
                MoveUp((FlythroughEvent<Rotation>)orientationsList.SelectedItem);
        }

        private void orientationsList_SelectedValueChanged(object sender, EventArgs e) {
            if (orientationsList.SelectedItem != null) {
                mGuiUpdate = true;
                UserControl panel = ((FlythroughEvent<Rotation>)orientationsList.SelectedItem).ControlPanel;
                if (mCurrentOrientationPanel != panel) {
                    if (mCurrentOrientationPanel != null)
                        mCurrentOrientationPanel.Visible = false;
                    mCurrentOrientationPanel = panel;
                    mCurrentOrientationPanel.Visible = true;
                }
                mGuiUpdate = false;
            }
        }
        private void copyCurrentOrientationButton_Click(object sender, EventArgs e) {
            AddEvent(new RotateToEvent(mEvent.Container, mEvent.Container.DefaultLength, new Rotation(mEvent.Container.Core.Orientation)));
        }

        private void orientationsList_MouseDown(object sender, MouseEventArgs e) {
            mDraggedOrientation = (FlythroughEvent<Rotation>) orientationsList.SelectedItem;
            mH = e.Y - (e.Y % orientationsList.ItemHeight);
        }

        private void orientationsList_MouseMove(object sender, MouseEventArgs e) {
            Drag(mDraggedOrientation, e, orientationsList);
        }

        private void orientationsList_MouseUp(object sender, MouseEventArgs e) {
            mDraggedOrientation = null;
        }

        private void orientationsList_MouseLeave(object sender, EventArgs e) {
            mDraggedOrientation = null;
        }

        private void orientationStartButton_Click(object sender, EventArgs e) {
            if (orientationsList.SelectedItem != null)
                mEvent.Container.Time = ((FlythroughEvent<Rotation>)orientationsList.SelectedItem).GlobalStartTime;
        }

        private void orientationFinishButton_Click(object sender, EventArgs e) {
            if (orientationsList.SelectedItem != null)
                mEvent.Container.Time = ((FlythroughEvent<Rotation>)orientationsList.SelectedItem).GlobalFinishTime;
        }

        private void positionsList_DoubleClick(object sender, EventArgs e) {
            if (positionsList.SelectedItem != null)
                mEvent.Container.Time = ((FlythroughEvent<Vector3>)positionsList.SelectedItem).GlobalFinishTime;
        }

        private void orientationsList_DoubleClick(object sender, EventArgs e) {
            if (orientationsList.SelectedItem != null)
                mEvent.Container.Time = ((FlythroughEvent<Rotation>)orientationsList.SelectedItem).GlobalFinishTime;
        }

        private void copyCurrentPairButton_Click(object sender, EventArgs e) {
            AddEvent(new MoveToEvent(mEvent.Container, mEvent.Container.DefaultLength, mEvent.Container.Core.Position));
            AddEvent(new RotateToEvent(mEvent.Container, mEvent.Container.DefaultLength, new Rotation(mEvent.Container.Core.Orientation)));
        }

        private void ComboPanel_VisibleChanged(object sender, EventArgs e) {
            if (Visible) {
                mEvent.CurrentOrientationEventChange += mOrientationEvtChanged;
                mEvent.CurrentPositionEventChange += mPositionEvtChanged;
                positionsList.SelectedItem = mEvent.CurrentPosition;
                orientationsList.SelectedItem = mEvent.CurrentOrientation;
            } else {
                mEvent.CurrentOrientationEventChange -= mOrientationEvtChanged;
                mEvent.CurrentPositionEventChange -= mPositionEvtChanged;
            }
        }

        private void orientationsList_KeyUp(object sender, KeyEventArgs e) {
            if ((e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back) && orientationsList.SelectedItem != null)
                removeOrientationToolStripItem_Click(sender, e);
        }

    }
}
