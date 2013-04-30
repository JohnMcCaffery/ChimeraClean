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
using Chimera.Interfaces.Overlay;
using Chimera.Overlay;
using Chimera.Overlay.States;
using Chimera.Overlay.Triggers;

namespace Chimera.Flythrough.Overlay {
    public class FlythroughState : State {
        private Flythrough mInput;
        private SlideshowWindow mSlideshow;
        private IImageTransition mSlideshowTransition;
        private string mSlideshowWindowName;
        private string mSlideshowFolder;
        private string mFlythrough;
        private bool mStepping = false;
        private ITrigger[] mStepTriggers = new ITrigger[0];
        private SimpleTrigger mTrigger;

        public FlythroughState(string name, StateManager manager, string flythrough)
            : base(name, manager) {

            mFlythrough = flythrough;
            mInput = manager.Coordinator.GetPlugin<Flythrough>();
        }

        public FlythroughState(string name, StateManager manager, string flythrough, State home, IWindowTransitionFactory transition, params ITrigger[] steps)
            : this(name, manager, flythrough) {

            mStepTriggers = steps;
            mStepping = true;

            mTrigger = new SimpleTrigger();
            mInput.SequenceFinished += new EventHandler(mInput_SequenceFinished);
            StateTransition trans = new StateTransition(manager, this, home, mTrigger, transition);
            AddTransition(trans);

            foreach (var step in steps) {
                step.Triggered += new Action(step_Triggered);
                if (step is IDrawable)
                    AddFeature(step as IDrawable);
            }
        }

        void mInput_SequenceFinished(object sender, EventArgs e) {
            if (mTrigger != null)
                mTrigger.Trigger();
        }

        public FlythroughState(string name, StateManager manager, string flythrough, State home, IWindowTransitionFactory transition, string slideshowWindow, string slideshowFolder, IImageTransition slideshowTransition, params ITrigger[] steps)
            : this(name, manager, flythrough, home, transition, steps) {

            mSlideshowWindowName = slideshowWindow;
            mSlideshowFolder = slideshowFolder;
            mSlideshowTransition = slideshowTransition;
        }

        void step_Triggered() {
            mInput.Step();
            //foreach (var step in mStepTriggers)
                //step.Active = false;
        }

        void mInput_CurrentEventChange(FlythroughEvent<Camera> old, FlythroughEvent<Camera> to) {
            foreach (var step in mStepTriggers)
                step.Active = true;
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
            Manager.Coordinator.ControlMode = ControlMode.Absolute;
            mInput.Enabled = true;
            mInput.Coordinator.EnableUpdates = true;
            mInput.Load(mFlythrough);

            if (mStepping) {
                mInput.AutoStep = false;
                mInput.Loop = false;
            } else {
                mInput.Loop = true;
                mInput.AutoStep = true;
            }

            mInput.Time = 0;
            //mInput.CurrentEventChange += mInput_CurrentEventChange;
            mInput.Play();
        }

        public override void TransitionFromFinish() {
            mInput.Paused = true;
            mInput.Enabled = false;
        }
    }
}
