using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chimera.GUI.Forms {
    public partial class OverlayWindow : Form {
        /// <summary>
        /// The input this overlay covers.
        /// </summary>
        private Window mWindow;
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

        public OverlayWindow() {
            InitializeComponent();
        }

        /// <param name="window">The window this overlay covers.</param>
        public OverlayWindow(Window window, Color transparentColour)
            : this() {
            Init(window);
            TransparencyKey = transparentColour;
        }
        
        /// <summary>
        /// Whether to go full screen.
        /// </summary>
        public bool Fullscreen {
            get { return FormBorderStyle == FormBorderStyle.None; }
            set { 
                FormBorderStyle = value ? FormBorderStyle.None : FormBorderStyle.Sizable;
                Location = mWindow.Monitor.Bounds.Location;
                Size = mWindow.Monitor.Bounds.Size;
            }
        }

        /// <summary>
        /// Whether moving the mouse over this overlay should control the cursor.
        /// </summary>
        public bool MouseControl {
            get { return mouseTimer.Enabled; }
            set { mouseTimer.Enabled = value; }
        }

        /// <summary>
        /// Redraw the input.
        /// </summary>
        public void Redraw() {
            Invoke(new Action(() => drawPanel.Invalidate()));
        }

        /// <summary>
        /// Link this form with a logical input.
        /// </summary>
        /// <param name="input">The input to link this form with.</param>
        public void Init(Window window) {
            mWindow = window;
            mWindow.MonitorChanged += new Action<Window,Screen>(mWindow_MonitorChanged);

            TopMost = true;
            StartPosition = FormStartPosition.Manual;
            Location = window.Monitor.Bounds.Location;
            Size = window.Monitor.Bounds.Size;
            mouseTimer.Enabled = mWindow.MouseControl;
        }

        private void mWindow_MonitorChanged(Window window, Screen screen) {
            Location = screen.Bounds.Location;
            Size = screen.Bounds.Size;
        }

        private void drawPanel_Paint(object sender, PaintEventArgs e) {
            if (mWindow.Coordinator.Overlay != null)
                mWindow.Coordinator.Overlay.Draw(e.Graphics, e.ClipRectangle, mWindow);
        }

        private void mouseTimer_Tick(object sender, EventArgs e) {
            if (Bounds.Contains(Cursor.Position) || mMouseOnScreen) {
                mMouseOnScreen = true;
                if (mLastMouse.X != Cursor.Position.X || mLastMouse.Y != Cursor.Position.Y) {
                    mWindow.UpdateCursor(Cursor.Position.X - Bounds.Left, Cursor.Position.Y - Bounds.Top);
                    mLastMouse = Cursor.Position;
                } else {
                    mWindow.UpdateCursor(mLastMouse.X - Bounds.Left + mJitter, Cursor.Position.Y - Bounds.Top);
                    mJitter *= -1;
                }
            } else
                mMouseOnScreen = false;
        }

        private void OverlayWindow_Load(object sender, EventArgs e) {
            Text = mWindow.Name + " Overlay";
        }
    }
}
