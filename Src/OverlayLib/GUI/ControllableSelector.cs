using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Overlay.Interfaces;

namespace Chimera.Overlay.GUI {
    public partial class ControllableSelector : UserControl {
        public ControllableSelector() {
            InitializeComponent();
        }
        public ControllableSelector(IEnumerable<IControllable> items)
            : this() {

                foreach (var item in items)
                    namesBox.Items.Add(item);
        }

        private void namesBox_SelectedIndexChanged(object sender, EventArgs e) {
            while (controlPanel.Controls.Count > 0)
                controlPanel.Controls.Remove(controlPanel.Controls[0]);

            controlPanel.Controls.Add(((IControllable)namesBox.SelectedItem).ControlPanel);
        }
    }
}
