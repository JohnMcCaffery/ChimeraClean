using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Interfaces;

namespace Chimera.GUI.Forms {
    public partial class SimpleOverlay : Form, IOverlayWindow {        /// <summary>
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

        public SimpleOverlay() {
            InitializeComponent();
        }
        /// <param name="window">The window this overlay covers.</param>
        public SimpleOverlay(OverlayController controller, Color transparentColour)
            : this() {
            Init(controller);
            TransparencyKey = transparentColour;
            // http://www.cursor.cc/
            Cursor = new Cursor("../Cursors/cursor.cur");

            drawPanel.BackColor = transparentColour;
        }

        /// <summary>
        /// Link this form with a logical input.
        /// </summary>
        /// <param name="input">The input to link this form with.</param>
        public void Init(OverlayController controller) {
            mController = controller;

            TopMost = true;
            StartPosition = FormStartPosition.Manual;
            Location = mController.Window.Monitor.Bounds.Location;
            Size = mController.Window.Monitor.Bounds.Size;
        }
        public void Foreground() {
            Invoke(new Action(() => BringToFront()));
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

        private void drawPanel_Paint(object sender, PaintEventArgs e) {
            using (Pen pen = new Pen(Color.Red, 50f)) {
                e.Graphics.DrawEllipse(pen, e.ClipRectangle);
            }
        }
    }
}
