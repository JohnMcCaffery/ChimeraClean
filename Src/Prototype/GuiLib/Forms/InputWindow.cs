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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera;
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
