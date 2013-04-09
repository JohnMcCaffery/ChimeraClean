using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;
using Chimera.GUI.Forms;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay.Triggers {
    public class CursorTrigger : ITrigger, IDrawable {
        private DateTime mEnter;
        private float mSelectMS = 1000f;
        private bool mHovering = false;
        private bool mClicked = false;
        private Cursor mSelectCursor;
        private IHoverSelectorRenderer mRenderer;
        private bool mActive;
        private Window mWindow;
        private int mR;

        public CursorTrigger(IHoverSelectorRenderer renderer, Window window) {
            mWindow = window;
            mRenderer = renderer;
            mWindow.Coordinator.Tick += new Action(coordinator_Tick);
            //mSelectCursor = new Cursor(new IntPtr(65571));
            mSelectCursor = new Cursor(new IntPtr(65569));
        }

        public double HoverTime {
            get { return DateTime.Now.Subtract(mEnter).TotalMilliseconds; }
        }


        void coordinator_Tick() {
            if (ProcessWrangler.GetGlobalCursor().Equals(mSelectCursor.Handle)) {
                if (mClicked)
                    return;
                if (!mHovering) {
                    mHovering = true;
                    mEnter = DateTime.Now;
                    Console.WriteLine("Hover Begin");
                }

                if (HoverTime > mSelectMS) {
                    mClicked = true;
                    Console.WriteLine("Clicking");
                    Thread.Sleep(20);
                    ProcessWrangler.Click();
                    mRenderer.Clear();
                    mWindow.OverlayManager.ForceRedraw();
                    if (Triggered != null)
                        Triggered();
                }
            } else if (mHovering) {
                mWindow.OverlayManager.ForceRedraw();
                mRenderer.Clear();
                mHovering = false;
                mClicked = false;
            }
        }

        #region ITrigger Members

        public event Action Triggered;

        public bool Active {
            get { return mActive; }
            set {
                mActive = value;
                if (!mActive)
                    mRenderer.Clear();
            }
        }

        #endregion

        #region IDrawable Members

        public bool NeedsRedrawn {
            get { return mHovering && !mClicked; }
        }

        public string Window {
            get { return mWindow.Name; }
        }

        private Rectangle mClip;

        public void RedrawStatic(Rectangle clip, Graphics graphics) {
            mClip = clip;
        }

        public void DrawDynamic(Graphics graphics) {
            if (mHovering && !mClicked) {
                int x = (int)(mClip.Width * mWindow.OverlayManager.CursorX);
                int y = (int)(mClip.Height * mWindow.OverlayManager.CursorY);
                Rectangle r = new Rectangle(x - mR, y - mR, mR * 2, mR * 2);
                mRenderer.DrawHover(graphics, r, HoverTime / mSelectMS );
            }
        }

        #endregion
    }
}
