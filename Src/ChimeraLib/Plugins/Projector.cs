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
using Chimera.Config;
using Chimera.Plugins;

namespace Chimera.Plugins {
    public enum AspectRatio {
        /// <summary>
        /// An aspect ratio of 16:9
        /// </summary>
        SixteenNine,
        /// <summary>
        /// An aspect ratio of 4:3
        /// </summary>
        FourThree
    }

    public enum LockedVariable {
        Width,
        Height,
        Position,
        Nothing
    }

    public class Projector {
        private Window mWindow;
        private Vector3 mPosition;
        private Rotation mOrientation;
        private double mThrowRatio;
        private AspectRatio mAspectRatio;
        private AspectRatio mNativeAspectRatio;
        private double mTargetH;
        private double mH;
        private double mW;
        private bool mLockHeight;
        private bool mDraw;
        private bool mDrawLabels;
        private bool mUpsideDown;
        private double mD;
        private double mVOffset;
        private bool mRedrawing;
        private LockedVariable mLock;
        private ProjectorPlugin mProjectorPlugin;


        private double mOldW;
        private double mOldH;

        public event Action Change;

        public Window Window { get { return mWindow; } }

        #region Settings

        public LockedVariable Lock {
            get { return mLock; }
            set {
                mLock = value;
                if (value != LockedVariable.Nothing)
                    Redraw(LockedVariable.Nothing);
            }
        }

        public bool UpsideDown {
            get { return mUpsideDown; }
            set {
                mUpsideDown = value;
                Redraw(LockedVariable.Position);
            }
        }

        public bool DrawDiagram {
            get { return mDraw; }
            set {
                mDraw = value;
                Redraw(LockedVariable.Position);
            }
        }

        public bool DrawLabels {
            get { return mDrawLabels; }
            set {
                mDrawLabels = value;
                Redraw(LockedVariable.Position);
            }
        }

        #endregion

        #region Constants

        public AspectRatio AspectRatio {
            get { return mAspectRatio; }
            set {
                switch (mNativeAspectRatio) {
                    case AspectRatio.SixteenNine:
                        mH = 9f / 16f; 
                        switch (value) {
                            case AspectRatio.SixteenNine:
                                mW = 1f;
                                break;
                            case AspectRatio.FourThree:
                                mW = 6 / 8f;
                                break;
                        }
                        break;
                    case AspectRatio.FourThree:
                        mW = 1f;
                        switch (value) {
                            case AspectRatio.SixteenNine: 
                                mH = 9f / 16f; 
                                break;
                            case AspectRatio.FourThree: 
                                mH = 3f / 4f; 
                                break;
                        }
                        break;
                }
                mAspectRatio = value;
                CalculateAngles();
                Redraw(LockedVariable.Position);
            }
        }

        public AspectRatio NativeAspectRatio {
            get { return mNativeAspectRatio; }
            set {
                mNativeAspectRatio = value;
                switch (value) {
                    case AspectRatio.SixteenNine: mH = 9f / 16f; break;
                    case AspectRatio.FourThree: mH = 3f / 4f; break;
                }
                AspectRatio = mAspectRatio;
            }
        }

        public double VOffset {
            get { return mVOffset; }
            set {
                mVOffset = value;
                CalculateAngles();
                Redraw(LockedVariable.Position);
            }
        }

        public double ThrowRatio {
            get { return mThrowRatio; }
            set {
                mThrowRatio = value;
                CalculateAngles();
                Redraw(LockedVariable.Position);
            }
        }

        #endregion

        #region Unknowns

        public Vector3 Position {
            get { return mPosition; }
            set { 
                mPosition = value;
                Redraw(LockedVariable.Position);
            }
        }

        public Vector3 RelativePosition {
            get { 
                Vector3 big = mProjectorPlugin.Big - mPosition; 
                Vector3 small = mProjectorPlugin.Small - mPosition;
                float x = Math.Abs(big.X) > Math.Abs(small.X) ? small.X : big.X;
                float y = Math.Abs(big.Y) > Math.Abs(small.Y) ? small.Y : big.Y;
                float z = Math.Abs(big.Z) > Math.Abs(small.Z) ? small.Z : big.Z;
                return new Vector3(x, y, z);
            }
        }

