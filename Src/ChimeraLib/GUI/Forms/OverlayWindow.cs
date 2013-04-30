using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Overlay;
using Chimera.Interfaces.Overlay;
using Chimera.Util;
using AxWMPLib;

namespace Chimera.GUI.Forms {
    public partial class OverlayWindow : Form {
        /// <summary>
        /// Statistics object which monitors how long ticks are taking.
        /// </summary>
        private readonly TickStatistics mStats = new TickStatistics();
        /// <summary>
        /// The manager which controls this overlay.
        /// </summary>
        private WindowOverlayManager mManager;
        /// <summary>
        /// Clip rectangle defining the drawable area any overlays draw on for this window.
        /// </summary>
        private Rectangle mClip;
        /// <summary>
        /// The background image which is saved and redrawn as the static portion of the overlay.
        /// </summary>
        private Bitmap mStaticBG;
        /// <summary>
        /// Flag to force the static portion of the overlay to be redrawn.
        /// </summary>
        private bool mRedrawStatic;
        private Cursor mDefaultCursor = new Cursor("../Cursors/cursor.cur");

        public event Action VideoFinished;

        public OverlayWindow() {
            InitializeComponent();

            Cursor = mDefaultCursor;
            TopMost = true;
        }

        public OverlayWindow(WindowOverlayManager manager)
            : this() {
            Init(manager);
        }

        public void Init(WindowOverlayManager manager) {
            mManager = manager;

            drawPanel.BackColor = manager.TransparencyKey;
            BackColor = manager.TransparencyKey;
            TransparencyKey = manager.TransparencyKey;
            Opacity = manager.Opacity;
            refreshTimer.Interval = manager.FrameLength;
            refreshTimer.Enabled = true;

            videoPlayer.PlayStateChange += new _WMPOCXEvents_PlayStateChangeEventHandler(videoPlayer_PlayStateChange);
        }

        private void videoPlayer_PlayStateChange(object source, _WMPOCXEvents_PlayStateChangeEvent args) {
            if (args.newState == 1 && VideoFinished != null) {
                VideoFinished();
                videoPlayer.Visible = false;
            }
        }

        public void RedrawStatic() {
            mRedrawStatic = true;
            drawPanel.Invalidate();
        }

        public int FrameLength {
            get { return refreshTimer.Interval; }
            set { Invoke(() => refreshTimer.Interval = value); }
        }

        /// <summary>
        /// Whether to go full screen.
        /// </summary>
        public bool Fullscreen {
            get { return FormBorderStyle == FormBorderStyle.None; }
            set { 
                FormBorderStyle = value ? FormBorderStyle.None : FormBorderStyle.Sizable;
                Location = mManager.Window.Monitor.Bounds.Location;
                Size = mManager.Window.Monitor.Bounds.Size;
            }
        }

        /// <summary>
        /// How see through the overlay is.
        /// </summary>
        public new double Opacity {
            get { return base.Opacity; }
            set { Invoke(() => base.Opacity = value); }
        }

        private void drawPanel_Paint(object sender, PaintEventArgs e) {
            if (mManager.CurrentDisplay != null) {
                if (!e.ClipRectangle.Width.Equals(mClip.Width) || !e.ClipRectangle.Height.Equals(mClip.Height) || mRedrawStatic) {
                    //if (!e.ClipRectangle.Width.Equals(mClip.Width) || !e.ClipRectangle.Height.Equals(mClip.Height))
                    mManager.CurrentDisplay.Clip = e.ClipRectangle;
                    mRedrawStatic = false;
                    Bitmap oldBG = mStaticBG;
                    mStaticBG = new Bitmap(e.ClipRectangle.Width, e.ClipRectangle.Height);
                    mClip = e.ClipRectangle;
                    using (Graphics g = Graphics.FromImage(mStaticBG))
                        mManager.CurrentDisplay.DrawStatic(g);
                    drawPanel.Image = mStaticBG;
                    if (oldBG != null)
                        oldBG.Dispose();
                }

                mManager.CurrentDisplay.DrawDynamic(e.Graphics);
            }
        }

        private void refreshTimer_Tick(object sender, EventArgs e) {
            mStats.Begin();
            if (mManager.CurrentDisplay != null && mManager.CurrentDisplay.NeedsRedrawn)
                drawPanel.Invalidate();
            mStats.Tick();
        }

        public TickStatistics Statistics { get { return mStats; } }

        internal void SetCursor(Cursor value) {
            Invoke(() => Cursor = value);
        }

        private void Invoke(Action a) {
            if (!InvokeRequired)
                a();
            else if (Created && !IsDisposed && !Disposing)
                base.Invoke(a);
        }

        internal void ForceRedraw() {
            Invoke(() => drawPanel.Invalidate());
        }

        public override void ResetCursor() {
            Invoke(() => Cursor = mDefaultCursor);
        }

        public bool AlwaysOnTop {
            get { return TopMost; }
            set { Invoke(() => TopMost = value); }
        }
        

        public void BringOverlayToFront() {
            Invoke(() => {
                BringToFront();
            });
        }

        public void PlayVideo(string uri) {
            //videoPlayer.uiMode = "Mini";
            videoPlayer.Visible = true;
            videoPlayer.URL = uri;

            videoPlayer.uiMode = "none";
            videoPlayer.stretchToFit = true;
            videoPlayer.windowlessVideo = true;
            //videoPlayer.Dock = DockStyle.Fill;
            //videoPlayer.enableContextMenu = false;
            //videoPlayer.fullScreen = true;

            videoPlayer.Ctlcontrols.play();
        }

        public void StopPlayback() {
            videoPlayer.Ctlcontrols.stop();
        }
    }
}
