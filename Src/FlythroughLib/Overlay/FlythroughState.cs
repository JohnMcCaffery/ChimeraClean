using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay;
using Chimera.Overlay.States;

namespace Chimera.Flythrough.Overlay {
    public class FlythroughState : State {
        private Flythrough mInput;
        private SlideshowWindow mSlideshow;
        private IImageTransition mSlideshowTransition;
        private string mSlideshowWindowName;
        private string mSlideshowFolder;
        private string mFlythrough;
        private bool mStepping;

        public FlythroughState(string name, StateManager manager, string flythrough)
            : base(name, manager) {

            mFlythrough = flythrough;
            mInput = manager.Coordinator.GetInput<Flythrough>();
        }

        public FlythroughState(string name, StateManager manager, string flythrough, params ITrigger[] steps)
            : this(name, manager, flythrough) {

            mStepping = true;

            foreach (var step in steps) {
                step.Triggered += new Action(step_Triggered);
                if (step is IDrawable)
                    AddFeature(step as IDrawable);
            }
        }

        public FlythroughState(string name, StateManager manager, string flythrough, string slideshowWindow, string slideshowFolder, IImageTransition slideshowTransition, params ITrigger[] steps)
            : this(name, manager, flythrough, steps) {

            mSlideshowWindowName = slideshowWindow;
            mSlideshowFolder = slideshowFolder;
            mSlideshowTransition = slideshowTransition;
        }

        void step_Triggered() {
            mInput.Step();
        }

        public override IWindowState CreateWindowState(Window window) {
            if (window.Name.Equals(mSlideshowWindowName)) {
                mSlideshow = new SlideshowWindow(window.OverlayManager, mSlideshowFolder, mSlideshowTransition);
                return mSlideshow;
            }
            return new WindowState(window.OverlayManager);
        }

        protected override void TransitionToFinish() { }

        protected override void TransitionFromStart() { }

        public override void TransitionToStart() {
            mInput.Coordinator.EnableUpdates = true;
            mInput.Load(mFlythrough);

            if (mStepping)
                mInput.AutoStep = false;
            else
                mInput.Loop = true;

            mInput.Play();
        }

        public override void TransitionFromFinish() {
            mInput.Paused = true;
        }
    }
}
