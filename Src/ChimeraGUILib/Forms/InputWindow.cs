using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChimeraLib;
using NuiLibDotNet;

namespace KinectLib {
    public partial class InputWindow : Form {
        public static readonly int CURSOR_R = 15;
        private PointSurface mPointSurface;

        public InputWindow() {
            InitializeComponent();
        }

        public void Init(KinectManager m, PointSurface surface) {
            mPointSurface = surface;
            configPanel.Init(m, surface);
            surface.OnChange += (sface) => {
                if (Created && !Disposing && !IsDisposed)
                    Invoke(new Action(() => drawPanel.Refresh()));
            };
        }

        public PointSurface Surface {
            get { return mPointSurface; }
            set { mPointSurface = value; }
        }

        private void InputWindow_Paint(object sender, PaintEventArgs e) {
            return;
            //e.Graphics.Clear(Color.Transparent);
            if (mPointSurface != null && mPointSurface.Active) {
                int x = (int) (Width * mPointSurface.X);
                int y = (int) (Height * mPointSurface.Y);
                e.Graphics.FillEllipse(Brushes.Red, new Rectangle(x - CURSOR_R, y - CURSOR_R, CURSOR_R * 2, CURSOR_R * 2));
            }
        }

        public void AddActiveArea(ActiveArea area) {
            drawPanel.Controls.Add(area.MakePictureBox(drawPanel));
        }

        private void drawPanel_MouseMove(object sender, MouseEventArgs e) {
            //if (mPointSurface != null)
                //mPointSurface.OverridePosition((float)e.X/ (float)Width , (float)e.Y/ (float)Height);
        }
    }
}
