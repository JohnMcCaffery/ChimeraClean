using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FlythroughLib;

namespace ChimeraGUILib.Controls.FlythroughEventPanels {
    public partial class BlankPanel : UserControl {
        private BlankEvent mEvent;

        public BlankPanel() {
            InitializeComponent();
        }

        public BlankPanel(BlankEvent evt)
            : this() {
            mEvent = evt;

            if (mEvent.Length == 0f)
                mEvent.Length = (int)lengthValue.Value;
            else
                lengthValue.Value = mEvent.Length;

            lengthValue.ValueChanged += (source, args) => mEvent.Length = (int)lengthValue.Value;
            evt.OnStep += (source, args) => {
                Invoke(new Action(() => {
                    progressBar.Maximum = evt.TotalSteps;
                    progressBar.Value = Math.Min(evt.CurrentStep, progressBar.Maximum);
                }));
            };
        }
    }
}
