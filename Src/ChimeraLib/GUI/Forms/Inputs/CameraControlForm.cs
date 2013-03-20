using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chimera.Inputs {
    public partial class CameraControlForm : Form {
        private KBMouseInput mInput;
        private bool mCleared;

        public CameraControlForm() {
            InitializeComponent();
        }

        public CameraControlForm(KBMouseInput input) : this() { 
            Init(input);
        }

        public void Init(KBMouseInput input) {
            mInput = input;

            MouseDown += new MouseEventHandler(mInput.panel_MouseDown);
            MouseUp += new MouseEventHandler(mInput.panel_MouseUp);
            MouseMove += new MouseEventHandler(mInput.panel_MouseMove);
            MouseWheel += new MouseEventHandler(CameraControlForm_MouseWheel);
        }

        void CameraControlForm_MouseWheel(object sender, MouseEventArgs e) {
            int newVal = Math.Max(1, Math.Min(1000, mInput.KBScale + (e.Delta / 6)));
            if (mInput != null)
                mInput.KBScale = newVal;
        }

        private void CameraControlForm_KeyDown(object sender, KeyEventArgs e) {
            if (mInput != null)
                mInput.Source.TriggerKeyboard(true, e);
        }

        private void CameraControlForm_KeyUp(object sender, KeyEventArgs e) {
            if (mInput != null)
                mInput.Source.TriggerKeyboard(false, e);
        }

        private void CameraControlForm_MouseUp(object sender, MouseEventArgs e) {
            Point centre = new Point(Width / 2, Height / 2);
            System.Windows.Forms.Cursor.Position = PointToScreen(centre);
            Refresh();
        }

        private void CameraControlForm_MouseMove(object sender, MouseEventArgs e) {
            if (mInput != null && (mInput.MouseDown || !mCleared)) {
                Refresh();
                mCleared = true;
            }
        }

        private void CameraControlForm_Paint(object sender, PaintEventArgs e) {
            if (mInput != null) {
                if (mInput.MouseDown) {
                    e.Graphics.DrawLine(new Pen(Color.Black), mInput.X, mInput.Y, mInput.CurrentX, mInput.IgnorePitch ? mInput.Y : mInput.CurrentY);
                    mCleared = false;
                } else {
                    //e.Graphics.Clear();
                    string text = "Click and Drag to Mouselook";
                    Point pos = new Point((e.ClipRectangle.Width / 2) - 70, e.ClipRectangle.Height / 2);
                    e.Graphics.DrawString(text, Form.DefaultFont, Brushes.Black, pos);
                }
            }
        }
    }
}
