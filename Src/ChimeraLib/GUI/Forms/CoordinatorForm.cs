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
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Chimera.GUI.Forms {
    public partial class CoordinatorForm : Form {
        private bool mGuiUpdate;
        private bool mEventUpdate;
        private Coordinator mCoordinator;
        private Bitmap mHeightmap;

        private Action<Coordinator, CameraUpdateEventArgs> mCameraUpdated;
        private Action<Coordinator, EventArgs> mEyeUpdated;
        private Action<Coordinator, KeyEventArgs> mClosed;
        private EventHandler<HeightmapChangedEventArgs> mHeightmapChanged;

        public CoordinatorForm() {
            InitializeComponent();
        }

        public CoordinatorForm(Coordinator coordinator)
            : this() {
            Init(coordinator);
        }

        public void Init(Coordinator coordinator) {
            mCoordinator = coordinator;

            Disposed += new EventHandler(CoordinatorForm_Disposed);

            mCameraUpdated = new Action<Coordinator, CameraUpdateEventArgs>(mCoordinator_CameraUpdated);
            mEyeUpdated = new Action<Coordinator, EventArgs>(mCoordinator_EyeUpdated);
            mClosed = new Action<Coordinator, KeyEventArgs>(mCoordinator_Closed);
            mHeightmapChanged = new EventHandler<HeightmapChangedEventArgs>(mCoordinator_HeightmapChanged);

            mCoordinator.CameraUpdated += mCameraUpdated;
            mCoordinator.EyeUpdated += mEyeUpdated;
            mCoordinator.Closed += mClosed;
            mCoordinator.HeightmapChanged += mHeightmapChanged;

            Rotation orientation = new Rotation(mCoordinator.Orientation);
            virtualPositionPanel.Value = mCoordinator.Position;
            virtualOrientationPanel.Value = orientation;
            eyePositionPanel.Value = mCoordinator.EyePosition;

            mHeightmap = new Bitmap(mCoordinator.Heightmap.GetLength(0), mCoordinator.Heightmap.GetLength(1), PixelFormat.Format24bppRgb);

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

            foreach (var input in mCoordinator.Inputs) {
                TabPage inputTab = new TabPage();
                CheckBox enableCheck = new CheckBox();
                // 
                // inputTab
                // 
                inputTab.Controls.Add(enableCheck);
                inputTab.Controls.Add(input.ControlPanel);
                inputTab.Location = new System.Drawing.Point(4, 22);
                inputTab.Name = input.Name + "Tab";
                inputTab.Padding = new System.Windows.Forms.Padding(3);
                inputTab.Size = new System.Drawing.Size(419, 239);
                inputTab.TabIndex = 0;
                inputTab.Text = input.Name;
                inputTab.UseVisualStyleBackColor = true;
                // 
                // enableCheck
                // 
                enableCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
                enableCheck.AutoSize = true;
                enableCheck.BackColor = System.Drawing.Color.Transparent;
                enableCheck.Location = new System.Drawing.Point(355, 6);
                enableCheck.Name = "enable" + input.Name + "Check";
                enableCheck.Size = new System.Drawing.Size(59, 17);
                enableCheck.TabIndex = 1;
                enableCheck.Text = input.Name;
                enableCheck.Checked = input.Enabled;
                enableCheck.CheckStateChanged += new EventHandler((source, args) => 
                    mCoordinator.Inputs.First(i => enableCheck.Name.Equals("enable" + i.Name + "Check")).Enabled = enableCheck.Checked);
                //enableCheck.UseVisualStyleBackColor = false;
                // 
                // inputPanel
                // 
                input.ControlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
                input.ControlPanel.Location = new System.Drawing.Point(3, 3);
                input.ControlPanel.Name = input.Name + "Panel";
                input.ControlPanel.Size = new System.Drawing.Size(413, 233);
                input.ControlPanel.TabIndex = 0;

                inputsTab.Controls.Add(inputTab);
            }
        }

        private void CoordinatorForm_Disposed(object sender, EventArgs e) {
            mCoordinator.CameraUpdated -= mCameraUpdated;
            mCoordinator.EyeUpdated -= mEyeUpdated;
            mCoordinator.Closed -= mClosed;
            mCoordinator.HeightmapChanged -= mHeightmapChanged;
        }

        private Thread mHeightmapUpdateThread;
        private readonly Queue<HeightmapChangedEventArgs> mHeightmapUpdates = new Queue<HeightmapChangedEventArgs>();

        private void mCoordinator_HeightmapChanged(object source, HeightmapChangedEventArgs args) {
            lock (mHeightmapUpdates) {
                mHeightmapUpdates.Enqueue(args);
                //If there's no thread
                if (mHeightmapUpdateThread == null) {
                    mHeightmapUpdateThread = new Thread(HeightmapUpdateThread);
                    mHeightmapUpdateThread.Name = "Heightmap update thread.";
                    mHeightmapUpdateThread.Start();
                }
            }
        }

        private void HeightmapUpdateThread() {
            while (true) {
                HeightmapChangedEventArgs e;
                lock (mHeightmapUpdates) {
                    if (mHeightmapUpdates.Count == 0) {
                        mHeightmapUpdateThread = null;
                        break;
                    } else
                        e = mHeightmapUpdates.Dequeue();
                }

                int w = e.Heights.GetLength(0);
                int h = e.Heights.GetLength(1);
                Rectangle affectedRect = new Rectangle(0, 0, mHeightmap.Width, mHeightmap.Height);
                BitmapData dat = mHeightmap.LockBits(affectedRect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                int bytesPerPixel = 3;
                byte[] rgbValues = new byte[mHeightmap.Height * dat.Stride];

                Marshal.Copy(dat.Scan0, rgbValues, 0, rgbValues.Length);

                for (int y = 0; y < h; y++) {
                    int i = ((e.StartY + y) * dat.Stride) + (e.StartX * bytesPerPixel);
                    for (int x = 0; x < w; x++) {
                        float height = e.Heights[x, y];
                        float floatVal = ((float) byte.MaxValue) * (height / 100f);
                        byte val = (byte) floatVal;

                        rgbValues[i++] = val;
                        rgbValues[i++] = val;
                        rgbValues[i++] = val;
                    }
                }
                Marshal.Copy(rgbValues, 0, dat.Scan0, rgbValues.Length);
                mHeightmap.UnlockBits(dat);

                if (!IsDisposed && !Disposing && Created)
                    Invoke(new Action(() => heightmapPanel.Image = new Bitmap(mHeightmap)));
            }
        }

        private void mCoordinator_CameraUpdated(Coordinator coordinator, CameraUpdateEventArgs args) {
            if (!mGuiUpdate && Created && !IsDisposed && !Disposing) {
                mEventUpdate = true;
                Invoke(new Action(() => {
                    virtualPositionPanel.Value = args.position;
                    virtualOrientationPanel.Pitch = args.rotation.Pitch;
                    virtualOrientationPanel.Yaw = args.rotation.Yaw;
                    if (heightmapTab == diagramHeightmapTab.SelectedTab) {
                        heightmapPanel.Invalidate();
                    }
                }));
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
                mCoordinator.Update(virtualPositionPanel.Value, Vector3.Zero, new Rotation(virtualOrientationPanel.Pitch, virtualOrientationPanel.Yaw), Rotation.Zero);
                mGuiUpdate = false;
            }
        }

        private void virtualRotation_OnChange(object sender, EventArgs e) {
            if (!mEventUpdate) {
                mGuiUpdate = true;
                mCoordinator.Update(virtualPositionPanel.Value, Vector3.Zero, new Rotation(virtualOrientationPanel.Pitch, virtualOrientationPanel.Yaw), Rotation.Zero);
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
            new Thread(() => { throw new Exception("Crashy crashy. Not transition GUI."); }).Start();
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

        private void triggerHelpButton_Click(object sender, EventArgs e) {
            //if (mCoordinator != null)
                //foreach (var window in mCoordinator.Windows)
                    //window.Overlay.TriggerHelp();
        }

        private void heightmapPanel_Paint(object sender, PaintEventArgs e) {
            if (mCoordinator != null) {
                int x = (int)((mCoordinator.Position.X / (float)mCoordinator.Heightmap.GetLength(0)) * e.ClipRectangle.Width);
                int y = e.ClipRectangle.Height - (int)((mCoordinator.Position.Y / (float)mCoordinator.Heightmap.GetLength(1)) * e.ClipRectangle.Height);
                int r = 5;
                Vector3 p2 = mCoordinator.Position + (mCoordinator.Orientation.LookAtVector * 20);
                int x2 = (int)((p2.X / (float)mCoordinator.Heightmap.GetLength(0)) * e.ClipRectangle.Width);
                int y2 = e.ClipRectangle.Height - (int)((p2.Y / (float)mCoordinator.Heightmap.GetLength(1)) * e.ClipRectangle.Height);
                e.Graphics.FillEllipse(Brushes.Red, x - r, y - r, r * 2, r * 2);
                e.Graphics.DrawLine(Pens.Red, x, y, x2, y2);
            }
        }
    }
}
