using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Interfaces;

namespace Touchscreen.GUI {
    public partial class TouchscreenForm : Form {
        private TouchscreenPlugin mPlugin;

        public TouchscreenForm() {
            InitializeComponent();
        }
        public TouchscreenForm(TouchscreenPlugin plugin) : this() {
            mPlugin = plugin;
            mPlugin.Left.SizeChanged += new Action(Left_SizeChanged);
            mPlugin.Right.SizeChanged += new Action(Left_SizeChanged);
            mPlugin.Single.SizeChanged += new Action(Left_SizeChanged);
        }

        void Left_SizeChanged() {
            if (InvokeRequired)
                BeginInvoke(new Action(() => Invalidate()));
            else
                Invalidate();
        }

        private void TouchscreenForm_Paint(object sender, PaintEventArgs e) {
            TouchscreenForm_Paint(e.Graphics, e.ClipRectangle, mPlugin);
        }

        private static string GetBinding(AxisBinding binding) {
            switch (binding) {
                case AxisBinding.X: return "Move";
                case AxisBinding.Pitch: return "Camera";
                case AxisBinding.Z: return "Fly";
            }
            return "Unknown";
        }

        internal static void TouchscreenForm_Paint(Graphics g, Rectangle clip, TouchscreenPlugin plugin) {
            int w = clip.Width;
            int h = clip.Height;
            using (Font font = new Font("Verdana", 30f)) {

                Rectangle rect = new Rectangle(
                    (int)(w * (plugin.Left.StartH + plugin.Left.PaddingH)),
                    (int)(h * plugin.Left.PaddingV),
                    (int)(w * plugin.Left.W),
                    (int)(h * plugin.Left.H));
                g.DrawRectangle(Pens.Black, rect);

                string msg = GetBinding(plugin.LeftY.Binding);
                SizeF s = g.MeasureString(msg, font);
                int textW = (int)s.Width;
                int textH = (int)s.Height;
                g.DrawString(msg, font, Brushes.Black, new Point(rect.X + ((rect.Width / 2) - (textW / 2)), rect.Y - textH));


                int centreX = (rect.Width / 2) + rect.X;
                int centreY = (rect.Height / 2) + rect.Y;
                rect = new Rectangle(
                    (int)(centreX - (w * plugin.LeftX.Deadzone.Value)),
                    (int)(centreY - (h * plugin.LeftY.Deadzone.Value)),
                    (int)(w * plugin.LeftX.Deadzone.Value * 2),
                    (int)(h * plugin.LeftY.Deadzone.Value * 2));
                g.DrawRectangle(Pens.Black, rect);




                rect = new Rectangle(
                    (int)(w * (plugin.Right.StartH + plugin.Right.PaddingH)),
                    (int)(h * plugin.Right.PaddingV),
                    (int)(w * plugin.Right.W),
                    (int)(h * plugin.Right.H));
                g.DrawRectangle(Pens.Black, rect);

                msg = GetBinding(plugin.RightY.Binding);
                s = g.MeasureString(msg, font);
                textW = (int)s.Width;
                textH = (int)s.Height;
                g.DrawString(msg, font, Brushes.Black, new Point(rect.X + ((rect.Width / 2) - (textW / 2)), rect.Y - textH));

                centreX = (rect.Width / 2) + rect.X;
                centreY = (rect.Height / 2) + rect.Y;
                rect = new Rectangle(
                    (int)(centreX - (w * plugin.RightX.Deadzone.Value)),
                    (int)(centreY - (h * plugin.RightY.Deadzone.Value)),
                    (int)(w * plugin.RightX.Deadzone.Value * 2),
                    (int)(h * plugin.RightY.Deadzone.Value * 2));
                g.DrawRectangle(Pens.Black, rect);






                rect = new Rectangle(
                    (int)(w * (plugin.Single.StartH + plugin.Left.PaddingH)),
                    (int)(h * plugin.Single.PaddingV),
                    (int)(w * plugin.Single.W),
                    (int)(h * plugin.Single.H));
                g.DrawRectangle(Pens.Black, rect);

                msg = GetBinding(plugin.Single.Binding);
                s = g.MeasureString(msg, font);
                textW = (int)s.Width;
                textH = (int)s.Height;
                g.DrawString(msg, font, Brushes.Black, new Point(rect.X + ((rect.Width / 2) - (textW / 2)), rect.Y - textH));

                centreX = (rect.Width / 2) + rect.X;
                centreY = (rect.Height / 2) + rect.Y;
                rect = new Rectangle(
                    rect.X,
                    (int)(centreY - (h * plugin.Single.Deadzone.Value)),
                    rect.Width,
                    (int)(h * plugin.Single.Deadzone.Value * 2));
                g.DrawRectangle(Pens.Black, rect);
            }
        }
    }
}
