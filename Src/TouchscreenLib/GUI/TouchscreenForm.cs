using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
            int w = e.ClipRectangle.Width;
            int h = e.ClipRectangle.Height;
            Rectangle rect = new Rectangle(
                (int)(w * (mPlugin.Left.StartH + mPlugin.Left.PaddingH)),
                (int)(h * mPlugin.Left.PaddingV),
                (int)(w * mPlugin.Left.W),
                (int)(h * mPlugin.Left.H));
            e.Graphics.DrawRectangle(Pens.Black, rect);
            
            int centreX = (rect.Width / 2) + rect.X;
            int centreY = (rect.Height / 2) + rect.Y;
            rect = new Rectangle(
                (int) (centreX - (w * mPlugin.LeftX.Deadzone.Value)),
                (int) (centreY - (h * mPlugin.LeftY.Deadzone.Value)),
                (int) (w * mPlugin.LeftX.Deadzone.Value * 2),
                (int) (h * mPlugin.LeftY.Deadzone.Value * 2));
            e.Graphics.DrawRectangle(Pens.Black, rect);




            rect = new Rectangle(
                (int)(w * (mPlugin.Right.StartH + mPlugin.Right.PaddingH)),
                (int)(h * mPlugin.Right.PaddingV),
                (int)(w * mPlugin.Right.W),
                (int)(h * mPlugin.Right.H));
            e.Graphics.DrawRectangle(Pens.Black, rect);
            
            centreX = (rect.Width / 2) + rect.X;
            centreY = (rect.Height / 2) + rect.Y;
            rect = new Rectangle(
                (int) (centreX - (w * mPlugin.RightX.Deadzone.Value)),
                (int) (centreY - (h * mPlugin.RightY.Deadzone.Value)),
                (int) (w * mPlugin.RightX.Deadzone.Value * 2),
                (int) (h * mPlugin.RightY.Deadzone.Value * 2));
            e.Graphics.DrawRectangle(Pens.Black, rect);




                

            rect = new Rectangle(
                (int)(w * (mPlugin.Single.StartH + mPlugin.Left.PaddingH)),
                (int)(h * mPlugin.Single.PaddingV),
                (int)(w * mPlugin.Single.W),
                (int)(h * mPlugin.Single.H));
            e.Graphics.DrawRectangle(Pens.Black, rect);
            
            centreX = (rect.Width / 2) + rect.X;
            centreY = (rect.Height / 2) + rect.Y;
            rect = new Rectangle(
                rect.X,
                (int) (centreY - (h * mPlugin.Single.Deadzone.Value)),
                rect.Width,
                (int) (h * mPlugin.Single.Deadzone.Value * 2));
            e.Graphics.DrawRectangle(Pens.Black, rect);



        }
    }
}
