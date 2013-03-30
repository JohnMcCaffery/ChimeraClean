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
    public class Window {
        /// <summary>
        /// The system which this input is registered with.
        /// </summary>
        private Coordinator mCoordinator;
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
        /// The matrix which will project objects in virtual space onto the flat input.
        /// </summary>
        private Matrix4 mProjectionMatrix;
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
        /// The manager for the overlay which can be renderered on this window.
        /// </summary>
        private WindowOverlayManager mOverlayManager;

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
            set {
                mWidth = value;
                TriggerChanged();
            }
        }

        /// <summary>
        /// The height of the screen, in mm.
        /// </summary>
        public double Height {
            get { return mHeight ; }
            set {
                mHeight = value;
                TriggerChanged();
            }
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
                mWidth = mHeight / value;
                TriggerChanged();
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
                mHeight = value * Math.Sin(tan);
                mWidth = value * Math.Cos(tan);
                TriggerChanged();
            }
        }

        /// <summary>
        /// The horizontal field of view the screen shows, in radians. 
        /// Changing this will change the height and width of the screen according to the aspect ratio.
        /// Calculated as the tangent of <code>width / (2 * d)</code> where d is distance to the screen.
        /// </summary>
        public double HFieldOfView {
            get {
                Vector3 left = new Vector3(0f, (float)(mWidth / 2.0), 0f);
                Vector3 right = left * -1;
                Quaternion q = Quaternion.CreateFromEulers(0f, (float)(mOrientation.Pitch * Rotation.DEG2RAD), 0f);
                left *= q;
                right *= q;

                left += Centre;
                right += Centre;

                float dot = Vector3.Dot(Vector3.Normalize(left - mCoordinator.EyePosition), Vector3.Normalize(right - mCoordinator.EyePosition));
                //return Math.Acos(dot);

                return Math.Atan2(mHeight, ScreenDistance);
            }
            set {
                //if (Math.Abs(fov) < TOLERANCE || value <= 0.0)
                if (value <= 0.0)
                    return;
                double aspectRatio = AspectRatio;
                mWidth = 2 * ScreenDistance * Math.Cos(value / 2.0);
                double a = Math.Cos(value / 2);
                if (a != 0.0)
                    mWidth /= a;
                mHeight = mWidth * aspectRatio;
                TriggerChanged();
            }
        }

        /// <summary>
        /// The vertical field of view the screen shows, in radians. 
        /// Changing this will change the height and width of the screen according to the aspect ratio.
        /// Calculated as the tangent of <code>height / (2 * d)</code> where d is distance to the screen.
        /// </summary>
        public double VFieldOfView {
            get {
                Vector3 top = new Vector3(0f, 0f, (float)(mHeight / 2.0));
                Vector3 bottom = top * -1;
                Quaternion q = Quaternion.CreateFromEulers(0f, (float)(mOrientation.Pitch * Rotation.DEG2RAD), 0f);
                top *= q;
                bottom *= q;

                top += Centre;
                bottom += Centre;

                float dot = Vector3.Dot(Vector3.Normalize(top - mCoordinator.EyePosition), Vector3.Normalize(bottom - mCoordinator.EyePosition));
                //return Math.Acos(dot);

                return Math.Atan2(mHeight, ScreenDistance);
            }
            set {
                //if (Math.Abs(fov) < TOLERANCE || value <= 0.0)
                if (value <= 0.0)
                    return;
                double aspectRatio = AspectRatio;
                mHeight = 2 * ScreenDistance * Math.Sin(value / 2.0);
                double a = Math.Cos(value / 2);
                if (a != 0.0)
                    mHeight /= a;
                mWidth = mHeight / aspectRatio;
                TriggerChanged();
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
            get { return mProjectionMatrix ; }
            set { mProjectionMatrix  = value; }
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
    }
}
