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
using Chimera.Util;

namespace Chimera.Overlay.Transitions {
    public class OpacityTransitionFactory : XmlLoader, ITransitionStyleFactory {
        private readonly ILog Logger = LogManager.GetLogger("Overlay.OpacityTransition");

        public string Name {
            get { return "OpacityTransition"; }
        }

        public ITransitionStyle Create(OverlayPlugin manager, XmlNode node) {
            Logger.Info("Creating Opacity Window Transition");
            string transition = GetString(node, "Fade", "Transition");
            double length = GetDouble(node, 5000.0, "Length");
            bool fadeOut = GetBool(node, true, "FadeOut");
            return new OpacityFadeWindowTransitionFactory(length, fadeOut);
        }

        public ITransitionStyle Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }
    }
    public class OpacityFadeWindowTransitionFactory : XmlLoader, ITransitionStyle {
        /// <summary>
        /// How long the transition should last.
        /// </summary>
        private double mLengthMS;
        private bool mFadeOut;

        public OpacityFadeWindowTransitionFactory(double lengthMS, bool fadeOut) {
            mLengthMS = lengthMS;
            mFadeOut = fadeOut;
        }

        public IWindowTransition Create(StateTransition transition, FrameOverlayManager manager) {
            return new OpacityFadeWindowTransition(transition, manager, mLengthMS, !mFadeOut);
        }
    }
    public class OpacityFadeOutWindowTransitionFactory : OpacityFadeWindowTransitionFactory {
        public OpacityFadeOutWindowTransitionFactory(double lengthMS) : base(lengthMS, true) { }
    }
    public class OpacityFadeInWindowTransitionFactory : OpacityFadeWindowTransitionFactory {
        public OpacityFadeInWindowTransitionFactory(double lengthMS) : base (lengthMS, false) { }
    }
    public class OpacityFadeWindowTransition : FrameTransition {
        private Action mTickListener;
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

#if DEBUG
        private TickStatistics mStatistics = new TickStatistics();
#endif

        /// <summary>
        /// Initialise the fade transition, specifying how long the fade should last, in ms.
        /// </summary>
        /// <param name="transition">The transition this fade is part of.</param>
        /// <param name="manager">The window this fade is to be drawn on.</param>
        /// <param name="lengthMS">The length of time, in ms, the fade should last.</param>
        public OpacityFadeWindowTransition(StateTransition transition, FrameOverlayManager manager, double lengthMS, bool fadeIn)
            : base(transition, manager) {
            mLengthMS = lengthMS;
            mFadeIn = fadeIn;
            mTickListener = new Action(Coordinator_Tick);

            Finished += new Action<IWindowTransition>(OpacityFadeWindowTransition_Finished);

#if DEBUG
            StatisticsCollection.AddStatistics(mStatistics, transition.Name + " " + manager.Name);
#endif
        }

        void OpacityFadeWindowTransition_Finished(IWindowTransition transition) {
            Transition.Manager.Core.Tick -= mTickListener;
        }

        void Coordinator_Tick() {
            if (!mTransitioning)
                return;

#if DEBUG
            mStatistics.Begin();
#endif
            double time = DateTime.Now.Subtract(mTransitionStart).TotalMilliseconds;
            if (time > mLengthMS) {
                mTransitioning = false;
                //Manager.Opacity = mFadeIn ? 1.0 : 0.0;
                Manager.Opacity = 1.0;
                if (Finished != null)
                    Finished(this);
            }
            else {
                double done = time / mLengthMS;
                Manager.Opacity = mFadeIn ? done : 1.0 - done;
            }
#if DEBUG
            mStatistics.End();
#endif
        }

        #region IWindowTransition Members

        public override event Action<IWindowTransition> Finished;

        public override void Begin() {
            base.Begin();
            mTransitionStart = DateTime.Now;
            mTransitioning = true;
            Transition.Manager.Core.Tick += mTickListener;
        }

        public override void Cancel() {
            mTransitioning = false;
        }

        public override bool NeedsRedrawn {
            get { return true; }
        }

        public override void DrawStatic(Graphics graphics) {
            if (mFadeIn)
                To.DrawStatic(graphics);
            else
                From.DrawStatic(graphics);
        }

        public override void DrawDynamic(Graphics graphics) { }

        #endregion
    }
}
