using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Overlay;

namespace Chimera.GUI.Forms {
    public partial class OverlayWindow : Form {
        private WindowOverlayManager mManager;

        public OverlayWindow() {
            InitializeComponent();
        }

        public OverlayWindow(WindowOverlayManager manager) {
            Init(manager);
        }

        public void Init(WindowOverlayManager manager) {
            mManager = manager;
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
    }
}
