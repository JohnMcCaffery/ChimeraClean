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
using Chimera.Overlay.Features;
using System.Drawing;
using System.Xml;
using Chimera.Interfaces;

namespace Chimera.Flythrough.Overlay {
    public class FlythroughStateFactory : IStateFactory {
        private IMediaPlayer mPlayer = null;

        public FlythroughStateFactory() { }

        public FlythroughStateFactory(IMediaPlayer player) {
            mPlayer = player;
        }

        #region IFactory<State> Members

        public string Name {
            get { return "Flythrough"; }
        }

        public State Create(OverlayPlugin manager, XmlNode node) {
            return new FlythroughState(manager, node, mPlayer);
        }

        public State Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }

        #endregion
    }

    public class FlythroughState : State {
        private static string FONT = "Verdana";
        private static float FONT_SIZE = 20;
        private static Color FONT_COLOUR = Color.Red;
        private static PointF STEP_TEXT_POS = new PointF(.005f, .05f);

        private Dictionary<int, Step> mSteps = new Dictionary<int, Step>();
        private FlythroughPlugin mInput;
        private FrameOverlayManager mDefaultWindow;
        private SlideshowWindow mSlideshow;
        private IFeatureTransition mSlideshowTransition;
        private List<ITrigger> mStepTriggers = new List<ITrigger>();
        private IMediaPlayer mPlayer;
        private Text mStepText;
        private Text mSubtitlesText;
        private Step mCurrentStep;
        private string mSlideshowWindowName;
        private string mSlideshowFolder;
        private string mFlythrough;
        private bool mAutoStepping = false;
        private bool mLoop = false;

        private Action<int> mStepListener;

        public FlythroughState(string name, OverlayPlugin manager, string flythrough)
            : base(name, manager) {

            mFlythrough = flythrough;
            mInput = manager.Core.GetPlugin<FlythroughPlugin>();
            mStepListener = new Action<int>(mInput_StepStarted);
        }

        public FlythroughState(string name, OverlayPlugin manager, string flythrough, params ITrigger[] stepTriggers)
            : this(name, manager, flythrough) {

            mAutoStepping = true;
            Font f = new Font(FONT, FONT_SIZE, FontStyle.Bold);
            mStepText = new StaticText("1/" + mInput.Count, manager[0], f, FONT_COLOUR, STEP_TEXT_POS);
            AddFeature(mStepText);

            //mInput.CurrentEventChange += new Action<FlythroughEvent<Camera>,FlythroughEvent<Camera>>(mInput_CurrentEventChange);
            mInput.StepStarted += new Action<int>(mInput_StepStarted);

            foreach (var trigger in stepTriggers)
                AddStepTrigger(trigger);
        }

        public FlythroughState(string name, OverlayPlugin manager, string flythrough, string slideshowWindow, string slideshowFolder, IFeatureTransition slideshowTransition, params ITrigger[] steps)
            : this(name, manager, flythrough, steps) {

            mSlideshowWindowName = slideshowWindow;
            mSlideshowFolder = slideshowFolder;
            mSlideshowTransition = slideshowTransition;
        }

        public FlythroughState(OverlayPlugin manager, XmlNode node, IMediaPlayer player)
            : base(GetName(node, "flythrough state"), manager) {

            mStepListener = new Action<int>(mInput_StepStarted);

            mInput = manager.Core.GetPlugin<FlythroughPlugin>();
            bool displaySubtitles = GetBool(node, false, "DisplaySubtitles");
            mFlythrough = GetString(node, null, "File");
            mAutoStepping = GetBool(node, true, "AutoStep");
            mLoop = GetBool(node, true, "Loop");

            if (mFlythrough == null)
                throw new ArgumentException("Unable to load flythrough state. No flythrough file specified.");

            mPlayer = player;
            if (mPlayer != null)
                mDefaultWindow = Manager[0];

            if (displaySubtitles) {
                mSubtitlesText = Manager.MakeText(node.SelectSingleNode("child::SubtitleText"));
            }

            XmlNode stepTextNode = node.SelectSingleNode("child::StepText");
            if (stepTextNode != null)
                mStepText = Manager.MakeText(stepTextNode);

            //mInput.CurrentEventChange += new Action<FlythroughEvent<Camera>,FlythroughEvent<Camera>>(mInput_CurrentEventChange);
            int subtitleTimeout = GetInt(node, 20, "SubtitleTimeout");

            XmlNode stepsRoot = node.SelectSingleNode("child::Steps");
            if (stepsRoot != null) {
                foreach (XmlNode child in stepsRoot.ChildNodes) {
                    if (child is XmlElement) {
                        Step step = new Step(this, child, mSubtitlesText, subtitleTimeout, mPlayer);
                        mSteps.Add(step.StepNum, step);
                    }
                }
            }
            
            if (displaySubtitles)
                AddFeature(mSubtitlesText);
            if (mStepText != null)
                AddFeature(mStepText);

            XmlNode triggersRoot = node.SelectSingleNode("child::Triggers");
            if (triggersRoot != null) {
                foreach (XmlNode child in triggersRoot.ChildNodes)
                    AddStepTrigger(manager.GetTrigger(child, "flythrough step", null));
            }
        }

        private void AddStepTrigger(ITrigger trigger) {
            if (trigger == null)
                //TODO - debug?
                return;
            mStepTriggers.Add(trigger);
            trigger.Triggered += new Action(step_Triggered);
            if (trigger is IFeature)
                AddFeature(trigger as IFeature);
        }

        //void mInput_CurrentEventChange(FlythroughEvent<Camera> old, FlythroughEvent<Camera> n) {
        void mInput_StepStarted(int step) {
            if (mCurrentStep != null)
                mCurrentStep.Finish();

            mCurrentStep = null;
            if (mSteps.ContainsKey(step)) {
                mCurrentStep = mSteps[step];
                mCurrentStep.Start();
            }

            if (step == mInput.Count - 1) {
                foreach (var trigger in mStepTriggers)
                    trigger.Active = false;
                foreach (var manager in Manager.OverlayManagers)
                    manager.ForceRedrawStatic();
            }

            if (mStepText != null)
                mStepText.TextString = (step + 1) + "\\" + mInput.Count;
        }

        void step_Triggered() {
            mInput.Step();
            //foreach (var step in mStepTriggers)
                //step.Active = false;
        }


        public override IFrameState CreateWindowState(FrameOverlayManager manager) {
            if (manager.Name.Equals(mSlideshowWindowName)) {
                mSlideshow = new SlideshowWindow(manager, mSlideshowFolder, mSlideshowTransition);
                return mSlideshow;
            }
            return new FrameState(manager);
        }

        protected override void TransitionToFinish() {
            foreach (var step in mSteps.Values)
                step.Prep();
        }

        protected override void TransitionFromStart() {
            if (mCurrentStep != null)
                mCurrentStep.Finish();
        }

        protected override void TransitionToStart() {
            mInput.StepStarted += mStepListener;
            Manager.ControlPointers = false;

            if (mPlayer != null)
                mDefaultWindow.AddControl(mPlayer.Player, new RectangleF(0f, 0f, 0f, 0f));

            if (mSubtitlesText != null)
                mSubtitlesText.Active = true;

            Manager.Core.ControlMode = ControlMode.Absolute;
            mInput.Enabled = true;
            mInput.Core.EnableUpdates = true;
            mInput.Load(mFlythrough);

            mInput.AutoStep = mAutoStepping;
            mInput.Loop = mLoop;
            foreach (var trigger in mStepTriggers)
                trigger.Active = true;

            Manager.Core.ControlMode = ControlMode.Absolute;
            mInput.Time = 0;
            mInput.Play();
        }

        protected override void TransitionFromFinish() {
            mInput.StepStarted -= mStepListener;
            mInput.Paused = true;
            mInput.Enabled = false;
            if (mPlayer != null)
                mDefaultWindow.RemoveControl(mPlayer.Player);
            if (mSubtitlesText != null)
                mSubtitlesText.Active = false;
            foreach (var trigger in mStepTriggers)
                trigger.Active = false;
        }
    }
}
