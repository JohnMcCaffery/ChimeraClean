using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace Chimera.Overlay.States {
    public class SlideshowState : State {
        private readonly List<SlideshowWindow> mWindows = new List<SlideshowWindow>();
        private readonly IImageTransitionFactory mTransition;
        private readonly ITrigger mNext;
        private readonly ITrigger mPrev;
        private readonly string mFolder;
        private double mFadeLengthMS;
        private int mFinishedCount = 0;

        public SlideshowState(string name, StateManager manager, string folder, ITrigger next, ITrigger prev, IImageTransitionFactory transition, double fadeLengthMS)
            : base(name, manager) {

            mNext = next;
            mPrev = prev;
            mFolder = folder;
            mTransition = transition;
            mFadeLengthMS = fadeLengthMS;

            next.Triggered += new Action(next_Triggered);
            prev.Triggered += new Action(prev_Triggered);

            if (mNext is IDrawable) {
                AddFeature(mNext as IDrawable);
            }
            if (mPrev is IDrawable) {
                AddFeature(mPrev as IDrawable);
            }
        }

        void prev_Triggered() {
            if (Active) {
                mFinishedCount = 0;
                mNext.Active = false;
                mPrev.Active = false;
                foreach (var window in mWindows)
                    window.Prev();
            }
        }

        void next_Triggered() {
            if (Active) {
                mFinishedCount = 0;
                mNext.Active = false;
                mPrev.Active = false;
                foreach (var window in mWindows)
                    window.Next();
            }
        }

        public override IWindowState CreateWindowState(Window window) {
            IImageTransition trans = mTransition.Create(mFadeLengthMS);
            SlideshowWindow windowState = new SlideshowWindow(window.OverlayManager, mFolder, trans);
            /*
            if (mNext is IDrawable) {
                IDrawable next = mNext as IDrawable;
                if (window.Name.Equals(next.Window))
                    windowState.AddFeature(next);
            }
            if (mPrev is IDrawable) {
                IDrawable prev = mPrev as IDrawable;
                if (window.Name.Equals(prev.Window))
                    windowState.AddFeature(prev);
            }
            */
            mWindows.Add(windowState);
            trans.Finished += new Action(trans_Finished);
            return windowState;
        }

        void trans_Finished() {
            mFinishedCount++;
            if (mFinishedCount == mWindows.Count) {
                mNext.Active = true;
                mPrev.Active = true;
            }
        }

        public override void TransitionToStart() { }

        protected override void TransitionToFinish() {
            mNext.Active = true;
            mPrev.Active = true;
        }

        protected override void TransitionFromStart() {
            mNext.Active = false;
            mPrev.Active = false;
        }

        public override void TransitionFromFinish() {
            mNext.Active = false;
            mPrev.Active = false;
        }
    }
}
