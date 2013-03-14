using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Chimera.Overlay {
    public class ImageArea : ISelectable {
        private readonly double mSelectTime = 1500;

        private IOverlayState mState;
        private ISelectionRenderer mRenderer;
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
        private bool mActive = true;

        public Bitmap ScaledImage {
            get { return mScaledImage; }
            set {
            }
        }

        public Bitmap Image {
            get { return mImage; }
            set {
                mImage = value;
                window_MonitorChanged(mWindow, mWindow.Monitor);
                if (ImageChanged != null)
                    ImageChanged(this, null);
            }
        }

        public ImageArea(Bitmap image, double x, double y, double w, double h) {
            mImage = image;
            mRenderer = new NumberSelectionRenderer();
            mRenderer.Init(this);
            
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }

        public ImageArea(string imageFile, double x, double y, double w, double h)
            : this(new Bitmap(imageFile), x, y, w, h) {
        }

        public ImageArea(Window window, Bitmap image, double x, double y, double w, double h)
            : this(image, x, y, w, h) {
            Init(window);
        }

        public ImageArea(Window window, string imageFile, double x, double y, double w, double h)
            : this(window, new Bitmap(imageFile), x, y, w, h) {
        }

        private void CheckState() {
            if (mBounds.Contains(mWindow.Cursor) && mActive) {
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

        public event Action<ISelectable> StaticChanged;

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

        public bool Active {
            get { return mActive; }
            set { mActive = value; }
        }

        public bool CurrentlySelected {
            get { return mSelected; }
        }

        public ISelectionRenderer SelectionRenderer {
            get { return mRenderer; }
        }

        public Rectangle Bounds {
            get { return mBounds; }
        }

        public void Init(Window window) {
            mWindow = window;

            mWindow.MonitorChanged += new Action<Window, Screen>(window_MonitorChanged);
            mWindow.CursorMoved += new Action<Window,EventArgs>(window_CursorMoved);

            window_MonitorChanged(mWindow, mWindow.Monitor);
        }

        public void DrawDynamic(Graphics graphics, Rectangle clipRectangle) {
            CheckState();
            if (mSelected)
                mRenderer.DrawSelected(graphics, clipRectangle);
            else if (mHovering)
                mRenderer.DrawHover(graphics, clipRectangle, mHoverStart, mSelectTime);
        }

        public void DrawStatic(Graphics graphics, Rectangle clipRectangle) {
            graphics.DrawImage(mScaledImage, mBounds.Location);
        }

        public void Show() {
            Visible = true;
        }

        public void Hide() {
            Visible = false;
        }

        #endregion

        /// <summary>
        /// Triggered when the image this area renders as its selectable area changes.
        /// </summary>
        public event EventHandler ImageChanged;
    }
}
