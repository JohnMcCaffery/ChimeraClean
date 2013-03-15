using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Chimera.Overlay {
    public abstract class SelectableArea : ISelectable {
        private readonly double mSelectTime = 1500;

        private ISelectionRenderer mRenderer;
        private Window mWindow;
        private Rectangle mBounds;
        private DateTime mHoverStart;
        private double x, y;
        private double w, h;
        private bool mHovering;
        private bool mSelected;
        private bool mVisible;
        private bool mActive = true;

        public SelectableArea(double x, double y, double w, double h) {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }

        public SelectableArea(ISelectionRenderer selector, double x, double y, double w, double h)
            : this(x, y, w, h) {

            mRenderer = selector;
        }

        protected void SetRenderer(ISelectionRenderer renderer) {
            mRenderer = renderer;
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
        }

        #region ISelectable Members

        public abstract event Action<ISelectable> StaticChanged;

        public virtual event Action<ISelectable> Selected;

        public virtual event Action<ISelectable> Shown;

        public virtual event Action<ISelectable> Hidden;

        public virtual string DebugState {
            get { return ""; }
        }

        public virtual bool Visible {
            get { return mVisible; }
            set {
                mVisible = value;
                if (value && Shown != null)
                    Shown(this);
                else if (!value && Hidden != null)
                    Hidden(this);
            }
        }

        public virtual bool Active {
            get { return mActive; }
            set { mActive = value; }
        }

        public virtual bool CurrentlyHovering {
            get { return mHovering; }
        }

        public virtual bool CurrentlySelected {
            get { return mSelected; }
        }

        public virtual ISelectionRenderer SelectionRenderer {
            get { return mRenderer; }
        }

        public virtual Rectangle Bounds {
            get { return mBounds; }
        }

        public virtual void Init(Window window) {
            mWindow = window;

            mWindow.MonitorChanged += new Action<Window, Screen>(window_MonitorChanged);
            mWindow.CursorMoved += new Action<Window,EventArgs>(window_CursorMoved);

            window_MonitorChanged(mWindow, mWindow.Monitor);
        }

        public virtual void DrawDynamic(Graphics graphics, Rectangle clipRectangle) {
            if (mSelected)
                mRenderer.DrawSelected(graphics, clipRectangle);
            else if (mHovering)
                mRenderer.DrawHover(graphics, clipRectangle, mHoverStart, mSelectTime);
        }

        public abstract void DrawStatic(Graphics graphics, Rectangle clipRectangle);

        public abstract void Show();

        public abstract void Hide();

        #endregion
    }
}
