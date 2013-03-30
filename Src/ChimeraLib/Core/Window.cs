﻿using System;
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
            get { return CalculateFOV(mWidth, new Vector3(0f, (float)(mWidth / 2.0), 0f)); }
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
            get { return CalculateFOV(mHeight, new Vector3(0f, 0f, (float)(mHeight / 2.0))); }
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
            Vector3 side = new Vector3(0f, 0f, (float)mHeight) * mOrientation.Quaternion;

            graphics.DrawLine(Pens.Red, to2D(mTopLeft), to2D(mTopLeft + top));
            graphics.DrawLine(Pens.Red, to2D(mTopLeft), to2D(mTopLeft + side));
            graphics.DrawLine(Pens.Red, to2D(mTopLeft + top), to2D(mTopLeft + top + side));
            graphics.DrawLine(Pens.Red, to2D(mTopLeft + side), to2D(mTopLeft + top + side));

            graphics.FillPolygon(Brushes.Red, new Point[] {
                to2D(mTopLeft),
                to2D(mTopLeft + top),
                to2D(mTopLeft + top + side),
                to2D(mTopLeft + side)
            });

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

        private Matrix4 SkewedProjection() {
            float f = (float) VFieldOfView;
            float aspect = (float) AspectRatio;
            float zNear = .1f;
            float zFar = 1024f;
            float hSkew = (float) (HSkew / mWidth);
            float vSkew = (float) (VSkew / mHeight);
            return new Matrix4(
                f / aspect, 0,  hSkew,                              0,
                0,          f,  vSkew,                              0,
                0,          0,  (zFar + zNear) / (zNear - zFar),    (2f * zFar * zNear) / (zNear - zFar),
                0,          0,  -1f,                                0);
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

            float x1 = Math.Min(upperRight.Y, lowerLeft.Y);
            float x2 = Math.Max(upperRight.Y, lowerLeft.Y);
            float y1 = Math.Max(upperRight.Z, lowerLeft.Z);
            float y2 = Math.Min(upperRight.Z, lowerLeft.Z);
            float dn = (diff.Length() / diff.X) * .1f;
            float df = (512f * 100f) * dn;

		    return new Matrix4(
    			(2*dn) / (x2-x1),   0,              (x2+x1)/(x2-x1),   0,
    			0,                  (2*dn)/(y1-y2), (y1+y2)/(y1-y2),   0,
    			0,                  0,              -(df+dn)/(df-dn),   -(2.0f*df*dn)/(df-dn),
    			0,                  0,              -1.0f,              0);
        }
    }
}
