using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay {
    public abstract class HoverSelector : ITrigger, IDrawable {
        private readonly double mSelectTime = 1500;

        public event Action Triggered;

        private IHoverSelectorRenderer mRenderer;
        private Window mWindow;
        private RectangleF mBounds;
        private DateTime mHoverStart;
        private Rectangle mClip;
        private bool mHovering;
        private bool mSelected;
        private bool mVisible;
        private bool mActive = true;

        public bool NeedsRedrawn {
            get { return mHovering || mSelected; }
        }

        public HoverSelector(float x, float y, float w, float h) {
            mBounds = new RectangleF(x, y, w, h);
        }

        public HoverSelector(IHoverSelectorRenderer selector, float x, float y, float w, float h)
            : this(x, y, w, h) {

            mRenderer = selector;
        }

        protected void SetRenderer(IHoverSelectorRenderer renderer) {
            mRenderer = renderer;
        }

        private void CheckState() {
            if (mActive && mBounds.Contains(mWindow.OverlayManager.Cursor)) {
                if (!mHovering) {
                    mHovering = true;
                    mHoverStart = DateTime.Now;
                }

                if (!mSelected && DateTime.Now.Subtract(mHoverStart).TotalMilliseconds > mSelectTime) {
                    if (Triggered != null)
                        Triggered();
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

        #region IHoverSelector Members

        public virtual bool Visible {
            get { return mVisible; }
            set { mVisible = value; }
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

        protected IHoverSelectorRenderer HoverSelectorRenderer {
            get { return mRenderer; }
        }

        protected Rectangle Clip {
            get { return mClip; }
        }

        /// <summary>
        /// Draw the elements of the drawable that change more frequently than when the drawing area is resized.
        /// </summary>
        /// <param name="graphics">The object with which to draw the elements.</param>
        public virtual void DrawDynamic(Graphics graphics) {
            if (mActive && !mBounds.Contains(mWindow.OverlayManager.Cursor))
                mHovering = false;
            if (mSelected)
                mRenderer.DrawSelected(graphics, mClip);
            else if (mHovering) {
                mRenderer.DrawHover(graphics, ScaledBounds, mHoverStart, mSelectTime);
            }
        }

        /// <summary>
        /// Notify the drawable that the area on which it is to draw has changed. Should draw any elements which only change when the area is resized to the supplied graphics object.
        /// </summary>
        /// <param name="clip">The area in which this drawable will be drawn.</param>
        /// <param name="graphics">The object with which to to draw any elements which only change when the area is resized.</param>
        public virtual void ChangeClip(Rectangle clip, Graphics graphics) {
            mClip = clip;
        }

        public virtual void Init(Window window) {
            mWindow = window;

            mWindow.MonitorChanged += new Action<Window, Screen>(window_MonitorChanged);
            mWindow.OverlayManager.CursorMoved += new Action<WindowOverlayManager,EventArgs>(window_CursorMoved);
            mWindow.Coordinator.Tick += new Action(Coordinator_Tick);

            window_MonitorChanged(mWindow, mWindow.Monitor);
        }

        public abstract void Show();

        public abstract void Hide();

        #endregion
    }
}
