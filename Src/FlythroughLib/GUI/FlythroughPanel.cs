using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;

namespace Chimera.FlythroughLib.GUI {
    public partial class FlythroughPanel : UserControl {
        private Flythrough mContainer;
        private UserControl mCurrentPanel;
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
            mContainer.Tick += new Action<int>(mContainer_Tick);
            mContainer.LengthChange += new Action<int>(mContainer_LengthChange);
            mContainer.SequenceFinished += new EventHandler(mContainer_SequenceFinished);

            autoStepCheck.Checked = mContainer.AutoStep;
            loopCheck.Checked = mContainer.Loop;
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

        private void playButton_Click(object sender, EventArgs e) {
            if (playButton.Text == "Play") {
                playButton.Text = "Pause";
                mContainer.Play();
            } else {
                playButton.Text = "Play";
                mContainer.Pause = true;
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
            eventsList.BeginUpdate();
            eventsList.Items.Add(evt);
            eventsList.EndUpdate();
            evt.TimeChange += (e, time) => {
                if (!GUIUpdate && !IsDisposed && Created)
                    Invoke(new Action(() => { if (eventsList.SelectedItem != evt) eventsList.SelectedItem = evt; }));
            };
            evt.ControlPanel.Dock = DockStyle.Fill;
            evt.ControlPanel.Visible = false;
            eventPanel.Controls.Add(evt.ControlPanel);
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e) {
            if (eventsList.SelectedItem != null) {
                ComboEvent evt = (ComboEvent)eventsList.SelectedItem;
                eventsList.Items.Remove(evt);
                eventPanel.Controls.Remove(evt.ControlPanel);
            }
        }

        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e) {
            if (eventsList.SelectedItem != null)
                mContainer.MoveUp((ComboEvent)eventsList.SelectedItem);
        }

        private void eventsList_SelectedValueChanged(object sender, EventArgs e) {
            if (eventsList.SelectedItem != null && !TickUpdate) {
                GUIUpdate = true;
                mContainer.Time = ((ComboEvent)eventsList.SelectedItem).StartTime;
                timeLabel.Text = "Time: " + Math.Round((double) mContainer.Time / 1000.0, 2);
                UserControl panel = ((ComboEvent)eventsList.SelectedItem).ControlPanel;
                if (mCurrentPanel != panel) {
                    if (mCurrentPanel != null)
                        mCurrentPanel.Visible = false;
                    mCurrentPanel = panel;
                    mCurrentPanel.Visible = true;
                }
                GUIUpdate = false;
            }
        }

        private void timeSlider_Scroll(object sender, EventArgs e) {
            mContainer.Time = timeSlider.Value;
            timeLabel.Text = "Time: " + Math.Round((double)timeSlider.Value / 1000.0, 2);
        }
    }
}
