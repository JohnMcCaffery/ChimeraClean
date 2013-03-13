using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Chimera {
    public class ImageArea : ISelectable {
        private readonly double mSelectTime = 3000;
        private readonly double mTickLength;

        private IOverlayState mState;
        private Window mWindow;
        private Bitmap mImage;
        private Bitmap mScaledImage;
        private Rectangle mBounds;
        private DateTime mHoverStart;
        private double x, y;
        private double w, h;
        private bool mHovering;
        private bool mSelected;
        private bool mVisible;

        public ImageArea(string imageFile) {
            mImage = new Bitmap(imageFile);
            mTickLength = mSelectTime / 3;
        }

        private void CheckState() {
            if (mWindow.CursorX >= mBounds.Left && mWindow.CursorX <= mBounds.Right &&
                mWindow.CursorY >= mBounds.Top  && mWindow.CursorY <= mBounds.Bottom) {
                if (!mHovering) {
                    mHovering = true;
                    mHoverStart = DateTime.Now;
                }

                if (!mSelected && DateTime.Now.Subtract(mHoverStart).TotalMilliseconds > mSelectTime) {
                    if (Selected != null)
                        Selected(this);
                    mSelected = true;
                } else
                    new Thread(updateThread_Update).Start();

            } else {
                mHovering = false;
                mSelected = false;
            }
        }

        private void updateThread_Update() {
            Thread.Sleep(20);
            mWindow.RedrawOverlay();
            CheckState();
        }

        private void window_CursorMoved(Window window, EventArgs args) {
            CheckState();
        }

        private void window_MonitorChanged(Window window, Screen screen) {
            mBounds = new Rectangle(
                (int) (x * screen.Bounds.Width),
                (int) (y * screen.Bounds.Height),
                (int) (w * screen.Bounds.Width),
                (int) (h * screen.Bounds.Height));
            mScaledImage = new Bitmap(mImage, mBounds.Width, mBounds.Height);
        }

        #region Selectable Members

        public event Action<ISelectable> Selected;

        public event Action<ISelectable> Shown;

        public event Action<ISelectable> Hidden;

        public string DebugState {
            get { return ""; }
        }

        public bool Visible {
            get { return mVisible; }
            set {
                mVisible = value;
                if (value && Shown != null)
                    Shown(this);
                else if (!value && Hidden != null)
                    Hidden(this);
            }
        }

        public bool CurrentlySelected {
            get { return mSelected; }
        }

        public IOverlayState OverlayState {
            get { return mState; }
        }

        public void Init(Window window, IOverlayState state) {
            mWindow = window;
            mState = state;

            window.MonitorChanged += new Action<Window, Screen>(window_MonitorChanged);
            window.CursorMoved += new Action<Window,EventArgs>(window_CursorMoved);
        }

        public void Draw(Graphics graphics, Rectangle clipRectangle, Color transparentColour) {
            graphics.DrawImage(mScaledImage, mBounds.Location);
            if (mHovering && !mSelected) {
                double hoverTime = DateTime.Now.Subtract(mHoverStart).TotalMilliseconds;
                int tick = (int) (hoverTime / mTickLength) + 1;
                double progress = hoverTime % mTickLength;
                int fontSize = (int) (progress * (mBounds.Height / 2.0));
                Font font = new Font(FontFamily.GenericMonospace, 30f, FontStyle.Bold);
                float x = mBounds.X + ((mBounds.Width / 2) - (fontSize / 2));
                float y = mBounds.Y + ((mBounds.Height / 2) - (fontSize / 2));
                graphics.DrawString(tick.ToString(), font, Brushes.Black, x, y);
            }
        }

        public void Show() {
            Visible = true;
        }

        public void Hide() {
            Visible = false;
        }

        #endregion


        public ISelectionRenderer SelectionRenderer {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
    }
}
