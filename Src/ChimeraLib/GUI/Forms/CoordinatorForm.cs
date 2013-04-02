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

        private Action<Coordinator, CameraUpdateEventArgs> mCameraUpdatedListener;
        private Action<Coordinator, EventArgs> mEyeUpdatedListener;
        private Action<Coordinator, KeyEventArgs> mClosedListener;
        private Action mTickListener;
        private EventHandler<HeightmapChangedEventArgs> mHeightmapChangedListener;

        private static float sPerspectiveScaleMin = .0001f;
        private static float sPerspectiveScaleMax = .5f;
        private static float sPerspectiveScaleRange = sPerspectiveScaleMax - sPerspectiveScaleMin;

        public CoordinatorForm() {
            InitializeComponent();

            mPerspectives.Add(Perspective.X, new TwoDPerspective(Perspective.X));
            mPerspectives.Add(Perspective.Y, new TwoDPerspective(Perspective.Y));
            mPerspectives.Add(Perspective.Z, new TwoDPerspective(Perspective.Z));
            mCurrentPerspective = mPerspectives[Perspective.Z];
            float normalized = (mCurrentPerspective.Scale - sPerspectiveScaleMin) / sPerspectiveScaleRange;
            float sRange = realSpaceScale.Maximum - realSpaceScale.Minimum;
            realSpaceScale.Value = (int)(realSpaceScale.Minimum + normalized * sRange);

            heightmapPanel.MouseWheel += new MouseEventHandler(heightmapPanel_MouseWheel);
            realSpacePanel.MouseWheel += new MouseEventHandler(realSpaceScale_MouseWheel);
        }

        void realSpaceScale_MouseWheel(object sender, MouseEventArgs e) {
            if (e.Delta != 0) {
                int newVal = realSpaceScale.Value + e.Delta;
                realSpaceScale.Value = Math.Max(realSpaceScale.Minimum, Math.Min(realSpaceScale.Maximum, newVal));
            }
        }

        void heightmapPanel_MouseWheel(object sender, MouseEventArgs e) {
            if (e.Delta != 0) {
                int newVal = heightmapScale.Value + e.Delta;
                heightmapScale.Value = Math.Max(heightmapScale.Minimum, Math.Min(heightmapScale.Maximum, newVal));
            }
        }

        public CoordinatorForm(Coordinator coordinator)
            : this() {
            Init(coordinator);
        }

        public void Init(Coordinator coordinator) {
            mCoordinator = coordinator;

            Disposed += new EventHandler(CoordinatorForm_Disposed);

            mCameraUpdatedListener = new Action<Coordinator, CameraUpdateEventArgs>(mCoordinator_CameraUpdated);
            mEyeUpdatedListener = new Action<Coordinator, EventArgs>(mCoordinator_EyeUpdated);
            mClosedListener = new Action<Coordinator, KeyEventArgs>(mCoordinator_Closed);
            mHeightmapChangedListener = new EventHandler<HeightmapChangedEventArgs>(mCoordinator_HeightmapChanged);
            mTickListener = new Action(mCoordinator_Tick);

            mCoordinator.CameraUpdated += mCameraUpdatedListener;
            mCoordinator.EyeUpdated += mEyeUpdatedListener;
            mCoordinator.Closed += mClosedListener;
            mCoordinator.Tick += mTickListener;
            mCoordinator.HeightmapChanged += mHeightmapChangedListener;
            mCoordinator.WindowAdded += new Action<Window,EventArgs>(mCoordinator_WindowAdded);

            Rotation orientation = new Rotation(mCoordinator.Orientation);
            virtualPositionPanel.Value = mCoordinator.Position;
            virtualOrientationPanel.Value = orientation;
            eyePositionPanel.Value = mCoordinator.EyePosition;

            mHeightmap = new Bitmap(mCoordinator.Heightmap.GetLength(0), mCoordinator.Heightmap.GetLength(1), PixelFormat.Format24bppRgb);
            mHeightmapPerspective = new HeightmapPerspective(mCoordinator, heightmapPanel);



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

        private void mCoordinator_Tick() {
            if (Created && !IsDisposed && !Disposing)
                BeginInvoke(new Action(() => {
                    tpsLabel.Text = "Ticks / Second: " + mCoordinator.Statistics.TicksPerSecond;

                    meanTickLabel.Text = "Mean Tick Length: " + mCoordinator.Statistics.MeanTickLength;
                    longestTickLabel.Text = "Longest Tick: " + mCoordinator.Statistics.LongestTick;
                    shortestTickLabel.Text = "Shortest Tick: " + mCoordinator.Statistics.ShortestTick;

                    meanWorkLabel.Text = "Mean Work Length: " + mCoordinator.Statistics.MeanWorkLength;
                    longestWorkLabel.Text = "Longest Work: " + mCoordinator.Statistics.LongestWork;
                    shortestWorkLabel.Text = "Shortest Work: " + mCoordinator.Statistics.ShortestWork;

                    tickCountLabel.Text = "Tick Count: " + mCoordinator.Statistics.TickCount;
                }));
        }

        private void window_Changed(Window window, EventArgs args) {
            Invoke(() => {
                realSpacePanel.Invalidate();
                heightmapPanel.Invalidate();
            });
        }

        private void CoordinatorForm_Disposed(object sender, EventArgs e) {
            mClosing = true;
            mCoordinator.CameraUpdated -= mCameraUpdatedListener;
            mCoordinator.EyeUpdated -= mEyeUpdatedListener;
            mCoordinator.Closed -= mClosedListener;
            mCoordinator.HeightmapChanged -= mHeightmapChangedListener;
            mCoordinator.Tick -= mTickListener;
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
                    int totH = mHeightmap.Height;
                    Rectangle affectedRect = new Rectangle(0, 0, mHeightmap.Width, mHeightmap.Height);
                    BitmapData dat = mHeightmap.LockBits(affectedRect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                    int bytesPerPixel = 3;
                    byte[] rgbValues = new byte[mHeightmap.Height * dat.Stride];

                    Marshal.Copy(dat.Scan0, rgbValues, 0, rgbValues.Length);

                    for (int y = 0; y < h; y++) {
                        int startY = e.StartY + y + 1;
                        int i = ((totH - startY) * dat.Stride) + (e.StartX * bytesPerPixel);
                        for (int x = 0; x < w; x++) {
                            float height = e.Heights[x, y] + 25f;
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
                    //heightmapPanel.Invalidate();
                });
                mEventUpdate = false;
            }
        }

        private void mCoordinator_EyeUpdated(Coordinator coordinator, EventArgs args) {
            if (!mGuiUpdate) {
                mEventUpdate = true;
                eyePositionPanel.Value = coordinator.EyePosition;
                //heightmapPanel.Invalidate();
                mEventUpdate = false;
            }
        }

        private void virtualPositionPanel_OnChange(object sender, EventArgs e) {
            if (!mEventUpdate) {
                mGuiUpdate = true;
                mCoordinator.Update(virtualPositionPanel.Value, Vector3.Zero, new Rotation(virtualOrientationPanel.Pitch, virtualOrientationPanel.Yaw), Rotation.Zero);
                //heightmapPanel.Invalidate();
                mGuiUpdate = false;
            }
        }

        private void virtualRotation_OnChange(object sender, EventArgs e) {
            if (!mEventUpdate) {
                mGuiUpdate = true;
                mCoordinator.Update(virtualPositionPanel.Value, Vector3.Zero, new Rotation(virtualOrientationPanel.Pitch, virtualOrientationPanel.Yaw), Rotation.Zero);
                //heightmapPanel.Invalidate();
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
                lock (mHeightmap) {
                    map = new Bitmap(mHeightmapPerspective.Crop.Width, mHeightmapPerspective.Crop.Height, mHeightmap.PixelFormat);
                    using (Graphics g = Graphics.FromImage(map))
                        g.DrawImage(mHeightmap, mHeightmapPerspective.Crop.Location);
                }
                map = new Bitmap(map, e.ClipRectangle.Width, e.ClipRectangle.Height);
                mHeightmapPerspective.Clip = e.ClipRectangle;
                e.Graphics.DrawImage(map, Point.Empty);
                mCoordinator.Draw(mHeightmapPerspective.To2D, e.Graphics, e.ClipRectangle);
            }
        }

        private void realSpacePanel_Paint(object sender, PaintEventArgs e) {
            if (mCoordinator != null) {
                mCurrentPerspective.Clip = e.ClipRectangle;
                float greenEdge = (e.ClipRectangle.Width * 2) / mCurrentPerspective.Scale;
                float blueEdge = (e.ClipRectangle.Height * 2) / mCurrentPerspective.Scale;
                float redEdge = mCurrentPerspective.Perspective == Perspective.Y ? greenEdge : blueEdge;
                e.Graphics.DrawLine(Pens.Red, mCurrentPerspective.To2D(new Vector3(-greenEdge, 0f, 0f)), mCurrentPerspective.To2D(new Vector3(greenEdge, 0f, 0f)));
                e.Graphics.DrawLine(Pens.Green, mCurrentPerspective.To2D(new Vector3(0f, -greenEdge, 0f)), mCurrentPerspective.To2D(new Vector3(0f, greenEdge, 0f)));
                e.Graphics.DrawLine(Pens.Blue, mCurrentPerspective.To2D(new Vector3(0f, 0f, -blueEdge)), mCurrentPerspective.To2D(new Vector3(0f, 0f, blueEdge)));
                mCoordinator.Draw(mCurrentPerspective.To2D, e.Graphics, e.ClipRectangle);
            }
        }


        private bool mVirtualMouseDown;
        private Point mVirtualMousePosition;
        private bool mRealMouseDown;
        private Point mRealMousePosition;
        private Point mHeightmapTopLeft = new Point(0, 0);
        private TwoDPerspective mCurrentPerspective;
        private HeightmapPerspective mHeightmapPerspective;
        private Dictionary<Perspective, TwoDPerspective> mPerspectives = new Dictionary<Perspective,TwoDPerspective>();


        private void PerspectiveButton_CheckedChanged(object sender, EventArgs e) {
            if (xPerspectiveButton.Checked)
                mCurrentPerspective = mPerspectives[Perspective.X];
            if (yPerspectiveButton.Checked)
                mCurrentPerspective = mPerspectives[Perspective.Y];
            if (zPerspectiveButton.Checked)
                mCurrentPerspective = mPerspectives[Perspective.Z];
            realSpacePanel.Invalidate();
        }

        private void heightmapPanel_MouseDown(object sender, MouseEventArgs e) {
            mVirtualMouseDown = true;
            mVirtualMousePosition = e.Location;
        }

        private void heightmapPanel_MouseUp(object sender, MouseEventArgs e) {
            mVirtualMouseDown = false;
        }

        private void heightmapPanel_MouseMove(object sender, MouseEventArgs e) {
            if (mVirtualMouseDown) {
                int xDelta = e.X - mVirtualMousePosition.X;
                int yDelta = e.Y - mVirtualMousePosition.Y;
                mVirtualMousePosition = e.Location;

                mHeightmapPerspective.Drag(xDelta, yDelta);
            }
        }

        private void virtualZoom_Scroll(object sender, EventArgs e) {
            mHeightmapPerspective.Scale = (float) (heightmapScale.Value / 1000f);
        }


        private void realSpacePanel_Resize(object sender, EventArgs e) {
            realSpacePanel.Invalidate();
        }

        private void realSpacePanel_MouseDown(object sender, MouseEventArgs e) {
            mRealMouseDown = true;
            mRealMousePosition = e.Location;
        }

        private void realSpacePanel_MouseUp(object sender, MouseEventArgs e) {
            mRealMouseDown = false;
        }

        private void realSpacePanel_MouseMove(object sender, MouseEventArgs e) {
            if (mRealMouseDown) {
                int xDelta = e.X - mRealMousePosition.X;
                int yDelta = e.Y - mRealMousePosition.Y;
                mRealMousePosition = e.Location;

                mCurrentPerspective.Drag(xDelta, yDelta);
                realSpacePanel.Invalidate();
            }
        }

        private void realSpaceScale_Scroll(object sender, EventArgs e) {
            float normalized = (float) realSpaceScale.Value / ((float) realSpaceScale.Maximum - (float) realSpaceScale.Minimum);
            sPerspectiveScaleMin = .001f;
            sPerspectiveScaleMax = .5f;
            sPerspectiveScaleRange = sPerspectiveScaleMax - sPerspectiveScaleMin;
            mCurrentPerspective.Scale = (normalized * sPerspectiveScaleRange) + sPerspectiveScaleMin;
            realSpacePanel.Invalidate();
        }
        private class TwoDPerspective {
            private readonly Perspective mPerspective;
            private Vector3 mCentre = Vector3.Zero;
            private float mScale = .05f;
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

            public float Scale {
                get { return mScale; }
                set { mScale = value; }
            }

            public virtual Point To2D(Vector3 point) {
                point -= mCentre;
                Vector2 ret = new Vector2();
                switch (mPerspective) {
                    case Perspective.X: ret = new Vector2(point.Y, point.Z); break;
                    case Perspective.Y: ret = new Vector2(point.X, point.Z); break;
                    case Perspective.Z: ret = new Vector2(point.Y, point.X); break;
                }

                ret.X *= mScale;
                ret.Y *= mScale;
                return new Point((int)ret.X + mCentreX, mCentreY - (int)ret.Y);
            }

            internal void Drag(int xDelta, int yDelta) {
                if (mPerspective == Perspective.Z)
                    mCentre.X += (yDelta / mScale);
                if (mPerspective == Perspective.X || mPerspective == Perspective.Z)
                    mCentre.Y -= (xDelta / mScale);

                if (mPerspective == Perspective.Y)
                    mCentre.X -= (xDelta / mScale);

                if (mPerspective == Perspective.X || mPerspective == Perspective.Y)
                    mCentre.Z += (yDelta / mScale);
            }

            public Perspective Perspective { get { return mPerspective; } }
        }

        private class HeightmapPerspective {
            private Coordinator mCoordinator;
            private Matrix4 mWorldMatrix;
            private Matrix4 mClipMatrix;
            private Matrix4 mWorldScale;
            private Size mSize;
            private RectangleF mCrop;
            private Rectangle mClip;
            private float mScale = 1f;
            private PictureBox mDrawPanel;
            private bool mResized = false;

            public Rectangle Crop {
                get { return new Rectangle((int) mCrop.X, (int) mCrop.Y, (int) mCrop.Width, (int) mCrop.Height); }
            }

            public Rectangle Clip {
                get { return mClip; }
                set { 
                    mClip = value;
                    if (mResized) {
                        mResized = false;
                        CalculateClipMatrix();
                    }
                }
            }

            public float Scale {
                get { return mScale; }
                set {
                    mScale = value;
                    mCrop.X += ((mSize.Width / value) - mCrop.Width) / 2f;
                    mCrop.Y += ((mSize.Height / value) - mCrop.Height) / 2f;
                    mCrop.Width = mSize.Width / value;
                    mCrop.Height = mSize.Height / value;
                    CheckTopLeft();
                    CalculateClipMatrix();
                }
            }

            public void Drag(int xDelta, int yDelta) {
                Vector3 delta = new Vector3(xDelta, yDelta, 0f);
                delta *= mWorldScale;

                mCrop.X += delta.X;
                mCrop.Y += delta.Y;

                CheckTopLeft();
                CalculateClipMatrix();
            }

            private void CheckTopLeft() {
                mCrop.X = Math.Min(0f, mCrop.X);
                mCrop.Y = Math.Min(0f, mCrop.Y);
                mCrop.X = Math.Max(mCrop.Width - mSize.Width , mCrop.X);
                mCrop.Y = Math.Max(mCrop.Height - mSize.Height, mCrop.Y);
            }

            private void CalculateWorldMatrix() {
                Matrix4 toWorldScale = Matrix4.CreateScale(new Vector3(.001f, -.001f, .001f));
                Matrix4 toWorldOrientation = Matrix4.CreateRotationZ((float) (mCoordinator.Orientation.Yaw * Math.PI / 180.0));
                Matrix4 toWorldCoords = Matrix4.CreateTranslation(mCoordinator.Position);
                mWorldMatrix = Matrix4.Identity;
                mWorldMatrix *= toWorldScale;
                mWorldMatrix *= toWorldOrientation;
                mWorldMatrix *= toWorldCoords;

                CalculateClipMatrix();
            }

            private void CalculateClipMatrix() {
                mWorldScale = Matrix4.CreateScale(
                    new Vector3(mSize.Width / (mScale * mClip.Width), mSize.Height / (mScale * mClip.Height), 1f));

                //TODO - mSize.[W,H] may have to be reversed
                Matrix4 toClipScale = Matrix4.CreateScale(
                    new Vector3((mScale * mClip.Width) / mSize.Width, (mScale * mClip.Height) / mSize.Height, 1f));

                Vector3 topLeft = new Vector3(mCrop.X, mCrop.Y, 0f);
                Matrix4 toClipTranslate = Matrix4.CreateTranslation(topLeft);

                mClipMatrix = Matrix4.Identity;
                //mClipMatrix *= mWorldMatrix;
                mClipMatrix *= toClipTranslate;
                mClipMatrix *= toClipScale;

                if (mDrawPanel.InvokeRequired)
                    mDrawPanel.BeginInvoke(new Action(() => mDrawPanel.Invalidate()));
                else
                    mDrawPanel.Invalidate();
            }

            public HeightmapPerspective(Coordinator coordinator, PictureBox drawPanel) {
                mCoordinator = coordinator;
                mDrawPanel = drawPanel;
                mSize = new Size(coordinator.Heightmap.GetLength(0), coordinator.Heightmap.GetLength(1));
                mCrop = new Rectangle(0, 0, mSize.Width, mSize.Height);
                mClip = new Rectangle(0, 0, mSize.Width, mSize.Height);
                mCoordinator.CameraUpdated += (coord, args) => CalculateWorldMatrix();
                mCoordinator.EyeUpdated += (coord, args) => CalculateWorldMatrix();
                mDrawPanel.Resize += new EventHandler(mDrawPanel_Resize);

                CalculateWorldMatrix();
            }

            void mDrawPanel_Resize(object sender, EventArgs e) {
                mResized = true;
            }

            public Point To2D(Vector3 point) {
                point *= mWorldMatrix;
                //TODO put this flip into the matrices - translate to middle, rotate 180 deg around yaw, translate back from middle
                point.Y = mSize.Height - point.Y;
                point *= mClipMatrix;
                return new Point((int) point.X, (int) point.Y);
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
    }
}
