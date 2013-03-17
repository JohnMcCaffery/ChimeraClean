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

            this.tabPage2.Controls.Add(pointCursorPanel);

            mWindow.CursorMoved += mCursor_CursorMove;
        }

        private void mCursor_CursorMove(Window window, EventArgs args) {
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
                int x = (int) (mWindow.CursorX * e.ClipRectangle.Width);
                int y = (int) (mWindow.CursorY * e.ClipRectangle.Height);

                int r = 5;
                e.Graphics.FillEllipse(Brushes.Red, x - r, y - r, r * 2, r * 2);
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
