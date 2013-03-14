using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.Drawing;
using Chimera.Util;
using System.IO;
using System.Windows.Forms;
using Chimera.GUI.Forms;

namespace Chimera {
    public class Window {
        /// <summary>
        /// The system which this coordinator is registered with.
        /// </summary>
        private Coordinator mCoordinator;
        /// <summary>
        /// Output object used to actually render the view 'through' the coordinator. Can be null.
        /// </summary>
        private IOutput mOutput;
        /// <summary>
        /// Where on the window the cursor is.
        /// </summary>
        private double mCursorX;
        /// <summary>
        /// Where on the window the cursor is.
        /// </summary>
        private double mCursorY;
        /// <summary>
        /// The height of the screen, in mm.
        /// </summary>
        private double mHeight;
        /// <summary>
        /// The unique name by which the coordinator is known.
        /// </summary>
        private string mName;
        /// <summary>
        /// The matrix which will project objects in virtual space onto the flat coordinator.
        /// </summary>
        private Matrix4 mProjectionMatrix;
        /// <summary>
        /// The orientation of the coordinator in real space.
        /// </summary>
        private Rotation mOrientation = Rotation.Zero;
        /// <summary>
        /// The position of the coordinator in real space, in mm.
        /// </summary>
        private Vector3 mTopLeft;
        /// <summary>
        /// The width of the screen, in mm.
        /// </summary>
        private double mWidth;
        /// <summary>
        /// The monitor this coordinator should display on.
        /// </summary>
        private Screen mMonitor;
        /// <summary>
        /// Whether to launch the overlay window at startup.
        /// </summary>
        private bool mOverlayActive;
        /// <summary>
        /// Whether to launch the overlay window fullscreen at startup.
        /// </summary>
        private bool mOverlayFullscreen;
        /// <summary>
        /// Whether to use mouse control when the window first starts up.
        /// </summary>
        private bool mMouseControl = false;
        /// <summary>
        /// The window which will render the overlay.
        /// </summary>
        private OverlayWindow mOverlayWindow;
        /// <summary>
        /// The colour that will show up as transparent on this window's overlay.
        /// </summary>
        private Color mTransparentColour = Color.Purple;

        /// <summary>
        /// Triggered whenever the position of this coordinator changes.
        /// </summary>
        public event Action<Window, EventArgs> Changed;

        /// <summary>
        /// Triggered whenever the position of the cursor on this coordinator changes.
        /// </summary>
        public event Action<Window, EventArgs> CursorMoved;

        /// <summary>
        /// Triggered whenever the monitor that the coordinator is to display on changes.
        /// </summary>
        public event Action<Window, Screen> MonitorChanged;

        /// <summary>
        /// Triggered when the overlay window is launched.
        /// </summary>
        public event EventHandler OverlayLaunched;

        /// <summary>
        /// Triggered when the overlay form for this window is closed.
        /// </summary>
        public event EventHandler OverlayClosed;

        /// <summary>
        /// Create a coordinator. It is necessary to specify a unique name for the coordinator.
        /// </summary>
        /// <param name="name">The name this coordinator is known by within the system.</param>
        /// <param name="overlayAreas">The overlay areas mapped to this coordinator.</param>
        public Window(string name, params ISelectable[] overlayAreas) {
            mName = name;

            WindowConfig cfg = new WindowConfig(name);
            mMonitor = Screen.AllScreens.FirstOrDefault(s => s.DeviceName.Equals(cfg.Monitor));
            mOverlayActive = cfg.LaunchOverlay;
            mMouseControl = cfg.MouseControl;
            mOverlayFullscreen = cfg.Fullscreen;

            mWidth = cfg.Width;
            mHeight = cfg.Height;
            mTopLeft = cfg.TopLeft;
            mOrientation = new Rotation(cfg.Pitch, cfg.Yaw);

            mOrientation.Changed += mOrientation_Changed;

            if (mMonitor == null)
                mMonitor = Screen.PrimaryScreen;
        }

