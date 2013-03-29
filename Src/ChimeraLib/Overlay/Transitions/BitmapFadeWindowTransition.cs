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
    public class BitmapFadeWindowTransition : WindowTransition {        /// <summary>
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
        /// Initialise the fade transition, specifying how long the fade should last, in ms.
        /// </summary>
        /// <param name="transition">The transition this fade is part of.</param>
        /// <param name="window">The window this fade is to be drawn on.</param>
        /// <param name="lengthMS">The length of time, in ms, the fade should last.</param>
        public BitmapFadeWindowTransition(StateTransition transition, Window window, double lengthMS)
            : base(transition, window) {
            mLengthMS = lengthMS;
        }

        #region IWindowTransition Members

        public override event Action<IWindowTransition> Finished;

        public override void Begin() {
            mTransitionStart = DateTime.Now;
        }

        public override void Cancel() {
            throw new NotImplementedException();
        }

        public override bool NeedsRedrawn {
            get { return true; }
        }

        public override void RedrawStatic(Rectangle clip, Graphics graphics) {
            To.RedrawStatic(clip, graphics);
            mFromBG = new Bitmap(clip.Width, clip.Height);
            using (Graphics g = Graphics.FromImage(mFromBG))
                From.RedrawStatic(clip, g);
        }

        public override void DrawDynamic(System.Drawing.Graphics graphics) {
            double time = DateTime.Now.Subtract(mTransitionStart).TotalMilliseconds;
            if (time > mLengthMS) {
                mFromBG = null;
                if (Finished != null)
                    Finished(this);
            }
            else if (mFromBG != null) {
                Rectangle affectedRect = new Rectangle(0, 0, mFromBG.Width, mFromBG.Height);
                BitmapData dat = mFromBG.LockBits(affectedRect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                byte[] argbValues = new byte[mFromBG.Height * dat.Stride];

                Marshal.Copy(dat.Scan0, argbValues, 0, argbValues.Length);

                byte a = (byte) ((double) byte.MaxValue * (1.0 - (time / mLengthMS)));
                for (int i = 3; i < argbValues.Length; i += 4)
                    argbValues[i] = a;

                Marshal.Copy(argbValues, 0, dat.Scan0, argbValues.Length);
                mFromBG.UnlockBits(dat);

                graphics.DrawImage(mFromBG, 0, 0);
            }
        }

        #endregion
    }
}
