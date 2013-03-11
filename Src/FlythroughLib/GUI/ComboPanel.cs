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

namespace Chimera.FlythroughLib.GUI {
    public partial class ComboPanel : UserControl {
        private ComboEvent mEvent;
        private UserControl mCurrentPositionPanel;
        private UserControl mCurrentOrientationPanel;
        private bool mGuiUpdate;
        private bool mTickUpdate;

        public ComboPanel() {
            InitializeComponent();
        }

        public ComboPanel(ComboEvent evt)
            : this() {
            mEvent = evt;
            mEvent.Positions.CurrentEventChange += new Action<FlythroughEvent<Vector3>,FlythroughEvent<Vector3>>(Positions_CurrentEventChange);
            mEvent.Orientations.CurrentEventChange += new Action<FlythroughEvent<Rotation>,FlythroughEvent<Rotation>>(Orientations_CurrentEventChange);

            foreach (var e in mEvent.Positions) {
                AddEvent(e);
            } foreach (var e in mEvent.Orientations) {
                AddEvent(e);
            }
        }

        private void AddEvent(FlythroughEvent<Vector3> evt) {
            mEvent.AddEvent(evt);
            AddEvent(evt, positionsList, positionPanel);
        }

        private void AddEvent(FlythroughEvent<Rotation> evt) {
            mEvent.AddEvent(evt);
            AddEvent(evt, orientationsList, orientationPanel);
        }

        private void AddEvent<T>(FlythroughEvent<T> evt, ListBox list, Panel panel) {
            list.BeginUpdate();
            list.Items.Add(evt);
            list.EndUpdate();
            evt.TimeChange += (e, time) => {
                if (!mGuiUpdate && !IsDisposed && Created)
                Invoke(new Action(() => { if (list.SelectedItem != evt) list.SelectedItem = evt; }));
            };
            evt.ControlPanel.Dock = DockStyle.Fill;
            evt.ControlPanel.Visible = false;
            panel.Controls.Add(evt.ControlPanel);
        }

        private void RemoveEvent(FlythroughEvent<Vector3> evt) {
            mEvent.RemoveEvent(evt);
            RemoveEvent(evt, positionsList, positionPanel);
        }

        private void RemoveEvent(FlythroughEvent<Rotation> evt) {
            mEvent.RemoveEvent(evt);
            RemoveEvent(evt, orientationsList, orientationPanel);
        }

        private void RemoveEvent<T>(FlythroughEvent<T> evt, ListBox list, Panel panel) {
            list.Items.Remove(evt);
            panel.Controls.Remove(evt.ControlPanel);
        }

        // ----- Positions -----

        private void Positions_CurrentEventChange(FlythroughEvent<Vector3> o, FlythroughEvent<Vector3> n) {
            Invoke(new Action(() => positionsList.SelectedItem = n));
        }

        private void moveToEventToolStripMenuItem_Click(object sender, EventArgs e) {
            AddEvent(new MoveToEvent(mEvent.Container, 5000, new Vector3(128f, 128f, 60f)));
        }

        private void blankPositionEventToolStripMenuItem_Click(object sender, EventArgs e) {
            AddEvent(new BlankEvent<Vector3>(mEvent.Container, 5000));
        }

        private void removePositionToolStripMenuItem_Click(object sender, EventArgs e) {
            if (positionsList.SelectedItem != null)
                RemoveEvent((FlythroughEvent<Vector3>) positionsList.SelectedItem, positionsList, positionPanel);
        }

        private void moveUpPositionToolStripMenuItem_Click(object sender, EventArgs e) {
            if (positionsList.SelectedItem != null)
                mEvent.Positions.MoveUp((FlythroughEvent<Vector3>)positionsList.SelectedItem);
        }

        private void positionsList_SelectedValueChanged(object sender, EventArgs e) {
            if (positionsList.SelectedItem != null && !mTickUpdate) {
                mGuiUpdate = true;
                mEvent.Container.Time = ((FlythroughEvent<Vector3>)positionsList.SelectedItem).StartTime;
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

        // ----- Orientations -----

        private void Orientations_CurrentEventChange(FlythroughEvent<Rotation> o, FlythroughEvent<Rotation> n) {
            Invoke(new Action(() => orientationsList.SelectedItem = n));
        }

        private void rotateToEventToolStripMenuItem_Click(object sender, EventArgs e) {
            AddEvent(new RotateToEvent(mEvent.Container, 5000));
        }

        private void blankOrientationEventToolStripItem_Click(object sender, EventArgs e) {
            AddEvent(new BlankEvent<Rotation>(mEvent.Container, 5000));
        }

        private void removeOrientationToolStripItem_Click(object sender, EventArgs e) {
            if (orientationsList.SelectedItem != null)
                RemoveEvent((FlythroughEvent<Rotation>)orientationsList.SelectedItem, orientationsList, orientationPanel);
        }

        private void moveUpOrientationToolStripItem_Click(object sender, EventArgs e) {
            if (orientationsList.SelectedItem != null)
                mEvent.Orientations.MoveUp((FlythroughEvent<Rotation>)orientationsList.SelectedItem);
        }

        private void orientationsList_SelectedValueChanged(object sender, EventArgs e) {
            if (orientationsList.SelectedItem != null && !mTickUpdate) {
                mGuiUpdate = true;
                mEvent.Container.Time = ((FlythroughEvent<Rotation>)orientationsList.SelectedItem).StartTime;
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
    }
}
