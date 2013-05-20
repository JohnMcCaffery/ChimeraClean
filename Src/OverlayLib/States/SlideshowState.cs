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
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using Chimera.Overlay.Transitions;

namespace Chimera.Overlay.States {
    public class SlideshowStateFactory : IStateFactory {
        public string Name {
            get { return "Slideshow"; }
        }

        public State Create(OverlayPlugin manager, XmlNode node) {
            Console.WriteLine("Creating Slideshow State");
            return new SlideshowState(manager, node);
        }

        public State Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }
    }

    public class SlideshowState : State {
        private readonly List<SlideshowWindow> mWindows = new List<SlideshowWindow>();
        private readonly IImageTransitionFactory mTransition;
        private readonly string mFolder;
        private double mFadeLengthMS;
        private int mFinishedCount = 0;
        private List<ITrigger> mTriggers = new List<ITrigger>();


        public SlideshowState(string name, OverlayPlugin manager, string folder, ITrigger next, ITrigger prev, IImageTransitionFactory transition, double fadeLengthMS)
            : base(name, manager) {

            mFolder = folder;
            mTransition = transition;
            mFadeLengthMS = fadeLengthMS;


            AddTrigger(true, next);
            AddTrigger(false, prev);
        }

        public SlideshowState(OverlayPlugin manager, XmlNode node)
            : base(GetName(node), manager) {

            mTransition =  manager.GetImageTransitionFactory(node, "slideshow state", "Transition");
            if (mTransition == null) {
                Console.WriteLine("Unable to look up custom transition for slideshow. Using default, fade.");
                mTransition = new BitmapFadeFactory();
            }
            mFolder = GetString(node, null, "Folder");
            if (mFolder == null)
                throw new ArgumentException("Unable to load slideshow state. No Folder specified.");

            LoadTriggers(manager, node, true);
            LoadTriggers(manager, node, false);
        }

        private void LoadTriggers(OverlayPlugin manager, XmlNode node, bool next) {
            foreach (XmlNode child in GetChildrenOfChild(node, (next ? "Next" : "Prev") + "Triggers"))
                AddTrigger(next, manager.GetTrigger(child, "slideshow state " + (next ? "next" : "prev")));
        }

        private void AddTrigger(bool next, ITrigger trigger) {
            if (next)
                trigger.Triggered += new Action(next_Triggered);
            else
                trigger.Triggered += new Action(prev_Triggered);

            mTriggers.Add(trigger);

            if (trigger is IFeature)
                AddFeature(trigger as IFeature);
        }

        void prev_Triggered() {
            if (Active) {
                mFinishedCount = 0;
                SetTriggerState(false);
                foreach (var window in mWindows)
                    window.Prev();
            }
        }

        void next_Triggered() {
            if (Active) {
                mFinishedCount = 0;
                SetTriggerState(false);
                foreach (var window in mWindows)
                    window.Next();
            }
        }

        public override IWindowState CreateWindowState(WindowOverlayManager manager) {
            IImageTransition trans = mTransition.Create(mFadeLengthMS);
            SlideshowWindow windowState = new SlideshowWindow(manager, mFolder, trans);
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
                SetTriggerState(true);
            }
        }

        public override void TransitionToStart() { }

        protected override void TransitionToFinish() {
            SetTriggerState(true);
        }

        protected override void TransitionFromStart() {
            SetTriggerState(false);
        }

        public override void TransitionFromFinish() {
            SetTriggerState(false);
        }

        private void SetTriggerState(bool state) {
            //foreach (var trigger in mTriggers)
                //trigger.Active = state;
        }
    }
}
