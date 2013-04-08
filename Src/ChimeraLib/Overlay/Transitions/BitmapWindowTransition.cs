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
        }

        private void transitionEffect_Finished() {
            if (Finished != null)
                Finished(this);
        }

        #region IWindowTransition Members

        public override event Action<IWindowTransition> Finished;

        public override void Begin() { }

        public override void Cancel() {
            throw new NotImplementedException();
        }

        public override bool NeedsRedrawn {
            get { return true; }
        }

        public override void RedrawStatic(Rectangle clip, Graphics graphics) {
            To.RedrawStatic(clip, graphics);

            if (!clip.Width.Equals(mLastClip.Width) || !clip.Height.Equals(mLastClip.Height)) {
                mLastClip = clip;
                Bitmap from = new Bitmap(clip.Width, clip.Height);
                Bitmap to = new Bitmap(clip.Width, clip.Height);
                using (Graphics g = Graphics.FromImage(from))
                    From.RedrawStatic(clip, g);
                using (Graphics g = Graphics.FromImage(to))
                    To.RedrawStatic(clip, g);

                mTransition.Init(from, to);
            }

            Begin();
        }

        public override void DrawDynamic(Graphics graphics) {
            mTransition.DrawDynamic(graphics);
        }

        #endregion
    }
}
