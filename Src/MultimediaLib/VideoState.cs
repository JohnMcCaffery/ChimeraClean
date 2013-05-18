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

namespace Chimera.Multimedia {
    public class VideoStateFactory : IStateFactory {
        public string Name {
            get { return "VideoState"; }
        }

        public State Create(StateManager manager, XmlNode node) {
            return new VideoState(manager, node);
        }

        public State Create(StateManager manager, XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }
    }

    public class VideoState : ImageBGState {
        private string mVideo;
        private WindowOverlayManager mMainWindow;
        private Process mPlayer;
        private SimpleTrigger mTrigger;
        private RectangleF mBounds = new RectangleF(0f, 0f, 1f, 1f);
        private bool mAdded;

        private List<ITrigger> mStartTriggers = new List<ITrigger>();        private List<ITrigger> mStopTriggers = new List<ITrigger>();

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

        public VideoState(string name, WindowOverlayManager mainWindow, string video, State parent, IWindowTransitionFactory transition)
            : base(name, mainWindow.Window.Coordinator.StateManager, DefaultBG) {

            mMainWindow = mainWindow;
            mVideo = Path.GetFullPath(video);
            VideoManager.VideoFinished += VideoManager_VideoFinished;

            mTrigger = new SimpleTrigger();
            AddTransition(new StateTransition(Manager, this, parent, mTrigger, transition));
        }

        public VideoState(StateManager manager, XmlNode node)
            : base(manager, node) {


            mVideo = GetString(node, null, "File");
            if (mVideo == null)
                throw new ArgumentException("Unable to load VideoState. No File attribute specified.");
            mVideo = Path.GetFullPath(mVideo);
            if (!File.Exists(mVideo))
                throw new ArgumentException("Unable to load VideoState. The file '" + mVideo + "' does not exist.");

            mBounds = GetBounds(node, "video state");

            XmlAttribute toAttr = node.Attributes["FinishState"];
            if (toAttr != null && manager.GetState(toAttr.Value) != null) {
                mTrigger = new SimpleTrigger();
                IWindowTransitionFactory transition = manager.GetTransition(node);
                if (transition == null)
                    transition = manager.DefaultTransition;
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

        protected override void TransitionToFinish() { }

        void VideoManager_VideoFinished() {
            if (mTrigger != null)
                mTrigger.Trigger();
            mMainWindow.RemoveControl(VideoManager.Player);
            mAdded = false;
        }

        protected override void TransitionFromStart() {
            VideoManager.StopPlayback();
            foreach (var trigger in mStartTriggers)
                trigger.Active = false;
            foreach (var trigger in mStopTriggers)
                trigger.Active = false;
        }

        public override void TransitionFromFinish() {
            if (mAdded) {
                mMainWindow.RemoveControl(VideoManager.Player);
                mAdded = false;
            }
        }

        private void Start() {            mMainWindow.AddControl(VideoManager.Player, mBounds);
            mAdded = true;
            VideoManager.PlayVideo(mVideo);
            foreach (var trigger in mStartTriggers)
                trigger.Active = false;
            foreach (var trigger in mStopTriggers)
                trigger.Active = true;
        }

        private void Stop() {
            VideoManager.StopPlayback();
            foreach (var trigger in mStartTriggers)
                trigger.Active = false;
            foreach (var trigger in mStopTriggers)
                trigger.Active = true;
        }
    }
}
