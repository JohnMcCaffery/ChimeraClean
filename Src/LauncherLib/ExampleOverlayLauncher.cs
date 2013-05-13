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
using Chimera.Kinect;
using Chimera.Plugins;
using Chimera.OpenSim;
using Chimera.Overlay.Triggers;
using System.Drawing;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay.Drawables;
using Chimera.Overlay.Transitions;
using Chimera.Overlay.States;
using Chimera.Overlay;
using Chimera.Flythrough.Overlay;
using Joystick;
using Chimera.Kinect.GUI;
using Chimera.Flythrough;
using Chimera.Kinect.Overlay;
using Touchscreen.Overlay;

namespace Chimera.Launcher {
    public class ExampleOverlayLauncher : Launcher {
        protected override void InitOverlay() {
            ImageBGState splash = new ImageBGState("Splash", Coordinator.StateManager, "../Images/Example/ExampleBG.png");
            State idle = new FlythroughState("Idle", Coordinator.StateManager, "../Flythroughs/SimpleFlythrough.xml");
            State explore;
            if (Config.UseClicks)
                explore = new TouchscreenState("ExploreFree", false, Coordinator);
            else
                explore = new KinectControlState("ExploreFree", Coordinator.StateManager, false);

            Rectangle clip = new Rectangle(0, 0, 1920, 1080);
            IWindowTransitionFactory fadeOut = new OpacityFadeOutTransitionFactory(5000);

            InvisTrans(splash, explore, new Point(300, 300), new Point(800, 600), clip, mMainWindowProxy.Window, fadeOut);

            Coordinator.StateManager.AddState(splash);
            Coordinator.StateManager.AddState(explore);
            Coordinator.StateManager.AddState(idle);
        }
    }
}
