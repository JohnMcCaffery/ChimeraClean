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
using System.Drawing;
using Chimera.Interfaces.Overlay;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Xml;
using Chimera.Overlay.Transitions;
using System.Threading;


namespace Chimera.Overlay.Transitions {
    public class FeatureFadeFactory : XmlLoader, IFeatureTransitionFactory {
        #region IImageTransitionFactory Members

        public string Name { get { return "Fade"; } }

        public IFeatureTransition Create(OverlayPlugin manager, XmlNode node) {
            return new FadeTransition(manager, node);
        }

        public IFeatureTransition Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }

        public IFeatureTransition Create(double length) {
            return new FadeTransition(length);
        }

        #endregion
    }

    public class FadeTransition : XmlLoader, IFeatureTransition {
        /// <summary>
        /// When the transition began.
        /// </summary>
        private DateTime mTransitionStart;
        /// <summary>
        /// How long the transition should last.
        /// </summary>
        private double mLengthMS;
        /// <summary>
        /// The image for the background of the state that is fading out.
        /// </summary>
        private Bitmap mStartImg;
        /// <summary>
        /// The image for the background of the state that is fading out.
        /// </summary>
        private Bitmap mFinishImg;
        /// <summary>
        /// The feature the fade starts at.
        /// </summary>
        private IFeature mStart;
        /// <summary>
        /// The feature the fade goes to.
        /// </summary>
        private IFeature mFinish;
        /// <summary>
        /// Whether this transition is active as a drawable.
        /// </summary>
        private bool mActive = false;
        /// <summary>
        /// Whether the transition has completed.
        /// </summary>
        private bool mCompleted;
        /// <summary>
        /// The clip rectangle bounding the area this item will be drawn to.
        /// </summary>
        private Rectangle mClip;
        /// <summary>
        /// Whether to pre load the fade images on activate.
        /// </summary>
        private bool mPreload;
        /// <summary>
        /// How many steps to create per second if preloading steps.
        /// </summary>
        private double mStepsPerS = 15.0;
        /// <summary>
        /// The preloaded steps.
        /// </summary>
        private Bitmap[] mSteps;
 


        public event Action Finished;

        public FadeTransition(double lengthMS) {
            mLengthMS = lengthMS;
        }

        public FadeTransition(double lengthMS, IFeature start, IFeature finish) {
            mLengthMS = lengthMS;
            mStart = start;
            mFinish = finish;
        }

        public FadeTransition(OverlayPlugin manager, XmlNode node) {
            mLengthMS = GetDouble(node, 2000, "Length");
            mStart = manager.GetFeature(node.SelectSingleNode("child::Start"), "Fade Feature", null);
            mFinish = manager.GetFeature(node.SelectSingleNode("child::Finish"), "Fade Feature", null);
            if (mStart == null && mFinish == null)
                throw new Exception("No from or to features specified.");
            if (mStart == null)
                throw new Exception("No from feature specified.");
            if (mFinish == null)
                throw new Exception("No to feature specified.");

            mPreload = GetBool(node, mPreload, "Preload");
        }

        public FadeTransition(OverlayPlugin manager, XmlNode node, bool preload)
            : this(manager, node) {
            mPreload = preload;
        }

        public void Begin() {
            mCompleted = false;
            mTransitionStart = DateTime.Now;
        }

        public virtual bool Active {
            get { return mActive; }
            set {
                if (value != mActive) {
                    mActive = value;
                    mStart.Active = value;
                    mFinish.Active = value;
                    if (value)
                        Init();
                    else 
                        Dispose();
                }
            }
        }

        #region IFeatureTransition Members

        public IFeature Start {
            get { return mStart; }
            set { mStart = value; }
        }

        public IFeature Finish {
            get { return mFinish; }
            set { mFinish = value; }
        }

        public void Cancel() {
            mCompleted = true;
        }

        #endregion

        public virtual Rectangle Clip {
            get { return mClip; }
            set { 
                mClip = value;
                mStart.Clip = value;
                mFinish.Clip = value;
                if (mActive)
                    Init();
            }
        }


        #region IDrawable Members

        public bool NeedsRedrawn {
            get { return !mCompleted; }
        }

        public string Frame {
            get { return mStart.Frame; }
        }

        public void DrawStatic(Graphics graphics) {
            Console.WriteLine("Drawing static fade");
            if (mFinishImg != null)
                graphics.DrawImage(mFinishImg, 0, 0);
        }

        public void DrawDynamic(System.Drawing.Graphics graphics) {
            Console.WriteLine("Drawing fade");
            double time = DateTime.Now.Subtract(mTransitionStart).TotalMilliseconds;
            if (time > mLengthMS && !mCompleted) {
                mCompleted = true;
                if (Finished != null)
                    Finished();
            }

            else if (!mCompleted) {
                if (mPreload) {
                    int i = (int)(time / mStepsPerS);
                    graphics.DrawImage(mSteps[i], 0, 0);
                } else {
                    using (Bitmap step = CreateStep(time))
                        graphics.DrawImage(step, 0, 0);
                }
            }
        }

        #endregion
        private void Init() {
            if (mClip.Width == 0 || mClip.Height == 0)
                return;

            if (mStartImg != null)
                Dispose();

            Console.WriteLine("Initialising fade");

            mStartImg = new Bitmap(mClip.Width, mClip.Height);
            mFinishImg = new Bitmap(mClip.Width, mClip.Height);
            using (Graphics g = Graphics.FromImage(mStartImg))
                Start.DrawStatic(g);
            using (Graphics g = Graphics.FromImage(mFinishImg))
                Finish.DrawStatic(g);

            if (mPreload) {
                Thread t = new Thread(Preload);
                t.Priority = ThreadPriority.BelowNormal;
                t.Name = "FadePreload";
                t.Start();
            }
        }

        private void Dispose() {
            if (mStartImg == null)
                return;

            Console.WriteLine("Disposing of fade");

            mStartImg.Dispose();
            mFinishImg.Dispose();
            mStartImg = null;
            mFinishImg = null;

            if (mPreload)
                foreach (var step in mSteps)
                    step.Dispose();
        }

        private void Preload() {
            Console.WriteLine("Preloading fade");

            double stepLength = mStepsPerS / 1000.0;
            int steps = (int)(mLengthMS / stepLength);
            mSteps = new Bitmap[steps];
            for (int i = 0; i < steps; i++)
                mSteps[i] = CreateStep(i * stepLength);
        }

        private Bitmap CreateStep(double time) {
            Console.WriteLine("Fade creating step");

            Bitmap image = new Bitmap(mStartImg);
            Rectangle affectedRect = new Rectangle(0, 0, image.Width, image.Height);
            BitmapData dat = image.LockBits(affectedRect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            byte[] argbValues = new byte[image.Height * dat.Stride];

            Marshal.Copy(dat.Scan0, argbValues, 0, argbValues.Length);

            byte a = (byte)((double)byte.MaxValue * (1.0 - (time / mLengthMS)));

            for (int i = 3; i < argbValues.Length; i += 4)
                argbValues[i] = a;

            Marshal.Copy(argbValues, 0, dat.Scan0, argbValues.Length);
            image.UnlockBits(dat);
            return image;
        }
    }
}
