using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay.States {
    public class VideoState : State {
        private string mVideo;
        private string mPlayer = "C:\\Program Files (x86)\\VideoLAN\\VLC\\vlc";
        private string mArgs = "-f";

        public VideoState(string name, StateManager manager, string video)
            : base(name, manager) {

            mVideo = video;
        }

        public override IWindowState CreateWindowState(Window window) {
            throw new NotImplementedException();
        }

        public override void TransitionToStart() {
            throw new NotImplementedException();
        }

        protected override void TransitionToFinish() {
            throw new NotImplementedException();
        }

        protected override void TransitionFromStart() {
            throw new NotImplementedException();
        }

        public override void TransitionFromFinish() {
            throw new NotImplementedException();
        }
    }
}
