using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chimera.Plugins {
    public partial class KBMousePanel : UserControl {
        private KBMousePlugin mPlugin;
        private CameraControlForm mForm;
        private bool mCleared;
        private bool mGuiInput;

        public KBMousePanel() {
            InitializeComponent();
        }

        public KBMousePanel(KBMousePlugin plugin)
            : this() {
            Init(plugin);
        }

        public void Init(KBMousePlugin plugin) {
            mPlugin = plugin;

            mousePanel.MouseDown += new MouseEventHandler(mPlugin.panel_MouseDown);
            mousePanel.MouseUp += new MouseEventHandler(mPlugin.panel_MouseUp);
            mousePanel.MouseMove += new MouseEventHandler(mPlugin.panel_MouseMove);

            mPlugin.MouseScale = mouseScaleSlider.Value;
            mPlugin.KBScale = keyboardScaleSlider.Value;
            mPlugin.KBScaleChange += new Action<int>(mPlugin_KBScaleChange);
            mPlugin.MouseScaleChange += new Action<int>(mPlugin_MouseScaleChange);
        }

        private void mPlugin_KBScaleChange (int newScale) {
            if (!mGuiInput) {
                keyboardScaleSlider.Minimum = Math.Min(newScale, keyboardScaleSlider.Minimum);
                keyboardScaleSlider.Maximum = Math.Max(newScale, keyboardScaleSlider.Maximum);
                keyboardScaleSlider.Value = newScale;
            }
        }

        private void mPlugin_MouseScaleChange(int newScale) {
            if (!mGuiInput) {
                mouseScaleSlider.Minimum = Math.Min(newScale, mouseScaleSlider.Minimum);
                mouseScaleSlider.Maximum = Math.Max(newScale, mouseScaleSlider.Maximum);
                mouseScaleSlider.Value = newScale;
            }
        }

        private void mousePanel_MouseUp(object sender, MouseEventArgs e) {
            Point centre = new Point(mousePanel.Width / 2, mousePanel.Height / 2);
            System.Windows.Forms.Cursor.Position = mousePanel.PointToScreen(centre);
            mousePanel.Refresh();
        }

        private void mousePanel_MouseMove(object sender, MouseEventArgs e) {
            if (mPlugin != null && (mPlugin.MouseDown || !mCleared)) {
                mousePanel.Refresh();
                mCleared = true;
            }
        }

        private void mousePanel_Paint(object sender, PaintEventArgs e) {
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

        internal void Stop() { }

        private void mouseScaleSlider_Scroll(object sender, EventArgs e) {
            if (mPlugin != null) {
                mGuiInput = true;
                mPlugin.MouseScale = mouseScaleSlider.Value;
                mGuiInput = false;
            }
        }

        private void keyboardScaleSlider_Scroll(object sender, EventArgs e) {
            if (mPlugin != null) {
                mGuiInput = true;
                mPlugin.KBScale = keyboardScaleSlider.Value;
                mGuiInput = false;
            }
        }

        private void ignorePitchCheck_CheckedChanged(object sender, EventArgs e) {
            if (mPlugin != null)
                mPlugin.IgnorePitch = ignorePitchCheck.Checked;
        }

        private void showWindowButton_Click(object sender, EventArgs e) {
            if (mPlugin != null && showWindowButton.Text == "Show Window") {
                mForm = new CameraControlForm(mPlugin);
                mForm.FormClosed += (source, args) => showWindowButton.Text = "Show Window";
                mForm.Show(this);
                    showWindowButton.Text = "Hide Window";
            } else {
                if (mForm != null)
                    mForm.Close();
                else
                    showWindowButton.Text = "Show Window";
            }
        }
    }
}
