﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Interfaces;

namespace Chimera.GUI.Forms {
    public class OverlayWindowFactory : IOverlayWindowFactory {
        public IOverlayWindow Make(OverlayController controller) {
            return new OverlayWindow(controller);
        }
    }
    public partial class OverlayWindow : Form , IOverlayWindow {
        /// <summary>
        /// The last position the mouse was at.
        /// </summary>
        private Point mLastMouse = new Point(-1, -1);
        /// <summary>
        /// how much to jitter the mouse by next tick.
        /// </summary>
        private int mJitter = 1;
        /// <summary>
        /// True if the mouse has been on the screen, used to do one extra refresh when the mouse leaves the screen.
        /// </summary>
        private bool mMouseOnScreen;
        /// <summary>
        /// The controller which controls this overlay.
        /// </summary>
        private OverlayController mController;

        public OverlayWindow() {
            InitializeComponent();
        }

        /// <param name="window">The window this overlay covers.</param>
        public OverlayWindow(OverlayController controller)
            : this() {
            Init(controller);
            TransparencyKey = controller.TransparentColour;
            // http://www.cursor.cc/
            Cursor = new Cursor("../Cursors/cursor.cur");
        }
        
        /// <summary>
        /// Whether to go full screen.
        /// </summary>
        public bool Fullscreen {
            get { return FormBorderStyle == FormBorderStyle.None; }
            set { 
                FormBorderStyle = value ? FormBorderStyle.None : FormBorderStyle.Sizable;
                Location = mController.Window.Monitor.Bounds.Location;
                Size = mController.Window.Monitor.Bounds.Size;
            }
        }

        /// <summary>
        /// Redraw the input.
        /// </summary>
        public void Redraw() {
            if (!IsDisposed && Created)
                Invoke(new Action(() => {
                    if (!IsDisposed && Created)
                        drawPanel.Invalidate();
                }));
        }

        /// <summary>
        /// Link this form with a logical input.
        /// </summary>
        /// <param name="input">The input to link this form with.</param>
        public void Init(OverlayController controller) {
            mController = controller;
            mController.Window.MonitorChanged += new Action<Window, Screen>(mWindow_MonitorChanged);

            TopMost = true;
            StartPosition = FormStartPosition.Manual;
            Location = mController.Window.Monitor.Bounds.Location;
            Size = mController.Window.Monitor.Bounds.Size;
        }

        public void Foreground() {
            Invoke(new Action(() => BringToFront()));
        }

        private void mWindow_MonitorChanged(Window window, Screen screen) {
            Location = screen.Bounds.Location;
            Size = screen.Bounds.Size;
        }

        private void drawPanel_Paint(object sender, PaintEventArgs e) {
            if (mController.Window.Coordinator.Overlay != null)
                mController.Window.Coordinator.Overlay.Draw(e.Graphics, e.ClipRectangle, mController.Window);
        }

        private void OverlayWindow_Load(object sender, EventArgs e) {
            Text = mController.Window.Name + " Overlay";
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
        }
    }
}