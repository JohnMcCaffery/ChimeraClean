using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemCursor = System.Windows.Forms.Cursor;
using System.Drawing;
using Chimera.GUI.Forms;
using System.Windows.Forms;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay {
    public class WindowOverlayManager {
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
        private OverlayWindow mOverlayWindow;
        /// <summary>
        /// The window this overlay covers.
        /// </summary>
        private Window mWindow;
        /// <summary>
        /// The colour that will show up as transparent on this window's overlay.
        /// </summary>
        private Color mTransparentColour = Color.Purple;
        /// <summary>
        /// The drawable which should currently be drawn to the window.
        /// </summary>
        private IDrawable mCurrentDisplay;
        /// <summary>
        /// The current opacity for the overlay window.
        /// </summary>
        private double mOpacity;
        /// <summary>
        /// How long each frame will display for.
        /// </summary>
        private int mFrameLength;

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
        public event Action<WindowOverlayManager, EventArgs> CursorMoved;

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

        public OverlayWindow OverlayWindow {
            get { return mOverlayWindow; }
        }

        /// <summary>
        /// True if the overlay has been launched.
        /// </summary>
        public bool Visible {
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
        public bool Fullscreen {
            get { return mOverlayFullscreen; }
            set {
                mOverlayFullscreen = value;
                if (mOverlayWindow != null)
                    mOverlayWindow.Fullscreen = value;
            }
        }
        public IDrawable CurrentDisplay {
            get { return mCurrentDisplay; }
            set { 
                mCurrentDisplay = value;
                ForceRedrawStatic();
            }
        }

        public int FrameLength {
            get { return mFrameLength; }
            set {
                mFrameLength = value;
                if (mOverlayWindow != null)
                    mOverlayWindow.FrameLength = value;
            }
        }

        /// <summary>
        /// How see through the overlay is.
        /// </summary>
        public double Opacity {
            get { return mOpacity; }
            set {
                mOpacity = value;
                if (mOverlayWindow != null)
                    mOverlayWindow.Opacity = value;
            }
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
        /// CreateWindowState and show the overlay window if it is not already created.
        /// </summary>
        public void Launch() {
            if (mOverlayWindow == null) {
                mOverlayActive = true;
                mOverlayWindow = new OverlayWindow(this);
                mOverlayWindow.Show();
                mOverlayWindow.FormClosed += new FormClosedEventHandler(mOverlayWindow_FormClosed);
                mOverlayActive = false;
                if (mOverlayFullscreen)
                    mOverlayWindow.Fullscreen = true;
                if (OverlayLaunched != null)
                    OverlayLaunched(this, null);
            }
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

        public WindowOverlayManager(Window window) {
            mWindow = window;

            WindowConfig cfg = new WindowConfig(mWindow.Name);
            mOverlayActive = cfg.LaunchOverlay;
            mOverlayFullscreen = cfg.Fullscreen;
            mControlPointer = cfg.ControlPointer;
        }

        private void MoveCursorOffScreen() {
            SystemCursor.Position = new Point(mWindow.Monitor.Bounds.X + mWindow.Monitor.Bounds.Width, mWindow.Monitor.Bounds.Y + mWindow.Monitor.Bounds.Height);
        }

        void mOverlayWindow_FormClosed(object sender, FormClosedEventArgs e) {
            mOverlayActive = false;
            if (OverlayClosed != null)
                OverlayClosed(this, null);
        }

        public void ForceRedrawStatic() {
            if (mOverlayWindow != null)
                mOverlayWindow.RedrawStatic();
        }
    }
}
