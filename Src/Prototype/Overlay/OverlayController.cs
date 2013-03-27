using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.GUI.Forms;
using System.Drawing;
using System.Windows.Forms;
using Chimera.Util;
using SystemCursor = System.Windows.Forms.Cursor;
using Chimera.Interfaces;

namespace Chimera {
    public class OverlayController {
        /// <summary>
        /// Where on the window the cursor is.
        /// </summary>
        private double mCursorX;

        /// <summary>
        /// Where on the window the cursor is.
        /// </summary>
        private double mCursorY;

        /// <summary>
        /// Whether to launch the overlay window at startup.
        /// </summary>
        private bool mOverlayActive;

        /// <summary>
        /// Whether to launch the overlay window fullscreen at startup.
        /// </summary>
        private bool mOverlayFullscreen;
        /// <summary>
        /// Whether the overlay should control the cursor position.
        /// </summary>
        private bool mControlPointer;
        /// <summary>
        /// The window used to render the overlay.
        /// </summary>
        private IOverlayWindow mOverlayWindow;
        /// <summary>
        /// The window this overlay covers.
        /// </summary>
        private Window mWindow;
        /// <summary>
        /// Factory for creating the overlay window.
        /// </summary>
        private IOverlayWindowFactory mOverlayWindowFactory = new OverlayWindowFactory();

        /// <summary>
        /// The colour that will show up as transparent on this window's overlay.
        /// </summary>
        private Color mTransparentColour = Color.Purple;

        /// <summary>
        /// Triggered when the overlay window is launched.
        /// </summary>
        public event EventHandler OverlayLaunched;

        /// <summary>
        /// Triggered when the overlay form for this window is closed.
        /// </summary>
        public event EventHandler OverlayClosed;

        /// <summary>
        /// Triggered whenever the position of the cursor on this input changes.
        /// </summary>
        public event Action<OverlayController, EventArgs> CursorMoved;

        /// <summary>
        /// Way for user to signal help.
        /// </summary>
        public event Action HelpTriggered;

        /// <summary>
        /// Where on the monitor the cursor is. Specified as percentages.
        /// 0,0 = top left, 1,1 = bottom right.
        /// </summary>
        public PointF Cursor {
            get { return new PointF((float)mCursorX, (float)mCursorY); }
        }
        /// <summary>
        /// Where on the monitor the cursor is, specified in pixels.
        /// </summary>
        public Point MonitorCursor {
            get {
                Rectangle b = mWindow.Monitor.Bounds;
                int x = (int)(mCursorX * b.Width) + b.X;
                int y = (int)(mCursorY * b.Height) + b.Y;
                return new Point(x, y); 
            }
        }

        /// <summary>
        /// Whether the overlay should control the position of the cursor in the wider system.
        /// </summary>
        public bool ControlPointer {
            get { return mControlPointer; }
            set { 
                mControlPointer = value;
                if (!value)
                    MoveCursorOffScreen();
            }
        }
        /// <summary>
        /// Where on the monitor the cursor is.
        /// Specified as a percentage. 1 is at the left, 0 is at the left.
        /// </summary>
        public double CursorX {
            get { return mCursorX; }
        }
        /// <summary>
        /// Where on the screen the cursor is.
        /// Specified as a percentage. 1 is at the bottom, 0 is at the top.
        /// </summary>
        public double CursorY {
            get { return mCursorY; }
        }

        /// <summary>
        /// Where on the physical window the cursor is.
        /// </summary>
        public double WindowX {
            get { return mCursorX; }
        }

        /// <summary>
        /// Where on the physical window the cursor is.
        /// </summary>
        public double WindowY {
            get { return mCursorY; }
        }

        public Window Window {
            get { return mWindow; }
        }

        /// <summary>
        /// True if the overlay has been launched.
        /// </summary>
        public bool OverlayVisible {
            get { return mOverlayActive; }
        }

        /// <summary>
        /// Colour that can be used to make things transparent on this window's overlay.
        /// </summary>
        public Color TransparentColour {
            get { return mTransparentColour; }
        }

