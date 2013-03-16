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

namespace Test {
    public partial class KinectCursorForm : Form {
        private Window mWindow;
        private IKinectCursorWindow mCursor;

        public KinectCursorForm() {
            InitializeComponent();
        }

        public KinectCursorForm(IKinectCursorWindow cursor, Window window)
            : this() {
            mWindow = window;
            mCursor = cursor;
            mCursor.Init(window, Vector3.Zero, new Rotation(0.0, 180.0));

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

            this.tabPage2.Controls.Add(pointCursorPanel);

            mCursor.CursorMove += new Action<int, int>(mCursor_CursorMove);
        }

        private void mCursor_CursorMove(int x, int y) {
            if (mCursor.OnScreen && !IsDisposed && Created)
                Invoke(new Action(() => {
                    cursorPanel.Refresh();
                    tabPage1.Refresh();
                    Refresh();
                }));

            Invalidate();
            tabPage1.Invalidate();
            cursorPanel.Invalidate();
        }

        private void cursorPanel_Paint(object sender, PaintEventArgs e) {
            if (mCursor.OnScreen) {
                int x = (int) (((double) mCursor.Location.X / (double) mWindow.Monitor.Bounds.Width) * (double) e.ClipRectangle.Width);
                int y = (int) (((double) mCursor.Location.Y / (double) mWindow.Monitor.Bounds.Height) * (double) e.ClipRectangle.Height);

                int r = 5;
                e.Graphics.FillEllipse(Brushes.Red, x - r, y - r, r * 2, r * 2);
            }
        }
    }
}
