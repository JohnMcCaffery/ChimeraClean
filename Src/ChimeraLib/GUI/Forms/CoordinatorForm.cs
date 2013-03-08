using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;
using Chimera.Util;
using Chimera.GUI.Controls;
using System.Threading;

namespace Chimera.GUI.Forms {
    public partial class CoordinatorForm : Form {
        private bool mGuiUpdate;
        private bool mEventUpdate;
        private Coordinator mCoordinator;

        public CoordinatorForm() {
            InitializeComponent();
        }

        public CoordinatorForm(Coordinator coordinator)
            : this() {
            Init(coordinator);
        }

        public void Init(Coordinator coordinator) {
            mCoordinator = coordinator;

            mCoordinator.CameraUpdated += new Action<Coordinator,CameraUpdateEventArgs>(mCoordinator_CameraUpdated);
            mCoordinator.EyeUpdated += new Action<Coordinator,EventArgs>(mCoordinator_EyeUpdated);
            mCoordinator.Closed += new Action<Coordinator,KeyEventArgs>(mCoordinator_Closed);

            foreach (var window in mCoordinator.Windows) {
                // 
                // windowPanel
                // 
                WindowPanel windowPanel = new WindowPanel(window);
                windowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
                windowPanel.Location = new System.Drawing.Point(3, 3);
                windowPanel.Name = window.Name + "Panel";
                windowPanel.Size = new System.Drawing.Size(401, 233);
                windowPanel.TabIndex = 0;
                // 
                // windowTab
                // 
                TabPage windowTab = new System.Windows.Forms.TabPage();
                windowTab.Controls.Add(windowPanel);
                windowTab.Location = new System.Drawing.Point(4, 22);
                windowTab.Name = window.Name + "Tab";
                windowTab.Padding = new System.Windows.Forms.Padding(3);
                windowTab.Size = new System.Drawing.Size(407, 239);
                windowTab.TabIndex = 0;
                windowTab.Text = window.Name;
                windowTab.UseVisualStyleBackColor = true;

                windowsTab.Controls.Add(windowTab);
            }
        }

        private void mCoordinator_CameraUpdated(Coordinator coordinator, CameraUpdateEventArgs args) {
            if (!mGuiUpdate) {
                mEventUpdate = true;
                virtualPositionPanel.Value = args.position;
                virtualRotationPanel.Pitch = args.rotation.Pitch;
                virtualRotationPanel.Yaw = args.rotation.Yaw;
                mEventUpdate = false;
            }
        }
        private void mCoordinator_EyeUpdated(Coordinator coordinator, EventArgs args) {
            if (!mGuiUpdate) {
                mEventUpdate = true;
                eyePositionPanel.Value = coordinator.EyePosition;
                mEventUpdate = false;
            }
        }

        private void virtualPositionPanel_OnChange(object sender, EventArgs e) {
            if (!mEventUpdate) {
                mGuiUpdate = true;
                mCoordinator.Update(virtualPositionPanel.Value, Vector3.Zero, new Rotation(virtualRotationPanel.Pitch, virtualRotationPanel.Yaw), new Rotation());
                mGuiUpdate = false;
            }
        }

        private void virtualRotation_OnChange(object sender, EventArgs e) {
            if (!mEventUpdate) {
                mGuiUpdate = true;
                mCoordinator.Update(virtualPositionPanel.Value, Vector3.Zero, new Rotation(virtualRotationPanel.Pitch, virtualRotationPanel.Yaw), new Rotation());
                mGuiUpdate = false;
            }
        }

        private void eyePositionPanel_OnChange(object sender, EventArgs e) {
            if (!mEventUpdate) {
                mGuiUpdate = true;
                mCoordinator.EyePosition = eyePositionPanel.Value;
                mGuiUpdate = false;
            }
        }

        private void testButton_Click(object sender, EventArgs e) {
            if (mCoordinator != null) {
                throw new Exception("Test Exception");
                //mCoordinator.Update(
                    //mCoordinator.Position + new Vector3(5f, 5f, 5f),
                    //Vector3.Zero,
                    //new Rotation(mCoordinator.Rotation.Pitch + 5, mCoordinator.Rotation.Yaw + 5),
                    //new Rotation());
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            new Thread(() => { throw new Exception("Crashy crashy. Not from GUI."); }).Start();
        }

        private void mCoordinator_Closed(Coordinator coordinator, EventArgs args) {
            if (!mGuiUpdate) {
                mEventUpdate = true;
                Close();
                mEventUpdate = false;
            }
        }

        private void CoordinatorForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (mCoordinator != null) {
                mGuiUpdate = true;
                mCoordinator.Close();
                mGuiUpdate = false;
            }
        }

        private void CoordinatorForm_KeyDown(object sender, KeyEventArgs e) {
            if (mCoordinator != null)
                mCoordinator.TriggerKeyboard(true, e);
        }

        private void CoordinatorForm_KeyUp(object sender, KeyEventArgs e) {
            if (mCoordinator != null)
                mCoordinator.TriggerKeyboard(false, e);
        }
    }
}
