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
using Chimera.Interfaces;
using Chimera.Overlay;

namespace Chimera {
    /// <summary>
    /// The different mechanisms used to calculate the projection matrix.
    /// </summary>
    public enum ProjectionStyle { 
        /// <summary>
        /// Just uses Field of View and Aspect Ratio. The same as the SL Viewer's projection matrix.
        /// </summary>
        Simple, 
        /// <summary>
        /// Calculated using Fiend of View, Aspect Ratio with horizontal and vertical skews taken into account.
        /// </summary>
        Skewed, 
        /// <summary>
        /// Calculated using the method from this paper http://lds62-112-144-233.my-simplyroot.de/wp-content/uploads/downloads/2011/12/0008.pdf.
        /// </summary>
        Calculated 
    }
    /// <summary>
    /// When adjusting the height and width of a window, which point should stay anchored.
    /// </summary>
    public enum WindowAnchor { 
        /// <summary>
        /// The top left point should stay in place. The window will extend further right and down.
        /// </summary>
        TopLeft, 
        /// <summary>
        /// The centre will stay in place and the window will extend in all directions.
        /// </summary>
        Centre 
    }
    public class Window { 
        /// <summary>
        /// The system which this input is registered with.
        /// </summary>
        private Coordinator mCoordinator;     
        /// The manager for the overlay which can be renderered on this window.
        /// </summary>
        private WindowOverlayManager mOverlayManager;        
        /// <summary>
        /// Output object used to actually render the view 'through' the input. Can be null.
        /// </summary>
        private IOutput mOutput;
        /// <summary>
        /// The height of the screen, in mm.
        /// </summary>
        private double mHeight;
        /// <summary>
        /// The unique name by which the input is known.
        /// </summary>
        private string mName;
        /// <summary>
        /// The orientation of the input in real space.
        /// </summary>
        private Rotation mOrientation = Rotation.Zero;
        /// <summary>
        /// The position of the input in real space, in mm.
        /// </summary>
        private Vector3 mTopLeft;
        /// <summary>
        /// The width of the screen, in mm.
        /// </summary>
        private double mWidth;
        /// <summary>
        /// The monitor this input should display on.
        /// </summary>
        private Screen mMonitor;
        /// <summary>
        /// Whether to use mouse control when the window first starts up.
        /// </summary>
        private bool mMouseControl = false;
        /// <summary>
        /// The anchor for the window as the width and height change.
        /// </summary>
        private WindowAnchor mWindowAnchor = WindowAnchor.TopLeft; 
        /// <summary>
        /// The method used to calculate the projection matrix.
        /// </summary>
        private ProjectionStyle mProjection = ProjectionStyle.Simple;

        /// <summary>
        /// Triggered whenever the position of this input changes.
        /// </summary>
        public event Action<Window, EventArgs> Changed;

        /// <summary>
        /// Triggered whenever the monitor that the input is to display on changes.
        /// </summary>
        public event Action<Window, Screen> MonitorChanged;

        /// <summary>
        /// CreateWindowState a input. It is necessary to specify a unique name for the input.
        /// </summary>
        /// <param name="name">The name this input is known by within the system.</param>
        /// <param name="overlayAreas">The overlay areas mapped to this input.</param>
        public Window(string name) {
            mName = name;

            WindowConfig cfg = new WindowConfig(name);
            mMonitor = Screen.AllScreens.FirstOrDefault(s => s.DeviceName.Equals(cfg.Monitor));
            mMouseControl = cfg.ControlPointer;
            mWidth = cfg.Width;
            mHeight = cfg.Height;
            mTopLeft = cfg.TopLeft;
            mOrientation = new Rotation(cfg.Pitch, cfg.Yaw);
            mOverlayManager = new WindowOverlayManager(this);

            mOrientation.Changed += mOrientation_Changed;

            if (mMonitor == null)
                mMonitor = Screen.PrimaryScreen;
        }

        /// <summary>
        /// CreateWindowState a input. It is necessary to specify a unique name for the input.
        /// </summary>
        /// <param name="name">The name this input is known by within the system.</param>
        /// <param name="overlayAreas">The overlay areas mapped to this input.</param>
        public Window(string name, IOutput output)
            : this(name) {
            mOutput = output;
        }

