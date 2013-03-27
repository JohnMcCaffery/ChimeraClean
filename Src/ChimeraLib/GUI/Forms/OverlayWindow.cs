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
        public OverlayWindow() {
            InitializeComponent();
        }

        public Chimera.Overlay.WindowOverlayManager WindowOverlayManager {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }
    }
}
