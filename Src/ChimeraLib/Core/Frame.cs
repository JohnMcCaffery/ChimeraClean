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
using OpenMetaverse;
using System.Drawing;
using Chimera.Util;
using System.IO;
using System.Windows.Forms;
using Chimera.GUI.Forms;
using Chimera.Interfaces;
using Chimera.Plugins;
using Chimera.Config;

namespace Chimera {
    /// <summary>
    /// The different mechanisms used to calculate the projection matrix.
    /// </summary>
    public enum ProjectionStyle { 
        /// <summary>
        /// Use an orthogonal projection matrix rather than a perspective matrix.
        /// </summary>
        Orthogonal, 
        /// <summary>
        /// Calculated using the method from this paper http://lds62-112-144-233.my-simplyroot.de/wp-content/uploads/downloads/2011/12/0008.pdf.
        /// </summary>
        Calculated,
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
    public class Frame { 
        /// <summary>
        /// The system which this input is registered with.
        /// </summary>
        private Core mCoordinator;     
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
        private ProjectionStyle mProjection = ProjectionStyle.Calculated;
        /// <summary>
        /// Whether to include the eye position and resulting perspective lines when drawing the diagram.
        /// </summary>
        private bool mDrawEye = true;
        /// <summary>
        /// Whether to draw this window onto the diagram.
        /// </summary>
        private bool mDraw = true;
        /// <summary>
        /// Redraw functions to force the GUI to re-draw;
        /// </summary>
        private HashSet<Action> mRedraws = new HashSet<Action>();

        /// <summary>
        /// Triggered whenever the position of this input changes.
        /// </summary>
        public event Action<Frame, EventArgs> Changed;

        /// <summary>
        /// Triggered whenever the monitor that the input is to display on changes.
        /// </summary>
        public event Action<Frame, Screen> MonitorChanged;

        /// <summary>
        /// CreateWindowState a input. It is necessary to specify a unique name for the input.
        /// </summary>
        /// <param name="frameName">The name this input is known by within the system.</param>
        /// <param name="overlayAreas">The overlay areas mapped to this input.</param>
        public Frame(string frameName) {
            mName = frameName;

            FrameConfig cfg = new FrameConfig(frameName);
            mMonitor = Screen.AllScreens.FirstOrDefault(s => s.DeviceName.Equals(cfg.Monitor));
            mWidth = cfg.Width;
            mHeight = cfg.Height;
            mTopLeft = cfg.TopLeft;
            mOrientation = new Rotation(cfg.Pitch, cfg.Yaw);
            mCentre = Centre;
            mDraw = cfg.Draw;
            mDrawEye = cfg.DrawEye;

            mOrientation.Changed += mOrientation_Changed;

            if (mMonitor == null)
                mMonitor = Screen.PrimaryScreen;
        }

        /// <summary>
        /// CreateWindowState a input. It is necessary to specify a unique name for the input.
        /// </summary>
        /// <param name="name">The name this input is known by within the system.</param>
        /// <param name="overlayAreas">The overlay areas mapped to this input.</param>
        public Frame(string name, IOutput output)
            : this(name) {
            mOutput = output;
        }

