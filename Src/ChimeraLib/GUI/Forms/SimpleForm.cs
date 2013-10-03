using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chimera.GUI.Forms {
    public partial class SimpleForm : Form {
        private readonly Core mCore;

        public SimpleForm() {
            InitializeComponent();
        }

        public SimpleForm(Core core)
            : this() {
            mCore = core;

            Frame[] frames = core.Frames;
            int w = (hSplit.Width - ((frames.Length - 1) * 4)) / frames.Length;

            SplitContainer container = hSplit;
            SplitterPanel panel = hSplit.Panel2;
            for (int i = 0; i < frames.Length; i++) {
                Frame frame = frames[i];
                Button b = new Button();
                b.Text = "Restart " + frame.Name;
                b.Dock = DockStyle.Fill;
                b.Click += (source, args) => frame.Output.Restart("GUI");

                if (i < frames.Length - 1) {
                    SplitContainer split = new SplitContainer();
                    split.IsSplitterFixed = true;
                    split.FixedPanel = FixedPanel.None;
                    split.SplitterDistance = w;
                    split.Panel1.Controls.Add(b);
                    split.Dock = DockStyle.Fill;

                    panel.Controls.Add(split);
                    panel = split.Panel2;
                    container = split;
                } else
                    container.Panel2.Controls.Add(b);
            }

            foreach (var plugin in mCore.Plugins)
                plugin.SetForm(this);
        }

        private void SimpleForm_FormClosing(object sender, FormClosingEventArgs e) {
            mCore.Close("Shut down");
        }

        private void shutdownButton_Click(object sender, EventArgs e) {
            Close();
        }
    }
}
