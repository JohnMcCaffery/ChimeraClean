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
        /// <summary>
        /// The factory used to create the transitions which will be rendered.
        /// </summary>
        private IImageTransitionFactory mFactory;

        public BitmapFadeTransitionFactory(IImageTransitionFactory factory, double lengthMS) {
            mFactory = factory;
            mLengthMS = lengthMS;
        }

        public IWindowTransition Create(StateTransition transition, Window window) {
            return new BitmapWindowTransition(transition, window, mFactory.Create(mLengthMS));
        }
    }
    public class BitmapWindowTransition : WindowTransition {
        /// <summary>
        /// The last clip rectangle the images were redrawn for. Used to check if the images need to be recalculated.
        /// </summary>
        private Rectangle mLastClip;
        /// <summary>
        /// The object which will handle the actual transition.
        /// </summary>
        private IImageTransition mTransition;
        private bool mBegun;


        /// <summary>
        /// Initialise the fade transition, specifying how long the fade should last, in ms.
        /// </summary>
        /// <param name="transition">The transition this fade is part of.</param>
        /// <param name="window">The window this fade is to be drawn on.</param>
        /// <param name="lengthMS">The length of time, in ms, the fade should last.</param>
        public BitmapWindowTransition(StateTransition transition, Window window, IImageTransition transitionEffect)
            : base(transition, window) {

            mTransition = transitionEffect;
            transitionEffect.Finished += new Action(transitionEffect_Finished);

            AddFeature(mTransition);
        }

        private void transitionEffect_Finished() {
            if (Finished != null)
                Finished(this);
        }

        #region IWindowTransition Members

        public override event Action<IWindowTransition> Finished;

        public override void Begin() {
            base.Begin();
            mBegun = false;
        }

        public override void Cancel() {
            throw new NotImplementedException();
        }

        public override bool NeedsRedrawn {
            get { return true; }
        }

        public override Rectangle Clip {
            get { return base.Clip; }
            set {
                base.Clip = value;
                Bitmap from = new Bitmap(Clip.Width, Clip.Height);
                Bitmap to = new Bitmap(Clip.Width, Clip.Height);
                From.Clip = value;
                To.Clip = value;
                using (Graphics g = Graphics.FromImage(from))
                    From.DrawStatic(g);
                using (Graphics g = Graphics.FromImage(to))
                    To.DrawStatic(g);

                mTransition.Init(from, to);
            }
        }

        public override void DrawStatic(Graphics graphics) {
            base.DrawStatic(graphics);
            if (!mBegun) {
                mBegun = true;
                mTransition.Begin();
            }
        }

        #endregion
    }
}
