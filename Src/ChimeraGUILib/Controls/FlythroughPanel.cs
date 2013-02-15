using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilLib;
using OpenMetaverse;
using ChimeraGUILib.Controls.FlythroughEventPanels;

namespace FlythroughLib.Panels {
    public partial class FlythroughPanel : UserControl {
        private readonly Dictionary<string, FlythroughEvent> mEvents = new Dictionary<string, FlythroughEvent>();
        private readonly Dictionary<string, UserControl> mPanels = new Dictionary<string, UserControl>();

        private FlythroughManager mContainer = new FlythroughManager();
        private CameraMaster mMaster;
        private UserControl mCurrentPanel;

        public FlythroughPanel() {
            InitializeComponent();

            mContainer.OnPositionChange += (source, args) => mMaster.Position = mContainer.Position;
            mContainer.OnComplete += (source, args) => Invoke(new Action(() => playButton.Enabled = true));
            mContainer.OnRotationChange += (source, args) => {
                mMaster.Rotation.Pitch = mContainer.Rotation.Pitch;
                mMaster.Rotation.Yaw = mContainer.Rotation.Yaw;
            };
        }

        public CameraMaster Master {
            get { return mMaster; }
            set { mMaster = value; }
        }

        private void playButton_Click(object sender, EventArgs e) {
            mContainer.Play(mMaster.Position, mMaster.Rotation);
            playButton.Enabled = false;
        }

        private void moveToEventToolStripMenuItem_Click(object sender, EventArgs e) {
            MoveToEvent evt = new MoveToEvent(mContainer, 0, Vector3.Zero);
            MoveToPanel panel = new MoveToPanel(evt, mMaster);
            AddEvent(evt, panel);
        }

        private void comboEventToolStripMenuItem_Click(object sender, EventArgs e) {
            ComboEvent evt = new ComboEvent(mContainer);
            ComboPanel panel = new ComboPanel(evt, mMaster);
            AddEvent(evt, panel);
        }

        private void rotateToEventToolStripMenuItem_Click(object sender, EventArgs e) {
            RotateToEvent evt = new RotateToEvent(mContainer, 0);
            RotateToPanel panel = new RotateToPanel(evt, mMaster);
            AddEvent(evt, panel);
        }

        private void lookAtEventToolStripMenuItem_Click(object sender, EventArgs e) {
            LookAtEvent evt = new LookAtEvent(mContainer, 0);
            LookAtPanel panel = new LookAtPanel(evt, mMaster);
            AddEvent(evt, panel);
            mContainer.AddEvent(evt);
        }
        
        private void AddEvent(FlythroughEvent evt, UserControl panel) {
            mEvents.Add(evt.Name, evt);
            mPanels.Add(evt.Name, panel);
            eventsList.Items.Add(evt.Name);

            int left = eventsList.Width + eventsList.Location.X;
            panel.Size = new System.Drawing.Size(Width - left, Height);
            panel.Location = new Point(left, 0);
            panel.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            Controls.Add(panel);

            if (mCurrentPanel != null)
                mCurrentPanel.Visible = false;
            mCurrentPanel = panel;
        }

        private void eventsList_SelectedValueChanged(object sender, EventArgs e) {
            if (eventsList.SelectedItem == null) {
                mCurrentPanel = null;
                return;
            }
            if (mCurrentPanel != null)
                mCurrentPanel.Visible = false;
            mCurrentPanel = mPanels[(string) eventsList.SelectedItem];
            mCurrentPanel.Visible = true;
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e) {
            FlythroughEvent evt = mEvents[(string)eventsList.SelectedItem];
            UserControl panel = mPanels[(string)eventsList.SelectedItem];

            mCurrentPanel = null;
            panel.Visible = false;
            eventsList.Items.Remove(eventsList.SelectedItem);
            Controls.Remove(panel);

            mContainer.RemoveEvent(evt);
        }

        private void loadButton_Click(object sender, EventArgs e) {
            if (loadSequenceDialog.ShowDialog(this) == DialogResult.OK) {
                mContainer.Load(loadSequenceDialog.FileName);

                FlythroughEvent evt = mContainer.FirstEvent;
                while (evt != null) {
                    UserControl panel = GetPanel(evt);
                    if (panel != null)
                        AddEvent(evt, panel);

                    if (evt is ComboEvent) {
                        ComboEvent combo = (ComboEvent)evt;
                        ComboPanel comboPanel = (ComboPanel)panel;
                        FlythroughEvent streamEvent = combo.Stream1First;
                        while (streamEvent != null) {
                            UserControl p = GetPanel(streamEvent);
                            if (p != null)
                                comboPanel.AddEvent(evt, p, true);
                        }
                        streamEvent = combo.Stream2First;
                        while (streamEvent != null) {
                            UserControl p = GetPanel(streamEvent);
                            if (p != null)
                                comboPanel.AddEvent(evt, p, true);
                        }
                    }
                    evt = evt.NextEvent;
                }
            }
        }


        private UserControl GetPanel(FlythroughEvent evt) {
            if (evt is ComboEvent)
                return new ComboPanel((ComboEvent)evt, Master);
            if (evt is RotateToEvent)
                return new RotateToPanel((RotateToEvent)evt, Master);
            if (evt is MoveToEvent)
                return new MoveToPanel((MoveToEvent)evt, Master);
            if (evt is LookAtEvent)
                return new LookAtPanel((LookAtEvent)evt, Master);
            return null;
        }

        private void saveButton_Click(object sender, EventArgs e) {
            if (saveSequenceDialog.ShowDialog(this) == DialogResult.OK) {
                mContainer.Save(saveSequenceDialog.FileName);
            }
        }
    }
}
