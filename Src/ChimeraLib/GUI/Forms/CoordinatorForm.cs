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
        private bool mClosing;
        private Coordinator mCoordinator;
        private Bitmap mHeightmap;

        private Action<Coordinator, CameraUpdateEventArgs> mCameraUpdated;
        private Action<Coordinator, EventArgs> mEyeUpdated;
        private Action<Coordinator, KeyEventArgs> mClosed;
        private EventHandler<HeightmapChangedEventArgs> mHeightmapChanged;

        public CoordinatorForm() {
            InitializeComponent();

            mPerspectives.Add(Perspective.X, new TwoDPerspective(Perspective.X));
            mPerspectives.Add(Perspective.Y, new TwoDPerspective(Perspective.Y));
            mPerspectives.Add(Perspective.Z, new TwoDPerspective(Perspective.Z));
            mCurrentPerspective = mPerspectives[Perspective.Z];
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
            mCoordinator.WindowAdded += new Action<Window,EventArgs>(mCoordinator_WindowAdded);

            Rotation orientation = new Rotation(mCoordinator.Orientation);
            virtualPositionPanel.Value = mCoordinator.Position;
            virtualOrientationPanel.Value = orientation;
            eyePositionPanel.Value = mCoordinator.EyePosition;

            mHeightmap = new Bitmap(mCoordinator.Heightmap.GetLength(0), mCoordinator.Heightmap.GetLength(1), PixelFormat.Format24bppRgb);
            mHeightmapPerspective = new HeightmapPerspective(mCoordinator);
            mHeightmapPerspective.AspectRatio = (float) mHeightmap.Width / (float) mHeightmap.Height;
            mHeightmapPerspective.Scale = 1f;

            foreach (var window in mCoordinator.Windows) {
                mCoordinator_WindowAdded(window, null);
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

        private void mCoordinator_WindowAdded(Window window, EventArgs args) {
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
            window.Changed += new Action<Window, EventArgs>(window_Changed);
        }

        private void window_Changed(Window window, EventArgs args) {
            Invoke(() => realSpacePanel.Invalidate());
        }

        private void CoordinatorForm_Disposed(object sender, EventArgs e) {
            mClosing = true;
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
            while (!mClosing) {
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
                lock (mHeightmap) {
                    Rectangle affectedRect = new Rectangle(0, 0, mHeightmap.Width, mHeightmap.Height);
                    BitmapData dat = mHeightmap.LockBits(affectedRect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                    int bytesPerPixel = 3;
                    byte[] rgbValues = new byte[mHeightmap.Height * dat.Stride];

                    Marshal.Copy(dat.Scan0, rgbValues, 0, rgbValues.Length);

                    for (int y = 0; y < h; y++) {
                        int i = ((e.StartY + y) * dat.Stride) + (e.StartX * bytesPerPixel);
                        for (int x = 0; x < w; x++) {
                            float height = e.Heights[x, y];
                            float floatVal = ((float)byte.MaxValue) * (height / 100f);
                            byte val = (byte)floatVal;

                            rgbValues[i++] = val;
                            rgbValues[i++] = val;
                            rgbValues[i++] = val;
                        }
                    }
                    Marshal.Copy(rgbValues, 0, dat.Scan0, rgbValues.Length);
                    mHeightmap.UnlockBits(dat);
                }

                //Invoke(() => heightmapPanel.Image = new Bitmap(mHeightmap));
                Invoke(() => heightmapPanel.Invalidate());
            }
        }

        private void mCoordinator_CameraUpdated(Coordinator coordinator, CameraUpdateEventArgs args) {
            if (!mGuiUpdate && Created && !IsDisposed && !Disposing) {
                mEventUpdate = true;
                Invoke(() => {
                    virtualPositionPanel.Value = args.position;
                    virtualOrientationPanel.Pitch = args.rotation.Pitch;
                    virtualOrientationPanel.Yaw = args.rotation.Yaw;
                    heightmapPanel.Invalidate();
                });
                mEventUpdate = false;
            }
        }

        private void mCoordinator_EyeUpdated(Coordinator coordinator, EventArgs args) {
            if (!mGuiUpdate) {
                mEventUpdate = true;
                eyePositionPanel.Value = coordinator.EyePosition;
                heightmapPanel.Invalidate();
                mEventUpdate = false;
            }
        }

        private void virtualPositionPanel_OnChange(object sender, EventArgs e) {
            if (!mEventUpdate) {
                mGuiUpdate = true;
                mCoordinator.Update(virtualPositionPanel.Value, Vector3.Zero, new Rotation(virtualOrientationPanel.Pitch, virtualOrientationPanel.Yaw), Rotation.Zero);
                heightmapPanel.Invalidate();
                mGuiUpdate = false;
            }
        }

        private void virtualRotation_OnChange(object sender, EventArgs e) {
            if (!mEventUpdate) {
                mGuiUpdate = true;
                mCoordinator.Update(virtualPositionPanel.Value, Vector3.Zero, new Rotation(virtualOrientationPanel.Pitch, virtualOrientationPanel.Yaw), Rotation.Zero);
                heightmapPanel.Invalidate();
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
            mClosing = true;
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
            if (mCoordinator != null && mCoordinator.StateManager != null)
                mCoordinator.StateManager.TriggerCustom("Help");
        }

        private void Invoke(Action a) {
            if (!mClosing && !IsDisposed && !Disposing && Created) {
                if (InvokeRequired)
                    base.Invoke(a);
                else
                    a();
            }
        }
        private void heightmapPanel_Paint(object sender, PaintEventArgs e) {
            if (mCoordinator != null) {
                /*
                int x = (int)((mCoordinator.Position.X / (float)mCoordinator.Heightmap.GetLength(0)) * e.ClipRectangle.Width);
                int y = e.ClipRectangle.Height - (int)((mCoordinator.Position.Y / (float)mCoordinator.Heightmap.GetLength(1)) * e.ClipRectangle.Height);
                int r = 5;
                Vector3 p2 = mCoordinator.Position + (mCoordinator.Orientation.LookAtVector * 20);
                int x2 = (int)((p2.X / (float)mCoordinator.Heightmap.GetLength(0)) * e.ClipRectangle.Width);
                int y2 = e.ClipRectangle.Height - (int)((p2.Y / (float)mCoordinator.Heightmap.GetLength(1)) * e.ClipRectangle.Height);
                e.Graphics.FillEllipse(Brushes.Red, x - r, y - r, r * 2, r * 2);
                e.Graphics.DrawLine(Pens.Red, x, y, x2, y2);*/
                Bitmap map;
                lock (mHeightmap)
                    map = new Bitmap(mHeightmap, 
                        (int) ((float) e.ClipRectangle.Width / mHeightmapPerspective.Scale), 
                        (int) ((float) e.ClipRectangle.Height / mHeightmapPerspective.Scale));
                e.Graphics.DrawImage(map, mHeightmapTopLeft);
                mHeightmapPerspective.Clip = e.ClipRectangle;
                mCoordinator.Draw(mHeightmapPerspective.To2D, e.Graphics, e.ClipRectangle);

                e.Graphics.DrawLine(Pens.Red, e.ClipRectangle.Width / 2, 0, e.ClipRectangle.Width / 2, e.ClipRectangle.Height);
                e.Graphics.DrawLine(Pens.Red, 0, e.ClipRectangle.Height / 2, e.ClipRectangle.Width, e.ClipRectangle.Height / 2);
            }
        }

        private void realSpacePanel_Paint(object sender, PaintEventArgs e) {
            if (mCoordinator != null) {
                mCurrentPerspective.Clip = e.ClipRectangle;
                float edge = 100000f;
                e.Graphics.DrawLine(Pens.Red, mCurrentPerspective.To2D(new Vector3(-edge, 0f, 0f)), mCurrentPerspective.To2D(new Vector3(edge, 0f, 0f)));
                e.Graphics.DrawLine(Pens.Green, mCurrentPerspective.To2D(new Vector3(0f, -edge, 0f)), mCurrentPerspective.To2D(new Vector3(0f, edge, 0f)));
                e.Graphics.DrawLine(Pens.Blue, mCurrentPerspective.To2D(new Vector3(0f, 0f, -edge)), mCurrentPerspective.To2D(new Vector3(0f, 0f, edge)));
                mCoordinator.Draw(mCurrentPerspective.To2D, e.Graphics, e.ClipRectangle);
            }
        }

        private TwoDPerspective mCurrentPerspective;
        private TwoDPerspective mHeightmapPerspective;
        private Dictionary<Perspective, TwoDPerspective> mPerspectives = new Dictionary<Perspective,TwoDPerspective>();

        private class TwoDPerspective {
            private readonly Perspective mPerspective;
            private Vector3 mCentre = Vector3.Zero;
            private float mAspectRatio = 1f;
            private float mHScale = .05f;
            private float mVScale = .05f;
            private int mCentreX;
            private int mCentreY;

            public TwoDPerspective(Perspective perspective) {
                mPerspective = perspective;
            }

            public Vector3 Centre {
                get { return mCentre; }
                set { mCentre = value; }
            }

            public Rectangle Clip {
                get { return new Rectangle(0, 0, mCentreX * 2, mCentreY * 2); }
                set { 
                    mCentreX = value.Width / 2;
                    mCentreY = value.Height / 2;
                }
            }

            public float AspectRatio {
                get { return mAspectRatio; }
                set { mAspectRatio = value; }
            }

            public float Scale {
                get { return mVScale; }
                set {
                    mHScale = value * mAspectRatio;
                    mVScale = value;
                }
            }

            public virtual Point To2D(Vector3 point) {
                point -= mCentre;
                Vector2 ret = new Vector2();
                switch (mPerspective) {
                    case Perspective.X: ret = new Vector2(point.Y, point.Z); break;
                    case Perspective.Y: ret = new Vector2(point.X, point.Z); break;
                    case Perspective.Z: ret = new Vector2(point.Y, point.X); break;
                }

                ret.X *= mHScale;
                ret.Y *= mVScale;
                return new Point((int)ret.X + mCentreX, mCentreY - (int)ret.Y);
            }
        }

        private class HeightmapPerspective : TwoDPerspective {
            private Coordinator mCoordinator;

            public HeightmapPerspective(Coordinator coordinator)
                : base(Perspective.Z) {

                mCoordinator = coordinator;
            }

            public override Point To2D(Vector3 point) {
                point *= mCoordinator.Orientation.Quaternion;
                point /= 1000f;
                point += mCoordinator.Position; 

                /*
                Matrix4 toWorldScale = Matrix4.CreateScale(new Vector3(.001f, .001f, .001f));
                Matrix4 toWorldOrientation = Matrix4.CreateTranslation(mCoordinator.Orientation.LookAtVector);
                Matrix4 toWorldCoords = Matrix4.CreateTranslation(mCoordinator.Position);
                point *= toWorldScale;
                point *= toWorldOrientation;
                point *= toWorldCoords;
                */

                float w = Clip.Width / Scale;
                float h = Clip.Height / Scale;
                float hmW = mCoordinator.Heightmap.GetLength(0);
                float hmH = mCoordinator.Heightmap.GetLength(1);

                Matrix4 normalized = Matrix4.CreateScale(new Vector3(1f / hmW, 1f / hmH, 1f));
                point *= normalized;

                Matrix4 toClipScale = Matrix4.CreateScale(new Vector3(h, w, 1f));
                point *= toClipScale;

                Matrix4 toClipLocation = Matrix4.CreateTranslation(Centre);
                point *= toClipLocation;


                /*
                int x = (int)((point.X / (float)mCoordinator.Heightmap.GetLength(0)) * Clip.Width);
                int y = Clip.Height - (int)((point.Y / (float)mCoordinator.Heightmap.GetLength(1)) * Clip.Height);

                return new Point((int) (x * Scale), (int) (y * Scale));
                */
                return new Point((int) point.Y, (int) (h - point.X));

                /*
                int r = 5;
                Vector3 p2 = mCoordinator.Position + (mCoordinator.Orientation.LookAtVector * 20);
                int x2 = (int)((p2.X / (float)mCoordinator.Heightmap.GetLength(0)) * Clip.Width);
                int y2 = Clip.Height - (int)((p2.Y / (float)mCoordinator.Heightmap.GetLength(1)) * Clip.Height);
                e.Graphics.FillEllipse(Brushes.Red, x - r, y - r, r * 2, r * 2);
                e.Graphics.DrawLine(Pens.Red, x, y, x2, y2);
                */
            }
        }


        /// <summary>
        /// Which perspective to render.
        /// </summary>
        public enum Perspective {
            /// <summary>
            /// View down the X axis.
            /// </summary>
            X,
            /// <summary>
            /// View down the Y axis.
            /// </summary>
            Y,
            /// <summary>
            /// View down the Z axis.
            /// </summary>
            Z
        }

        private void PerspectiveButton_CheckedChanged(object sender, EventArgs e) {
            if (xPerspectiveButton.Checked)
                mCurrentPerspective = mPerspectives[Perspective.X];
            if (yPerspectiveButton.Checked)
                mCurrentPerspective = mPerspectives[Perspective.Y];
            if (zPerspectiveButton.Checked)
                mCurrentPerspective = mPerspectives[Perspective.Z];
            realSpacePanel.Invalidate();
        }

        private bool mMouseDown;
        private Point mMousePosition;
        private Point mHeightmapTopLeft = new Point(0, 0);

        private void heightmapPanel_MouseDown(object sender, MouseEventArgs e) {
            mMouseDown = true;
            mMousePosition = e.Location;
        }

        private void heightmapPanel_MouseUp(object sender, MouseEventArgs e) {
            mMouseDown = false;
        }

        private void heightmapPanel_MouseMove(object sender, MouseEventArgs e) {
            if (mMouseDown) {
                int xDelta = e.X - mMousePosition.X;
                int yDelta = e.Y - mMousePosition.Y;
                mMousePosition = e.Location;
                //mHeightmapTopLeft.X += xDelta;
                //mHeightmapTopLeft.Y += yDelta;

                int x = mHeightmapTopLeft.X + xDelta;
                int y = mHeightmapTopLeft.Y + yDelta;
                //x = Math.Max(0, x);
                //y = Math.Max(0, y);
                //x = Math.Min((int)(mHeightmapPerspective.Clip.Width * mHeightmapPerspective.Scale) - mHeightmapPerspective.Clip.Width, x);
                //y = Math.Min((int)(mHeightmapPerspective.Clip.Height * mHeightmapPerspective.Scale) - mHeightmapPerspective.Clip.Height, y);

                /*
                int farRight = (int) (mHeightmapPerspective.Clip.Width / mHeightmapPerspective.Scale);
                int farBottom = (int) (mHeightmapPerspective.Clip.Height / mHeightmapPerspective.Scale);


                if (x + mHeightmapPerspective.Clip.Width > farRight)
                    x = farRight - mHeightmapPerspective.Clip.Width;
                if (y + mHeightmapPerspective.Clip.Height > farBottom)
                    y = farRight - mHeightmapPerspective.Clip.Width;
                */

                mHeightmapTopLeft.X = x;
                mHeightmapTopLeft.Y = y;
                mHeightmapPerspective.Centre = new Vector3(-y, x, 0f);
                heightmapPanel.Invalidate();
            }
        }

        private void virtualZoom_Scroll(object sender, EventArgs e) {
            mHeightmapPerspective.Scale = (float) ((virtualZoom.Maximum - virtualZoom.Value) + virtualZoom.Minimum) / (float) virtualZoom.Maximum;
            heightmapPanel.Invalidate();
        }
    }
}
