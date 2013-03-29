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
    public class OpacityFadeOutTransitionFactory : IWindowTransitionFactory {
        /// <summary>
        /// How long the transition should last.
        /// </summary>
        private double mLengthMS;

        public OpacityFadeOutTransitionFactory(double lengthMS) {
            mLengthMS = lengthMS;
        }

        public IWindowTransition Create(StateTransition transition, Window window) {
            return new OpacityFadeWindowTransition(transition, window, mLengthMS, false);
        }
    }public class OpacityFadeInTransitionFactory : IWindowTransitionFactory {
        /// <summary>
        /// How long the transition should last.
        /// </summary>
        private double mLengthMS;

        public OpacityFadeInTransitionFactory(double lengthMS) {
            mLengthMS = lengthMS;
        }

        public IWindowTransition Create(StateTransition transition, Window window) {
            return new OpacityFadeWindowTransition(transition, window, mLengthMS, true);
        }
    }
    public class OpacityFadeWindowTransition : WindowTransition {
        /// <summary>
        /// When the transition began.
        /// </summary>
        private DateTime mTransitionStart;
        /// <summary>
        /// How long the transition should last.
        /// </summary>
        private double mLengthMS;
        /// <summary>
        /// Whethe transition is currently occurring.
        /// </summary>
        private bool mTransitioning;
        /// <summary>
        /// If true then the fade will go from invisible visible. Otherwise it will go from visible to invisible.
        /// </summary>
        private bool mFadeIn;

        /// <summary>
        /// Initialise the fade transition, specifying how long the fade should last, in ms.
        /// </summary>
        /// <param name="transition">The transition this fade is part of.</param>
        /// <param name="window">The window this fade is to be drawn on.</param>
        /// <param name="lengthMS">The length of time, in ms, the fade should last.</param>
        public OpacityFadeWindowTransition(StateTransition transition, Window window, double lengthMS, bool fadeIn)
            : base(transition, window) {
            mLengthMS = lengthMS;
            mFadeIn = fadeIn;
        }

        #region IWindowTransition Members

        public override event Action<IWindowTransition> Finished;

        public override void Begin() {
            mTransitionStart = DateTime.Now;
            mTransitioning = true;
        }

        public override void Cancel() {
            mTransitioning = false;
        }

        public override bool NeedsRedrawn {
            get { return true; }
        }

        public override void RedrawStatic(Rectangle clip, Graphics graphics) {
            if (mFadeIn)
                To.RedrawStatic(clip, graphics);
            else
                From.RedrawStatic(clip, graphics);
        }

        public override void DrawDynamic(System.Drawing.Graphics graphics) {
            if (!mTransitioning)
                return;

            double time = DateTime.Now.Subtract(mTransitionStart).TotalMilliseconds;
            if (time > mLengthMS) {
                mTransitioning = false;
                if (Finished != null)
                    Finished(this);
            }
            else
                Manager.Opacity = (mFadeIn ? 0.0 : 1.0) - time / mLengthMS;
        }

        #endregion
    }
}
