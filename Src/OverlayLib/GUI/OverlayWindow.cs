/*************************************************************************
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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Overlay;
using Chimera.Interfaces.Overlay;
using Chimera.Util;

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
            TransparencyKey = manager.TransparencyKey;
            BackColor = manager.TransparencyKey;
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
            set { Invoke(() => refreshTimer.Interval = value); }
        }

        /// <summary>
        /// Whether to go full screen.
        /// </summary>
        public bool Fullscreen {
            get { return FormBorderStyle == FormBorderStyle.None; }
            set {
                Invoke(() => {
                    FormBorderStyle = value ? FormBorderStyle.None : FormBorderStyle.Sizable;
                    Location = mManager.Window.Monitor.Bounds.Location;
                    Size = mManager.Window.Monitor.Bounds.Size;
                });
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

        public void Invoke(Action a) {
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

        public void AddControl(Control control, RectangleF pos) {
            Action setBounds = () => control.Bounds = new Rectangle((int) (Width * pos.X), (int) (Height * pos.Y), (int) (Width * pos.Width), (int) (Height * pos.Height));
            if (control.InvokeRequired)
                control.Invoke(setBounds);
            else
                setBounds();
            Invoke(() => drawPanel.Controls.Add(control));
        }

        public void RemoveControl(Control control) {
            Controls.Remove(control);
        }

        private void OverlayWindow_MouseDown(object sender, MouseEventArgs e) {
            mManager.Press(0);
        }

        private void OverlayWindow_MouseUp(object sender, MouseEventArgs e) {
            mManager.Release(0);
        }
    }
}