        /// <summary>
        /// The system which this input is registered with.
        /// </summary>
        public Coordinator Coordinator {
            get { return mCoordinator ; }
            set { mCoordinator  = value; }
        }

        /// <summary>
        /// Output object used to actually render the view 'through' the input. Can be null.
        /// </summary>
        public IOutput Output {
            get { return mOutput ; }
            set { mOutput  = value; }
        }

        /// <summary>
        /// The position of the top left corner of the window in real space, in mm.
        /// </summary>
        public Vector3 TopLeft {
            get { return mTopLeft ; }
            set {
                mTopLeft = value;
                TriggerChanged();
            }
        }

        /// <summary>
        /// The position of the centre of the window in real space, in mm.
        /// </summary>
        public Vector3 Centre {
            get {
                Vector3 diagonal = new Vector3(0f, (float)(mWidth / 2.0), (float)(mHeight / -2.0));
                diagonal *= mOrientation.Quaternion;
                return mTopLeft + diagonal;
            }
            set {
                Vector3 diagonal = new Vector3(0f, (float)(mWidth / 2.0), (float)(mHeight / -2.0));
                diagonal *= mOrientation.Quaternion;
                mTopLeft = value - diagonal;
                TriggerChanged();
            }
        }


        /// <summary>
        /// The orientation of the input in real space.
        /// </summary>
        public Rotation Orientation {
            get { return mOrientation ; }
            set {
                if (mOrientation != null)
                    mOrientation.Changed -= mOrientation_Changed;
                mOrientation  = value;
                mOrientation.Changed += mOrientation_Changed;
                TriggerChanged();
            }
        }

        /// <summary>
        /// The width of the screen, in mm.
        /// </summary>
        public double Width {
            get { return mWidth ; }
            set { ChangeDimesions(value, mHeight); }
        }

        /// <summary>
        /// The height of the screen, in mm.
        /// </summary>
        public double Height {
            get { return mHeight ; }
            set { ChangeDimesions(mWidth, value); }
        }

        /// <summary>
        /// The aspect ratio between the height and width of the screen. (h/w).
        /// Changing this will change the width of the screen.
        /// Calculated as height / width.
        /// </summary>
        public double AspectRatio {
            get { return mHeight / mWidth; }
            set {
                if (AspectRatio == value || value <= 0.0)
                    return;
                double width = mHeight / value;
                ChangeDimesions(width, mHeight);
            }
        }

        /// <summary>
        /// The diagonal size of the screen. Specified in mm.
        /// This is included for convenience. Most screens are rated in diagonal inches.
        /// Changing this will change the width and height according to the aspect ratio.
        /// </summary>
        public double Diagonal {
            get { return Math.Sqrt(Math.Pow(mWidth, 2.0) + Math.Pow(mHeight, 2.0)); }
            set {
                if (Diagonal == value || value <= 0.0)
                    return;
                double tan = Math.Atan(AspectRatio);
                double height = value * Math.Sin(tan);
                double width = value * Math.Cos(tan);
                ChangeDimesions(width, height);
            }
        }

        /// <summary>
        /// The horizontal field of view the screen shows, in radians. 
        /// Changing this will change the height and width of the screen according to the aspect ratio.
        /// Calculated as the tangent of <code>width / (2 * d)</code> where d is distance to the screen.
        /// </summary>
        public double HFieldOfView {
            get { return CalculateFOV(mWidth, new Vector3(0f, (float)(mWidth / 2.0), 0f)); }
            set {
                //if (Math.Abs(fov) < TOLERANCE || value <= 0.0)
                if (value <= 0.0)
                    return;
                double width = 2 * ScreenDistance * Math.Tan(value / 2.0);
                double height = mWidth * AspectRatio;
                ChangeDimesions(width, height);
            }
        }

