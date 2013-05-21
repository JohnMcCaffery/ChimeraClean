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
    public partial class ControllableSelector<T> : UserControl where T : IControllable {
        public T SelectedItem {
            get {  return (T)namesBox.SelectedItem; }
            set { namesBox.SelectedItem = value; }
        }

        public ControllableSelector() {
            InitializeComponent();
        }
        public ControllableSelector(IEnumerable<T> items)
            : this() {

                Init(items);
        }

        public void Init(IEnumerable<T> items) {
            foreach (var item in items)
                namesBox.Items.Add(item);
        }

        private void namesBox_SelectedIndexChanged(object sender, EventArgs e) {
            while (controlPanel.Controls.Count > 0)
                controlPanel.Controls.Remove(controlPanel.Controls[0]);

            Control c = ((T)namesBox.SelectedItem).ControlPanel;
            c.Dock = DockStyle.Fill;
            controlPanel.Controls.Add(c);
        }
    }
}
