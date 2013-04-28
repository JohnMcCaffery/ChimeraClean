/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.GUI;
using Chimera;

namespace Joystick.GUI {
    public partial class JoystickPanel : UserControl {
        private List<ScalarPanel> mAxisPanels = new List<ScalarPanel>();
        private JoystickInput mInput;
        private Action<IDeltaInput> mChangeListener;

        public JoystickPanel() {
            InitializeComponent();
        }

        public JoystickPanel(JoystickInput input)
            : this() {
            mInput = input;

            mChangeListener = new Action<IDeltaInput>(mInput_Change);

            Disposed += new EventHandler(JoystickPanel_Disposed);
            HandleCreated += new EventHandler(JoystickPanel_HandleCreated);
        }

        void JoystickPanel_Disposed(object sender, EventArgs e) {
            mInput.Change -= mChangeListener;
        }

        void JoystickPanel_HandleCreated(object sender, EventArgs e) {
            mInput.Change += mChangeListener;
        }

        private void mInput_Change(IDeltaInput input) {
            for (int i = 0; i < mInput.Sliders.Length; i++) {
                if (i == mAxisPanels.Count)
                    AddAxisPanel();

                mAxisPanels[i].Value = mInput.Sliders[i];
            }
        }

        private void AddAxisPanel() {            ScalarPanel panel = new ScalarPanel();            // 
            // scalarPanel1
            // 
            panel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            panel.Max = 10F;
            panel.Min = -10F;
            panel.MinimumSize = new System.Drawing.Size(95, 20);
            panel.Name = "ScalarPanel" + (mAxisPanels.Count + 2);
            panel.Size = new System.Drawing.Size(545, 20);
            panel.Location = new System.Drawing.Point(3, 3 + (panel.Size.Height * mAxisPanels.Count));
            panel.TabIndex = 0;
            panel.Value = 0F;

            mAxisPanels.Add(panel);
            Invoke(new Action(() => Controls.Add(panel)));
        }
    }
}