        public Rotation Orientation {
            get { return mOrientation; }
        }
        /// <summary>
        /// Pitch angle, in radians.
        /// </summary>
        private double P {
            get { return mOrientation.Pitch * (Math.PI / 180.0); }
        }

        public double D {
            get { return mD; }
            set {
                mD = value;
                Redraw(LockedVariable.Position);
            }
        }

        public double Clearance {
            get {
                double alpha = Math.Atan2(mVOffset, mThrowRatio);
                double beta = Math.Atan2(mVOffset + mH, mThrowRatio);

                double flip = mUpsideDown ? -1.0 : 1.0;

                double h = mTargetH;
                double d = mD;
                double angleB = mUpsideDown ? P - beta : P + alpha;

                double b = d * Math.Tan(angleB);
                Vector3 bottomCentre = new Vector3((float)d, 0f, (float)b);
                bottomCentre *= new Rotation(0.0, mOrientation.Yaw).Quaternion;
                bottomCentre += Origin;






                Vector3 projector = mProjectorPlugin.RoomPosition + mPosition;
                Vector3 planeDir = bottomCentre - projector;
                Vector3 planeNormal = Vector3.Cross(planeDir, Vector3.UnitY * new Rotation(0.0, mOrientation.Yaw).Quaternion);

                //Calculate the intersection of the plane defined by the point mPlaneTopLeft and the normal mPlaneNormal and the line defined by the point mPointStart and the direction mPointDir.
                float numerator = Vector3.Dot(Vector3.Zero - projector, planeNormal);
                float denominator = Vector3.Dot(planeNormal, Vector3.UnitZ);
                bool mIntersects = denominator != 0f;
                if (!mIntersects)
                    return -1f;
                //return (-numerator / denominator) + (mProjectorPlugin.Small.Z - mProjectorPlugin.RoomPosition.Z);
                return (-numerator / denominator) + (mProjectorPlugin.Size.Z - mProjectorPlugin.RoomPosition.Z);
            }
        }

        #endregion

        public Projector(Window window, ProjectorPlugin projectorPlugin) {
            mProjectorPlugin = projectorPlugin;
            mWindow = window;
            mTargetH = window.Height;

            ProjectorConfig cfg = new ProjectorConfig(window.Name);
            mThrowRatio = cfg.ThrowRatio;
            mOrientation = new Rotation(cfg.ProjectorPitch, cfg.ProjectorYaw);
            mUpsideDown = cfg.UpsideDown;
            mVOffset = cfg.VOffset;
            mLock = cfg.Lock;

            if (mLock == LockedVariable.Position || mLock == LockedVariable.Nothing) {
                mPosition = cfg.ProjectorPosition;
                mD = cfg.WallDistance;
            }

            CalculateAngles();
            AspectRatio = cfg.AspectRatio;
            NativeAspectRatio = cfg.NativeAspectRatio;

            mOldW = mWindow.Width;
            mOldH = mWindow.Height;

            mDraw = cfg.Draw;
            mDrawLabels = cfg.DrawLabels;


            mProjectorPlugin.RoomChanged += new Action(mProjectorPlugin_RoomChanged);
            mOrientation.Changed += new EventHandler(mOrientation_Changed);
            window.Changed += new Action<Window,EventArgs>(window_Changed);

        }

        void mProjectorPlugin_RoomChanged() {
            Redraw(LockedVariable.Position);
        }

        void window_Changed(Window w, EventArgs args) {
            if (w.Width != mOldW)
                Redraw(LockedVariable.Width);
            else
                Redraw(LockedVariable.Height);
            mOldW = mWindow.Width;
            mOldH = mWindow.Height;
        }

        void mOrientation_Changed(object sender, EventArgs e) {
            Redraw(LockedVariable.Position);
        }

        internal void Redraw(LockedVariable source) {
            if (!mRedrawing && mWindow != null) {
                mRedrawing = true;
                UpdateUnknowns(mLock == LockedVariable.Nothing ? source : mLock);
                mRedrawing = false;
            }

            mProjectorPlugin.Redraw();

            if (Change != null)
                Change();
        }

