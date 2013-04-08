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
        private readonly List<SlideshowWindowState> mWindows = new List<SlideshowWindowState>();
        private readonly IImageTransitionFactory mTransition;
        private readonly ITrigger mNext;
        private readonly ITrigger mPrev;
        private readonly string mFolder;
        private double mFadeLengthMS;

        public SlideshowState(string name, StateManager manager, string folder, ITrigger next, ITrigger prev, IImageTransitionFactory transition, double fadeLengthMS)
            : base(name, manager) {

            mNext = next;
            mPrev = prev;
            mFolder = folder;
            mTransition = transition;
            mFadeLengthMS = fadeLengthMS;

            next.Triggered += new Action(next_Triggered);
            prev.Triggered += new Action(prev_Triggered);
        }

        void prev_Triggered() {
            foreach (var window in mWindows)
                window.Prev();
        }

        void next_Triggered() {
            foreach (var window in mWindows)
                window.Next();
        }

        public override IWindowState CreateWindowState(Window window) {
            SlideshowWindowState windowState = new SlideshowWindowState(window.OverlayManager, mFolder, mTransition.Create(mFadeLengthMS));
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
            mWindows.Add(windowState);
            return windowState;
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

        public class SlideshowWindowState : WindowState {
            private readonly IImageTransition mTransition;
            private readonly Bitmap[] mImages;
            private readonly string mFolder;
            private int mCurrentImage = 0;

            public SlideshowWindowState(WindowOverlayManager manager, string folder, IImageTransition transition)
                : base(manager) {

                mFolder = folder;
                mTransition = transition;

                List<Bitmap> images = new List<Bitmap>();
                foreach (var file in Directory.GetFiles(Path.Combine(folder, manager.Window.Name))) {
                    if (Regex.IsMatch(Path.GetExtension(file), @"png$|jpe?g$|bmp$", RegexOptions.IgnoreCase)) {
                        images.Add(new Bitmap(file));
                    }
                }

                mImages = images.ToArray();

                AddFeature(transition);

                mTransition.Init(mImages[0], mImages[0]);
            }

            public void Prev() {
                Transition((mCurrentImage - 1) % mImages.Length);
            }

            public void Next() {
                Transition((mCurrentImage + 1) % mImages.Length);
            }

            private void Transition(int next) {
                mTransition.Init(mImages[mCurrentImage], mImages[next]);
                mCurrentImage = next;
                Manager.OverlayWindow.RedrawStatic();
            }
        }
    }
}
