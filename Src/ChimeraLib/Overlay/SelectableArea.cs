using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Chimera.Overlay {
    public abstract class HoverSelector : ISelectable, Chimera.Interfaces.Overlay.ITrigger, Chimera.Interfaces.Overlay.IDrawable {
        private readonly double mSelectTime = 1500;

        private ISelectionRenderer mRenderer;
        private Window mWindow;
        private RectangleF mBounds;
        private DateTime mHoverStart;
        private bool mHovering;
        private bool mSelected;
        private bool mVisible;
        private bool mActive = true;

        public HoverSelector(float x, float y, float w, float h) {
            mBounds = new RectangleF(x, y, w, h);
        }

        public HoverSelector(ISelectionRenderer selector, float x, float y, float w, float h)
            : this(x, y, w, h) {

            mRenderer = selector;
            mRenderer.Init(this);
        }

        protected void SetRenderer(ISelectionRenderer renderer) {
            mRenderer = renderer;
        }

        private void CheckState() {
            if (mActive && mBounds.Contains(mWindow.Overlay.Cursor)) {
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
                mSelected = false;
            }
        }

        private void window_CursorMoved(WindowOverlayManager window, EventArgs args) {
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

        protected virtual RectangleF Bounds {
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
            if (mActive && !mBounds.Contains(mWindow.Overlay.Cursor))
                mHovering = false;
            if (mSelected)
                mRenderer.DrawSelected(graphics, clipRectangle);
            else if (mHovering)
                mRenderer.DrawHover(graphics, clipRectangle, mHoverStart, mSelectTime);
        }

        public abstract void DrawStatic(Graphics graphics, Rectangle clipRectangle);

        public abstract void Show();

        public abstract void Hide();

        #endregion

        public event Action Triggered;

        public Chimera.IHoverSelectorRenderer HoverSelectorRenderer {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        public void ChangeClip() {
            throw new NotImplementedException();
        }

        public void DrawDynamic() {
            throw new NotImplementedException();
        }

        public void Init(WindowOverlayManager manager) {
            throw new NotImplementedException();
        }
    }
}
