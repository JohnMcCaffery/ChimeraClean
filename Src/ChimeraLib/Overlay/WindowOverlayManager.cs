/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemCursor = System.Windows.Forms.Cursor;
using System.Drawing;
using Chimera.GUI.Forms;
using System.Windows.Forms;
using Chimera.Interfaces.Overlay;
using Chimera.Util;

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
        /// Whether the overlay should be full screen.
        /// </summary>
        private bool mOverlayFullscreen;
        /// <summary>
        /// Whether to launch the overlay window at startup.
        /// </summary>
        private bool mOverlayActive;
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
        private double mOpacity = 1.0;
        /// <summary>
        /// How long each frame will display for.
        /// </summary>
        private int mFrameLength = 10;
        /// <summary>
        /// The configuration for this manager.
        /// </summary>
        private WindowConfig mConfig;

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
        /// Triggered whenever a video that has been played through the interface finishes.
        /// </summary>
        public event Action VideoFinished;

        public TickStatistics Statistics {
            get { return mOverlayWindow != null ? mOverlayWindow.Statistics : null; }
        }

        /// <summary>
        /// Where on the monitor the cursor is. Specified as percentages.
        /// 0,0 = top left, 1,1 = bottom right.
        /// </summary>
        public PointF CursorPosition {
            get { return new PointF((float)mCursorX, (float)mCursorY); }
        }

        public Cursor Cursor {
            get { return mOverlayWindow != null ? mOverlayWindow.Cursor : Cursor.Current; }
            set {
                if (mOverlayWindow != null)
                    mOverlayWindow.SetCursor(value);
            }
        }

        public void ResetCursor() {
            if (mOverlayWindow != null)
                mOverlayWindow.ResetCursor();
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
                mControlPointer = value && mConfig.ControlPointer;
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
        public Color TransparencyKey {
            get { return mTransparentColour; }
        }

        /// <summary>
        /// Whether the overlay window is currently fullscreen.
        /// </summary>
        public bool Fullscreen {
            get { return mOverlayWindow != null ? mOverlayWindow.Fullscreen : mOverlayFullscreen; }
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
            if (mWindow == null || mOverlayWindow == null)
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
                mOverlayWindow.VideoFinished += new Action(mOverlayWindow_VideoFinished);
                mOverlayWindow.AlwaysOnTop = mConfig.AlwaysOnTop;
                if (mOverlayFullscreen)
                    mOverlayWindow.Fullscreen = true;
                if (OverlayLaunched != null)
                    OverlayLaunched(this, null);
            }
        }

        void mOverlayWindow_VideoFinished() {
            if (VideoFinished != null)
                VideoFinished();
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

            mConfig = new WindowConfig(mWindow.Name);
            mOverlayActive = mConfig.LaunchOverlay;
            mControlPointer = mConfig.ControlPointer;
            mOverlayFullscreen = mConfig.Fullscreen;
        }

        public void MoveCursorOffScreen() {
            SystemCursor.Position = new Point(mWindow.Monitor.Bounds.X + mWindow.Monitor.Bounds.Width, mWindow.Monitor.Bounds.Y + mWindow.Monitor.Bounds.Height);
        }

        void mOverlayWindow_FormClosed(object sender, FormClosedEventArgs e) {
            mOverlayActive = false;
            mOverlayWindow = null;
            if (OverlayClosed != null)
                OverlayClosed(this, null);
        }

        public void ForceRedraw() {
            if (mOverlayWindow != null)
                mOverlayWindow.ForceRedraw();
        }

        public void ForceRedrawStatic() {
            if (mOverlayWindow != null)
                mOverlayWindow.RedrawStatic();
        }

        public bool AlwaysOnTop { 
            get { return mOverlayWindow != null ? mOverlayWindow.AlwaysOnTop : false; }
            set { 
                if (mOverlayWindow != null)
                    mOverlayWindow.AlwaysOnTop = value;
            }
        }

        public void ForegroundOverlay() {
            if (mOverlayWindow != null)
                mOverlayWindow.BringOverlayToFront();
        }

        public void PlayVideo(string uri) {
            if (mOverlayWindow != null)
                mOverlayWindow.PlayVideo(uri);
        }

        public void PlayVideo(string uri, RectangleF position) {
            if (mOverlayWindow != null)
                mOverlayWindow.PlayVideo(uri, position);
        }

        public void PlayAudio(string uri) {
            if (mOverlayWindow != null)
                mOverlayWindow.PlayAudio(uri);
        }

        public void StopPlayback() {
            if (mOverlayWindow != null)
                mOverlayWindow.StopPlayback();
        }
    }
}
