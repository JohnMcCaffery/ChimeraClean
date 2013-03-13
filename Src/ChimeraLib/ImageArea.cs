using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Chimera {
    public class ImageArea : ISelectable {
        private readonly double mSelectTime = 1500;

        private IOverlayState mState;
        private ISelectionRenderer mRenderer;
        private Window mWindow;
        private Bitmap mImage;
        private Bitmap mScaledImage;
        private Rectangle mBounds;
        private DateTime mHoverStart;
        private string mWindowName = "Main Window";
        private double x, y;
        private double w, h;
        private bool mHovering;
        private bool mSelected;
        private bool mVisible;

        public Bitmap ScaledImage {
            get { return mScaledImage; }
        }

        public ImageArea(string imageFile, double x, double y, double w, double h) {
            mImage = new Bitmap(imageFile);
            mRenderer = new NumberSelectionRenderer();
            mRenderer.Init(this);
            
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }

        public ImageArea(Window window, Bitmap image, double x, double y, double w, double h) {
            mImage = image;
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            SetWindow(window);
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
                }
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

        #region ISelectable Members

        public event Action<ISelectable> Selected;

        public event Action<ISelectable> Shown;

        public event Action<ISelectable> Hidden;

        public string WindowName {
            get { return mWindowName; }
        }

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

        public ISelectionRenderer SelectionRenderer {
            get { return mRenderer; }
        }

        public Rectangle Bounds {
            get { return mBounds; }
        }

        public void Init(IOverlayState state) {
            mState = state;
            SetWindow(state.Coordinator.Windows.First(w => w.Name.Equals(mWindowName)));
        }

        private void SetWindow(Window window) {
            mWindow = window;

            mWindow.MonitorChanged += new Action<Window, Screen>(window_MonitorChanged);
            mWindow.CursorMoved += new Action<Window,EventArgs>(window_CursorMoved);

            window_MonitorChanged(mWindow, mWindow.Monitor);
        }

        public void Draw(Graphics graphics, Rectangle clipRectangle) {
            CheckState();
            if (mHovering && !mSelected) {
                mRenderer.DrawHover(graphics, clipRectangle, mHoverStart, mSelectTime);
            }
        }

        public void DrawBG(Graphics graphics, Rectangle clipRectangle) {
            graphics.DrawImage(mScaledImage, mBounds.Location);
        }

        public void Show() {
            Visible = true;
        }

        public void Hide() {
            Visible = false;
        }

        #endregion
    }
}
