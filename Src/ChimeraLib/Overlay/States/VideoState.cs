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
        private IMediaPlayer mPlayer;

        private List<ITrigger> mStartTriggers = new List<ITrigger>();
        private List<ITrigger> mStopTriggers = new List<ITrigger>();

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
            : base(name, mainWindow.Window.Coordinator.StateManager, DefaultBG) {

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

            foreach (XmlElement child in node.ChildNodes) {
                if (child is XmlElement) {
                    ITrigger trigger = manager.GetTrigger(child);
                    if (trigger != null) {
                        if (GetBool(child, false, "TriggerStart"))
                            mStartTriggers.Add(trigger);
                        else
                            mStopTriggers.Add(trigger);
                    }
                }
            }
        }

        public override void TransitionToStart() {
            foreach (var window in Manager.Coordinator.Windows)
                window.OverlayManager.ControlPointer = false;
        }

        protected override void TransitionToFinish() {
            Start();
        }

        void mPlayer_VideoFinished() {
            if (mTrigger != null)
                mTrigger.Trigger();
            mMainWindow.RemoveControl(mPlayer.Player);
            mAdded = false;
        }

        protected override void TransitionFromStart() {
            mPlayer.StopPlayback();
            foreach (var trigger in mStartTriggers)
                trigger.Active = false;
            foreach (var trigger in mStopTriggers)
                trigger.Active = false;
        }

        public override void TransitionFromFinish() {
            if (mAdded) {
                mMainWindow.RemoveControl(mPlayer.Player);
                mAdded = false;
            }
        }

        private void Start() {
            mMainWindow.AddControl(mPlayer.Player, mBounds);
            mAdded = true;
            mPlayer.PlayVideo(mVideo);
            foreach (var trigger in mStartTriggers)
                trigger.Active = false;
            foreach (var trigger in mStopTriggers)
                trigger.Active = true;
        }

        private void Stop() {
            mPlayer.StopPlayback();
            foreach (var trigger in mStartTriggers)
                trigger.Active = false;
            foreach (var trigger in mStopTriggers)
                trigger.Active = true;
        }
    }
}
