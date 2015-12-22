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
    public class CoreConfig : ConfigFolderBase {
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
        public bool EnableInputUpdates;

	//Shared Screenshot 
        public string ScreenshotFolder;

	//Screenshot Sequence Presser
        public string Key;
        public double IntervalMS;
        public double StopM;
        public bool AutoShutdown;
        public string ScreenshotFile;

	//Photosphere
        public int PhotosphereCaptureDelayMS;
        public bool PhotosphereCapture3D;
        public int PhotosphereOutputWidth;
        public string PhotosphereName;
        public string PhotosphereBatcherExe;
        public bool PhotosphereAutoStartBatch;
        public bool PhotosphereAddBatch;
        public string PhotosphereFolder;

        public CoreConfig(params string[] args)
            : base("Main", args) {
        }

        public CoreConfig(string file, string[] args)
            : base("Main", args) {
        }

        public override string Group {
            get { return "Main"; }
        }

        protected override void InitConfig() {
            AddCommandLineKey(true, "AutoRestart", "r");
            AddCommandLineKey(true, "CrashLogFile", "l");
            AddCommandLineKey(true, "TickLength", "tl");

            CrashLogFile = GetStr("CrashLogFile", "CrashLog.log", "The file to log any crashes to.");
            AutoRestart = Get("AutoRestart", false, "Whether to automatically restart the system any time it crashes.");
            EnableInputUpdates = Get("EnableInputUpdates", false, "Whether to enable input by default.");

            TickLength = Get("TickLength", 20, "How long each tick should be for any system that uses ticks.");

            EyePosition = GetV("Camera", "EyePosition", new Vector3(0f, 0, 0), "The position of the eye in real world coordinates (mm).");
            Position = GetV("Camera", "CameraPosition", new Vector3(128f, 128f, 60f), "The position of the camera in virtual space coordinates.");
            Pitch = Get("Camera", "CameraPitch", 0.0, "The pitch of the virtual camera.");
            Yaw = Get("Camera", "CameraYaw", 0.0, "The yaw of the virtual camera.");

            XRegions = Get("Heightmap", "XRegions", 1, "The number of contiguous regions along the X axis that make up the environment.");
            YRegions = Get("Heightmap", "YRegions", 1, "The number of contiguous regions along the Y axis that make up the environment.");
            HeightmapDefault = Get("Heightmap", "HeightmapDefault", 0f, "The default heightmap height. Any square that does not have heightmap data set will revert to this.");
            Frames = GetStr("Frames", "MainWindow", "The name of all the windows to load, separated by commas.").Split(',');

            Get("Plugins", "|PLUGIN|Enabled", true, "Set whether |PLUGIN| is enabled at start-up.");

	    //Screenshot shared
            ScreenshotFolder = GetFolderSection("Screenshot", "Folder", "Images/TimeLapse/", "The folder where captured images will be stored.");

            //Button Presser
            IntervalMS = Get("ScreenshotSequence", "IntervalS", .5, "How long (in seconds) between each screenshot.") * 1000.0;
            StopM = Get("ScreenshotSequence", "ShutdownM", 1, "How many minutes the screenshot sequence should run before stopping.");
            AutoShutdown = Get("ScreenshotSequence", "AutoShutdown", false, "Whether to shut down the viewer when screenshots have stopped.");
            ScreenshotFile = GetSection("ScreenshotSequence", "File", "TimeLapse", "The prefix for all screenshot files, will be appended with _X.");

	    //Photosphere
            PhotosphereCaptureDelayMS = Get("Photosphere", "CaptureDelayMS", 150, "How long (in milliseconds) to wait for the view to adjust before capturing an image.");
            PhotosphereCapture3D = Get("Photosphere", "Capture3D", false, "Whether to capture images in triplets (centre, left, right) or just take one image at each orientation.");
            PhotosphereOutputWidth = Get("Photosphere", "OutputWidth", 8192, "The width of the final Photosphere that will be produced.");
            PhotosphereName = GetSection("Photosphere", "Name", "Photosphere", "The name for this Photosphere.");
            PhotosphereAutoStartBatch = Get("Photosphere", "AutostartBatch", false, "If the output PTO file is to be added to the batcher then this controls whether the batcher will be started once the PTO file is added.");
            PhotosphereBatcherExe = GetFileSection("Photosphere", "BatcherExe", "C:/Program Files/Hugin/bin/PTBatcherGUI.exe", "The hugin batcher executable.");
            PhotosphereAddBatch = Get("Photosphere", "Batch", false, "Whether to add the PTO project to hugin's batcher once the photosphere has been created.");
            PhotosphereFolder = GetFolderSection("Photosphere", "Folder", "Images/Photospheres", "The folder where photosphere material will be stored.");

        }

        internal bool PluginEnabled(ISystemPlugin plugin) {
            return Get("Plugins", plugin.Name, plugin.Enabled, "Set whether a plugin is enabled.");
        }
    }
}
