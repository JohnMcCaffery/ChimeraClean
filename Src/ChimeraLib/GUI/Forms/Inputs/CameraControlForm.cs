using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chimera.Plugins {
    public partial class CameraControlForm : Form {
        private KBMousePlugin mPlugin;
        private bool mCleared;

        public CameraControlForm() {
            InitializeComponent();
        }

        public CameraControlForm(KBMousePlugin input) : this() { 
            Init(input);
        }

        public void Init(KBMousePlugin input) {
            mPlugin = input;

            MouseDown += new MouseEventHandler(mPlugin.panel_MouseDown);
            MouseUp += new MouseEventHandler(mPlugin.panel_MouseUp);
            MouseMove += new MouseEventHandler(mPlugin.panel_MouseMove);
            MouseWheel += new MouseEventHandler(CameraControlForm_MouseWheel);
        }

        void CameraControlForm_MouseWheel(object sender, MouseEventArgs e) {
            int newVal = Math.Max(1, Math.Min(1000, mPlugin.KBScale + (e.Delta / 6)));
            if (mPlugin != null)
                mPlugin.KBScale = newVal;
        }

        private void CameraControlForm_KeyDown(object sender, KeyEventArgs e) {
            if (mPlugin != null)
                mPlugin.Source.TriggerKeyboard(true, e);
        }

        private void CameraControlForm_KeyUp(object sender, KeyEventArgs e) {
            if (mPlugin != null)
                mPlugin.Source.TriggerKeyboard(false, e);
        }

        private void CameraControlForm_MouseUp(object sender, MouseEventArgs e) {
            Point centre = new Point(Width / 2, Height / 2);
            System.Windows.Forms.Cursor.Position = PointToScreen(centre);
            Refresh();
        }

        private void CameraControlForm_MouseMove(object sender, MouseEventArgs e) {
            if (mPlugin != null && (mPlugin.MouseDown || !mCleared)) {
                Refresh();
                mCleared = true;
            }
        }

        private void CameraControlForm_Paint(object sender, PaintEventArgs e) {
            if (mPlugin != null) {
                if (mPlugin.MouseDown) {
                    e.Graphics.DrawLine(new Pen(Color.Black), mPlugin.X, mPlugin.Y, mPlugin.CurrentX, mPlugin.IgnorePitch ? mPlugin.Y : mPlugin.CurrentY);
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
