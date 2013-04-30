﻿/*************************************************************************
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
using Chimera.Flythrough;
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
            evt.OnStart += (source, args) => {
                Invoke(new Action(() => {
                    if (eventsList.Items.Count > 0 && (mSequence1 ? 
                            mEvent.Stream1First == mEvent.Stream1Current : 
                            mEvent.Stream2First == mEvent.Stream2Current))
                        eventsList.SelectedIndex = 0;
                }));
            };
            evt.OnNextEvent += (source, seq1) => {
                Invoke(new Action(() => {
                    if (seq1 == mSequence1 && eventsList.SelectedIndex < (eventsList.Items.Count - 1))
                        eventsList.SelectedIndex++;
                }));
            };
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
            if (mSequence1)
                mEvent.Stream1Current = mEvents[(string)eventsList.SelectedItem];
            else
                mEvent.Stream2Current = mEvents[(string)eventsList.SelectedItem];
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
                    eventsList.SelectedIndex = eventsList.SelectedIndex - 1;
                }
            }
        }

        private void resetButton_Click(object sender, EventArgs e) {
            if (mSequence1)
                mEvent.Stream1Current.Reset();
            else
                mEvent.Stream2Current.Reset();
        }
    }
}
