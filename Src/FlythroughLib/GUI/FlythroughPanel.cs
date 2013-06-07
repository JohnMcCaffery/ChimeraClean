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
    public partial class FlythroughPanel : UserControl {
        private FlythroughPlugin mContainer;
        private Control mCurrentPanel;
        private ComboEvent mStartEvt;
        private bool GUIUpdate;
        private bool TickUpdate;

        private Action<int> mTickListener;

        public FlythroughPanel() {
            InitializeComponent();
        }

        public FlythroughPanel(FlythroughPlugin container)
            : this() {

            Init(container);
        }

        public void Init(FlythroughPlugin container) {
            mContainer = container;

            mTickListener = new Action<int>(mContainer_Tick);

            mContainer.LengthChange += mContainer_LengthChange;
            mContainer.UnPaused += mContainer_UnPaused;
            mContainer.OnPaused += mContainer_OnPaused;
            mContainer.SequenceFinished += mContainer_SequenceFinished;
            mContainer.FlythroughLoaded += mContainer_FlythroughLoaded;
            mContainer.FlythroughLoading += mContainer_FlythroughLoading;

            Disposed += new EventHandler(FlythroughPanel_Disposed);
            HandleCreated += new EventHandler(FlythroughPanel_HandleCreated);

            autoStepCheck.Checked = mContainer.AutoStep;
            loopCheck.Checked = mContainer.Loop;

            mStartEvt = new ComboEvent(mContainer);
            mStartEvt.Name = "Begin";
            eventsList.Items.Add(mStartEvt);
            mCurrentPanel = startPanel;
        }

        void FlythroughPanel_HandleCreated(object sender, EventArgs e) {
            foreach (var evt in mContainer.Events)
                AddEventToGUI((ComboEvent)evt);
            startPositionPanel.Value = mContainer.Start.Position;
            startOrientationPanel.Value = mContainer.Start.Orientation;
            timeSlider.Maximum = mContainer.Length;
        }

        void mContainer_UnPaused() {
            Invoke(() => playButton.Text = "Pause");
        }

        void mContainer_OnPaused() {
            Invoke(() => playButton.Text = "Play");
        }

        private void Invoke(Action a) {
            if (!InvokeRequired)
                a();
            else if (Created && !IsDisposed && !Disposing)
                base.BeginInvoke(a);
        }

        void FlythroughPanel_Disposed(object sender, EventArgs e) {
            mContainer.TimeChange -= mTickListener;
        }

        void mContainer_FlythroughLoading() {
            Action a = () => {
                foreach (var evt in mContainer.Events)
                    eventsList.Items.Remove(evt);
            };
            if (InvokeRequired)
                Invoke(a);
            else
                a();
        }

        void mContainer_FlythroughLoaded() {
            Invoke(() => {
                //timeSlider.Maximum = mContainer.Length;
                foreach (var evt in mContainer.Events)
                    AddEventToGUI((ComboEvent)evt);
                startPositionPanel.Value = mContainer.Start.Position;
                startOrientationPanel.Value = mContainer.Start.Orientation;
                timeSlider.Maximum = mContainer.Length;
            });
        }

        void mContainer_SequenceFinished(object sender, EventArgs e) {
            Invoke(() => playButton.Text = "Play");
        }

        void mContainer_Tick(int time) {
            if (!GUIUpdate) {
                TickUpdate = true;
                Invoke(new Action(() => {
                    timeSlider.Maximum = mContainer.Length;
                    timeSlider.Value = time;
                    timeLabel.Text = "Time: " + Math.Round((double) time / 1000.0, 2);
                    playButton.Text = mContainer.Paused ? "Play" : "Pause";
                    loopCheck.Checked = mContainer.Loop;
                    autoStepCheck.Checked = mContainer.AutoStep;
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
            if (loadSequenceDialog.ShowDialog(this) == DialogResult.OK)
                mContainer.Load(loadSequenceDialog.FileName);
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
                Rotation orientation = mContainer.Coordinator.Orientation;
                startPositionPanel.Value = mContainer.Coordinator.Position;
                startOrientationPanel.Value = new Rotation(orientation);
            }
        }

        private void eventsList_DoubleClick(object sender, EventArgs e) {
            if (eventsList.SelectedIndex > 0)
                mContainer.Time = ((FlythroughEvent<Camera>)eventsList.SelectedItem).SequenceStartTime;
            else if (eventsList.SelectedIndex == 0)
                mContainer.Time = 0;
        }

        private void stepButton_Click(object sender, EventArgs e) {
            mContainer.Step();
        }

        private TabPage mPage;
        private TabControl mTabContainer;

        private void FlythroughPanel_Load(object sender, EventArgs e) {
            mPage = (TabPage)Parent;
            mTabContainer = (TabControl)Parent.Parent;

            mTabContainer.TabIndexChanged += new EventHandler(mTabContainer_TabIndexChanged);
            mTabContainer_TabIndexChanged(mTabContainer, null);
        }

        void mTabContainer_TabIndexChanged(object sender, EventArgs e) {
            if (mPage == mTabContainer.SelectedTab) {
                mContainer.TimeChange += mTickListener;
            } else {
                mContainer.TimeChange -= mTickListener;
            }
        }
    }
}
