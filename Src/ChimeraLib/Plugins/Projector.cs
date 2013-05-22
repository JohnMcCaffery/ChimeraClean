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

        public double ScreenDistance {
            get { return mScreenDistance; }
            set {
                mScreenDistance = value;
                Redraw();
            }
        }

        public double Clearance {
            get {
                Vector3 bottomCentre = GetCorner(0f, -1f);
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
            mScreenDistance = cfg.WallDistance;
            mOrientation = new Rotation(cfg.ProjectorPitch, cfg.ProjectorYaw);
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
            if (mLockHeight)
                CalculateDistanceFromHeight();

            mWindow.Orientation.Quaternion = new Rotation(0.0, mOrientation.Yaw).Quaternion;
            Vector3 topLeft = TopLeft;
            Vector3 topRight = TopRight;
            //mWindow.Width = Vector3.Mag(new Vector3(topLeft.X, topLeft.Y, 0f) - new Vector3(topRight.X, topRight.Y, 0f));
            //mWindow.Height = TopLeft.Z - BottomLeft.Z;
            double p = mOrientation.Pitch * (Math.PI / 180.0);
            double h = (mScreenDistance * mH) / mThrowRatio;
            double a = Math.Cos(mAlpha) * h;
            double H = a / Math.Cos(mAlpha + p);
            mWindow.Height = H;
            mWindow.Width = (mW * H) / mH;
            mWindow.TopLeft = topLeft;
        }

        private double mAlpha;

        private void CalculateDistanceFromHeight() {
            double p = mOrientation.Pitch * (Math.PI / 180.0);
            double h = (mTargetH * Math.Cos(p + mAlpha)) / Math.Cos(mAlpha);
            mScreenDistance = (h * mThrowRatio) / mH;
        }

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
            mScreenDistance = screenDistance * Math.Cos(mOrientation.Pitch * Math.PI / 180.0);
        }

        private Vector3 TopLeft {
            get { return GetCorner(-1f, 1f); }
        }
        private Vector3 TopRight {
            get { return GetCorner(1f, 1f); }
        }
        private Vector3 BottomLeft {
            get { return GetCorner(-1f, -1f); }
        }
        private Vector3 BottomRight {
            get { return GetCorner(1f, -1f); }
        }

        private Vector3 GetCorner(float left, float up) {
            double m = ((mH / 2f) + mVOffset) * (mUpsideDown ? -1f : 1f);
            double z = m + ((mH / 2f) * up);
            Vector3 originalVector = new Vector3((float)mThrowRatio, (float)((mW / 2) * left), (float) z);
            originalVector *= (float) mScreenDistance / originalVector.X;
            originalVector *= new Rotation(mOrientation.Pitch, 0.0).Quaternion;
            float xDiff = (float)(ScreenDistance - originalVector.X);
            float scale = xDiff / originalVector.X;

            Vector3 targetVector = originalVector + (originalVector * scale);
            targetVector *= new Rotation(0.0, mOrientation.Yaw).Quaternion;
            return targetVector + mPosition + mProjectorPlugin.RoomPosition;
        }

        private Vector3 GetProjectorCorner (float left, float up) {
            double m = ((mH / 2f) + mVOffset) * (mUpsideDown ? -1f : 1f);
            double z = m + ((mH / 2f) * up);
            Vector3 originalVector = new Vector3((float)mThrowRatio, (float)((mW / 2) * left), (float) z);
            originalVector *= (float) mScreenDistance / originalVector.X;
            originalVector *= mOrientation.Quaternion;
            return originalVector + mPosition + mProjectorPlugin.RoomPosition;
        }

        public void Draw(Graphics g, Func<Vector3, Point> to2D, Action redraw, Perspective perspective) {
            if (mDraw) {
                Point pos = to2D(mPosition + mProjectorPlugin.RoomPosition);
                g.DrawLine(Pens.Black, pos, to2D(TopLeft));
                g.DrawLine(Pens.Black, pos, to2D(TopRight));
                g.DrawLine(Pens.Black, pos, to2D(BottomLeft));
                g.DrawLine(Pens.Black, pos, to2D(BottomRight));

                if (perspective == Perspective.X || perspective == Perspective.Y) {
                    Vector3 tl = GetProjectorCorner(-1f, 1f);
                    Vector3 tr = GetProjectorCorner(1f, 1f);
                    Vector3 bl = GetProjectorCorner(-1f, -1f);
                    Vector3 br = GetProjectorCorner(1f, -1f);

                    g.DrawPolygon(Pens.Red, new Point[] { 
                        to2D(tl), 
                        to2D(tr),
                        to2D(br),
                        to2D(bl),
                        to2D(tl) 
                    });

                    Vector3 b = new Vector3((float) mScreenDistance, 0f, 0f) * mOrientation.Quaternion;
                    b += mPosition + mProjectorPlugin.RoomPosition;
                    g.DrawLine(Pens.Green, pos, to2D(b));
                }

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
