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
        private RectangleF mBounds;
        private DateTime mHoverStart;
        private bool mHovering;
        private bool mSelected;
        private bool mVisible;
        private bool mActive = true;

        public SelectableArea(float x, float y, float w, float h) {
            mBounds = new RectangleF(x, y, w, h);
        }

        public SelectableArea(ISelectionRenderer selector, float x, float y, float w, float h)
            : this(x, y, w, h) {

            mRenderer = selector;
            mRenderer.Init(this);
        }

        protected void SetRenderer(ISelectionRenderer renderer) {
            mRenderer = renderer;
        }

        private void CheckState() {
            if (mBounds.Contains(mWindow.Overlay.Cursor) && mActive) {
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

        private void window_CursorMoved(OverlayController window, EventArgs args) {
            CheckState();
        }

        private void Coordinator_Tick() {
            if (mHovering)
                CheckState();
        }

        private void window_MonitorChanged(Window window, Screen screen) {
            //What should happen when the window changes? Re-calculate?
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

        public virtual RectangleF Bounds {
            get { return mBounds; }
        }

        public virtual Rectangle ScaledBounds {
            get {
                return new Rectangle(
                    (int)(mWindow.Monitor.Bounds.Width * mBounds.X),
                    (int)(mWindow.Monitor.Bounds.Height * mBounds.Y),
                    (int)(mWindow.Monitor.Bounds.Width * mBounds.Width),
                    (int)(mWindow.Monitor.Bounds.Height * mBounds.Height));
                }
        }

        public virtual void Init(Window window) {
            mWindow = window;

            mWindow.MonitorChanged += new Action<Window, Screen>(window_MonitorChanged);
            mWindow.Overlay.CursorMoved += new Action<OverlayController,EventArgs>(window_CursorMoved);
            mWindow.Coordinator.Tick += new Action(Coordinator_Tick);

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
