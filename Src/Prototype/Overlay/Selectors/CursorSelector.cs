using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;
using Chimera.GUI.Forms;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Chimera.Overlay.Selectors {
    public class CursorSelector : ISelectionRenderer {
        private DateTime mEnter;
        private float mSelectMS = 1000f;
        private bool mHovering = false;
        private bool mClicked = false;
        private bool mClicking = false;
        private SimpleOverlay mOverlay;
        private Window mWindow;
        private Cursor mDefaultCursor;
        private Cursor mSelectCursor;

        public CursorSelector(SimpleOverlay overly, Window window) {
            mOverlay = overly;
            mWindow = window;
            window.Coordinator.Tick += new Action(coordinator_Tick);
            mDefaultCursor = overly.Cursor;
            mSelectCursor = new Cursor(new IntPtr(65571));
        }

        void coordinator_Tick() {
            if (ProcessWrangler.GetGlobalCursor().Equals(mSelectCursor.Handle)) {
                mOverlay.SetCursor(mSelectCursor);
                if (mClicked)
                    return;
                if (!mHovering) {
                    mHovering = true;
                    mEnter = DateTime.Now;
                    Console.WriteLine("Hover Begin");
                }
                mOverlay.Redraw();
                if (DateTime.Now.Subtract(mEnter).TotalMilliseconds > mSelectMS) {
                    mClicked = true;
                    mClicking = true;
                    mOverlay.Redraw();
                    Console.WriteLine("Clicking");
                    Thread.Sleep(20);
                    ProcessWrangler.Click();
                    mClicking = false;
                    mOverlay.SetCursor(mDefaultCursor);
                }
            } else if (mHovering) {
                mOverlay.SetCursor(mDefaultCursor);
                mHovering = false;
                mClicked = false;
                mOverlay.Redraw();
            }
        }
        
        #region SelectionRenderer Members

        public ISelectable Selectable {
            get { throw new NotImplementedException(); }
        }

        public void DrawHover(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipRectangle, DateTime mHoverStart, double selectTime) {
            if (!mHovering || mClicking)
                return;

            float x = (float) (clipRectangle.Width * mWindow.Overlay.CursorX);
            float y = (float) (clipRectangle.Height * mWindow.Overlay.CursorY);
            float r = 40f;
            if (!mClicked)
                graphics.FillPie(Brushes.Red, x - r, y - r, r*2f, r*2f, -90f, ((float) DateTime.Now.Subtract(mEnter).TotalMilliseconds / mSelectMS) * 360f);
            //else
                //graphics.FillEllipse(Brushes.Blue, x - r, y - 15f, r*2f, r*2f);
        }

        public void DrawSelected(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipRectangle) {
        }

        public void Init(ISelectable area) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
