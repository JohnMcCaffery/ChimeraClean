using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Interfaces;
using OpenMetaverse;
using Chimera.Util;
using NuiLibDotNet;
using Chimera.Kinect.Interfaces;

namespace Chimera.Kinect.GUI {
    public partial class KinectPanel : UserControl {
        private bool mGuiUpdate;
        private bool mExternalUpdate;
        private bool mStarted;
        private KinectInput mInput;
        private Dictionary<string, TabPage> mTabs = new Dictionary<string, TabPage>();

        public KinectPanel() {
            InitializeComponent();
        }

        public KinectPanel(KinectInput input)
            : this() {

            mInput = input;

            orientationPanel.Value = input.Orientation;
            positionPanel.Value = input.Position;
            startButton.Enabled = !mInput.KinectStarted;

            foreach (var window in input.Coordinator.Windows)
                AddWindow(window.Name);

            foreach (string movementController in mInput.MovementNames) {
                movementControllerPulldown.Items.Add(movementController);
                AddMovement(mInput.MovementController);
                if (movementControllerPulldown.SelectedIndex == -1)
                    movementControllerPulldown.SelectedIndex = 0;
            }
            foreach (string cursorController in mInput.CursorNames) {
                cursorControllerPulldown.Items.Add(cursorController);
                if (cursorControllerPulldown.SelectedIndex == -1)
                    cursorControllerPulldown.SelectedIndex = 0;
            }

            mInput.PositionChanged += new Action<Vector3>(mInput_PositionChanged);
            mInput.Coordinator.WindowAdded += new Action<Window,EventArgs>(Coordinator_WindowAdded);
        }

        private void Coordinator_WindowAdded(Window window, EventArgs args) {
            AddWindow(window.Name);
        }

        private void mInput_PositionChanged(Vector3 value) {
            if (!mGuiUpdate) {
                mExternalUpdate = true;
                positionPanel.Value = value;
                mExternalUpdate = false;
            }
        }

        private void positionPanel_ValueChanged(object sender, EventArgs e) {
            if (!mExternalUpdate) {
                mGuiUpdate = true;
                mInput.Position = positionPanel.Value;
                mGuiUpdate = false;
            }
        }

        private void startButton_Click(object sender, EventArgs e) {
            mInput.StartKinect();
            startButton.Enabled = false;
        }

        private void movementPulldown_SelectedIndexChanged(object sender, EventArgs e) {
            mInput.MovementController.ControlPanel.Visible = false;
            mInput.SetMovement(movementControllerPulldown.SelectedItem.ToString());
            mInput.MovementController.ControlPanel.Visible = true;
        }

        private void cursorControllerPulldown_SelectedIndexChanged(object sender, EventArgs e) {
            foreach (var window in mInput.Coordinator.Windows)
                mInput[window.Name].ControlPanel.Visible = false;

            mInput.SetCursor(cursorControllerPulldown.SelectedItem.ToString());

            foreach (var window in mInput.Coordinator.Windows) {
                if (mTabs[window.Name].Controls.Contains(mInput[window.Name].ControlPanel))
                    mInput[window.Name].ControlPanel.Visible = true;
                else
                    AddCursor(window.Name);
            }
        }

        private void AddMovement(IDeltaInput input) {
            // 
            // controlPanel
            // 
            UserControl controlPanel = input.ControlPanel;
            controlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            controlPanel.Location = new System.Drawing.Point(3, 3);
            controlPanel.Name = input.Name + "ControlPanel";
            controlPanel.Size = new System.Drawing.Size(695, 488);
            controlPanel.TabIndex = 0;
            controlPanel.Visible = false;

            movementTab.Controls.Add(controlPanel);
        }

        private void AddWindow(string windowName) {
            // 
            // windowTab
            // 
            TabPage windowTab = new TabPage();
            windowTab.Location = new System.Drawing.Point(4, 22);
            windowTab.Name = windowName + " Tab";
            windowTab.Padding = new System.Windows.Forms.Padding(3);
            windowTab.Size = new System.Drawing.Size(701, 494);
            windowTab.TabIndex = 2;
            windowTab.Text = windowName;
            windowTab.UseVisualStyleBackColor = true;
            windowTab.AutoScroll = true;
            mainTab.Controls.Add(windowTab);

            mTabs.Add(windowName, windowTab);
            AddCursor(windowName);
        }

        private void AddCursor(string windowName) {            // 
            // kinectWindowPanel
            // 
            UserControl windowPanel = mInput[windowName].ControlPanel;
            windowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            windowPanel.Location = new System.Drawing.Point(3, 3);
            windowPanel.Name = windowName + "WindowPanel";
            windowPanel.Size = new System.Drawing.Size(695, 488);
            windowPanel.TabIndex = 0;

            mTabs[windowName].Controls.Add(windowPanel);
        }
    }
}
