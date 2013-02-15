using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FlythroughLib;
using OpenMetaverse;
using UtilLib;

namespace ChimeraGUILib.Controls.FlythroughEventPanels {
    public partial class ComboSequencePanel : UserControl {
        private readonly Dictionary<string, FlythroughEvent> mEvents = new Dictionary<string, FlythroughEvent>();
        private readonly Dictionary<string, UserControl> mPanels = new Dictionary<string, UserControl>();

        private ComboEvent mEvent;
        private CameraMaster mMaster;
        private UserControl mCurrentPanel;
        private bool mSequence1;

        public ComboSequencePanel() {
            InitializeComponent();
        }

        public void Init(ComboEvent evt, bool sequence1, CameraMaster master) {
            mEvent = evt;
            mSequence1 = sequence1;
            mMaster = master;

            eventsLabel.Text = string.Format("Sequence {0} Events", sequence1 ? 1 : 2);
        }

        private void moveToEventToolStripMenuItem_Click(object sender, EventArgs e) {
            MoveToEvent evt = new MoveToEvent(mEvent.Container, 0, Vector3.Zero);
            MoveToPanel panel = new MoveToPanel(evt, mMaster);

            if (mSequence1)
                mEvent.AddStream1Event(evt);
            else
                mEvent.AddStream2Event(evt);
            AddEvent(evt, panel);
        }

        private void rotateToEventToolStripMenuItem_Click(object sender, EventArgs e) {
            RotateToEvent evt = new RotateToEvent(mEvent.Container, 0);
            RotateToPanel panel = new RotateToPanel(evt, mMaster);

            if (mSequence1)
                mEvent.AddStream1Event(evt);
            else
                mEvent.AddStream2Event(evt);
            AddEvent(evt, panel);
        }

        private void lookAtEventToolStripMenuItem_Click(object sender, EventArgs e) {
            LookAtEvent evt = new LookAtEvent(mEvent.Container, 0);
            LookAtPanel panel = new LookAtPanel(evt, mMaster);

            if (mSequence1)
                mEvent.AddStream1Event(evt);
            else
                mEvent.AddStream2Event(evt);
            AddEvent(evt, panel);
        }

        private void blankEventToolStripMenuItem_Click(object sender, EventArgs e) {
            BlankEvent evt = new BlankEvent(mEvent.Container, 0);
            BlankPanel panel = new BlankPanel(evt);

            if (mSequence1)
                mEvent.AddStream1Event(evt);
            else
                mEvent.AddStream2Event(evt);
            AddEvent(evt, panel);
        }
        
        public void AddEvent(FlythroughEvent evt, UserControl panel) {
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
            if (eventsList.SelectedItem == null)
                return;

            FlythroughEvent evt = mEvents[(string)eventsList.SelectedItem];
            UserControl panel = mPanels[(string)eventsList.SelectedItem];

            mCurrentPanel = null;
            panel.Visible = false;
            eventsList.Items.Remove(eventsList.SelectedItem);
            Controls.Remove(panel);

            mEvent.RemoveEvent(evt, mSequence1);
        }

        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e) {
            if (eventsList.SelectedItem != null) {
                mEvent.MoveUp(mEvents[(string)eventsList.SelectedItem]);
                if (eventsList.SelectedIndex > 0) {
                    object tmp = eventsList.Items[eventsList.SelectedIndex - 1];
                    eventsList.Items[eventsList.SelectedIndex - 1] = eventsList.SelectedItem;
                    eventsList.Items[eventsList.SelectedIndex] = tmp;
                }
            }
        }
    }
}
