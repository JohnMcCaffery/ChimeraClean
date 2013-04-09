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
        }

        void prev_Triggered() {
            mFinishedCount = 0;
            mNext.Active = false;
            mPrev.Active = false;
            foreach (var window in mWindows)
                window.Prev();
        }

        void next_Triggered() {
            mFinishedCount = 0;
            mNext.Active = false;
            mPrev.Active = false;
            foreach (var window in mWindows)
                window.Next();
        }

        public override IWindowState CreateWindowState(Window window) {
            IImageTransition trans = mTransition.Create(mFadeLengthMS);
            SlideshowWindowState windowState = new SlideshowWindowState(window.OverlayManager, mFolder, trans);
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

        public class SlideshowWindowState : WindowState {
            private readonly IImageTransition mTransition;
            private readonly Bitmap[] mRawImages;
            private readonly string mFolder;
            private Bitmap[] mImages;
            private int mCurrentImage = -1;
            private Rectangle mClip;

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

                mRawImages = images.ToArray();

                AddFeature(transition);
            }

            public override void RedrawStatic(Rectangle clip, Graphics graphics) {
                if (!clip.Width.Equals(mClip.Width) || !mClip.Height.Equals(clip.Height)) {
                    mClip = clip;
                    mImages = new Bitmap[mRawImages.Length];
                    for (int i = 0; i < mRawImages.Length; i++) {
                        Bitmap img = mRawImages[i];
                        Bitmap n = new Bitmap(clip.Width, clip.Height);
                        int x = (clip.Width - img.Width) / 2;
                        int y = (clip.Height - img.Height) / 2;
                        using (Graphics g = Graphics.FromImage(n)) {
                            g.FillRectangle(Brushes.Black, clip);
                            g.DrawImage(img, x, y, img.Width, img.Height);
                        }
                        mImages[i] = n;
                    }
                }
                if (mCurrentImage == -1) {
                    mCurrentImage = 0;
                    mTransition.Init(mImages[mCurrentImage], mImages[mCurrentImage]);
                }
                base.RedrawStatic(clip, graphics);
            }

            public void Prev() {
                int next = (mCurrentImage - 1) % mImages.Length;
                Transition(next >= 0 ? next : mImages.Length - 1);
            }

            public void Next() {
                Transition((mCurrentImage + 1) % mImages.Length);
            }

            private void Transition(int next) {
                mTransition.Init(mImages[mCurrentImage], mImages[next]);
                mTransition.Begin();
                mCurrentImage = next;
                Manager.OverlayWindow.RedrawStatic();
            }
        }
    }
}
