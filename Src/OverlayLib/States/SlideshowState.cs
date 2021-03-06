﻿/*************************************************************************
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
using log4net;

namespace Chimera.Overlay.States {
    public class SlideshowStateFactory : IStateFactory {
        public string Name {
            get { return "Slideshow"; }
        }

        public State Create(OverlayPlugin manager, XmlNode node) {
            return new SlideshowState(manager, node);
        }

        public State Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }
    }

    public class SlideshowState : State {
        private readonly ILog Logger = LogManager.GetLogger("Overlay.Slideshow");
        private int mCurrentStep = 0;
        private int mStepLengthS = -1;
        private List<ITrigger> mTriggers = new List<ITrigger>();
        private DateTime mLastStep;

        private readonly List<IFeature>[] mSteps;
        private readonly Action mTickListener;

        public SlideshowState(OverlayPlugin manager, XmlNode node)
            : base(GetName(node, "slideshow state"), manager, node) {

            mTickListener = new Action(Core_Tick);

            mStepLengthS = GetInt(node, -1, "StepLengthS");

            int count = 1;
            List<List<IFeature>> steps = new List<List<IFeature>>();
            foreach (var step in GetChildrenOfChild(node, "Steps")) {
                List<IFeature> stepFeatures = new List<IFeature>();
                foreach (var stepFeature in step.ChildNodes.OfType<XmlElement>()) {
                    IFeature feature = manager.GetFeature(stepFeature, "slideshow step " + count, null);
                    if (feature != null) {
                        stepFeatures.Add(feature);
                        AddFeature(feature);
                    }
                }
                steps.Add(stepFeatures);
                count++;
            }

            if (steps.Count == 0)
                throw new ArgumentException("Unable to create slideshow state. No steps specified.");

            mSteps = steps.ToArray();

            LoadTriggers(manager, node, true);
            LoadTriggers(manager, node, false);
        }

        private void LoadTriggers(OverlayPlugin manager, XmlNode node, bool next) {
            foreach (XmlNode child in GetChildrenOfChild(node, (next ? "Next" : "Prev") + "Triggers"))
                AddTrigger(next, manager.GetTrigger(child, "slideshow state " + (next ? "next" : "prev"), null));
        }

        private void AddTrigger(bool next, ITrigger trigger) {
            if (next)
                trigger.Triggered += new Action<ITrigger>(next_Triggered);
            else
                trigger.Triggered += new Action<ITrigger>(prev_Triggered);

            mTriggers.Add(trigger);

            if (trigger is IFeature)
                AddFeature(trigger as IFeature);
        }

        void prev_Triggered(ITrigger source) {
            if (Active)
                Increment(-1);
        }

        void next_Triggered(ITrigger source) {
            if (Active)
                Increment(1);
        }

        void Core_Tick() {
            if (DateTime.UtcNow.Subtract(mLastStep).TotalSeconds > mStepLengthS) {
                mLastStep = DateTime.UtcNow;
                Increment(+1);
            }
        }

        private void Increment(int step) {
            foreach (var feature in mSteps[mCurrentStep])
                feature.Active = false;
            mCurrentStep = (mCurrentStep + step) % mSteps.Length;
            if (mCurrentStep < 0)
                mCurrentStep = mSteps.Length - 1;
            foreach (var feature in mSteps[mCurrentStep])
                feature.Active = true;
            foreach (var man in Manager.OverlayManagers)
                man.ForceRedrawStatic();
        }

        protected override void TransitionToStart() {
            foreach (var feature in mSteps.Skip(1).Aggregate((seed, current) => new List<IFeature>(seed.Concat(current))))
                feature.Active = false;
        }

        protected override void TransitionToFinish() {
            SetTriggerState(true);
            TransitionToStart();

            if (mStepLengthS > 0)
                Manager.Core.Tick += mTickListener;
        }

        protected override void TransitionFromStart() {
            SetTriggerState(false);
        }

        protected override void TransitionFromFinish() {
            SetTriggerState(false);
            if (mStepLengthS > 0)
                Manager.Core.Tick -= mTickListener;
        }

        private void SetTriggerState(bool state) {
            foreach (var trigger in mTriggers)
                trigger.Active = state;
        }
    }
}
