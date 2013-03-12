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
    public partial class FlythroughPanel : UserControl {
        private Flythrough mContainer;
        private Control mCurrentPanel;
        private bool GUIUpdate;
        private bool TickUpdate;


        public FlythroughPanel() {
            InitializeComponent();
        }

        public FlythroughPanel(Flythrough container)
            : this() {

            Init(container);
        }

        public void Init(Flythrough container) {
            mContainer = container;
            mContainer.TimeChange += new Action<int>(mContainer_Tick);
            mContainer.LengthChange += new Action<int>(mContainer_LengthChange);
            mContainer.SequenceFinished += new EventHandler(mContainer_SequenceFinished);

            autoStepCheck.Checked = mContainer.AutoStep;
            loopCheck.Checked = mContainer.Loop;

            ComboEvent startEvt = new ComboEvent(mContainer);
            startEvt.Name = "Start";
            eventsList.Items.Add(startEvt);
            mCurrentPanel = startPanel;
        }

        void mContainer_SequenceFinished(object sender, EventArgs e) {
            Invoke(new Action(() => playButton.Text = "Play"));
        }

        void mContainer_Tick(int time) {
            if (!GUIUpdate) {
                TickUpdate = true;
                Invoke(new Action(() => {
                    timeSlider.Value = time;
                    timeLabel.Text = "Time: " + Math.Round((double) time / 1000.0, 2);
                }));
                TickUpdate = false;
            }
        }

        void mContainer_LengthChange(int length) {
            Invoke(new Action(() => {
                timeSlider.Maximum = length;
                lengthLabel.Text = "Length: " + Math.Round((double) length / 1000.0, 2);
            }));
        }

        private void AddEventToGUI(FlythroughEvent<Camera> evt) {
            eventsList.BeginUpdate();
            eventsList.Items.Add(evt);
            eventsList.EndUpdate();
            evt.TimeChange += (e, time) => {
                if (!GUIUpdate && !IsDisposed && Created && mContainer.Time > 0)
                    Invoke(new Action(() => { if (eventsList.SelectedItem != evt) eventsList.SelectedItem = evt; }));
            };
            evt.ControlPanel.Dock = DockStyle.Fill;
            evt.ControlPanel.Visible = false;
            eventPanel.Controls.Add(evt.ControlPanel);
        }

        private void playButton_Click(object sender, EventArgs e) {
            if (playButton.Text == "Play") {
                playButton.Text = "Pause";
                mContainer.Play();
            } else {
                playButton.Text = "Play";
                mContainer.Paused = true;
            }
        }

        private void autoStepBox_CheckedChanged(object sender, EventArgs e) {
            mContainer.AutoStep = autoStepCheck.Checked;
        }

        private void loopCheck_CheckedChanged(object sender, EventArgs e) {
            mContainer.Loop = loopCheck.Checked;
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs args) {
            ComboEvent evt = new ComboEvent(mContainer);
            mContainer.AddEvent(evt);
            AddEventToGUI(evt);
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e) {
            if (eventsList.SelectedIndex > 0) {
                ComboEvent evt = (ComboEvent)eventsList.SelectedItem;
                mContainer.RemoveEvent(evt);
                eventsList.Items.Remove(evt);
                eventPanel.Controls.Remove(evt.ControlPanel);
            }
        }

        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e) {
            if (eventsList.SelectedIndex > 1)
                mContainer.MoveUp((ComboEvent)eventsList.SelectedItem);
        }

        private void eventsList_SelectedValueChanged(object sender, EventArgs e) {
            if (eventsList.SelectedIndex > 0 && !TickUpdate) {
                GUIUpdate = true;
                ComboEvent evt = (ComboEvent) eventsList.SelectedItem;
                //mContainer.Time = evt.GlobalStartTime;
                timeLabel.Text = "Time: " + Math.Round((double) mContainer.Time / 1000.0, 2);
                timeSlider.Value = mContainer.Time;
                UserControl panel = evt.ControlPanel;
                if (mCurrentPanel != panel) {
                    if (mCurrentPanel != null)
                        mCurrentPanel.Visible = false;
                    mCurrentPanel = panel;
                    mCurrentPanel.Visible = true;
                }
                GUIUpdate = false;
            } else if (!TickUpdate && mCurrentPanel != startPanel) {
                if (mCurrentPanel != null)
                    mCurrentPanel.Visible = false;
                mCurrentPanel = startPanel;
                mCurrentPanel.Visible = true;
                timeSlider.Value = 0;
            }
        }

        private void timeSlider_Scroll(object sender, EventArgs e) {
            mContainer.Paused = true;
            mContainer.Time = timeSlider.Value;
            timeLabel.Text = "Time: " + Math.Round((double)timeSlider.Value / 1000.0, 2);
            playButton.Text = "Play";
        }

        private void startButton_Click(object sender, EventArgs e) {
            mContainer.Time = 0;
            mContainer.Play();
            playButton.Text = "Pause";
        }

        private void startPositionPanel_OnChange(object sender, EventArgs e) {
            mContainer.Start = new Camera(startPositionPanel.Value, new Rotation(mContainer.Coordinator.Orientation));
            mContainer.Time = 0;
        }

        private void startOrientationPanel_OnChange(object sender, EventArgs e) {
            mContainer.Start = new Camera(mContainer.Coordinator.Position, startOrientationPanel.Value);
            mContainer.Time = 0;
        }

        private void saveButton_Click(object sender, EventArgs e) {
            if (saveSequenceDialog.ShowDialog(this) == DialogResult.OK) {
                mContainer.Save(saveSequenceDialog.FileName);
            }
        }

        private void loadButton_Click(object sender, EventArgs e) {
            if (loadSequenceDialog.ShowDialog(this) == DialogResult.OK) {
                int i = 1;
                foreach (var evt in mContainer.Events) {
                    eventsList.Items.RemoveAt(i);
                    eventPanel.Controls.Remove(evt.ControlPanel);
                    i++;
                }
                mContainer.Load(loadSequenceDialog.FileName);
                //timeSlider.Maximum = mContainer.Length;
                startPositionPanel.Value = mContainer.Start.Position;
                startOrientationPanel.Value = mContainer.Start.Orientation;
                foreach (var evt in mContainer.Events)
                    AddEventToGUI((ComboEvent) evt);
            }
        }

        private void stepBackButton_Click(object sender, EventArgs e) {
            if (mContainer.Time > 0)
                mContainer.Time--;
        }

        private void stepForwardButton_Click(object sender, EventArgs e) {
            if (mContainer.Time < mContainer.Length - 1)
                mContainer.Time++;
        }

        private void currentPositionButton_Click(object sender, EventArgs e) {
            if (mContainer != null)
                startPositionPanel.Value = mContainer.Coordinator.Position;
        }

        private void takeOrientationButton_Click(object sender, EventArgs e) {
            if (mContainer != null)
                startOrientationPanel.Value = new Rotation(mContainer.Coordinator.Orientation);
        }

        private void takeCurrentCameraButton_Click(object sender, EventArgs e) {
            if (mContainer != null) {
                startPositionPanel.Value = mContainer.Coordinator.Position;
                startOrientationPanel.Value = new Rotation(mContainer.Coordinator.Orientation);
            }
        }

        private void eventsList_DoubleClick(object sender, EventArgs e) {
            if (eventsList.SelectedIndex > 0)
                mContainer.Time = ((FlythroughEvent<Camera>)eventsList.SelectedItem).SequenceStartTime;
            else if (eventsList.SelectedIndex == 0)
                mContainer.Time = 0;
        }
    }
}
