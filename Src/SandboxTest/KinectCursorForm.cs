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
using OpenMetaverse;
using Chimera.Kinect;
using Chimera.Util;
using Chimera.Kinect.Interfaces;
using Chimera.Overlay;

namespace Test {
    public partial class KinectCursorForm : Form, IKinectController {
        private Window mWindow;
        private IKinectCursor mCursor;

        public KinectCursorForm() {
            InitializeComponent();
        }

        public KinectCursorForm(IKinectCursor cursor, Window window)
            : this() {
            mWindow = window;
            mCursor = cursor;
            mCursor.Init(this, window);

            // 
            // pointCursorPanel
            // 
            UserControl pointCursorPanel = mCursor.ControlPanel;
            pointCursorPanel.AutoScroll = true;
            pointCursorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            pointCursorPanel.Location = new System.Drawing.Point(3, 3);
            pointCursorPanel.MinimumSize = new System.Drawing.Size(307, 325);
            pointCursorPanel.Name = "pointCursorPanel";
            pointCursorPanel.Size = new System.Drawing.Size(732, 544);
            pointCursorPanel.TabIndex = 0;

            splitContainer1.Panel2.Controls.Add(pointCursorPanel);

            mCursor.CursorMove += new Action<IKinectCursor,float,float>(mCursor_CursorMove);
            mWindow.OverlayManager.CursorMoved += mWindow_CursorMove;
        }

        private void mCursor_CursorMove(IKinectCursor cursor, float x, float y) {
            mWindow.OverlayManager.UpdateCursor(x, y);
        }

        private void mWindow_CursorMove(WindowOverlayManager overlay, EventArgs args) {
            if (mCursor.OnScreen && !IsDisposed && Created)
                Invoke(new Action(() => {
                    cursorPanel.Refresh();
                    splitContainer1.Panel1.Refresh();
                    Refresh();
                }));

            Invalidate();
            splitContainer1.Panel1.Invalidate();
            cursorPanel.Invalidate();
        }

        private void cursorPanel_Paint(object sender, PaintEventArgs e) {
            if (mCursor.OnScreen) {
                //int x = (int) (mManager.Overlay.CursorX * e.ClipRectangle.Width);
                //int y = (int) (mManager.Overlay.CursorY * e.ClipRectangle.Height);

                //int r = 5;
                //e.Graphics.FillEllipse(Brushes.Red, x - r, y - r, r * 2, r * 2);
            }
        }

        public Vector3 Position {
            get { return Vector3.Zero; }
        }

        public Rotation Orientation {
            get { return new Rotation(); }
        }

        public event Action<Vector3> PositionChanged;

        public event Action<Rotation> OrientationChanged;
    }
}
