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

namespace Chimera.Config {
    public class CoordinatorConfig : ConfigFolderBase {
        public string CrashLogFile;
        public int TickLength;
        public bool AutoRestart;
        public Vector3 Position;
        public Vector3 EyePosition;
        public double Pitch;
        public double Yaw;
        public int XRegions;
        public int YRegions;
        public float HeightmapDefault;
        public double OverlayOpacity;
        public string[] Frames;

        public CoordinatorConfig(params string[] args)
            : base("Main", args) {
        }

        public CoordinatorConfig(string file, string[] args)
            : base("Main", args) {
        }

        public override string Group {
            get { return "Main"; }
        }

        protected override void InitConfig() {
            AddCommandLineKey(true, "AutoRestart", "r");
            AddCommandLineKey(true, "CrashLogFile", "l");
            AddCommandLineKey(true, "TickLength", "tl");

            CrashLogFile = Get(true, "CrashLogFile", "CrashLog.log", "The file to log any crashes to.");
            AutoRestart = Get(true, "AutoRestart", false, "Whether to automatically restart the system any time it crashes.");

            TickLength = Get(true, "TickLength", 20, "How long each tick should be for any system that uses ticks.");

            EyePosition = GetV("Camera", "EyePosition", new Vector3(0f, 0, 0), "The position of the eye in real world coordinates (mm).");
            Position = GetV("Camera", "CameraPosition", new Vector3(128f, 128f, 60f), "The position of the camera in virtual space coordinates.");
            Pitch = Get("Camera", "CameraPitch", 0.0, "The pitch of the virtual camera.");
            Yaw = Get("Camera", "CameraYaw", 0.0, "The yaw of the virtual camera.");

            XRegions = Get("Heightmap", "XRegions", 1, "The number of contiguous regions along the X axis that make up the environment.");
            YRegions = Get("Heightmap", "YRegions", 1, "The number of contiguous regions along the Y axis that make up the environment.");
            HeightmapDefault = Get("Heightmap", "HeightmapDefault", 0f, "The default heightmap height. Any square that does not have heightmap data set will revert to this.");
            Frames = Get(true, "Frames", "MainWindow", "The name of all the windows to load, separated by commas.").Split(',');

            Get("Plugins", "|PLUGIN|Enabled", true, "Set whether |PLUGIN| is enabled at start-up.");
        }

        internal bool PluginEnabled(ISystemPlugin plugin) {
            return Get("Plugins", plugin.Name, true, "Set whether a plugin is enabled.");
        }
    }
}