        /// <summary>
        /// Whether the overlay window is currently fullscreen.
        /// </summary>
        public bool OverlayFullscreen {
            get { return mOverlayFullscreen; }
            set {
                mOverlayFullscreen = value;
                if (mOverlayWindow != null)
                    mOverlayWindow.Fullscreen = value;
            }
        }
        /// <summary>
        /// Force the overlay input to redraw, if it is visible.
        /// </summary>
        public void RedrawOverlay() {
            if (mOverlayWindow != null)
                mOverlayWindow.Redraw();
        }

        /// <summary>
        /// Set the position of the cursor on the window, specified as values percentages of the width and height.
        /// 0,0 is top left, 1,1 is bottom right.
        /// </summary>
        /// <param name="x">The percentage across the screen the cursor is (1 = all the way across).</param>
        /// <param name="y">The percentage down the screen the cursor is (1 = all the way down).</param>
        public void UpdateCursor(double x, double y) {
            if (mWindow == null)
                return;
            bool wasOn = mWindow.Monitor.Bounds.Contains(MonitorCursor);
            mCursorX = x;
            mCursorY = y;
            if (mControlPointer && mWindow.Monitor.Bounds.Contains(MonitorCursor))
                SystemCursor.Position = MonitorCursor;
            else if (wasOn && mControlPointer)
                MoveCursorOffScreen();

            if (CursorMoved != null && (mWindow.Monitor.Bounds.Contains(MonitorCursor) || wasOn))
                CursorMoved(this, null);
        }

        /// <summary>
        /// CustomTrigger the HelpTriggered event
        /// </summary>
        public void TriggerHelp() {
            if (HelpTriggered != null)
                HelpTriggered();
        }

        /// <summary>
        /// Create and show the overlay window if it is not already created.
        /// </summary>
        public void Launch() {
            if (mOverlayWindow == null) {
                mOverlayActive = true;
                mOverlayWindow = mOverlayWindowFactory.Make(this);
                //mOverlayWindow = new OverlayWindow(this, mTransparentColour);
                mOverlayWindow.Show();
                mOverlayWindow.FormClosed += new FormClosedEventHandler(mOverlayWindow_FormClosed);
                mOverlayActive = false;
                if (mOverlayFullscreen)
                    mOverlayWindow.Fullscreen = true;
                if (OverlayLaunched != null)
                    OverlayLaunched(this, null);
            }
        }

        public void SetOverlayWindowFactory(IOverlayWindowFactory overlayWindowFactory) {
            mOverlayWindowFactory = overlayWindowFactory;
        }

        /// <summary>
        /// Force the overlay window to the top of the Z buffer.
        /// </summary>
        public void ForegroundOverlay() {
            if (mOverlayWindow != null)
                mOverlayWindow.Foreground();
        }

        /// <summary>
        /// Force the output window to the top of the Z buffer.
        /// </summary>
        public void ForegroundOutput() {
            if (mWindow.Output != null && mWindow.Output.Active)
                ProcessWrangler.BringToFront(mWindow.Output.Process);
        }

        /// <summary>
        /// Close the overlay window, if it has been created.
        /// </summary>
        public void Close() {
            if (mOverlayWindow != null) {
                mOverlayWindow.Close();
                mOverlayWindow = null;
            }
        }

        public void Init(Window window) {
            mWindow = window;

            WindowConfig cfg = new WindowConfig(mWindow.Name);
            mOverlayActive = cfg.LaunchOverlay;
            mOverlayFullscreen = cfg.Fullscreen;
            mControlPointer = cfg.ControlPointer;
        }

        private void MoveCursorOffScreen() {
                SystemCursor.Position = new Point(mWindow.Monitor.Bounds.X+mWindow.Monitor.Bounds.Width, mWindow.Monitor.Bounds.Y+mWindow.Monitor.Bounds.Height);
        }

        void mOverlayWindow_FormClosed(object sender, FormClosedEventArgs e) {
            mOverlayActive = false;
            if (OverlayClosed != null)
                OverlayClosed(this, null);
        }
    }
}
