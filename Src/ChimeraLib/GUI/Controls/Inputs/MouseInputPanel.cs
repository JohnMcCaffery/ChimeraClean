using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Inputs;
using Chimera.Util;

namespace Chimera.GUI.Controls.Inputs {
    public partial class MouseInputPanel : UserControl {
        private MouseInput mInput;
        private Action<int, int> mMouseMovedListener;

        public MouseInputPanel() {
            InitializeComponent();
            mMouseMovedListener = new Action<int, int>(mInput_MouseMoved);
            HandleCreated += new EventHandler(MouseInputPanel_HandleCreated);
            Disposed += new EventHandler(MouseInputPanel_Disposed);
        }

        void MouseInputPanel_HandleCreated(object sender, EventArgs e) {
            if (mInput != null)
                mInput.MouseMoved += mMouseMovedListener;
        }

        void MouseInputPanel_Disposed(object sender, EventArgs e) {
            if (mInput != null)
                mInput.MouseMoved -= mMouseMovedListener;
        }

        public MouseInputPanel(MouseInput input)
            : this() {
            Init(input);
        }

        public void Init(MouseInput input) {
            mInput = input;
        }

        private void mInput_MouseMoved(int x, int y) {
            if (Created && !IsDisposed && !Disposing)
                Invoke(new Action(() => {
                    positionLabel.Text = string.Format("{0,-4},{1,-4}", x, y);
                }));
        }
    }
}
