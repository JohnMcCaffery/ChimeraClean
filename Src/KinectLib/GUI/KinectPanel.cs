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
using Chimera.Plugins;

namespace Chimera.Kinect.GUI {
    public partial class KinectPanel : UserControl {
        private bool mGuiUpdate;
        private bool mExternalUpdate;
        private bool mStarted;
        private KinectInput mInput;
        private Dictionary<string, TabPage> mTabs = new Dictionary<string, TabPage>();

        private ChangeDelegate mKinectTick;

        public KinectPanel() {
            InitializeComponent();
        }

        public KinectPanel(KinectInput input)
            : this() {

            mInput = input;

            mKinectTick = new ChangeDelegate(Nui_Tick);

            orientationPanel.Value = input.Orientation;
            orientationPanel.Text = "Orientation (deg)";
            positionPanel.Value = input.Position / 10f;
            positionPanel.Text = "Position (cm)";
            startButton.Enabled = !mInput.KinectStarted;
            headCheck.Checked = mInput.HeadEnabled;
            headPanel.Vector = new VectorUpdater(Nui.joint(Nui.Head));

            foreach (var window in input.Coordinator.Windows)
                AddWindow(window.Name);

            foreach (var movementController in mInput.MovementControllers) {
                movementControllerPulldown.Items.Add(movementController.Name);
                AddMovement(movementController);
                if (movementControllerPulldown.SelectedIndex == -1)
                    movementControllerPulldown.SelectedIndex = 0;
            }
            foreach (var helpTrigger in mInput.HelpTriggers) {
                helpTriggerPulldown.Items.Add(helpTrigger.Name);
                AddHelpTrigger(helpTrigger);
                if (helpTriggerPulldown.SelectedIndex == -1)
                    helpTriggerPulldown.SelectedIndex = 0;
            }
            foreach (string cursorController in mInput.CursorNames) {
                cursorControllerPulldown.Items.Add(cursorController);
                if (cursorControllerPulldown.SelectedIndex == -1)
                    cursorControllerPulldown.SelectedIndex = 0;
            }

            mInput.EnabledChanged += new Action<IPlugin,bool>(mInput_EnabledChanged);
            mInput.PositionChanged += new Action<Vector3>(mInput_PositionChanged);
            mInput.Coordinator.WindowAdded += new Action<Window,EventArgs>(Coordinator_WindowAdded);

            Disposed += new EventHandler(KinectPanel_Disposed);
            HandleCreated += new EventHandler(KinectPanel_HandleCreated);
        }

        void KinectPanel_HandleCreated(object sender, EventArgs e) {
            mHead = Nui.joint(Nui.Head);
            mHandR = Nui.joint(Nui.Hand_Right);
            mHandL = Nui.joint(Nui.Hand_Left);
            mWristR = Nui.joint(Nui.Wrist_Right);
            mWristL = Nui.joint(Nui.Wrist_Left);
            mElbowR = Nui.joint(Nui.Elbow_Right);
            mElbowL = Nui.joint(Nui.Elbow_Left);
            mShoulderR = Nui.joint(Nui.Shoulder_Right);
            mShoulderL = Nui.joint(Nui.Shoulder_Left);
            mShoulderC = Nui.joint(Nui.Shoulder_Centre);
            mHipC = Nui.joint(Nui.Hip_Centre);
            mHipR = Nui.joint(Nui.Hip_Right);
            mHipL = Nui.joint(Nui.Hip_Left);
            mKneeR = Nui.joint(Nui.Knee_Right);
            mKneeL = Nui.joint(Nui.Knee_Left);
            mAnkleR = Nui.joint(Nui.Ankle_Right);
            mAnkleL = Nui.joint(Nui.Ankle_Left);
            mFootR = Nui.joint(Nui.Foot_Right);
            mFootL = Nui.joint(Nui.Foot_Left);
            Nui.Tick += mKinectTick;
        }

        void KinectPanel_Disposed(object sender, EventArgs e) {
            Nui.Tick -= mKinectTick;
        }

        private Vector mHead;
        private Vector mHandR;
        private Vector mHandL;
        private Vector mWristR;
        private Vector mWristL;
        private Vector mElbowR;
        private Vector mElbowL;
        private Vector mShoulderR;
        private Vector mShoulderL;
        private Vector mShoulderC;
        private Vector mHipC;
        private Vector mHipR;
        private Vector mHipL;
        private Vector mKneeR;
        private Vector mKneeL;
        private Vector mAnkleR;
        private Vector mAnkleL;
        private Vector mFootR;
        private Vector mFootL;
        private static readonly int R = 5;
        private Size mSize = new Size(R * 2, R * 2);

        private void Nui_Tick() {
            bool update = false;
            bool depth = true;
            Image oldFrame = null;
            Invoke(new Action(() => {
                update = mainTab.SelectedTab == frameTab;
                depth = depthFrameButton.Checked;
                oldFrame = frameImage.Image;
            }));
            if (update) {
                Bitmap frame = depth ? Nui.DepthFrame : Nui.ColourFrame;
                Func<Vector, Point> toP = v => {
                    Point p = depth ? Nui.SkeletonToDepth(v) : Nui.SkeletonToColour(v);
                    return new Point(p.X - R, p.Y - R);
                };

                if (Nui.HasSkeleton) {
                    using (Graphics g = Graphics.FromImage(frame)) {
                        using (Pen p = new Pen(Color.Red, R / 2)) {
                            g.DrawEllipse(p, new Rectangle(toP(mHead), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mHandL), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mHandR), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mWristL), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mWristR), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mElbowL), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mElbowL), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mShoulderL), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mShoulderR), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mShoulderC), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mHipC), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mHipL), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mHipR), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mKneeL), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mKneeR), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mAnkleL), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mAnkleR), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mFootL), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mFootR), mSize));
                        }
                    }
                }

                BeginInvoke(new Action(() => {
                    frameImage.Image = frame;
                    if (oldFrame != null)
                        oldFrame.Dispose();
                }));
            }
        }

            private void mInput_EnabledChanged(IPlugin input, bool enabled) {
            if (!mGuiUpdate) {
                mExternalUpdate = true;
                headCheck.Checked = mInput.HeadEnabled;
                mExternalUpdate = false;
            }
        }

        private void Coordinator_WindowAdded(Window window, EventArgs args) {
            AddWindow(window.Name);
        }

        private void mInput_PositionChanged(Vector3 value) {
            if (!mGuiUpdate) {
                mExternalUpdate = true;
                positionPanel.Value = value / 10f;
                mExternalUpdate = false;
            }
        }

        private void positionPanel_ValueChanged(object sender, EventArgs e) {
            if (!mExternalUpdate) {
                mGuiUpdate = true;
                mInput.Position = positionPanel.Value * 10f;
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

        private void helpTriggerPulldown_SelectedIndexChanged(object sender, EventArgs e) {
            mInput.HelpTrigger.ControlPanel.Visible = false;
            mInput.SetHelpTrigger(helpTriggerPulldown.SelectedItem.ToString());
            mInput.HelpTrigger.ControlPanel.Visible = true;
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

        private void AddMovement(DeltaBasedInput input) {
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

        private void AddHelpTrigger(IHelpTrigger input) {
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

            triggerTab.Controls.Add(controlPanel);
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

        private void AddCursor(string windowName) {
            // 
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

        private void headCheck_CheckedChanged(object sender, EventArgs e) {
            if (!mExternalUpdate) {
                mGuiUpdate = true;
                mInput.HeadEnabled = headCheck.Checked;
                mGuiUpdate = false;
            }
        }
    }
}