        /// <summary>
        /// The vertical field of view the screen shows, in radians. 
        /// Changing this will change the height and width of the screen according to the aspect ratio.
        /// Calculated as the tangent of <code>height / (2 * d)</code> where d is distance to the screen.
        /// </summary>
        public double VFieldOfView {
            get { return CalculateFOV(mWidth, new Vector3(0f, 0f, (float)(mHeight / 2.0))); }
            set {
                //if (Math.Abs(fov) < TOLERANCE || value <= 0.0)
                if (value <= 0.0)
                    return;
                double height = 2 * ScreenDistance * Math.Tan(value / 2.0);
                double width = mHeight / AspectRatio;
                ChangeDimesions(width, height);
            }
        }

        /// <summary>
        /// How far away the screen is transition the origin along the direction the screen is rotated. (mm)
        /// </summary>
        public double ScreenDistance {
            get { return Vector3.Dot(Centre - mCoordinator.EyePosition, Vector3.Normalize(mOrientation.LookAtVector)); }
            //get { return (double) (screenPosition - eyePosition).Length(); }
        }

        /// <summary>
        /// The matrix which will project objects in virtual space onto the flat input.
        /// </summary>
        public Matrix4 ProjectionMatrix {
            get { 
                switch (mProjection) {
                    case ProjectionStyle.Simple: return SimpleProjection();
                }
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The unique name by which the input is known.
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
                        dump += "Unable to get stats for input " + mOutput.Type + ". " + ex.Message + Environment.NewLine;
                        dump += ex.StackTrace;
                    }

                return dump;
            }
        }

        /// <summary>
        /// The monitor which this input should display on.
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
        /// The manager for the overlay which sits on top of this window.
        /// </summary>
        public WindowOverlayManager OverlayManager {
            get { return mOverlayManager; }
        }

        /// <summary>
        /// Initialise the input, giving it a reference to the input it is linked to.
        /// </summary>
        /// <param name="input">The input object the input can control.</param>
        public void Init(Coordinator coordinator) {
            mCoordinator = coordinator;
            if (mOutput != null)
                mOutput.Init(this);

        }

        /// <summary>
        /// Called when the input is to be disposed of.
        /// </summary>
        public void Close() {
            if (mOutput != null)
                mOutput.Close();
        }

        /// <summary>
        /// DrawSelected any relevant information about this input onto a diagram.
        /// </summary>
        /// <param name="perspective">The perspective to render along.</param>
        /// <param name="graphics">The graphics object to draw with.</param>
        public void Draw(Chimera.Perspective perspective, Graphics graphics) {
            throw new System.NotImplementedException();
        }

        void mOrientation_Changed(object sender, EventArgs e) {
            if (Changed != null)
                Changed(this, null);
        }

        private void TriggerChanged() {
            if (Changed != null)
                Changed(this, null);
        }
        private double CalculateFOV(double o, Vector3 min) {
                Vector3 max = min * -1;
                Quaternion q = Quaternion.CreateFromEulers(0f, (float)(mOrientation.Pitch * Rotation.DEG2RAD), 0f);
                min *= q;
                max *= q;

                Vector3 centre = Centre;
                min += centre;
                max += centre;

                float dot = Vector3.Dot(Vector3.Normalize(min - mCoordinator.EyePosition), Vector3.Normalize(max - mCoordinator.EyePosition));
                //return Math.Acos(dot);

                return Math.Atan2(o, ScreenDistance);
        }
        private void ChangeDimesions(double width, double height) {
            Vector3 centre = mWindowAnchor == WindowAnchor.Centre ? Centre : Vector3.Zero;
            mWidth = width;
            mHeight = height;
            if (mWindowAnchor == WindowAnchor.Centre) {
                Vector3 diagonal = new Vector3(0f, (float)(mWidth / 2.0), (float)(mHeight / 2.0));
                diagonal *= mOrientation.Quaternion;
                mTopLeft = centre - diagonal;
            }
            TriggerChanged();
        }

        private Matrix4 SimpleProjection() {
            float f = (float) VFieldOfView;
            float aspect = (float) AspectRatio;
            float zNear = .1f;
            float zFar = 1024f;
            return new Matrix4(
                f / aspect, 0,  0,                                  0,
                0,          f,  0,                                  0,
                0,          0,  (zFar + zNear) / (zNear - zFar),    (2f * zFar * zNear) / (zNear - zFar),
                0,          0,  -1f,                                0);
        }
    }
}
