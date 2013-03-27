using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera.Overlay {
    public class TestState : Chimera.IState {
        private int mXShift = 20, mYShift = 20;
        private bool mActive;

        #region From Members

        public event Action<IState> Activated;

        public event Action<IState> Deactivated;

        public string State {
            get { throw new NotImplementedException(); }
        }

        public string Name {
            get { return "Test State"; }
        }

        public string Type {
            get { return "Test State type"; }
        }

        public bool Active {
            get { return mActive; }
            set {
                mActive = value;
                if (value && Activated != null)
                    Activated(this);
                else if (!value && Deactivated != null)
                    Deactivated(this);
            }
        }

        public void Init(IOverlay overlay) { }

        public void DrawDynamic(Graphics graphics, Rectangle clipRectangle, Window window) {
            mXShift *= -1;
            mYShift *= -1;
            using (Pen pen = new Pen(Brushes.Red, 20f)) {
                Point p = new Point((int) (clipRectangle.Width / 2f) + mXShift, (int) (clipRectangle.Height / 2f) + mYShift);
                graphics.DrawEllipse(pen, new Rectangle(p, new Size(10, 10)));
                graphics.DrawEllipse(pen, clipRectangle);
            }
        }

        public void Deactivate() {
            Console.WriteLine("Deactivated test event.");
            Active = false;
        }

        public void Activate() {
            Console.WriteLine("Activated test event.");
            Active = true;
        }

        public void DrawStatic(Graphics graphics, Rectangle clipRectangle, Window window) {
            //Bitmap bg = new Bitmap("D:\\Pictures\\My Photos\\g.jpg");
            //bg = new Bitmap(bg, window.Monitor.PhysicalBounds.Width, window.Monitor.PhysicalBounds.Height);
            //graphics.DrawImage(bg, Point.Empty);
        }

        #endregion
    }
}
