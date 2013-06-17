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
using log4net;

namespace Chimera.Overlay.Transitions {
    public class FeatureTransitionFactory : XmlLoader, ITransitionStyleFactory {
        private readonly ILog Logger = LogManager.GetLogger("Overlay.BitmapTransition");

        public string Name {
            get { return "BitmapTransition"; }
        }

        public ITransitionStyle Create(OverlayPlugin manager, XmlNode node) {
            Logger.Info("Creating Bitmap Window Transition");
            double length = GetDouble(node, 5000.0, "Length");
            IFeatureTransitionFactory transition = manager.GetImageTransitionFactory(node, "transition style", "Style");
            if (transition == null) {
                Logger.Debug("Unable to look up custom transition for transition style. Using default, fade.");
                transition = new FeatureFadeFactory();
            }
            return new FeatureFrameTransitionFactory(transition, length);
        }

        public ITransitionStyle Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }
    }

    public class FeatureFrameTransitionFactory : XmlLoader, ITransitionStyle {
        /// <summary>
        /// How long the transition should last.
        /// </summary>
        private double mLengthMS;
        /// <summary>
        /// The factory used to create the transitions which will be rendered.
        /// </summary>
        private IFeatureTransitionFactory mFactory;

        public FeatureFrameTransitionFactory(IFeatureTransitionFactory factory, double lengthMS) {
            mFactory = factory;
            mLengthMS = lengthMS;
        }

        public IWindowTransition Create(StateTransition transition, FrameOverlayManager manager) {
            return new FeatureFrameTransition(transition, manager, mFactory.Create(mLengthMS));
        }
    }

    public class FeatureFrameTransition : FrameTransition {
        /// <summary>
        /// The last clip rectangle the images were redrawn for. Used to check if the images need to be recalculated.
        /// </summary>
        private Rectangle mLastClip;
        /// <summary>
        /// The object which will handle the actual transition.
        /// </summary>
        private IFeatureTransition mTransition;
        private bool mBegun;


        /// <summary>
        /// Initialise the fade transition, specifying how long the fade should last, in ms.
        /// </summary>
        /// <param name="transition">The transition this fade is part of.</param>
        /// <param name="manager">The window this fade is to be drawn on.</param>
        /// <param name="lengthMS">The length of time, in ms, the fade should last.</param>
        public FeatureFrameTransition(StateTransition transition, FrameOverlayManager manager, IFeatureTransition transitionEffect)
            : base(transition, manager) {

            mTransition = transitionEffect;
            mTransition.Start = From;
            mTransition.Finish = To;
            mTransition.Finished += new Action(transitionEffect_Finished);

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
            mTransition.Cancel();
        }

        public override bool NeedsRedrawn {
            get { return true; }
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
