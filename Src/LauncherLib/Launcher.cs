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
using Chimera.GUI.Forms;
using Chimera.Overlay.Drawables;
using Chimera.Overlay.Triggers;
using Chimera.Overlay;
using System.Drawing;
using Chimera.Interfaces.Overlay;
using Chimera.Kinect.Overlay;

namespace Chimera.Launcher {
    public abstract class Launcher {
        private readonly Coordinator mCoordinator;
        private CoordinatorForm mForm;

        public Coordinator Coordinator {
            get { return mCoordinator; }
        }
        public CoordinatorForm Form {
            get {
                if (mForm == null)
                    mForm = new CoordinatorForm(mCoordinator);
                return mForm;
            }
        }

        public Launcher(params string[] args) {
            mCoordinator = new Coordinator(GetWindows(), GetInputs());
            if (new OverlayConfig(args).InitOverlay)
                InitOverlay();
        }

        protected abstract ISystemPlugin[] GetInputs();

        protected abstract Window[] GetWindows();

        protected abstract void InitOverlay();

        protected static void InvisTrans(State from, State to, Point topLeft, Point bottomRight, Rectangle clip, Window window, IHoverSelectorRenderer renderer, IWindowTransitionFactory transition) {
            InvisibleHoverTrigger splashExploreTrigger = new InvisibleHoverTrigger(
                window.OverlayManager, 
                renderer, 
                topLeft.X, 
                topLeft.Y, 
                bottomRight.X - topLeft.X, 
                bottomRight.Y - topLeft.Y, 
                clip);
            StateTransition splashExploreTransition = new StateTransition(window.Coordinator.StateManager, from, to, splashExploreTrigger, transition);
            from.AddTransition(splashExploreTransition);
        }

        protected static void ImgTrans(State from, State to, string image, float x, float y, Window window, IHoverSelectorRenderer renderer, IWindowTransitionFactory transition) {
            ImgTrans(from, to, image, x, y, -1f, window, renderer, transition);
        }
        protected static void ImgTrans(State from, State to, string image, float x, float y, float w, Window window, IHoverSelectorRenderer renderer, IWindowTransitionFactory transition) {
            OverlayImage exploreButton = new OverlayImage(new Bitmap("../Images/Caen/Buttons/" + image + ".png"), x, y, w, window.Name);
            ImageHoverTrigger splashExploreTrigger = new ImageHoverTrigger(window.OverlayManager, renderer, exploreButton);
            StateTransition splashExploreTransition = new StateTransition(window.Coordinator.StateManager, from, to, splashExploreTrigger, transition);
            from.AddTransition(splashExploreTransition);
        }

        protected static void TxtTrans(State from, State to, string text, float x, float y, Font font, Color colour, Rectangle clip, Window window, IHoverSelectorRenderer renderer, IWindowTransitionFactory transition) {
            Text txt = new StaticText(text, window.Name, font, colour, new PointF(x, y));
            TextHoverTrigger splashExploreTrigger = new TextHoverTrigger(window.OverlayManager, renderer, txt, clip);
            StateTransition splashExploreTransition = new StateTransition(window.Coordinator.StateManager, from, to, splashExploreTrigger, transition);
            from.AddTransition(splashExploreTransition);
        }

        protected void InitIdle(State idle, State home, IWindowTransitionFactory fadeInTransition, IWindowTransitionFactory fadeOutTransition, int timeout) {
            ITrigger skeletonLost = new SkeletonLostTrigger(Coordinator, timeout);
            ITrigger skeletonFound = new SkeletonFoundTrigger();

            foreach (var state in Coordinator.StateManager.States) {
                if (state != idle) {
                    StateTransition trans = new StateTransition(Coordinator.StateManager, state, idle, skeletonLost, fadeOutTransition);
                    state.AddTransition(trans);
                }
            }

            StateTransition back = new StateTransition(Coordinator.StateManager, idle, home, skeletonFound, fadeInTransition);
            idle.AddTransition(back);
        }
    }
}
