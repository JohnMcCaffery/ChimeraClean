using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay.GUI.Features {
    public partial class FeatureControl : UserControl {
        private IFeature mFeature;

        public FeatureControl() {
            InitializeComponent();
        }

        public FeatureControl(IFeature feature) {
            InitializeComponent();
            mFeature = feature;

            activeLabel.Text = mFeature.Active ? "Active" : "Inactive";

            /*
            mFeature.ActiveChanged += active => {
                if (InvokeRequired)
                    Invoke(new Action(() => activeLabel.Text = active ? "Active" : "Inactive"));
            };
            */
        }

        private void activateButton_Click(object sender, EventArgs e) {
            mFeature.Active = true;
        }

        private void deactivateButton_Click(object sender, EventArgs e) {
            mFeature.Active = false;
        }

        public void SetActive(bool active) {
            Invoke(new Action(() => activeLabel.Text = active ? "Active" : "Inactive"));
        }
    }
}
