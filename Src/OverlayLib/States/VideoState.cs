using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay.Features;
using System.Drawing;
using System.Diagnostics;
using Chimera.Util;
using System.IO;
using System.Threading;
using Chimera.Overlay.Triggers;
using Chimera.Overlay;
using Chimera.Overlay.States;
using System.Xml;
using System.Windows.Forms;
using Chimera.Interfaces;
using Chimera.Overlay.Transitions;
using log4net;

namespace Chimera.Overlay.States {
    public class VideoStateFactory : IStateFactory {
        private IMediaPlayer mPlayer;

        public VideoStateFactory() {
            throw new Exception("Unable to load Video State factory. No MediaPlayer supplied.");
        }
        public VideoStateFactory(IMediaPlayer player) {
            mPlayer = player;
        }

        public string Name {
            get { return "Video"; }
        }

        public State Create(OverlayPlugin manager, XmlNode node) {
            return new VideoState(manager, node, mPlayer);
        }

        public State Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }
    }

    public class VideoState : ImageBGState {
        private readonly ILog Logger = LogManager.GetLogger("Overlay.Video");
        private string mVideo;
        private FrameOverlayManager mMainWindow;
        private SimpleTrigger mTrigger;
        private RectangleF mBounds = new RectangleF(0f, 0f, 1f, 1f);
        private bool mAdded;
        private bool mRestartMode;
        private bool mRestarted = true;
        private IMediaPlayer mPlayer;

        private List<ITrigger> mStartTriggers = new List<ITrigger>();
        private List<ITrigger> mStopTriggers = new List<ITrigger>();
        private List<ITrigger> mResetTriggers = new List<ITrigger>();

        private static Bitmap mDefaultBG;

        private static Bitmap DefaultBG {
            get {
                if (mDefaultBG == null) {
                    mDefaultBG = new Bitmap(50, 50);
                    using (Graphics g = Graphics.FromImage(mDefaultBG))
                        g.FillEllipse(Brushes.Black, 0, 0, 50, 50);
                }
                return mDefaultBG;
            }
        }

        public VideoState(string name, FrameOverlayManager mainWindow, string video, State parent, ITransitionStyle transition, IMediaPlayer player)
            : base(name, mainWindow.Manager, DefaultBG) {

            mPlayer = player;
            mMainWindow = mainWindow;
            mVideo = Path.GetFullPath(video);
            mPlayer.PlaybackFinished += mPlayer_VideoFinished;
            mPlayer.PlaybackStarted += mPlayer_VideoStarted;

            mTrigger = new SimpleTrigger();
            AddTransition(new StateTransition(Manager, this, parent, mTrigger, transition));
        }

        public VideoState(OverlayPlugin manager, XmlNode node, IMediaPlayer player)
            : base(manager, node) {

            mPlayer = player;
            mVideo = GetString(node, null, "File");
            if (mVideo == null)
                throw new ArgumentException("Unable to load VideoState. No File attribute specified.");
            mVideo = Path.GetFullPath(mVideo);
            if (!File.Exists(mVideo))
                throw new ArgumentException("Unable to load VideoState. The file '" + mVideo + "' does not exist.");

            mPlayer.PlaybackFinished += new Action(mPlayer_VideoFinished);
            mPlayer.PlaybackStarted += new Action(mPlayer_VideoStarted);
            mMainWindow = GetManager(manager, node, "video state");
            mBounds = manager.GetBounds(node, "video state");

            XmlAttribute toAttr = node.Attributes["FinishState"];
            if (toAttr != null && manager.GetState(toAttr.Value) != null) {
                mTrigger = new SimpleTrigger();
                ITransitionStyle transition = manager.GetTransition(node, "video state finish transition", new FeatureFrameTransitionFactory(new FeatureFadeFactory(), 2000), "Transition");
                if (transition == null) {
                    Logger.Debug("No transition specified for VideoState. using default 2s bitmap fade transition.");
                    transition = new FeatureFrameTransitionFactory(new FeatureFadeFactory(), 2000);
                }
                AddTransition(new StateTransition(Manager, this, manager.GetState(toAttr.Value), mTrigger, transition));
            }

            LoadTriggers(node, manager, "StartTriggers", mStartTriggers, new Action<ITrigger>(StartTriggered));
            LoadTriggers(node, manager, "StopTriggers", mStopTriggers, new Action<ITrigger>(StopTriggered));

            mRestartMode = GetBool(node, false, "RestartMode");
            if (mRestartMode) {
                LoadTriggers(node, manager, "ResetTriggers", mResetTriggers, new Action<ITrigger>(RestartTriggered));
            }
        }

        private void StartTriggered(ITrigger source) {
            Start();
        }

        private void StopTriggered(ITrigger source) {
            Stop(false);
        }

        private void RestartTriggered(ITrigger source) {
            mRestarted = true;
        }

        private void LoadTriggers(XmlNode node, OverlayPlugin manager, string triggerType, List<ITrigger> list, Action<ITrigger> onTrigger) {
            foreach (XmlElement child in GetChildrenOfChild(node, triggerType)) {
                ITrigger trigger = manager.GetTrigger(child, "video " + triggerType.TrimEnd('s'), null);
                if (trigger != null) {
                    if (!GetBool(child, false, "AlwaysOn")) {
                        list.Add(trigger);
                        trigger.Active = false;
                    }
                    if (trigger is IFeature)
                        AddFeature(trigger as IFeature);
                    trigger.Triggered += onTrigger;
                }
            }
        }

        protected override void TransitionToStart() {
            SetTriggers(true);
            ControlTriggers(mResetTriggers, true);
            Manager.ControlPointers = false;
        }

        protected override void TransitionToFinish() {
            if (!mRestartMode || mRestarted) {
                mRestarted = false;
                Start();
            }
            Manager.ControlPointers = false;
        }

        void mPlayer_VideoFinished() {
            if (mTrigger != null)
                mTrigger.Trigger();
        }

        void mPlayer_VideoStarted()
        {
            if (Active)
            {
                foreach (var transition in Transitions)
                    transition.Active = true;
            }
        }

        protected override void TransitionFromStart() {
            Stop(true);
        }

        protected override void TransitionFromFinish() {
            Stop(true);
            
        }

        private void Start() {
            if (!mAdded) {
                mMainWindow.AddControl(mPlayer.Player, mBounds);
                mAdded = true;
                mMainWindow.OverlayWindow.Invoke(() => Cursor.Hide());
            }
            foreach (var transition in Transitions)
                transition.Active = false;
            mPlayer.PlayVideo(mVideo);
            new Thread(() => SetTriggers(false)).Start();
        }

        private void Stop(bool remove) {
            if (mAdded) {
                mPlayer.StopPlayback();
                SetTriggers(true);
                 mMainWindow.OverlayWindow.Invoke(() => Cursor.Show());
                mMainWindow.RemoveControl(mPlayer.Player);
                mAdded = false;
                if (remove) {
                    ControlTriggers(mStartTriggers, false);
                    ControlTriggers(mResetTriggers, false);
                }
            }
        }

        private void SetTriggers(bool start) {
            ControlTriggers(mStartTriggers, start);
            ControlTriggers(mStopTriggers, !start);
        }

        private void ControlTriggers(List<ITrigger> triggers, bool active) {
            foreach (var trigger in triggers)
                trigger.Active = active;
        }
    }
}
