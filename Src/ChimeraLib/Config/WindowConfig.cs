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
using Chimera.Util;
using System.IO;
using OpenMetaverse;
using Chimera.Plugins;

namespace Chimera.Config {
    public class WindowConfig : ConfigFolderBase {
        public string Monitor;
        public double Height;
        public Vector3 TopLeft;
        public double Pitch;
        public double Yaw;
        public double Width;

        private string mWindow = "MainWindow";
        public bool Draw;
        public bool DrawEye;

        public WindowConfig() : base ("Frames") { }

        public WindowConfig(string window, params string[] args)
            : base(window, "Frames", args) {
            mWindow = window;
        }

        public override string Group {
            get { return "Window"; }
        }

        protected override void InitConfig() {
            AddCommandLineKey(false, "Monitor", "m");
            AddCommandLineKey(false, "LaunchOverlay", "l");
            AddCommandLineKey(false, "Fullscreen", "f");
            AddCommandLineKey(false, "MouseControl", "mc");
           

            Monitor = Get(false, "Monitor", "\\\\.\\DISPLAY2", "The monitor on which this window should render.");

            TopLeft = GetV(false, "TopLeft", new Vector3(1000f, -500f, 0f), "The position of the top left corner of the window in real world coordinates (mm).");
            Yaw = Get(false, "Yaw", 0.0, "The yaw for the direction the monitor faces in the real world.");
            Pitch = Get(false, "Pitch", 0.0, "The pitch for the direction the monitor faces in the real world.");
            Width = Get(false, "Width", 1000.0, "The width of the window in the real world (mm).");
            Height = Get(false, "Height", 1000.0, "The height of the window in the real world (mm).");

            Draw = Get(false, "Draw", true, "Whether to draw the window on the diagram.");
            DrawEye = Get(false, "DrawEye", true, "Whether to draw perspective lines for the window on the diagram.");
        }
    }
}
