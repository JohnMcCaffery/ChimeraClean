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
using Chimera.Kinect.Interfaces;
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

namespace Chimera.Launcher {
    public class MinimumLauncher : Launcher {
        private SetWindowViewerOutput mMainWindowProxy = new SetWindowViewerOutput("MainWindow");

        protected override ISystemPlugin[] GetInputs() {
            return new ISystemPlugin[] { 
                new KinectCamera(), 
                new TimespanAxisPlugin(),
                new SimpleCursor(),
                new RaiseArmHelpTrigger(),
                new KBMousePlugin(), 
                new MousePlugin(), 
                new HeightmapPlugin(), 
                new FlythroughPlugin(), 
                mMainWindowProxy 
            };
        }

        protected override Window[] GetWindows() {
            return new Window[] { new Window("MainWindow", mMainWindowProxy)};
        }

        protected override void InitOverlay() {
        }
    }
}
