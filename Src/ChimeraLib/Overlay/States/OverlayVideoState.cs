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
using Chimera.Overlay.Drawables;
using System.Drawing;
using System.Diagnostics;
using Chimera.Util;
using System.IO;
using System.Threading;
using Chimera.Overlay.Triggers;

namespace Chimera.Overlay.States {
    public class OverlayVideoState : State {

        private string mVideo;
        private WindowOverlayManager mMainWindow;
        private Process mPlayer;
        private SimpleTrigger mTrigger;

        public OverlayVideoState(string name, StateManager manager, WindowOverlayManager mainWindow, string video, State parent, IWindowTransitionFactory transition)
            : base(name, manager) {

            mMainWindow = mainWindow;
            mVideo = Path.GetFullPath(video);
            mMainWindow.VideoFinished += mMainWindow_VideoFinished;

            mTrigger = new SimpleTrigger();
            AddTransition(new StateTransition(manager, this, parent, mTrigger, transition));
        }

        public override IWindowState CreateWindowState(Window window) {
            return new VideoWindow(window.OverlayManager);
        }

        public override void TransitionToStart() {
            foreach (var window in Manager.Coordinator.Windows)
                window.OverlayManager.ControlPointer = false;
        }

        protected override void TransitionToFinish() {
            mMainWindow.PlayVideo(mVideo);
        }

        void mMainWindow_VideoFinished() {
            mTrigger.Trigger();
        }

        protected override void TransitionFromStart() { }

        public override void TransitionFromFinish() { }

        private class VideoWindow : WindowState {
            public VideoWindow(WindowOverlayManager manager)
                : base(manager) {
            }

            public override void DrawStatic(Graphics graphics) {
                graphics.FillRectangle(Brushes.Black, Clip);
                base.DrawStatic(graphics);
            }
        }
    }
}