        private void CalculateAngles() {
            mAlpha = Math.Atan2(mVOffset, mThrowRatio);
            mBeta = Math.Atan2(mVOffset + mH, mThrowRatio);
            mGamma = Math.Atan2(mW / 2.0, mThrowRatio);
        }

        private double CalculateW() {
            double alphaW = (mD * 2 * Math.Tan(mGamma)) / Math.Cos(P + mAlpha);
            double betaW = (mD * 2 * Math.Tan(mGamma)) / Math.Cos(P + mBeta);
            return Math.Min(alphaW, betaW);
        }

        private double CalculateH() {
            return mD * CalculateHeightAngle();
        }

        private double CalculateDFromH() {
            return mWindow.Height / CalculateHeightAngle();
        }

        private double CalculateHeightAngle() {
            double top = mUpsideDown ? P - mAlpha : P + mBeta;
            double bottom = mUpsideDown ? P - mBeta : P + mAlpha;
            return Math.Tan(top) - Math.Tan(bottom);
        }

        private double CalculateDFromW() {
            double flip = mUpsideDown ? -1.0 : 1.0;
            double alphaA = Math.Cos(P + (mAlpha * flip));
            double betaA = Math.Cos(P + (mBeta * flip));
            double tanGamma = 2 * Math.Tan(mGamma);
            double alphaD = (alphaA * mWindow.Width) / tanGamma;
            double betaD = (betaA * mWindow.Width) / tanGamma;
            Console.WriteLine("{0,2} P: {1,6:.1} A: {2,6:.1} B: {3,6:.1} DiffA: {4,6:.1} DiffB: {5,6:.1}", 
                alphaD > betaD ? "-1" : "1", 
                mOrientation.Pitch, 
                (180.0 * mAlpha) / Math.PI,
                (180.0 * mBeta) / Math.PI,
                (180.0 * (P + mAlpha * flip)) / Math.PI,
                (180.0 * (P + mBeta * flip)) / Math.PI);
            return Math.Max(alphaD, betaD);
        }

        private Vector3 CalculateTopLeft() {
            double t = mD * Math.Tan(P + (mUpsideDown ? -mAlpha : mBeta));
            Vector3 topLeft = new Vector3((float)mD, (float)(mWindow.Width / -2), (float)t);
            topLeft *= new Rotation(0.0, mOrientation.Yaw).Quaternion;
            topLeft += Origin;
            return topLeft;
        }

        private Vector3 CalculatePositionFromH() {
            double offsetH = mWindow.Height * mVOffset / mH;
            double offsetZ = offsetH * Math.Cos(P);
            if (!mUpsideDown) {
                offsetZ += mWindow.Height;
                offsetZ *= -1.0;
            }

            double angleT = !mUpsideDown ? P + mBeta : P - mAlpha;

            Vector3 line = new Vector3((float)mD, 0f, 0f);
            line *= new Rotation(0.0, mWindow.Orientation.Yaw).Quaternion;
            Vector3 target = mWindow.Centre;
            double z = mD * Math.Tan(angleT);
            target.Z = mWindow.TopLeft.Z - (float)z;
            return (target - line) - mProjectorPlugin.RoomPosition;
        }

        private void UpdateUnknowns(LockedVariable l) {
            switch (l) {
                case LockedVariable.Width:
                    mD = CalculateDFromW();
                    mWindow.Height = CalculateH();
                    mPosition = CalculatePositionFromH();
                    break;
                case LockedVariable.Height:
                    mD = CalculateDFromH();
                    mWindow.Width = CalculateW();
                    mPosition = CalculatePositionFromH();
                    break;
                case LockedVariable.Position:
                    mWindow.Width = CalculateW();
                    mWindow.Height = CalculateH();
                    mWindow.TopLeft = CalculateTopLeft();
                    break;
            }
        }

