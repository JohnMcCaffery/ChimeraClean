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
        /// The coordinator this overlay covers.
        /// </summary>
        private Window mWindow;

        public OverlayWindow() {
            InitializeComponent();
        }

        /// <param name="window">The window this overlay covers.</param>
        public OverlayWindow(Window window) : this() {
            Init(window);
        }

        /// <summary>
        /// Whether to go full screen.
        /// </summary>
        public bool Fullscreen {
            get { return FormBorderStyle == FormBorderStyle.None; }
            set { FormBorderStyle = value ? FormBorderStyle.Sizable : FormBorderStyle.None; }
        }

        /// <summary>
        /// Redraw the coordinator.
        /// </summary>
        public void Redraw() {
            Invoke(new Action(() => drawPanel.Refresh()));
        }

        /// <summary>
        /// Link this form with a logical coordinator.
        /// </summary>
        /// <param name="coordinator">The coordinator to link this form with.</param>
        public void Init(Window window) {
            Invoke(new Action(() => {
                Text = window.Name + " Overlay";
            }));
            mWindow = window;
        }

        private void drawPanel_Paint(object sender, PaintEventArgs e) {
            //if (mWindow.Coordinator.ActiveState != null)
                //mWindow.Coordinator.ActiveState.Draw(e.Graphics, e.ClipRectangle, TransparencyKey, mWindow);
        }
    }
}
