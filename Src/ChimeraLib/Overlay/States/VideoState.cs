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

namespace Chimera.Overlay.States {
    public class VideoState : State {
        private class Trigger : ITrigger {
            public event Action Triggered;

            public bool Active {
                get { return true; }
                set { }
            }

            public void TriggerEvt() {
                if (Triggered != null)
                    Triggered();
            }
        }

        private string mVideo;
        private string mPlayerExe = "C:\\Program Files (x86)\\VideoLAN\\VLC\\vlc";
        private string mArgs = "-f --video-on-top --play-and-exit";
        private string mMainWindow;
        private Process mPlayer;
        private Trigger mTrigger;

        public VideoState(string name, StateManager manager, string mainWindow, string video, State parent, IWindowTransitionFactory transition)
            : base(name, manager) {

            mMainWindow = mainWindow;
            mVideo = Path.GetFullPath(video);
            mArgs = mArgs + " " + mVideo;

            mTrigger = new Trigger();
            AddTransition(new StateTransition(manager, this, parent, mTrigger, transition));
        }

        public override IWindowState CreateWindowState(Window window) {
            return new VideoWindow(window.OverlayManager);
        }

        public override void TransitionToStart() {
            Manager.Coordinator[mMainWindow].OverlayManager.AlwaysOnTop = false;
            mPlayer = ProcessWrangler.InitProcess(mPlayerExe, Path.GetDirectoryName(mPlayerExe), mArgs);
            mPlayer.EnableRaisingEvents = true;
            mPlayer.Start();
            mPlayer.Exited += new EventHandler(mPlayer_Exited);
            Thread.Sleep(50);
            ProcessWrangler.SetMonitor(mPlayer, Manager.Coordinator[mMainWindow].Monitor);

            Console.WriteLine(mPlayer.StartInfo.FileName + " " + mPlayer.StartInfo.Arguments);

            foreach (var window in Manager.Coordinator.Windows)
                window.OverlayManager.ControlPointer = false;
        }

        protected override void TransitionToFinish() { }

        void mPlayer_Exited(object sender, EventArgs e) {
            mPlayer = null;
            mTrigger.TriggerEvt();
            //if (Transitions.Length > 0)
                //Manager.BeginTransition(Transitions[0]);
        }

        protected override void TransitionFromStart() { }

        public override void TransitionFromFinish() {
            if (mPlayer != null)
                ProcessWrangler.PressKey(mPlayer, "{F4}", false, true, false);
        }

        private class VideoWindow : WindowState {
            public VideoWindow(WindowOverlayManager manager)
                : base(manager) {
            }

            public override bool Active {
                get { return base.Active; }
                set {
                    base.Active = value;
                    Manager.AlwaysOnTop = !value;
                }
            }

            public override void DrawStatic(Graphics graphics) {
                graphics.FillRectangle(Brushes.Black, Clip);
                base.DrawStatic(graphics);
            }
        }
    }
}
