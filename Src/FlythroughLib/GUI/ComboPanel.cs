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
        private bool mGuiUpdate;

        public ComboPanel() {
            InitializeComponent();
        }

        public ComboPanel(ComboEvent evt)
            : this() {
            mEvent = evt;
            mEvent.Positions.CurrentEventChange += new Action<FlythroughEvent<Vector3>,FlythroughEvent<Vector3>>(Positions_CurrentEventChange);
            mEvent.Orientations.CurrentEventChange += new Action<FlythroughEvent<Rotation>,FlythroughEvent<Rotation>>(Orientations_CurrentEventChange);

            foreach (var e in mEvent.Positions) {
                AddEvent(e, positionsList, positionPanel);
            } foreach (var e in mEvent.Orientations) {
                AddEvent(e, orientationsList, orientationPanel);
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
            evt.TimeChange += (e, time) => {
                if (!mGuiUpdate && !IsDisposed && Created)
                Invoke(new Action(() => { if (list.SelectedItem != evt) list.SelectedItem = evt; }));
            };
            evt.ControlPanel.Dock = DockStyle.Fill;
            panel.Controls.Add(evt.ControlPanel);
            list.BeginUpdate();
            list.Items.Add(evt);
            list.SelectedIndex = list.Items.Count - 1;
            list.EndUpdate();
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
                RemoveEvent((FlythroughEvent<Vector3>) positionsList.SelectedItem);
        }

        private void moveUpPositionToolStripMenuItem_Click(object sender, EventArgs e) {
            if (positionsList.SelectedItem != null && positionsList.SelectedIndex > 0) {
                FlythroughEvent<Vector3> up = (FlythroughEvent<Vector3>)positionsList.SelectedItem;
                positionsList.Items[positionsList.SelectedIndex] = positionsList.Items[positionsList.SelectedIndex - 1];
                positionsList.Items[positionsList.SelectedIndex - 1] = up;
                mEvent.Positions.MoveUp(up);
                positionsList.SelectedIndex--;
            }
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

        // ----- Orientations -----

        private void Orientations_CurrentEventChange(FlythroughEvent<Rotation> o, FlythroughEvent<Rotation> n) {
            Invoke(new Action(() => orientationsList.SelectedItem = n));
        }

        private void rotateToEventToolStripMenuItem_Click(object sender, EventArgs e) {
            AddEvent(new RotateToEvent(mEvent.Container, 5000));
        }

        private void lookAtEventToolStripMenuItem_Click(object sender, EventArgs e) {
            AddEvent(new LookAtEvent(mEvent.Container, 5000));
        }

        private void blankOrientationEventToolStripItem_Click(object sender, EventArgs e) {
            AddEvent(new BlankEvent<Rotation>(mEvent.Container, 5000));
        }

        private void removeOrientationToolStripItem_Click(object sender, EventArgs e) {
            if (orientationsList.SelectedItem != null)
                RemoveEvent((FlythroughEvent<Rotation>)orientationsList.SelectedItem);
        }

        private void moveUpOrientationToolStripItem_Click(object sender, EventArgs e) {
            if (orientationsList.SelectedItem != null) {
                FlythroughEvent<Rotation> up = (FlythroughEvent<Rotation>)orientationsList.SelectedItem;
                orientationsList.Items[orientationsList.SelectedIndex] = orientationsList.Items[orientationsList.SelectedIndex - 1];
                orientationsList.Items[orientationsList.SelectedIndex - 1] = up;
                mEvent.Orientations.MoveUp(up);
                orientationsList.SelectedIndex--;
            }
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

        private void positionStartButton_Click(object sender, EventArgs e) {
            if (positionsList.SelectedItem != null)
                mEvent.Container.Time = ((FlythroughEvent<Vector3>)positionsList.SelectedItem).GlobalStartTime;
        }

        private void positionEndButton_Click(object sender, EventArgs e) {
            if (positionsList.SelectedItem != null)
                mEvent.Container.Time = ((FlythroughEvent<Vector3>)positionsList.SelectedItem).GlobalFinishTime;
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
                mEvent.Container.Time = ((FlythroughEvent<Vector3>)positionsList.SelectedItem).GlobalStartTime;
        }

        private void orientationsList_DoubleClick(object sender, EventArgs e) {
            if (orientationsList.SelectedItem != null)
                mEvent.Container.Time = ((FlythroughEvent<Rotation>)orientationsList.SelectedItem).GlobalStartTime;
        }

        private void copyCurrentPositionButton_Click(object sender, EventArgs e) {
            AddEvent(new MoveToEvent(mEvent.Container, 5000, mEvent.Container.Coordinator.Position));
        }

        private void copyCurrentOrientationButton_Click(object sender, EventArgs e) {
            AddEvent(new RotateToEvent(mEvent.Container, 5000, mEvent.Container.Coordinator.Orientation));
        }

        private void copyCurrentPairButton_Click(object sender, EventArgs e) {
            AddEvent(new MoveToEvent(mEvent.Container, 5000, mEvent.Container.Coordinator.Position));
            AddEvent(new RotateToEvent(mEvent.Container, 5000, new Rotation(mEvent.Container.Coordinator.Orientation)));
        }
    }
}
