using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chimera.Inputs {
    public partial class KBMousePanel : UserControl {
        private KBMouseInput mInput;

        public KBMousePanel() {
            InitializeComponent();
        }

        public KBMousePanel(KBMouseInput input)
            : this() {
            Init(input);
        }

        public void Init(KBMouseInput input) {
            mInput = input;

            moveTimer.Interval = input.Coordinator.TickLength;
            moveTimer.Enabled = true;
            moveTimer.Tick += new EventHandler(input.panel_Tick);

            mousePanel.MouseDown += new MouseEventHandler(mInput.panel_MouseDown);
            mousePanel.MouseUp += new MouseEventHandler(mInput.panel_MouseUp);
            mousePanel.MouseMove += new MouseEventHandler(mInput.panel_MouseMove);

            mInput.MouseScale = mouseScaleSlider.Value;
            mInput.KBScale = keyboardScaleSlider.Value;
        }

        private void mousePanel_MouseUp(object sender, MouseEventArgs e) {
            Point centre = new Point(mousePanel.Width / 2, mousePanel.Height / 2);
            System.Windows.Forms.Cursor.Position = mousePanel.PointToScreen(centre);
            mousePanel.Refresh();
        }

        private void mousePanel_MouseMove(object sender, MouseEventArgs e) {
                mousePanel.Refresh();
        }

        private void mousePanel_Paint(object sender, PaintEventArgs e) {
            if (mInput != null) {
                if (mInput.MouseDown)
                    e.Graphics.DrawLine(new Pen(Color.Black), mInput.X, mInput.Y, mInput.CurrentX, mInput.IgnorePitch ? mInput.Y : mInput.CurrentY);
                else {
                    e.Graphics.Clear(mousePanel.BackColor);
                    string text = "Click and Drag to Mouselook";
                    Point pos = new Point((e.ClipRectangle.Width / 2) - 70, e.ClipRectangle.Height / 2);
                    e.Graphics.DrawString(text, Form.DefaultFont, Brushes.Black, pos);
                }
            }
        }

        internal void Stop() {
            moveTimer.Enabled = false;
        }

        private void mouseScaleSlider_Scroll(object sender, EventArgs e) {
            if (mInput != null)
                mInput.MouseScale = mouseScaleSlider.Value;
        }

        private void keyboardScaleSlider_Scroll(object sender, EventArgs e) {
            if (mInput != null)
                mInput.KBScale = keyboardScaleSlider.Value;
        }

        private void ignorePitchCheck_CheckedChanged(object sender, EventArgs e) {
            if (mInput != null)
                mInput.IgnorePitch = ignorePitchCheck.Checked;
        }
    }
}
