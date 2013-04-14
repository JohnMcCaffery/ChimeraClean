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
            InitOverlay();
        }

        protected abstract ISystemInput[] GetInputs();

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
