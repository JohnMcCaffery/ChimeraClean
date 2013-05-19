using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay.Drawables;
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

        public State Create(StateManager manager, XmlNode node) {
            Console.WriteLine("Creating Video State");
            return new VideoState(manager, node, mPlayer);
        }

        public State Create(StateManager manager, XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }
    }

    public class VideoState : ImageBGState {
        private string mVideo;
        private WindowOverlayManager mMainWindow;
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

        public VideoState(string name, WindowOverlayManager mainWindow, string video, State parent, IWindowTransitionFactory transition, IMediaPlayer player)
            : base(name, mainWindow.Manager, DefaultBG) {

            mPlayer = player;
            mMainWindow = mainWindow;
            mVideo = Path.GetFullPath(video);
            mPlayer.PlaybackFinished += mPlayer_VideoFinished;

            mTrigger = new SimpleTrigger();
            AddTransition(new StateTransition(Manager, this, parent, mTrigger, transition));
        }

        public VideoState(StateManager manager, XmlNode node, IMediaPlayer player)
            : base(manager, node) {

            mPlayer = player;
            mVideo = GetString(node, null, "File");
            if (mVideo == null)
                throw new ArgumentException("Unable to load VideoState. No File attribute specified.");
            mVideo = Path.GetFullPath(mVideo);
            if (!File.Exists(mVideo))
                throw new ArgumentException("Unable to load VideoState. The file '" + mVideo + "' does not exist.");

            mPlayer.PlaybackFinished += new Action(mPlayer_VideoFinished);
            mMainWindow = GetManager(manager, node, "video state");
            mBounds = manager.GetBounds(node, "video state");

            XmlAttribute toAttr = node.Attributes["FinishState"];
            if (toAttr != null && manager.GetState(toAttr.Value) != null) {
                mTrigger = new SimpleTrigger();
                IWindowTransitionFactory transition = manager.GetTransition(node);
                if (transition == null) {
                    Console.WriteLine("No transition specified for VideoState. using default transition " + manager.DefaultTransition.GetType().Name.Replace("Factory", "") + ".");
                    transition = manager.DefaultTransition;
                }
                AddTransition(new StateTransition(Manager, this, manager.GetState(toAttr.Value), mTrigger, transition));
            }

            LoadTriggers(node, manager, "StartTriggers", mStartTriggers, new Action(StartTriggered));
            LoadTriggers(node, manager, "StopTriggers", mStopTriggers, new Action(StopTriggered));

            mRestartMode = GetBool(node, false, "RestartMode");
            if (mRestartMode) {
                LoadTriggers(node, manager, "ResetTriggers", mResetTriggers, new Action(RestartTriggered));
            }
        }

        private void StartTriggered() {
            Start();
        }

        private void StopTriggered() {
            Stop(false);
        }

        private void RestartTriggered() {
            mRestarted = true;
        }

        private void LoadTriggers(XmlNode node, StateManager manager, string triggerType, List<ITrigger> list, Action onTrigger) {
            foreach (XmlElement child in GetChildrenOfChild(node, triggerType)) {
                ITrigger trigger = manager.GetTrigger(child);
                if (trigger != null) {
                    if (!GetBool(child, false, "AlwaysOn"))
                        list.Add(trigger);
                    if (trigger is IDrawable)
                        AddFeature(trigger as IDrawable);
                    trigger.Triggered += onTrigger;
                }
            }
        }

        public override void TransitionToStart() {
            SetTriggers(true);
            ControlTriggers(mResetTriggers, true);
            foreach (var manager in Manager.OverlayManagers)
                manager.ControlPointer = false;
        }

        protected override void TransitionToFinish() {
            if (!mRestartMode || mRestarted) {
                mRestarted = false;
                Start();
            }
        }

        void mPlayer_VideoFinished() {
            if (mTrigger != null)
                mTrigger.Trigger();
        }

        protected override void TransitionFromStart() {
            Stop(true);
        }

        public override void TransitionFromFinish() {
            Stop(true);
        }

        private void Start() {
            if (!mAdded) {
                mMainWindow.AddControl(mPlayer.Player, mBounds);
                mAdded = true;
            }
            mPlayer.PlayVideo(mVideo);
            SetTriggers(false);
        }

        private void Stop(bool remove) {
            if (mAdded) {
                mPlayer.StopPlayback();
                SetTriggers(true);
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
