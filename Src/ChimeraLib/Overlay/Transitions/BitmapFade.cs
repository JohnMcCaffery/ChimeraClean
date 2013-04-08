using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Chimera.Interfaces.Overlay;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Chimera.Overlay.Transitions {
    public class FadeFactory : IImageTransitionFactory {
        #region IImageTransitionFactory Members

        public IImageTransition Create(double length) {
            return new FadeTransition(length);
        }

        #endregion
    }
    public class FadeTransition : IImageTransition {
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
        private Bitmap mFrom;
        /// <summary>
        /// The image for the background of the state that is fading out.
        /// </summary>
        private Bitmap mTo;
        /// <summary>
        /// The steps that can be used to render the from image fading into the back image.
        /// </summary>
        private Bitmap[] mStepImages;
        /// <summary>
        /// Whether this transition is active as a drawable.
        /// </summary>
        private bool mActive;
        /// <summary>
        /// Whether the transition has completed.
        /// </summary>
        private bool mCompleted;


        public event Action Finished;

        public FadeTransition(double lengthMS) {
            mLengthMS = lengthMS;
        }

        public FadeTransition(double lengthMS, Bitmap from, Bitmap to) {
            mLengthMS = lengthMS;
            Init(from, to);
        }

        public void Begin() {
            mCompleted = false;
            mTransitionStart = DateTime.Now;
        }

        public void Init(Bitmap from, Bitmap to) {
            if (mFrom != null)
                mFrom.Dispose();
            if (mTo != null)
                mTo.Dispose();

            mFrom = from;
            mTo = to;
            mStepImages = new Bitmap[(int)(mLengthMS / 20)];
        }

        #region IImageTransition Members

        #endregion

        #region IDrawable Members

        public bool Active {
            get { return mActive; }
            set { mActive = value; }
        }

        public bool NeedsRedrawn {
            get { return !mCompleted; }
        }

        public string Window {
            get { throw new NotImplementedException(); }
        }

        public void RedrawStatic(Rectangle clip, Graphics graphics) {
            graphics.DrawImage(mTo, 0, 0);
        }

        public void DrawDynamic(System.Drawing.Graphics graphics) {
            double time = DateTime.Now.Subtract(mTransitionStart).TotalMilliseconds;
            if (time > mLengthMS) {
                mCompleted = true;
                if (Finished != null)
                    Finished();
            }

            else if (mFrom != null) {
                DateTime start = DateTime.Now;

                int i = (int) (time / 20.0);
                if (mStepImages[i] == null) {
                    mStepImages[i] = CreateStep(time);
                    graphics.DrawImage(mStepImages[i], 0, 0);
                } else
                    graphics.DrawImage(mStepImages[i], 0, 0);
            }
        }

        #endregion

        private Bitmap CreateStep(double time) {
            Bitmap image = new Bitmap(mFrom);
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
