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
using Chimera.GUI.Forms;

namespace Chimera.Flythrough.GUI {
    public partial class FlythroughPanel : UserControl {
        private FlythroughPlugin mPlugin;
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
            mPlugin = container;

            mTickListener = new Action<int>(mContainer_Tick);

            mPlugin.LengthChange += mContainer_LengthChange;
            mPlugin.UnPaused += mContainer_UnPaused;
            mPlugin.OnPaused += mContainer_OnPaused;
            mPlugin.SequenceFinished += mContainer_SequenceFinished;
            mPlugin.FlythroughLoaded += mContainer_FlythroughLoaded;
            mPlugin.FlythroughLoading += mContainer_FlythroughLoading;

            Disposed += new EventHandler(FlythroughPanel_Disposed);
            HandleCreated += new EventHandler(FlythroughPanel_HandleCreated);

            autoStepCheck.Checked = mPlugin.AutoStep;
            loopCheck.Checked = mPlugin.Loop;

            mStartEvt = new ComboEvent(mPlugin);
            mStartEvt.Name = "Begin";
            eventsList.Items.Add(mStartEvt);
            mCurrentPanel = startPanel;
        }

        void FlythroughPanel_HandleCreated(object sender, EventArgs e) {
            foreach (var evt in mPlugin.Events)
                AddEventToGUI((ComboEvent)evt);
            startPositionPanel.Value = mPlugin.Start.Position;
            startOrientationPanel.Value = mPlugin.Start.Orientation;
            timeSlider.Maximum = mPlugin.Length;
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
            mPlugin.TimeChange -= mTickListener;
        }

