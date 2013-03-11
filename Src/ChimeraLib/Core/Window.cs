using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.Drawing;
using Chimera.Util;
using System.IO;
using System.Windows.Forms;

namespace Chimera {
    public class Window {
        /// <summary>
        /// The system which this window is registered with.
        /// </summary>
        private Coordinator mCoordinator;
        /// <summary>
        /// Output object used to actually render the view 'through' the window. Can be null.
        /// </summary>
        private IOutput mOutput;
        /// <summary>
        /// Active areas on the home screen overlay which can be triggered.
        /// </summary>
        private readonly List<IOverlayArea> mOverlayAreas = new List<IOverlayArea>();
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
        /// The unique name by which the window is known.
        /// </summary>
        private string mName;
        /// <summary>
        /// The matrix which will project objects in virtual space onto the flat window.
        /// </summary>
        private Matrix4 mProjectionMatrix;
        /// <summary>
        /// The orientation of the window in real space.
        /// </summary>
        private Rotation mRotation = new Rotation();
        /// <summary>
        /// The position of the window in real space, in mm.
        /// </summary>
        private Vector3 mTopLeft;
        /// <summary>
        /// The width of the screen, in mm.
        /// </summary>
        private double mWidth;
        /// <summary>
        /// The monitor this window should display on.
        /// </summary>
        private Screen mMonitor;

        /// <summary>
        /// Triggered whenever the position of this window changes.
        /// </summary>
        public event Action<Window, EventArgs> Change;

        /// <summary>
        /// Triggered whenever the position of the cursor on this window changes.
        /// </summary>
        public event Action<Window, EventArgs> CursorMove;

        /// <summary>
        /// Triggered whenever the monitor that the window is to display on changes.
        /// </summary>
        public event Action<Window, Screen> MonitorChanged;

        /// <summary>
        /// Create a window. It is necessary to specify a unique name for the window.
        /// </summary>
        /// <param name="name">The name this window is known by within the system.</param>
        /// <param name="overlayAreas">The overlay areas mapped to this window.</param>
        public Window(string name, params IOverlayArea[] overlayAreas) {
            mName = name;
            mOverlayAreas = new List<IOverlayArea>(overlayAreas);

            WindowConfig cfg = new WindowConfig(name);
            mMonitor = Screen.AllScreens.FirstOrDefault(s => s.DeviceName.Equals(cfg.Monitor));
            if (mMonitor == null)
                mMonitor = Screen.PrimaryScreen;
        }

        /// <summary>
        /// Create a window. It is necessary to specify a unique name for the window.
        /// </summary>
        /// <param name="name">The name this window is known by within the system.</param>
        /// <param name="overlayAreas">The overlay areas mapped to this window.</param>
        public Window(string name, IOutput output, params IOverlayArea[] overlayAreas)
            : this(name, overlayAreas) {
            mOutput = output;
        }

        /// <summary>
        /// Active areas on the home screen overlay which can be triggered.
        /// </summary>
        public IOverlayArea[] MainMenuAreas {
            get { return mOverlayAreas.ToArray(); }
        }

        /// <summary>
        /// The system which this window is registered with.
        /// </summary>
        public Coordinator Coordinator {
            get { return mCoordinator ; }
            set { mCoordinator  = value; }
        }

        /// <summary>
        /// Output object used to actually render the view 'through' the window. Can be null.
        /// </summary>
        public IOutput Output {
            get { return mOutput ; }
            set { mOutput  = value; }
        }

        /// <summary>
        /// Where on the window the cursor is.
        /// </summary>
        public double CursorX {
            get { return mCursorX ; }
        }

        /// <summary>
        /// Where on the window the cursor is.
        /// </summary>
        public double CursorY {
            get { return mCursorY ; }
        }

        /// <summary>
        /// The position of the window in real space, in mm.
        /// </summary>
        public Vector3 TopLeft {
            get { return mTopLeft ; }
            set { mTopLeft  = value; }
        }

        /// <summary>
        /// The orientation of the window in real space.
        /// </summary>
        public Rotation Rotation {
            get { return mRotation ; }
            set { mRotation  = value; }
        }

        /// <summary>
        /// The width of the screen, in mm.
        /// </summary>
        public double Width {
            get { return mWidth ; }
            set { mWidth  = value; }
        }

        /// <summary>
        /// The height of the screen, in mm.
        /// </summary>
        public double Height {
            get { return mHeight ; }
            set { mHeight  = value; }
        }

        /// <summary>
        /// The matrix which will project objects in virtual space onto the flat window.
        /// </summary>
        public Matrix4 ProjectionMatrix {
            get { return mProjectionMatrix ; }
            set { mProjectionMatrix  = value; }
        }

        /// <summary>
        /// The unique name by which the window is known.
        /// </summary>
        public string Name {
            get { return mName ; }
            set { mName  = value; }
        }

        /// <summary>
        /// A multi line string that can be printed to file to store a record of state in the event of a crash.
        /// </summary>
        public string State {
            get {
                string dump = "----" + Name + "----" + Environment.NewLine;
                Vector3 centre = new Vector3(0f, (float) mWidth, (float) mHeight) * mRotation.Quaternion;
                dump += "Centre: " + centre + Environment.NewLine;
                dump += "Orientation | Yaw: " + mRotation.Yaw + ", Pitch: " + mRotation.Pitch + Environment.NewLine;
                dump += "Distance: " + (centre - mCoordinator.EyePosition).Length() + Environment.NewLine;
                dump += "Width: " + mWidth + ", Height: " + mHeight + Environment.NewLine;

                if (mOutput != null)
                    try {
                        dump += mOutput.State;
                    } catch (Exception ex) {
                        dump += "Unable to get stats for window " + mOutput.Type + ". " + ex.Message + Environment.NewLine;
                        dump += ex.StackTrace;
                    }

                return dump;
            }
        }

        /// <summary>
        /// The monitor which this window should display on.
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
        /// Initialise the window, giving it a reference to the coordinator it is linked to.
        /// </summary>
        /// <param name="coordinator">The coordinator object the input can control.</param>
        public void Init(Coordinator coordinator) {
            mCoordinator = coordinator;
            mOutput.Init(this);
            foreach (var area in mOverlayAreas)
                area.Init(this);
        }

        /// <summary>
        /// Set the position of the cursor on the window.
        /// </summary>
        /// <param name="x">The X coordinate of the cursor.</param>
        /// <param name="y">The Y coordinate of the cursor.</param>
        public void PositionCursor(double x, double y) {
            mCursorX = x;
            mCursorY = y;
            if (CursorMove != null)
                CursorMove(this, null);
        }

        /// <summary>
        /// Called when the window is to be disposed of.
        /// </summary>
        public void Close() {
            foreach (var area in mOverlayAreas)
                area.Mode.Close();
            if (mOutput != null)
                mOutput.Close();
        }

        /// <summary>
        /// Draw any relevant information about this input onto a diagram.
        /// </summary>
        /// <param name="perspective">The perspective to render along.</param>
        /// <param name="graphics">The graphics object to draw with.</param>
        public void Draw(Chimera.Perspective perspective, Graphics graphics) {
            throw new System.NotImplementedException();
        }
    }
}