        /// <summary>
        /// The system which this input is registered with.
        /// </summary>
        public Core Core {
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
        /// Whether to draw the eye and resulting perspective lines when drawing the diagram.
        /// </summary>
        public bool DrawWindow {
            get { return mDraw; }
            set { 
                mDraw = value;
                foreach (var redraw in mRedraws)
                    redraw();
            }
        }

        /// <summary>
        /// Whether to draw the eye and resulting perspective lines when drawing the diagram.
        /// </summary>
        public bool DrawEye {
            get { return mDrawEye; }
            set { 
                mDrawEye = value;
                foreach (var redraw in mRedraws)
                    redraw();
            }
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
            get { return CalculateFrustumOffset(v => new Vector2(v.X, v.Y), mOrientation.LookAtVector); }
            set { TopLeftFromSkew(ScreenDistance, value, VSkew); }
        }

        /// <summary>
        /// How far the view is skewed away from the direction it is facing along the veritcal axis.
        /// </summary>
        public double VSkew {
            get { return CalculateFrustumOffset(v => new Vector2((float) Math.Sqrt(Math.Pow(v.X, 2) + Math.Pow(v.Y, 2)), v.Z), Vector3.UnitZ); }
            set { TopLeftFromSkew(ScreenDistance, HSkew, value); }
        }

        /// <summary>
        /// The horizontal field of view the screen shows, in radians. 
        /// Changing this will change the height and width of the screen according to the aspect ratio.
        /// Calculated as the tangent of <code>width / (2 * d)</code> where d is distance to the screen.
        /// </summary>
        public double HFieldOfView {
            get { return CalculateFOV(mWidth, new Vector3(0f, (float)mWidth, 0f)); }
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
            get { return CalculateFOV(mHeight, new Vector3(0f, 0f, (float)-mHeight)); }
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
                    case ProjectionStyle.Orthogonal: return CalculateOrthogonalMatrix();
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
        /// Initialise the input, giving it a reference to the input it is linked to.
        /// </summary>
        /// <param name="input">The input object the input can control.</param>
        public void Init(Core coordinator) {
            mCoordinator = coordinator;
            mCoordinator.EyeUpdated += new Action<Chimera.Core,EventArgs>(mCoordinator_EyeUpdated);
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
        public void Draw(Func<Vector3, Point> to2D, Graphics graphics, Rectangle clipRectangle, Action redraw, Perspective perspective) {
            if (!mRedraws.Contains(redraw))
                mRedraws.Add(redraw);

            if (!mDraw)
                return;

            Vector3 top = new Vector3(0f, (float)mWidth, 0f) * mOrientation.Quaternion;
            Vector3 side = new Vector3(0f, 0f, (float)-mHeight) * mOrientation.Quaternion;
            Vector3 miniTop = Vector3.Zero;
            Vector3 miniSide = Vector3.Zero;

            using (Pen p = new Pen(mColour, 3f)) {
                Point bottomR = to2D(mTopLeft + top + side);
                graphics.DrawLine(p, to2D(mTopLeft), to2D(mTopLeft + top));
                graphics.DrawLine(p, to2D(mTopLeft), to2D(mTopLeft + side));
                graphics.DrawLine(p, to2D(mTopLeft + top), bottomR);
                graphics.DrawLine(p, to2D(mTopLeft + side), bottomR);
            }

            using (Brush b = new SolidBrush(mColour)) {
                graphics.FillPolygon(Brushes.Red, new Point[] {
                    to2D(mTopLeft),
                    to2D(mTopLeft + top),
                    to2D(mTopLeft + top + side),
                    to2D(mTopLeft + side)
                });
                graphics.DrawPolygon(Pens.Black, new Point[] {
                    to2D(mTopLeft),
                    to2D(mTopLeft + top),
                    to2D(mTopLeft + top + side),
                    to2D(mTopLeft + side)
                });
            }

            if (mDrawEye) {
                float perspectiveLineScale = 5f;
                Vector3 topLeftLine = ((mTopLeft - mCoordinator.EyePosition) * perspectiveLineScale) + mCoordinator.EyePosition;
                Vector3 topRightLine = (((mTopLeft + top) - mCoordinator.EyePosition) * perspectiveLineScale) + mCoordinator.EyePosition;
                Vector3 bottomLeftLine = (((mTopLeft + top + side) - mCoordinator.EyePosition) * perspectiveLineScale) + mCoordinator.EyePosition;
                Vector3 bottomRightLine = (((mTopLeft + side) - mCoordinator.EyePosition) * perspectiveLineScale) + mCoordinator.EyePosition;

                Point eye = to2D(mCoordinator.EyePosition);
                graphics.DrawLine(Pens.DarkViolet, eye, to2D(topLeftLine));
                graphics.DrawLine(Pens.DarkViolet, eye, to2D(topRightLine));
                graphics.DrawLine(Pens.DarkViolet, eye, to2D(bottomLeftLine));
                graphics.DrawLine(Pens.DarkViolet, eye, to2D(bottomRightLine));

                Vector3 look = new Vector3((float)ScreenDistance, 0f, 0f) * mOrientation.Quaternion;
                graphics.DrawLine(Pens.CornflowerBlue, eye, to2D(look + mCoordinator.EyePosition));

                Vector3 normal = new Vector3((float)(Diagonal / 2.0), 0f, 0f) * mOrientation.Quaternion;
                graphics.DrawLine(Pens.Crimson, to2D(Centre), to2D(normal + Centre));

                using (Pen p = new Pen(Color.BlueViolet, 4f))
                    graphics.DrawLine(p, to2D(look + mCoordinator.EyePosition), to2D(Centre));
            }
        }

        void mCoordinator_EyeUpdated(Core source, EventArgs args) {
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

        private double CalculateFrustumOffset(Func<Vector3, Vector2> to2D, Vector3 b) {
            Vector2 h = to2D(Centre - mCoordinator.EyePosition);
            Vector2 a = to2D(mOrientation.LookAtVector);
            float dot = Vector2.Dot(Vector2.Normalize(h), Vector2.Normalize(a));
            return ApplySign(Math.Sin(Math.Acos(dot)) * h.Length(), new Vector3(h, 0f), new Vector3(a, 0f));
            //return ApplySign(Math.Sin(Math.Acos(dot)) * h.Length(), Centre - mCoordinator.EyePosition, b);
            //return ApplySign(Math.Sin(Math.Acos(dot)) * h.Length(), Centre - mCoordinator.EyePosition, mOrientation.LookAtVector);
        }

        private double CalculateFOV(double o, Vector3 edge) {
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

        private Matrix4 CalculatedProjection() {
            float dn = .1f;
            float df = 1024f;
            double scale = 1f / ScreenDistance;

            float hFoV = (float)(2.0 / (mWidth * scale));
            float vFoV = (float)(2.0 / (mHeight * scale));
            float hShift = (float)(HSkew / (mWidth / 2.0));
            float vShift = (float)(VSkew / (mHeight / 2.0));
            float clip1 = -(df + dn) / (df - dn);
            float clip2 = -(2.0f * df * dn) / (df - dn);

		    return new Matrix4(
                hFoV,   0f,     hShift, 0f,
                0f,     vFoV,   vShift, 0f,
                0f,     0f,     clip1,  clip2,
                0f,     0f,     -1f,    0f);
        }

        private Matrix4 CalculateOrthogonalMatrix() {
            float dn = .1f;
            float df = 1024f;
            double scale = 1f / ScreenDistance;
            float r = (float) ((mWidth * scale) / 2.0);
            float t = (float) ((mHeight * scale) / 2.0);
            return new Matrix4(
                1f / r, 0f, 0f, 0f,
                0f, 1f / t, 0f, 0f,
                0f, 0f, -2f / (df - dn), (df + dn) / (df - dn),
                0f, 0f, 0f, 1f);
        }

        public override string ToString() {
            return mName;
        }

        public event Action Restarted;

        internal void Restart() {
            if (Restarted != null)
                Restarted();
            if (mOutput != null)
                mOutput.Restart("User");
        }
    }
}