        /// <summary>
        /// Create a coordinator. It is necessary to specify a unique name for the coordinator.
        /// </summary>
        /// <param name="name">The name this coordinator is known by within the system.</param>
        /// <param name="overlayAreas">The overlay areas mapped to this coordinator.</param>
        public Window(string name, IOutput output, params ISelectable[] overlayAreas)
            : this(name, overlayAreas) {
            mOutput = output;
        }

        /// <summary>
        /// The system which this coordinator is registered with.
        /// </summary>
        public Coordinator Coordinator {
            get { return mCoordinator ; }
            set { mCoordinator  = value; }
        }

        /// <summary>
        /// Output object used to actually render the view 'through' the coordinator. Can be null.
        /// </summary>
        public IOutput Output {
            get { return mOutput ; }
            set { mOutput  = value; }
        }

        /// <summary>
        /// Where on the monitor the cursor is.
        /// </summary>
        public double CursorX {
            get { return mCursorX ; }
        }

        /// <summary>
        /// Where on the monitor the cursor is.
        /// </summary>
        public double CursorY {
            get { return mCursorY ; }
        }

        /// <summary>
        /// Where on the monitor the cursor is.
        /// </summary>
        public Point Cursor {
            get { return new Point((int) mCursorX, (int) mCursorY) ; }
        }

        /// <summary>
        /// The position of the coordinator in real space, in mm.
        /// </summary>
        public Vector3 TopLeft {
            get { return mTopLeft ; }
            set { 
                mTopLeft  = value;
                if (Changed != null)
                    Changed(this, null);
            }
        }

        /// <summary>
        /// The orientation of the coordinator in real space.
        /// </summary>
        public Rotation Orientation {
            get { return mOrientation ; }
            set {
                if (mOrientation != null)
                    mOrientation.Changed -= mOrientation_Changed;
                mOrientation  = value;
                mOrientation.Changed += mOrientation_Changed;
                if (Changed != null)
                    Changed(this, null);
            }
        }

        /// <summary>
        /// The width of the screen, in mm.
        /// </summary>
        public double Width {
            get { return mWidth ; }
            set { 
                mWidth  = value;
                if (Changed != null)
                    Changed(this, null);
            }
        }

        /// <summary>
        /// The height of the screen, in mm.
        /// </summary>
        public double Height {
            get { return mHeight ; }
            set {
                mHeight = value;
                if (Changed != null)
                    Changed(this, null);
            }
        }

        /// <summary>
        /// The matrix which will project objects in virtual space onto the flat coordinator.
        /// </summary>
        public Matrix4 ProjectionMatrix {
            get { return mProjectionMatrix ; }
            set { mProjectionMatrix  = value; }
        }

        /// <summary>
        /// The unique name by which the coordinator is known.
        /// </summary>
        public string Name {
            get { return mName ; }
            set { mName  = value; }
        }

        /// <summary>
        /// A multi line string that can be printed to file to store a record of window in the event of a crash.
        /// </summary>
        public string State {
            get {
                string dump = "----" + Name + "----" + Environment.NewLine;
                Vector3 centre = new Vector3(0f, (float) mWidth, (float) mHeight) * mOrientation.Quaternion;
                dump += "Centre: " + centre + Environment.NewLine;
                dump += "Orientation | Yaw: " + mOrientation.Yaw + ", Pitch: " + mOrientation.Pitch + Environment.NewLine;
                dump += "Distance: " + (centre - mCoordinator.EyePosition).Length() + Environment.NewLine;
                dump += "Width: " + mWidth + ", Height: " + mHeight + Environment.NewLine;

                if (mOutput != null)
                    try {
                        dump += mOutput.State;
                    } catch (Exception ex) {
                        dump += "Unable to get stats for coordinator " + mOutput.Type + ". " + ex.Message + Environment.NewLine;
                        dump += ex.StackTrace;
                    }

                return dump;
            }
        }

