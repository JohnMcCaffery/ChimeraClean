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

namespace Chimera.GUI.Forms {
    public partial class OverlayWindow : Form {
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

        public OverlayWindow() {
            InitializeComponent();
        }

        public OverlayWindow(WindowOverlayManager manager)
            : this() {
            Init(manager);
        }

        public void Init(WindowOverlayManager manager) {
            drawPanel.BackColor = manager.TransparencyKey;
            BackColor = manager.TransparencyKey;
            TransparencyKey = manager.TransparencyKey;
            mManager = manager;
            TopMost = true;
            Opacity = manager.Opacity;
            refreshTimer.Interval = manager.FrameLength;
            refreshTimer.Enabled = true;
        }

        public void RedrawStatic() {
            mRedrawStatic = true;
            drawPanel.Invalidate();
        }

        public int FrameLength {
            get { return refreshTimer.Interval; }
            set {
                Action a = () => refreshTimer.Interval = value;
                if (InvokeRequired)
                    Invoke(a);
                else
                    a();
            }
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
            set {
                Action a = () => base.Opacity = value;
                if (InvokeRequired)
                    Invoke(a);
                else
                    a();
            }
        }

        private void drawPanel_Paint(object sender, PaintEventArgs e) {
            if (mManager.CurrentDisplay != null) {
                if (!e.ClipRectangle.Equals(mClip) || mRedrawStatic) {
                    mRedrawStatic = false;
                    mStaticBG = new Bitmap(e.ClipRectangle.Width, e.ClipRectangle.Height);
                    mClip = e.ClipRectangle;
                    using (Graphics g = Graphics.FromImage(mStaticBG))
                        mManager.CurrentDisplay.RedrawStatic(e.ClipRectangle, g);
                }

                e.Graphics.DrawImage(mStaticBG, 0, 0);
                mManager.CurrentDisplay.DrawDynamic(e.Graphics);

                mStaticBG.Save("../Images/TestImage.png");
            }
        }

        private void refreshTimer_Tick(object sender, EventArgs e) {
            if (mManager.CurrentDisplay != null && mManager.CurrentDisplay.NeedsRedrawn)
                drawPanel.Invalidate();
        }
    }
}
