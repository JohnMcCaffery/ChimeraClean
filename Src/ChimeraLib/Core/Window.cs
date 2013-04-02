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
        Calculated,
        /// <summary>
        /// No skewing is used to achieve the offset effect. Instead the FoV and the orientation are adjusted.
        /// </summary>
        RotFoV
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
        /// Orientation if the FoV/Rot mechanism is being used to account for skewed perspectives.
        /// </summary>
        private Rotation mFoVRotOrientation = Rotation.Zero;
        /// <summary>
        /// The position of the top left corner of the screen in real space, in mm.
        /// </summary>
        private Vector3 mTopLeft;
        /// <summary>
        /// The centre of the window in real space, mm. This value is not cannoninical. Use mTopLeft.
        /// </summary>
        private Vector3 mCentre;
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
        /// Whether to link horizontal and vertical field of view's so that when one changes the aspect ratio is preserved.
        /// </summary>
        private bool mLinkFoVs = true;
        /// <summary>
        /// The colour this window will be when drawn onto a diagram.
        /// </summary>
        private Color mColour = Color.Red;
        /// <summary>
        /// The anchor for the window as the width and height change.
        /// </summary>
        private WindowAnchor mWindowAnchor = WindowAnchor.Centre; 
        /// <summary>
        /// The method used to calculate the projection matrix.
        /// </summary>
        private ProjectionStyle mProjection = ProjectionStyle.Skewed;

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
            mCentre = Centre;

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
                mCentre = Centre;
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
                TopLeft = value - diagonal;
            }
        }

        /// <summary>
        /// The orientation of the input in real space.
        /// </summary>
        public Rotation Orientation {
            get { return mOrientation; }
            set {
                if (mOrientation != null)
                    mOrientation.Changed -= mOrientation_Changed;
                mOrientation = value;
                mOrientation.Changed += mOrientation_Changed;
                TriggerChanged();
            }
        }

        /// <summary>
        /// The orientation of the input in real space.
        /// </summary>
        public Rotation OutputOrientation {
            get { 
                if (mProjection == ProjectionStyle.RotFoV) {
                    /*
                    Vector3 diffH = mTopLeft - mCoordinator.EyePosition;
                    Vector3 diffV = mTopLeft - mCoordinator.EyePosition;
                    diffH.Z = 0f;
                    //diffV.Y = (float) Math.Sqrt(Math.Pow(diffV.X, 2) + Math.Pow(diffV.Y, 2));
                    //diffV.X = 0f;
                    double hAngle = Math.Acos(Vector3.Dot(Vector3.Normalize(diffH), Vector3.UnitX));
                    double vAngle = Math.Acos(Vector3.Dot(Vector3.Normalize(diffV), Vector3.Normalize(diffH)));
                    //double vAngle = Math.Acos(Vector3.Mag(diffV) / Vector3.Mag(diffH));
                    hAngle = ApplySign(hAngle, diffH, Vector3.UnitX);
                    vAngle = ApplySign(vAngle, diffV, diffH);
                    double hdeg = hAngle * (180 / Math.PI);
                    double vdeg = vAngle * (180 / Math.PI);
                    return new Rotation((vAngle + (VFieldOfView / 2.0)) * (180 / Math.PI), (hAngle + (HFieldOfView / 2.0)) * (180.0 / Math.PI));
                    */
                    Rotation topLeftRot = new Rotation(mTopLeft - mCoordinator.EyePosition);
                    //Rotation topLeftRot = new Rotation(t - mCoordinator.EyePosition);
                    Rotation fovRot = new Rotation((VFieldOfView / 2.0) * (180.0 / Math.PI), (HFieldOfView / 2.0) * (180.0 / Math.PI));
                    Vector3 a = new Vector3((float) ScreenDistance, 0f, 0f);
                    Vector3 h = new Vector3(0f, 0f, mTopLeft.Z);
                    //double pitch = ApplySign(Math.Acos(Vector3.Dot(a, h)) * (180 / Math.PI), a, h);
                    double pitch = ApplySign(Math.Atan(mTopLeft.Z / ScreenDistance) * (180 / Math.PI), mTopLeft, Vector3.UnitX);
                    topLeftRot.Pitch = pitch;
                    return topLeftRot + fovRot;
                }
                return mOrientation;
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
        /// How far away the screen is relative to the origin along the direction the screen is rotated. (mm)
        /// Calculated as the projection of the distamce from eye to screen centre onto the look at vector for the screen orientation.
        /// </summary>
        public double ScreenDistance {
            get { return Vector3.Dot(Centre - mCoordinator.EyePosition, Vector3.Normalize(mOrientation.LookAtVector)); }
            set { TopLeftFromSkew(value, HSkew, VSkew); }
        }

        /// <summary>
        /// How far the view is skewed away from the direction it is facing along the horizontal axis.
        /// </summary>
        public double HSkew {
            get { return CalculateFrustumOffset(v => new Vector2(v.X, v.Y)); }
            set { TopLeftFromSkew(ScreenDistance, value, VSkew); }
        }

        /// <summary>
        /// How far the view is skewed away from the direction it is facing along the veritcal axis.
        /// </summary>
        public double VSkew {
            get { return CalculateFrustumOffset(v => new Vector2((float) Math.Sqrt(Math.Pow(v.X, 2) + Math.Pow(v.Y, 2)), v.Z)); }
            set { TopLeftFromSkew(ScreenDistance, HSkew, value); }
        }

        /// <summary>
        /// The horizontal field of view the screen shows, in radians. 
        /// Changing this will change the height and width of the screen according to the aspect ratio.
        /// Calculated as the tangent of <code>width / (2 * d)</code> where d is distance to the screen.
        /// </summary>
        public double HFieldOfView {
            get {
                /*
                if (mProjection == ProjectionStyle.RotFoV) {
                    Vector3 bottomRight = mTopLeft + (new Vector3(0f, (float)mWidth, (float)-mHeight) * mOrientation.Quaternion);
                    Rotation tlRot = new Rotation(mTopLeft);
                    Rotation brRot = new Rotation(bottomRight);
                    return (brRot - tlRot).Yaw * (Math.PI / 180.0);
                }
                */
                return CalculateFOV(mWidth, new Vector3(0f, (float)mWidth, 0f)); }
            set {
                if (value <= 0.0)
                    return;
                double width = 2 * ScreenDistance * Math.Tan(value / 2.0);
                double height = mLinkFoVs ? width * AspectRatio : mHeight;
                ChangeDimesions(width, height);
            }
        }

        /// <summary>
        /// The vertical field of view the screen shows, in radians. 
        /// Changing this will change the height and width of the screen according to the aspect ratio.
        /// Calculated as the tangent of <code>height / (2 * d)</code> where d is distance to the screen.
        /// </summary>
        public double VFieldOfView {
            get {
                /*
                if (mProjection == ProjectionStyle.RotFoV) {
                    Vector3 bottomRight = mTopLeft + (new Vector3(0f, (float)mWidth, (float)-mHeight) * mOrientation.Quaternion);
                    Rotation tlRot = new Rotation(mTopLeft);
                    Rotation brRot = new Rotation(bottomRight);
                    return (brRot - tlRot).Pitch * (Math.PI / 180.0);
                }
                */
                return CalculateFOV(mHeight, new Vector3(0f, 0f, (float)-mHeight)); }
            set {
                if (value <= 0.0)
                    return;
                double height = 2 * ScreenDistance * Math.Tan(value / 2.0);
                double width = mLinkFoVs ? height / AspectRatio : mWidth;
                ChangeDimesions(width, height);
            }
        }

        /// <summary>
        /// The matrix which will project objects in virtual space onto the flat input.
        /// </summary>
        public Matrix4 ProjectionMatrix {
            get { 
                switch (mProjection) {
                    case ProjectionStyle.Simple: return SimpleProjection();
                    case ProjectionStyle.Skewed: return SkewedProjection();
                    case ProjectionStyle.Calculated: return CalculatedProjection();
                }
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Whether to link horizontal and vertical field of view's so that when one changes the aspect ratio is preserved.
        /// </summary>
        public bool LinkFoVs {
            get { return mLinkFoVs; }
            set { mLinkFoVs = value; }
        }

        /// <summary>
        /// Where to anchor the position of the window when the width or height changes.
        /// </summary>
        public WindowAnchor Anchor {
            get { return mWindowAnchor; }
            set { mWindowAnchor = value; }
        }

        /// <summary>
        /// What algorithm should be used to a calculate the projection matrix.
        /// </summary>
        public ProjectionStyle Projection {
            get { return mProjection; }
            set { 
                mProjection = value;
                TriggerChanged();
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
            mCoordinator.EyeUpdated += new Action<Chimera.Coordinator,EventArgs>(mCoordinator_EyeUpdated);
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
        public void Draw(Func<Vector3, Point> to2D, Graphics graphics, Rectangle clipRectangle) {
            Vector3 top = new Vector3(0f, (float)mWidth, 0f) * mOrientation.Quaternion;
            Vector3 side = new Vector3(0f, 0f, (float)-mHeight) * mOrientation.Quaternion;
            Vector3 miniTop = Vector3.Zero;
            Vector3 miniSide = Vector3.Zero;

            if (mProjection == ProjectionStyle.RotFoV) {
                Vector3 hLeft = new Vector3(mTopLeft - mCoordinator.EyePosition);
                Vector3 vLeft = new Vector3(mTopLeft - mCoordinator.EyePosition);
                hLeft.Z = 0f;
                vLeft.X = (float) ScreenDistance;
                vLeft.Y = 0f;
                float hHyp = Vector3.Mag(hLeft);
                float vHyp = Vector3.Mag(vLeft);
                double w = (Math.Sin(HFieldOfView / 2.0) * hHyp) * 2.0;
                double h = (Math.Sin(VFieldOfView / 2.0) * vHyp) * 2.0;
                miniTop = new Vector3(0f, (float) w, 0f) * new Rotation(0.0, OutputOrientation.Yaw).Quaternion;
                miniSide = new Vector3(0f, 0f, (float) -h) * new Rotation(OutputOrientation.Pitch, 0.0).Quaternion;
            }

            using (Pen p = new Pen(mColour, 3f)) {
                Point bottomR = to2D(mTopLeft + top + side);
                graphics.DrawLine(p, to2D(mTopLeft), to2D(mTopLeft + top));
                graphics.DrawLine(p, to2D(mTopLeft), to2D(mTopLeft + side));
                graphics.DrawLine(p, to2D(mTopLeft + top), bottomR);
                graphics.DrawLine(p, to2D(mTopLeft + side), bottomR);

                if (mProjection == ProjectionStyle.RotFoV) {
                    Point botR = to2D(mTopLeft + miniTop + miniSide);
                    graphics.DrawLine(p, to2D(mTopLeft), to2D(mTopLeft + miniTop));
                    graphics.DrawLine(p, to2D(mTopLeft), to2D(mTopLeft + miniSide));
                    graphics.DrawLine(p, to2D(mTopLeft + miniTop), botR);
                    graphics.DrawLine(p, to2D(mTopLeft + miniSide), botR);
                }
            }

            using (Brush b = new SolidBrush(mColour)) {
                graphics.FillPolygon(Brushes.Red, new Point[] {
                    to2D(mTopLeft),
                    to2D(mTopLeft + top),
                    to2D(mTopLeft + top + side),
                    to2D(mTopLeft + side)
                });

                /*
                if (mProjection == ProjectionStyle.RotFoV) {
                    graphics.FillPolygon(Brushes.Red, new Point[] {
                        to2D(mTopLeft),
                        to2D(mTopLeft + miniTop),
                        to2D(mTopLeft + miniTop + miniSide),
                        to2D(mTopLeft + miniSide)
                    });
                }
                */
            }

            float perspectiveLineScale = 5f;
            Vector3 topLeftLine = ((mTopLeft - mCoordinator.EyePosition) * perspectiveLineScale) + mCoordinator.EyePosition;
            Vector3 topRightLine = (((mTopLeft + top) - mCoordinator.EyePosition) * perspectiveLineScale) + mCoordinator.EyePosition;
            Vector3 bottomLeftLine = (((mTopLeft + top + side) - mCoordinator.EyePosition) * perspectiveLineScale) + mCoordinator.EyePosition;
            Vector3 bottomRightLine = (((mTopLeft + side) - mCoordinator.EyePosition) * perspectiveLineScale) + mCoordinator.EyePosition;

            Point eye = to2D(mCoordinator.EyePosition);
            graphics.DrawLine(Pens.SeaShell, eye, to2D(topLeftLine));
            graphics.DrawLine(Pens.SeaShell, eye, to2D(topRightLine));
            graphics.DrawLine(Pens.SeaShell, eye, to2D(bottomLeftLine));
            graphics.DrawLine(Pens.SeaShell, eye, to2D(bottomRightLine));

            Vector3 look = new Vector3((float)ScreenDistance, 0f, 0f) * mOrientation.Quaternion;
            graphics.DrawLine(Pens.SeaShell, eye, to2D(look + mCoordinator.EyePosition));
        }

        void mCoordinator_EyeUpdated(Coordinator source, EventArgs args) {
            TriggerChanged();
        }

        void mOrientation_Changed(object sender, EventArgs e) {
            if (mWindowAnchor == WindowAnchor.Centre) {
                Vector3 diagonal = new Vector3(0f, (float)(mWidth / 2.0), (float)(mHeight / -2.0));
                diagonal *= mOrientation.Quaternion;
                mTopLeft = mCentre - diagonal;
            }
            mCentre = Centre;
            TriggerChanged();
        }

        private void TriggerChanged() {
            if (Changed != null)
                Changed(this, null);
        }

        private void TopLeftFromSkew(double distance, double hSkew, double vSkew) {
            Vector3 centre = new Vector3((float) distance, (float)hSkew, (float)vSkew);
            centre *= mOrientation.Quaternion;
            centre += mCoordinator.EyePosition;
            Centre = centre;
        }

        private Vector3 CrossProduct(Vector3 a, Vector3 b) {
            float x = (a.Y * b.Z) - (a.Z * b.Y);
            float y = (a.Z * b.X) - (a.X * b.Z);
            float z = (a.X * b.Y) - (a.Y * b.X);
            return new Vector3(x, y, z);
        }

        private double ApplySign(double val, Vector3 a, Vector3 b) {
            if ((a.X * b.Y) - (a.Y * b.X) > 0.0)
                return val * -1;
            return val;
        }

        private double CalculateFrustumOffset(Func<Vector3, Vector2> to2D) {
            Vector2 h = to2D(Centre - mCoordinator.EyePosition);
            Vector2 a = to2D(mOrientation.LookAtVector);
            float dot = Vector2.Dot(Vector2.Normalize(h), Vector2.Normalize(a));
            return ApplySign(Math.Sin(Math.Acos(dot)) * h.Length(), Centre - mCoordinator.EyePosition, mOrientation.LookAtVector);
        }

        private double CalculateFOV(double o, Vector3 edge) {
            if (mProjection == ProjectionStyle.RotFoV) {
                //Vector3 max = edge * -1;
                //Quaternion q = Quaternion.CreateFromEulers(0f, (float)(mOrientation.Pitch * Rotation.DEG2RAD), 0f);
                //edge *= q;
                //max *= q;
                //Vector3 centre = Centre;
                //edge += centre;
                //max += centre;

                edge *= mOrientation.Quaternion;
                //Vector3 otherCorner = mTopLeft + (edge * mOrientation.Quaternion);
                Vector3 otherCorner = mTopLeft + edge;
                Vector3 arm1 = mTopLeft - mCoordinator.EyePosition;
                Vector3 arm2 = otherCorner - mCoordinator.EyePosition;
                if (edge.Z == 0f) {
                    arm1.Z = 0f;
                    arm2.Z = 0f;
                } else {
                    //arm1.X = (float)Math.Sqrt(Math.Pow(arm1.X, 2.0) + Math.Pow(arm1.Y, 2.0));
                    //arm2.X = (float)Math.Sqrt(Math.Pow(arm2.X, 2.0) + Math.Pow(arm2.Y, 2.0));
                    arm1.X = (float) ScreenDistance;
                    arm2.X = (float) ScreenDistance;
                    arm1.Y = 0f;
                    arm2.Y = 0f;
                }
                //arm1 = Vector3.Normalize(arm1);
                //arm2 = Vector3.Normalize(arm2);
                //float mag1 = Vector3.Mag(arm1);
                //float mag2 = Vector3.Mag(arm2);
                //float dot = Vector3.Dot(Vector3.Normalize(mTopLeft - mCoordinator.EyePosition), Vector3.Normalize(otherCorner - mCoordinator.EyePosition));
                float dot = Vector3.Dot(Vector3.Normalize(arm1), Vector3.Normalize(arm2));
                double angle = Math.Acos(dot) * (180.0 / Math.PI);
                return Math.Acos(dot);
            } else 
                //TODO what happens with a skew?
                return Math.Atan2(o / 2, ScreenDistance) * 2;
        }

        private void ChangeDimesions(double width, double height) {
            Vector3 centre = mWindowAnchor == WindowAnchor.Centre ? Centre : Vector3.Zero;
            mWidth = width;
            mHeight = height;
            if (mWindowAnchor == WindowAnchor.Centre) {
                Vector3 diagonal = new Vector3(0f, (float)(mWidth / 2.0), (float)(mHeight / -2.0));
                diagonal *= mOrientation.Quaternion;
                mTopLeft = centre - diagonal;
            }
            mCentre = Centre;
            TriggerChanged();
        }

        private Matrix4 SimpleProjection() {
            float fH = (float) (1.0 /  Math.Tan(HFieldOfView / 2.0));
            float fV = (float) (1.0 /  Math.Tan(VFieldOfView / 2.0));
            float zNear = .1f;
            float zFar = 1024f;
            return new Matrix4(
                fH, 0,      0,                                  0,
                0,  fV,     0,                                  0,
                0,  0,      (zFar + zNear) / (zNear - zFar),    (2f * zFar * zNear) / (zNear - zFar),
                0,  0,      -1f,                                0);
        }

        private Matrix4 SkewedProjection() {
            float fH = (float)(1.0 / Math.Tan(HFieldOfView / 2.0));
            float fV = (float)(1.0 / Math.Tan(VFieldOfView / 2.0));
            float zNear = .1f;
            float zFar = 1024f;
            float hSkew = (float) (HSkew / mWidth);
            float vSkew = (float) (VSkew / mHeight);
            return new Matrix4(
                fH,     0,  hSkew,                              0,
                0,      fV, vSkew,                              0,
                0,      0,  (zFar + zNear) / (zNear - zFar),    (2f * zFar * zNear) / (zNear - zFar),
                0,      0,  -1f,                                0);
        }

        private Matrix4 CalculatedProjection() {
            Vector3 upperRight = new Vector3(0f, (float)(mWidth / 2.0), (float)(mHeight / 2.0));
            Vector3 lowerLeft = new Vector3(0f, (float)(mWidth / -2.0), (float)(mHeight / -2.0));
            Vector3 diff = Centre - mCoordinator.EyePosition;

            diff *= -mOrientation.Quaternion;
            //diff *= input.RotationOffset.Quaternion;

            upperRight += diff;
            lowerLeft += diff;

            //upperRight /= (Math.Abs(diff.X) - .01f);
            //lowerLeft /= (Math.Abs(diff.X) - .01f);
            upperRight /= (float) (diff.X * 10.0);
            lowerLeft /= (float) (diff.X * 10.0);

            float x1O = Math.Min(upperRight.Y, lowerLeft.Y);
            float x2O = Math.Max(upperRight.Y, lowerLeft.Y);
            float y1O = Math.Max(upperRight.Z, lowerLeft.Z);
            float y2O = Math.Min(upperRight.Z, lowerLeft.Z);
            float x1 = (float) ((mWidth /-2) + HSkew) / 10000f;
            float x2 = (float) ((mWidth / 2) + HSkew) / 10000f;
            float y1 = (float)((mHeight / 2) + VSkew) / 10000f;
            float y2 = (float)((mHeight /-2) + VSkew) / 10000f;
            float dn = (diff.Length() / diff.X) * .1f;
            float df = (512f * 100f) * dn;

		    Matrix4 ret = new Matrix4(
    			(2*dn) / (x2-x1),   0,              (x2+x1)/(x2-x1),   0,
    			0,                  (2*dn)/(y1-y2), (y1+y2)/(y1-y2),   0,
    			0,                  0,              -(df+dn)/(df-dn),   -(2.0f*df*dn)/(df-dn),
    			0,                  0,              -1.0f,              0);
    			 //(2*dn) / (x2-x1),   0,              (float) (HSkew/mWidth),   0,
    			//0,                  (2*dn)/(y2-y1), (float) (VSkew/mHeight),   0,
    			 //(2*dn) / ((float)mWidth),   0,              (float) (HSkew/mWidth),   0,
    			//0,                  (2*dn)/((float) mHeight), (float) (VSkew/mHeight),   0,
    			//0,                  0,              -(df+dn)/(df-dn),   -(2.0f*df*dn)/(df-dn),
    			//0,                  0,              -1.0f,              0);

            //Vector3 left = mTopLeft - mCoordinator.EyePosition;
            //Vector3 right = (mTopLeft + new Vector3(0f, 2000f, 0f)) - mCoordinator.EyePosition;
            //Vector3 middle = (mTopLeft + new Vector3(0f, 1000f, 0f)) - mCoordinator.EyePosition;
            Vector3 left = new Vector3(-1000f, 0f, 1000f);
            Vector3 middle = new Vector3(0f, 0f, 1000f);
            Vector3 right = new Vector3(1000f, 0f, 1000f);

            /*
            o = (left * ret) / Vector3.Mag(left);
            o = (right * ret) / Vector3.Mag(left);
            o = (middle * ret) / Vector3.Mag(left);
            */

            Vector3 oLeft = (left * ret) / left.Z;
            Vector3 oMiddle = (middle * ret) / left.Z;
            Vector3 oRight = (right * ret) / left.Z;

            left *= 3f;
            middle *= 3f;
            right *= 3f;

            oLeft = (left * ret) / left.Z;
            oMiddle = (middle * ret) / left.Z;
            oRight = (right * ret) / left.Z;

            return ret;
        }

        private Matrix4 FoVRotProjection() {
            float fH = (float)(1.0 / Math.Tan(HFieldOfView / 2.0));
            float fV = (float)(1.0 / Math.Tan(VFieldOfView / 2.0));
            float zNear = .1f;
            float zFar = 1024f;
            float hSkew = (float)(HSkew / mWidth);
            float vSkew = (float)(VSkew / mHeight);
            return new Matrix4(
                fH, 0, hSkew, 0,
                0, fV, vSkew, 0,
                0, 0, (zFar + zNear) / (zNear - zFar), (2f * zFar * zNear) / (zNear - zFar),
                0, 0, -1f, 0);
        }
    }
}