        /// <summary>
        /// The monitor which this coordinator should display on.
        /// </summary>
        public Screen Monitor {
            get { return mMonitor; }
            set {
                if (mMonitor != value) {
                    mMonitor = value;
                    if (MonitorChanged != null)
                        MonitorChanged(this, value);
                }
            }
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

        /// <summary>
        /// True if the overlay has been launched.
        /// </summary>
        public bool OverlayVisible {
            get { return mOverlayActive; }
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
        /// Whether moving the mouse over the window will cause the cursor to move.
        /// </summary>
        public bool MouseControl {
            get { return mMouseControl; }
            set {
                mMouseControl = value;
                if (mOverlayWindow != null)
                    mOverlayWindow.MouseControl = value; }
        }

        /// <summary>
        /// Colour that can be used to make things transparent on this window's overlay.
        /// </summary>
        public Color TransparentColour {
            get { return mTransparentColour; }
        }

        /// <summary>
        /// Initialise the coordinator, giving it a reference to the coordinator it is linked to.
        /// </summary>
        /// <param name="coordinator">The coordinator object the input can control.</param>
        public void Init(Coordinator coordinator) {
            mCoordinator = coordinator;
            mOutput.Init(this);
        }

        /// <summary>
        /// Set the position of the cursor on the window, specifying the position of the cursor on the monitor in pixels.
        /// </summary>
        /// <param name="x">The X coordinate of the cursor.</param>
        /// <param name="y">The Y coordinate of the cursor.</param>
        public void UpdateCursorPx(double x, double y) {
            mCursorX = x;
            mCursorY = y;
            RedrawOverlay();
            if (CursorMoved != null)
                CursorMoved(this, null);
        }

        /// <summary>
        /// Set the position of the cursor on the window, specifying the position of the cursor on the physical screen in cm.
        /// </summary>
        /// <param name="x">The X coordinate of the cursor.</param>
        /// <param name="y">The Y coordinate of the cursor.</param>
        public void UpdateCursorCm(double x, double y) {
            UpdateCursorPx((x / mWidth) * mMonitor.Bounds.Width, (y / mHeight) * mMonitor.Bounds.Height);
        }

        /// <summary>
        /// Called when the coordinator is to be disposed of.
        /// </summary>
        public void Close() {
            if (mOutput != null)
                mOutput.Close();
            CloseOverlay();
        }

        /// <summary>
        /// DrawSelected any relevant information about this input onto a diagram.
        /// </summary>
        /// <param name="perspective">The perspective to render along.</param>
        /// <param name="graphics">The graphics object to draw with.</param>
        public void Draw(Chimera.Perspective perspective, Graphics graphics) {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Force the overlay coordinator to redraw, if it is visible.
        /// </summary>
        public void RedrawOverlay() {
            if (mOverlayWindow != null)
                mOverlayWindow.Redraw();
        }

        /// <summary>
        /// Force the overlay window to the top of the Z buffer.
        /// </summary>
        public void ForegroundOverlay() {
            if (mOverlayWindow != null)
                mOverlayWindow.Invoke(new Action(() => mOverlayWindow.BringToFront()));
        }

        /// <summary>
        /// Force the output window to the top of the Z buffer.
        /// </summary>
        public void ForegroundOutput() {
            if (mOutput != null && mOutput.Active)
                ProcessWrangler.BringToFront(mOutput.Process);
        }

        /// <summary>
        /// Create and show the overlay window if it is not already created.
        /// </summary>
        public void LaunchOverlay() {
            if (mOverlayWindow == null) {
                mOverlayActive = true;
                mOverlayWindow = new OverlayWindow(this, mTransparentColour);
                mOverlayWindow.Show();
                mOverlayWindow.FormClosed += new FormClosedEventHandler(mOverlayWindow_FormClosed);
                mOverlayActive = false;
                if (OverlayLaunched != null)
                    OverlayLaunched(this, null);
            }
        }

        /// <summary>
        /// Close the overlay window, if it has been created.
        /// </summary>
        public void CloseOverlay() {
            if (mOverlayWindow != null) {
                mOverlayWindow.Close();
                mOverlayWindow = null;
            }
        }

        void mOverlayWindow_FormClosed(object sender, FormClosedEventArgs e) {
            mOverlayActive = false;
            if (OverlayClosed != null)
                OverlayClosed(this, null);
        }

        void mOrientation_Changed(object sender, EventArgs e) {
            if (Changed != null)
                Changed(this, null);
        }
    }
}