        private void ConfigureWindow() {
            double alpha = Math.Atan2(mVOffset, mThrowRatio);
            double beta = Math.Atan2(mVOffset + mH, mThrowRatio);


            double h = mTargetH;
            double angleT = !mUpsideDown ? P + beta : P - alpha;
            double angleB = !mUpsideDown ? P + alpha : P - beta;
            double tan = Math.Tan(angleT) - Math.Tan(angleB);
            if (mLockHeight) {
                mD = h / tan;

                double offsetH = h * mVOffset / mH;
                double offsetZ = offsetH * Math.Cos(P);
                if (!mUpsideDown) {
                    offsetZ += h;
                    offsetZ *= -1.0;
                }

                Vector3 line = new Vector3((float)mD, 0f, 0f);
                line *= new Rotation(0.0, mWindow.Orientation.Yaw).Quaternion;
                Vector3 target = mWindow.Centre;
                double z = mD * Math.Tan(angleT);
                target.Z = mWindow.TopLeft.Z - (float) z; 
                mPosition = (target - line) - mProjectorPlugin.RoomPosition;
            } else {
                mWindow.Height = mD * tan;

                double t = mD * Math.Tan(P + (mUpsideDown ? -alpha : beta));
                Vector3 topLeft = new Vector3((float)mD, (float)(mWindow.Width / -2), (float)t);
                topLeft *= new Rotation(0.0, mOrientation.Yaw).Quaternion;
                topLeft += Origin;
                mWindow.TopLeft = topLeft;
            }

            mWindow.Width = CalculateW();
        }

        private double mAlpha;
        private double mBeta;
        private double mGamma;

        private void ConfigureProjector() {
            double screenDistance = mLockHeight ?
                (mWindow.Height * mThrowRatio) / mH :
                (mWindow.Width * mThrowRatio) / mW;
            
            Vector3 line = mWindow.Orientation.LookAtVector * (float) screenDistance;
            Vector3 target = mWindow.Centre;
            target.Z = mWindow.TopLeft.Z;
            mPosition = (target - line) - mProjectorPlugin.RoomPosition;
            double offset = mUpsideDown ?
                mVOffset * mWindow.Width :
                (VOffset * -mWindow.Width) - (mWindow.Width * mH);
            mPosition.Z += (float)offset;
            mOrientation.Yaw = mWindow.Orientation.Yaw;
            mOrientation.Pitch = mWindow.Orientation.Pitch;
            mD = screenDistance;
        }

        private Vector3 GetProjectorCorner (float left, float up) {
            double m = ((mH / 2f) + mVOffset) * (mUpsideDown ? -1f : 1f);
            double z = m + ((mH / 2f) * up);
            Vector3 originalVector = new Vector3((float)mThrowRatio, (float)((mW / 2) * left), (float) z);
            originalVector *= (float) mD / originalVector.X;
            originalVector *= mOrientation.Quaternion;
            return originalVector + mPosition + mProjectorPlugin.RoomPosition;
        }

        public Vector3 Origin {
            get { return mPosition + mProjectorPlugin.RoomPosition; }
        }

