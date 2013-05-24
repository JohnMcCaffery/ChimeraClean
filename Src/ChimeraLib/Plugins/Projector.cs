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
        private bool mAutoUpdate;
        private bool mConfigureProjector;
        private bool mUpsideDown;
        private double mScreenDistance;
        private double mVOffset;
        private bool mRedrawing;

        private ProjectorPlugin mProjectorPlugin;

        public event Action Change;

        public Window Window { get { return mWindow; } }

        public bool ConfigureFromProjector {
            get { return mConfigureProjector; }
            set { 
                mConfigureProjector = value;
                Redraw();
            }
        }

        public Vector3 Position {
            get { return mPosition; }
            set { 
                mPosition = value;
                Redraw();
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

        public bool LockHeight {
            get { return mLockHeight; }
            set {
                if (value != mLockHeight) {
                    mLockHeight = value;
                    if (value)
                        mTargetH = mWindow.Height;
                }
            }
        }

        public bool AutoUpdate {
            get { return mAutoUpdate; }
            set { 
                mAutoUpdate = value;
                if (value)
                    Configure();
            }
        }

        public bool DrawDiagram {
            get { return mDraw; }
            set {
                mDraw = value;
                Redraw();
            }
        }

        public bool DrawLabels {
            get { return mDrawLabels; }
            set {
                mDrawLabels = value;
                Redraw();
            }
        }

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
                Redraw();
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

        public bool UpsideDown {
            get { return mUpsideDown; }
            set {
                mUpsideDown = value;
                Redraw();
            }
        }

        public double VOffset {
            get { return mVOffset; }
            set {
                mVOffset = value;
                mAlpha = Math.Atan2(value, mThrowRatio);
                Redraw();
            }
        }

        public double ThrowRatio {
            get { return mThrowRatio; }
            set {
                mThrowRatio = value;
                mAlpha = Math.Atan2(mVOffset, value);
                Redraw();
            }
        }

        /// <summary>
        /// Pitch angle, in radians.
        /// </summary>
        private double P {
            get { return mOrientation.Pitch * (Math.PI / 180.0); }
        }

        public double ScreenDistance {
            get { return mScreenDistance; }
            set {
                mScreenDistance = value;
                Redraw();
            }
        }

        public double Clearance {
            get {
                double alpha = Math.Atan2(mVOffset, mThrowRatio);
                double beta = Math.Atan2(mVOffset + mH, mThrowRatio);

                double flip = mUpsideDown ? -1.0 : 1.0;

                double h = mTargetH;
                double d = mScreenDistance;
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

        public Projector(Window window, ProjectorPlugin projectorPlugin) {
            mProjectorPlugin = projectorPlugin;
            mWindow = window;
            mTargetH = window.Height;

            ProjectorConfig cfg = new ProjectorConfig(window.Name);
            AspectRatio = cfg.AspectRatio;
            NativeAspectRatio = cfg.NativeAspectRatio;
            mThrowRatio = cfg.ThrowRatio;
            mPosition = cfg.ProjectorPosition;
            mOrientation = new Rotation(cfg.ProjectorPitch, cfg.ProjectorYaw);
            ScreenDistance = cfg.WallDistance;
            mConfigureProjector = !cfg.ConfigureWindow;
            mUpsideDown = cfg.UpsideDown;
            mVOffset = cfg.VOffset;
            mLockHeight = cfg.LockHeight;
            mAlpha = Math.Atan2(mVOffset, mThrowRatio);

            mDraw = cfg.Draw;
            mDrawLabels = cfg.DrawLabels;
            mAutoUpdate = cfg.AutoUpdate;

            mProjectorPlugin.RoomChanged += new Action(mProjectorPlugin_RoomChanged);
            mOrientation.Changed += new EventHandler(mOrientation_Changed);
            window.Changed += new Action<Window,EventArgs>(window_Changed);

            if (mAutoUpdate)
                Configure();
        }

        void mProjectorPlugin_RoomChanged() {
            if (mAutoUpdate && !mConfigureProjector)
                ConfigureWindow();
        }

        void window_Changed(Window w, EventArgs args) {
            //if (mConfigureProjector || (mLockHeight && mTargetH != w.Height)) {
            if (mConfigureProjector) {
                mTargetH = w.Height;
                if (mAutoUpdate)
                    Redraw();
            }
        }

        void mOrientation_Changed(object sender, EventArgs e) {
            Redraw();
        }

        internal void Redraw() {
            if (!mRedrawing && mAutoUpdate && mWindow != null) {
                mRedrawing = true;
                Configure();
                mRedrawing = false;
            }

            mProjectorPlugin.Redraw();

            if (Change != null)
                Change();
        }

        public void Configure() {
            if (mConfigureProjector)
                ConfigureProjector();
            else
                ConfigureWindow();
            Redraw();
        }

        private void ConfigureWindow() {
            double alpha = Math.Atan2(mVOffset, mThrowRatio);
            double beta = Math.Atan2(mVOffset + mH, mThrowRatio);
            double ceta = Math.Atan2(mW / 2, mThrowRatio);

            double flip = mUpsideDown ? -1.0 : 1.0;

            double h = mTargetH;
            double d = mScreenDistance;
            double angleT = mUpsideDown ? P - alpha : P + beta;
            double angleB = mUpsideDown ? P - beta : P + alpha;
            double tan = Math.Tan(angleT) - Math.Tan(angleB);
            if (mLockHeight)
                d = h / tan;
            else
                h = d * tan;

            double t = d * Math.Tan(P + (mUpsideDown ?  -alpha : beta));
            double w = d * Math.Tan(ceta) * 2;
            Vector3 topLeft = new Vector3((float) d, (float) (w / -2), (float) t);
            topLeft *= new Rotation(0.0, mOrientation.Yaw).Quaternion;
            topLeft += Origin;

            mWindow.Height = h;
            mWindow.Width = w;
            mWindow.TopLeft = topLeft;
                

            //double b = t - (h * flip);
            /*
            if (mLockHeight)
                CalculateDistanceFromHeight();

            mWindow.Orientation.Quaternion = new Rotation(0.0, mOrientation.Yaw).Quaternion;
            Vector3 topLeft = TopLeft;
            Vector3 topRight = TopRight;
            mWindow.Width = Vector3.Mag(new Vector3(topLeft.X, topLeft.Y, 0f) - new Vector3(topRight.X, topRight.Y, 0f));
            mWindow.Height = TopLeft.Z - BottomLeft.Z;
            mWindow.TopLeft = topLeft;
            */
        }

        private double mAlpha;

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
            mScreenDistance = screenDistance;
        }

        private Vector3 GetProjectorCorner (float left, float up) {
            double m = ((mH / 2f) + mVOffset) * (mUpsideDown ? -1f : 1f);
            double z = m + ((mH / 2f) * up);
            Vector3 originalVector = new Vector3((float)mThrowRatio, (float)((mW / 2) * left), (float) z);
            originalVector *= (float) mScreenDistance / originalVector.X;
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
                float y = Origin.Y;

                double alpha = Math.Atan2(mVOffset, mThrowRatio);
                double beta = Math.Atan2(mVOffset + mH, mThrowRatio);
                double ceta = Math.Atan2(mW / 2, mThrowRatio);

                double alphaDeg = (180.0 / Math.PI) * alpha;
                double betaDeg = (180.0 / Math.PI) * beta;

                double range = 12500.0;
                double bottomH = range * Math.Tan(alpha);
                double topH = range * Math.Tan(beta);

                double flip = mUpsideDown ? -1.0 : 1.0;

                double baseZ = range * Math.Sin(P);
                double bottomZ = baseZ + (bottomH * Math.Cos(P) * flip);
                double topZ = baseZ + (topH * Math.Cos(P) * flip);

                double baseX = range * Math.Cos(P);
                double bottomX = baseX - (bottomH * Math.Sin(P));
                double topX = baseX - (topH * Math.Sin(P));

                double bottomY = bottomX * Math.Tan(ceta);
                double topY = topX * Math.Tan(ceta);

                double pureBottomY = range * Math.Tan(alpha + P);
                double pureTopY = range * Math.Tan(beta + P);

                /*
                double h = mTargetH;
                double d = mScreenDistance;
                if (mLockHeight)
                    d = h / (Math.Tan(P + beta) - Math.Tan(P + alpha));
                else
                    h = d * (Math.Tan(beta + P) - Math.Tan(alpha + P));

                double t = d * Math.Tan(P + (beta * flip));
                */

                g.DrawLine(Pens.Red, pos, to2DR(new Vector3((float) baseX, y, (float) baseZ)));
                g.DrawLine(Pens.Green, pos, to2DR(new Vector3((float) bottomX, (float) -bottomY, (float) bottomZ)));
                g.DrawLine(Pens.Green, pos, to2DR(new Vector3((float) bottomX, (float) bottomY, (float) bottomZ)));
                g.DrawLine(Pens.Blue, pos, to2DR(new Vector3((float) topX, (float) -topY, (float) topZ)));
                g.DrawLine(Pens.Blue, pos, to2DR(new Vector3((float) topX, (float) topY, (float) topZ)));
                g.DrawLine(Pens.Violet, to2DR(new Vector3((float)mScreenDistance, 0f, (float) -range)), to2DR(new Vector3((float)mScreenDistance, 0f, (float)range)));

                /*
                using (Pen p = new Pen(Color.Red, 3f))
                    g.DrawLine(p, to2DR(new Vector3((float) d, 0f, (float) t)), to2DR(new Vector3((float) d, 0f, (float) (t - h))));
                */

                if (mDrawLabels) {
                    Vector3 toScreen = new Vector3((float) mScreenDistance, 0f, 0f) * new Rotation(0.0, mOrientation.Yaw).Quaternion;
                    Vector3 floor = new Vector3(0f, 0f, mProjectorPlugin.RoomPosition.Z + mProjectorPlugin.Small.Z);
                    Vector3 clearance = new Vector3(0f, 0f, (float) Clearance);
                    Vector3 toClearance = floor + clearance;

                    Font font = SystemFonts.DefaultFont;

                    using (Pen p2 = new Pen(Color.Black, 2f)) {
                        g.DrawLine(p2, pos, to2D(mProjectorPlugin.RoomPosition + mPosition + toScreen));
                        g.DrawString(String.Format("To Screen: {0:.#}cm", ScreenDistance / 10f), font, Brushes.Black, to2D(mProjectorPlugin.RoomPosition + mPosition + (toScreen / 2f)));

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
