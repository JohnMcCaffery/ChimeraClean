using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chimera.Overlay.GUI.Plugins {
    public partial class OverlayPluginPanel : UserControl {
        private OverlayPlugin mOverlayPlugin;

        public OverlayPluginPanel() {
            InitializeComponent();
        }

        public OverlayPluginPanel(OverlayPlugin overlayPlugin)
            : this() {
            // TODO: Complete member initialization
            mOverlayPlugin = overlayPlugin;
        }
    }
}
