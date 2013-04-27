using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Inputs;

namespace Chimera.GUI.Controls.Inputs {
    public partial class SelectableInputPanel : UserControl {
        private SelectableInput mInput;

        public SelectableInputPanel() {
            InitializeComponent();
        }

        public SelectableInputPanel(SelectableInput input)
            : this() {

            mInput = input;
            mInput.InputAdded += new Action<ISystemInput>(mInput_InputAdded);

            foreach (var inpt in mInput.Inputs)
                mInput_InputAdded(inpt);
        }

        public void mInput_InputAdded(ISystemInput input) {
            inputSelectionBox.Items.Add(input);
            if (mInput.CurrentInput == input)
                inputSelectionBox.SelectedItem = input;
        }

        private void inputSelectionBox_SelectedIndexChanged(object sender, EventArgs e) {
            mInput.CurrentInput = (ISystemInput)inputSelectionBox.SelectedItem;
        }
    }
}
