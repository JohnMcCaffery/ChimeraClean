using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chimera.Flythrough.GUI {
    public partial class BlankPanel<T> : UserControl {
        private BlankEvent<T> mEvent;

        public BlankPanel() {
            InitializeComponent();
        }

        public BlankPanel(BlankEvent<T> evt)
            : this() {
            mEvent = evt;

            if (mEvent.Length == 0f)
                mEvent.Length = (int)lengthValue.Value;
            else
                lengthValue.Value = mEvent.Length;

            lengthValue.ValueChanged += (source, args) => mEvent.Length = (int)lengthValue.Value;
            evt.TimeChange += (e, time) => {
                Invoke(new Action(() => {
                    progressBar.Maximum = evt.Length;
                    progressBar.Value = evt.Time;
                }));
            };
        }
    }
}
