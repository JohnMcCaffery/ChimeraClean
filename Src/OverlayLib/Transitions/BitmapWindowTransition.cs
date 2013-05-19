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
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using Chimera.Overlay;
using System.Xml;

namespace Chimera.Overlay.Transitions {
    public class BitmapTransitionFactory : XmlLoader, ITransitionStyleFactory {
        public string Name {
            get { return "BitmapTransition"; }
        }

        public IWindowTransitionFactory Create(OverlayPlugin manager, XmlNode node) {
            Console.WriteLine("Creating Bitmap Window Transition");
            double length = GetDouble(node, 5000.0, "Length");
            IImageTransitionFactory transition = manager.GetImageTransition(node, "bitmap window transition");
            if (transition == null) {
                Console.WriteLine("Unable to get specified transition. Using default " + manager.DefaultImageTransition.Name + ".");
                transition = manager.DefaultImageTransition;
            }
            return new BitmapWindowTransitionFactory(transition, length);
        }

        public IWindowTransitionFactory Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }
    }

    public class BitmapWindowTransitionFactory : IWindowTransitionFactory {
        /// <summary>
        /// How long the transition should last.
        /// </summary>
        private double mLengthMS;
        /// <summary>
        /// The factory used to create the transitions which will be rendered.
        /// </summary>
        private IImageTransitionFactory mFactory;

        public BitmapWindowTransitionFactory(IImageTransitionFactory factory, double lengthMS) {
            mFactory = factory;
            mLengthMS = lengthMS;
        }

        public IWindowTransition Create(StateTransition transition, WindowOverlayManager manager) {
            return new BitmapWindowTransition(transition, manager, mFactory.Create(mLengthMS));
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
        /// <param name="manager">The window this fade is to be drawn on.</param>
        /// <param name="lengthMS">The length of time, in ms, the fade should last.</param>
        public BitmapWindowTransition(StateTransition transition, WindowOverlayManager manager, IImageTransition transitionEffect)
            : base(transition, manager) {

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
