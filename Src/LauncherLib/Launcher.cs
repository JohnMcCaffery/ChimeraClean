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
using Chimera.Overlay.Transitions;
using Chimera.Config;

namespace Chimera.Launcher {
    public abstract class Launcher {
        private readonly Coordinator mCoordinator;
        private OverlayConfig mConfig;
        private CoordinatorForm mForm;
        private IHoverSelectorRenderer mRenderer;
        private string mButtonFolder = "../Images/Examples/";

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
        protected OverlayConfig Config {
            get { return mConfig; }
        }

        public Launcher(string buttonFolder, IHoverSelectorRenderer renderer, params string[] args)
            : this(args) {

            mRenderer = renderer;
            mButtonFolder = buttonFolder;
        }

        public Launcher(params string[] args) {
            mConfig = new OverlayConfig(args);
            mCoordinator = new Coordinator(GetWindows(), GetInputs());

            if (mConfig.InitOverlay) {
                InitOverlay();
                State home = mCoordinator.StateManager.States.FirstOrDefault(s => s.Name == mConfig.HomeState);
                if (mConfig.IdleState != "None") {
                    State idle = mCoordinator.StateManager.States.FirstOrDefault(s => s.Name == mConfig.IdleState);
                    if (idle != null && home != null)
                        InitIdle(idle, home, new OpacityFadeInTransitionFactory(5000), new OpacityFadeOutTransitionFactory(5000), mConfig.IdleTimeoutMs);
                    else
                        Console.WriteLine("Unable to create idle state. The idle state specified (" + mConfig.IdleState + ") was not found.");
                }

                if (home != null)
                    mCoordinator.StateManager.CurrentState = home;
            }
        }

        protected abstract ISystemPlugin[] GetInputs();

        protected abstract Window[] GetWindows();

        protected abstract void InitOverlay();

        protected void InvisTrans(State from, State to, Point topLeft, Point bottomRight, Rectangle clip, Window window, IWindowTransitionFactory transition) {
            ITrigger trigger  = mConfig.UseClicks ?
                (ITrigger) new ClickTrigger(window.OverlayManager, topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y, clip) :
                (ITrigger) new InvisibleHoverTrigger(
                window.OverlayManager, 
                mRenderer, 
                topLeft.X, 
                topLeft.Y, 
                bottomRight.X - topLeft.X, 
                bottomRight.Y - topLeft.Y, 
                clip);
            StateTransition splashExploreTransition = new StateTransition(window.Coordinator.StateManager, from, to, trigger, transition);
            from.AddTransition(splashExploreTransition);
        }

        protected void ImgTrans(State from, State to, string image, float x, float y, Window window, IWindowTransitionFactory transition) {
            ImgTrans(from, to, image, x, y, -1f, window, transition);
        }
        protected void ImgTrans(State from, State to, string image, float x, float y, float w, Window window, IWindowTransitionFactory transition) {
            OverlayImage exploreButton = new OverlayImage(new Bitmap(mButtonFolder + image + ".png"), x, y, w, window.Name);
            ITrigger trigger = mConfig.UseClicks ? 
                (ITrigger) new ImageClickTrigger(window.OverlayManager, exploreButton) :
                (ITrigger) new ImageHoverTrigger(window.OverlayManager, mRenderer, exploreButton);
            StateTransition splashExploreTransition = new StateTransition(window.Coordinator.StateManager, from, to, trigger, transition);
            from.AddTransition(splashExploreTransition);
        }

        protected void TxtTrans(State from, State to, string text, float x, float y, Font font, Color colour, Rectangle clip, Window window, IHoverSelectorRenderer renderer, IWindowTransitionFactory transition) {
            Text txt = new StaticText(text, window.Name, font, colour, new PointF(x, y));
            ITrigger trigger = mConfig.UseClicks ?
                (ITrigger) new TextClickTrigger(window.OverlayManager, txt, clip) :
                (ITrigger) new TextHoverTrigger(window.OverlayManager, renderer, txt, clip);
            StateTransition splashExploreTransition = new StateTransition(window.Coordinator.StateManager, from, to, trigger, transition);
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