        public void Draw(Graphics g, Func<Vector3, Point> to2D, Action redraw, Perspective perspective) {
            if (mDraw) {
                Func<Vector3, Point> to2DR = v => to2D(Origin + (v * new Rotation(0.0, mOrientation.Yaw).Quaternion));
                Point pos = to2DR(Vector3.Zero);

                double alpha = Math.Atan2(mVOffset, mThrowRatio);
                double beta = Math.Atan2(mVOffset + mH, mThrowRatio);
                double ceta = Math.Atan2(mW / 2, mThrowRatio);
                double range = 12500.0;

                double y = range * Math.Tan(ceta);
                double zt = range * Math.Tan(mUpsideDown ? -alpha : beta);
                double zb = range * Math.Tan(mUpsideDown ? -beta : alpha);

                Quaternion q = new Rotation(-mOrientation.Pitch, mOrientation.Yaw).Quaternion;
                Vector3 bse = new Vector3((float)range, 0f, 0f) * q;
                Vector3 tl = new Vector3((float)range, (float)-y, (float)zt) * q;
                Vector3 tr = new Vector3((float)range, (float)y, (float)zt) * q;
                Vector3 bl = new Vector3((float)range, (float)-y, (float)zb) * q;
                Vector3 br = new Vector3((float)range, (float)y, (float)zb) * q;

                g.DrawLine(Pens.Red, pos, to2DR(bse));
                g.DrawLine(Pens.Green, pos, to2DR(bl));
                g.DrawLine(Pens.Green, pos, to2DR(br));
                g.DrawLine(Pens.Blue, pos, to2DR(tl));
                g.DrawLine(Pens.Blue, pos, to2DR(tr));
                g.DrawLine(Pens.Violet, to2DR(new Vector3((float)mD, 0f, (float)-range)), to2DR(new Vector3((float)mD, 0f, (float)range)));

                double betaH = mD / Math.Cos(P + mBeta);
                double alphaH = mD / Math.Cos(P + mAlpha);
                double betaY = mD * Math.Tan(P + mBeta);
                double alphaY = mD * Math.Tan(P + mAlpha);
                //g.DrawLine(Pens.Purple, pos, to2DR(new Vector3((float)mD, 0f, (float) betaY)));
                //g.DrawLine(Pens.Purple, pos, to2DR(new Vector3((float)mD, 0f, (float) alphaY)));

                //g.DrawLine(Pens.Purple, pos, to2DR(new Vector3((float)betaH, 0f, 0f) * new Rotation(((P + mBeta) * -180.0) / Math.PI, 0.0).Quaternion));
                //g.DrawLine(Pens.Purple, pos, to2DR(new Vector3((float)alphaH, 0f, 0f) * new Rotation(((P + mAlpha) * -180.0) / Math.PI, 0.0).Quaternion));


                if (mDrawLabels) {
                    Vector3 toScreen = new Vector3((float) mD, 0f, 0f) * new Rotation(0.0, mOrientation.Yaw).Quaternion;
                    Vector3 floor = new Vector3(0f, 0f, mProjectorPlugin.RoomPosition.Z + mProjectorPlugin.Small.Z);
                    Vector3 clearance = new Vector3(0f, 0f, (float) Clearance);
                    Vector3 toClearance = floor + clearance;

                    Font font = SystemFonts.DefaultFont;

                    using (Pen p2 = new Pen(Color.Black, 2f)) {
                        g.DrawLine(p2, pos, to2D(mProjectorPlugin.RoomPosition + mPosition + toScreen));
                        g.DrawString(String.Format("To Screen: {0:.#}cm", D / 10f), font, Brushes.Black, to2D(mProjectorPlugin.RoomPosition + mPosition + (toScreen / 2f)));

                        if (perspective == Perspective.X || perspective == Perspective.Y) {
                            Vector3 toCeiling = new Vector3(0f, 0f, RelativePosition.Z);

                            g.DrawString(String.Format("Z: {0:.#}cm", RelativePosition.Z / 10f), font, Brushes.Black, to2D(mProjectorPlugin.RoomPosition + mPosition + (toCeiling / 2f)));
                            g.DrawLine(p2, pos, to2D(mProjectorPlugin.RoomPosition + mPosition + toCeiling));

                            g.DrawString(String.Format("Clearance: {0:.#}cm", Clearance / 10f), font, Brushes.Black, to2D(floor + (clearance / 2f)));
                            g.DrawLine(p2, to2D(floor), to2D(toClearance));
                        } if (perspective == Perspective.X || perspective == Perspective.Z) {
                            Vector3 toSide = new Vector3(0f, RelativePosition.Y, 0f);

                            g.DrawString(String.Format("Y: {0:.#}cm", RelativePosition.Y / 10f), font, Brushes.Black, to2D(mProjectorPlugin.RoomPosition + mPosition + (toSide / 2f)));
                            g.DrawLine(p2, pos, to2D(mProjectorPlugin.RoomPosition + mPosition + toSide));
                        } if (perspective == Perspective.Y || perspective == Perspective.Z) {
                            Vector3 toFar = new Vector3(RelativePosition.X, 0f, 0f);

                            g.DrawString(String.Format("X: {0:.#}cm", RelativePosition.X / 10f), font, Brushes.Black, to2D(mProjectorPlugin.RoomPosition + mPosition + (toFar / 2f)));
                            g.DrawLine(p2, pos, to2D(mProjectorPlugin.RoomPosition + mPosition + toFar));
                        }
                    }
                }
            }
        }

        private bool Contains(Vector3 p, Vector3[] wall) {
            return
                p.X >= wall[0].X && p.X <= wall[1].X &&
                p.Y >= wall[0].Y && p.Y <= wall[1].Y &&
                p.Z >= wall[0].Z && p.Z <= wall[2].Z;
        }
    }
}