        void mContainer_FlythroughLoading() {
            Action a = () => {
                foreach (var evt in mPlugin.Events)
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
                foreach (var evt in mPlugin.Events)
                    AddEventToGUI((ComboEvent)evt);
                startPositionPanel.Value = mPlugin.Start.Position;
                startOrientationPanel.Value = mPlugin.Start.Orientation;
                timeSlider.Maximum = mPlugin.Length;
            });
        }

        void mContainer_SequenceFinished(object sender, EventArgs e) {
            Invoke(() => playButton.Text = "Play");
        }

        void mContainer_Tick(int time) {
            if (!GUIUpdate) {
                TickUpdate = true;
                Invoke(new Action(() => {
                    timeSlider.Maximum = mPlugin.Length;
                    timeSlider.Value = time;
                    timeLabel.Text = "Time: " + Math.Round((double) time / 1000.0, 2);
                    playButton.Text = mPlugin.Paused ? "Play" : "Pause";
                    loopCheck.Checked = mPlugin.Loop;
                    autoStepCheck.Checked = mPlugin.AutoStep;
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
                if (!GUIUpdate && !IsDisposed && Created && mPlugin.Time > 0)
                    Invoke(new Action(() => { if (eventsList.SelectedItem != evt) eventsList.SelectedItem = evt; }));
            };
            evt.ControlPanel.Dock = DockStyle.Fill;
            evt.ControlPanel.Visible = false;
            eventPanel.Controls.Add(evt.ControlPanel);
        }

        private void playButton_Click(object sender, EventArgs e) {
            if (playButton.Text == "Play") {
                playButton.Text = "Pause";
                mPlugin.Play();
            } else {
                playButton.Text = "Play";
                mPlugin.Paused = true;
            }
        }

        private void autoStepBox_CheckedChanged(object sender, EventArgs e) {
            mPlugin.AutoStep = autoStepCheck.Checked;
        }

        private void loopCheck_CheckedChanged(object sender, EventArgs e) {
            mPlugin.Loop = loopCheck.Checked;
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs args) {
            ComboEvent evt = new ComboEvent(mPlugin);
            mPlugin.AddEvent(evt);
            AddEventToGUI(evt);
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e) {
            if (eventsList.SelectedIndex > 0) {
                ComboEvent evt = (ComboEvent)eventsList.SelectedItem;
                mPlugin.RemoveEvent(evt);
                eventsList.Items.Remove(evt);
                eventPanel.Controls.Remove(evt.ControlPanel);
            }
        }

        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e) {
            if (eventsList.SelectedIndex > 1)
                mPlugin.MoveUp((ComboEvent)eventsList.SelectedItem);
        }

        private void eventsList_SelectedValueChanged(object sender, EventArgs e) {
            if (eventsList.SelectedIndex > 0 && !TickUpdate) {
                GUIUpdate = true;
                ComboEvent evt = (ComboEvent) eventsList.SelectedItem;
                //mContainer.Time = evt.GlobalStartTime;
                timeLabel.Text = "Time: " + Math.Round((double) mPlugin.Time / 1000.0, 2);
                timeSlider.Value = mPlugin.Time;
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
            mPlugin.Paused = true;
            mPlugin.Time = timeSlider.Value;
            timeLabel.Text = "Time: " + Math.Round((double)timeSlider.Value / 1000.0, 2);
            playButton.Text = "Play";
        }

        private void startButton_Click(object sender, EventArgs e) {
            mPlugin.Time = 0;
            mPlugin.Play();
            playButton.Text = "Pause";
        }

        private void startPositionPanel_OnChange(object sender, EventArgs e) {
            mPlugin.Start = new Camera(startPositionPanel.Value, new Rotation(mPlugin.Core.Orientation));
            mPlugin.Time = 0;
        }

        private void startOrientationPanel_OnChange(object sender, EventArgs e) {
            mPlugin.Start = new Camera(mPlugin.Core.Position, startOrientationPanel.Value);
            mPlugin.Time = 0;
        }

        private void saveButton_Click(object sender, EventArgs e) {
            if (saveSequenceDialog.ShowDialog(this) == DialogResult.OK) {
                mPlugin.Save(saveSequenceDialog.FileName);
            }
        }

        private void loadButton_Click(object sender, EventArgs e) {
            if (loadSequenceDialog.ShowDialog(this) == DialogResult.OK)
                mPlugin.Load(loadSequenceDialog.FileName);
        }

        private void stepBackButton_Click(object sender, EventArgs e) {
            if (mPlugin.Time > 0)
                mPlugin.Time--;
        }

        private void stepForwardButton_Click(object sender, EventArgs e) {
            if (mPlugin.Time < mPlugin.Length - 1)
                mPlugin.Time++;
        }

        private void currentPositionButton_Click(object sender, EventArgs e) {
            if (mPlugin != null)
                startPositionPanel.Value = mPlugin.Core.Position;
        }

        private void takeOrientationButton_Click(object sender, EventArgs e) {
            if (mPlugin != null)
                startOrientationPanel.Value = new Rotation(mPlugin.Core.Orientation);
        }

        private void takeCurrentCameraButton_Click(object sender, EventArgs e) {
            if (mPlugin != null) {
                Rotation orientation = mPlugin.Core.Orientation;
                startPositionPanel.Value = mPlugin.Core.Position;
                startOrientationPanel.Value = new Rotation(orientation);
            }
        }

        private void eventsList_DoubleClick(object sender, EventArgs e) {
            if (eventsList.SelectedIndex > 0)
                mPlugin.Time = ((FlythroughEvent<Camera>)eventsList.SelectedItem).SequenceStartTime;
            else if (eventsList.SelectedIndex == 0)
                mPlugin.Time = 0;
        }

        private void stepButton_Click(object sender, EventArgs e) {
            mPlugin.Step();
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
                mPlugin.TimeChange += mTickListener;
            } else {
                mPlugin.TimeChange -= mTickListener;
            }
        }

        private void synchLengthsCheck_CheckedChanged(object sender, EventArgs e) {
            mPlugin.SynchLengths = synchLengthsCheck.Checked;
        }

        private StatisticsForm mStatsForm;

        private void statsButton_Click(object sender, EventArgs e) {
            if (mStatsForm == null) {
                mStatsForm = new StatisticsForm(mPlugin.Core, mPlugin.Statistics);
                mStatsForm.FormClosed += new FormClosedEventHandler(mStatsForm_FormClosed);
                mStatsForm.Show(this);
            } else
                mStatsForm.Close();
        }

        void mStatsForm_FormClosed(object sender, FormClosedEventArgs e) {
            mStatsForm = null;
        }
    }
}
