using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Chimera.Interfaces.Overlay;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using Chimera.Overlay;

namespace Chimera.Overlay.Transitions {
    public class BitmapFadeTransitionFactory : IWindowTransitionFactory {
        /// <summary>
        /// How long the transition should last.
        /// </summary>
        private double mLengthMS;

        public BitmapFadeTransitionFactory(double lengthMS) {
            mLengthMS = lengthMS;
        }

        public IWindowTransition Create(StateTransition transition, Window window) {
            return new BitmapFadeWindowTransition(transition, window, mLengthMS);
        }
    }
    public class BitmapFadeWindowTransition : WindowTransition {
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
        private Bitmap mFromBG;
        /// <summary>
        /// The steps that can be used to render the from image fading into the back image.
        /// </summary>
        private Bitmap[] mStepImages;
        /// <summary>
        /// The last clip rectangle the images were redrawn for. Used to check if the images need to be recalculated.
        /// </summary>
        private Rectangle mLastClip;

        /// <summary>
        /// Initialise the fade transition, specifying how long the fade should last, in ms.
        /// </summary>
        /// <param name="transition">The transition this fade is part of.</param>
        /// <param name="window">The window this fade is to be drawn on.</param>
        /// <param name="lengthMS">The length of time, in ms, the fade should last.</param>
        public BitmapFadeWindowTransition(StateTransition transition, Window window, double lengthMS)
            : base(transition, window) {
            mLengthMS = lengthMS;

            mStepImages = new Bitmap[(int)(lengthMS / 20)];
        }

        #region IWindowTransition Members

        public override event Action<IWindowTransition> Finished;

        public override void Begin() {
            mTransitionStart = DateTime.Now;
            mStepCount = 0;
        }

        public override void Cancel() {
            throw new NotImplementedException();
        }

        public override bool NeedsRedrawn {
            get { return true; }
        }

        public override void RedrawStatic(Rectangle clip, Graphics graphics) {
            To.RedrawStatic(clip, graphics);

            if (!clip.Equals(mLastClip)) {
                mFromBG = new Bitmap(clip.Width, clip.Height);
                using (Graphics g = Graphics.FromImage(mFromBG))
                    From.RedrawStatic(clip, g);
                mLastClip = clip;
                mStepImages = new Bitmap[(int)(mLengthMS / 20)];
            }
        }

        private Bitmap CreateStep(double time) {
            Bitmap image = new Bitmap(mFromBG);
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

        private int mStepCount;

        public override void DrawDynamic(System.Drawing.Graphics graphics) {
            double time = DateTime.Now.Subtract(mTransitionStart).TotalMilliseconds;
            if (time > mLengthMS) {
                Console.WriteLine("Transition completed in {0} steps.", mStepCount);
                //mFromBG = null;
                if (Finished != null)
                    Finished(this);
            }
            else if (mFromBG != null) {
                mStepCount++;

                DateTime start = DateTime.Now;

                int i = (int) (time / 20.0);
                if (mStepImages[i] == null) {
                    mStepImages[i] = CreateStep(time);
                    graphics.DrawImage(mStepImages[i], 0, 0);
                } else
                    graphics.DrawImage(mStepImages[i], 0, 0);
                Console.WriteLine("Step {0:000} - Opacity: {1:000} - Took: {2}ms", mStepCount, mStepImages[(int) (time / 20.0)].GetPixel(0, 0).A, DateTime.Now.Subtract(start).TotalMilliseconds);
            }
        }

        #endregion
    }
}
