using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;
using System.IO;
using OpenMetaverse;

namespace Chimera {
    public class CoordinatorConfig : ConfigBase {
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

        public CoordinatorConfig(params string[] args)
            : base(args) {
        }

        public CoordinatorConfig(string file, string[] args)
            : base("Coordinator", file, args) {
        }

        public override string Group {
            get { return "Core"; }
        }

        protected override void InitConfig() {
            AddCommandLineKey(true, "AutoRestart", "r");
            AddCommandLineKey(true, "CrashLogFile", "l");
            AddCommandLineKey(true, "TickLength", "tl");

            CrashLogFile = Get(true, "CrashLogFile", "CrashLog.log", "The file to log any crashes to.");
            AutoRestart = Get(true, "AutoRestart", false, "Whether to automatically restart the system any time it crashes.");

            TickLength = Get(true, "TickLength", 20, "How long each tick should be for any system that uses ticks.");

            EyePosition = GetV(true, "CameraPosition", new Vector3(-1000f, 0, 0), "The position of the eye in real world coordinates (mm).");
            Position = GetV(true, "CameraPosition", new Vector3(128f, 128f, 60f), "The position of the camera in virtual space coordinates.");
            Pitch = Get(true, "CameraPitch", 0.0, "The pitch of the virtual camera.");
            Yaw = Get(true, "CameraYaw", 0.0, "The yaw of the virtual camera.");

            XRegions = Get(true, "XRegions", 1, "The number of contiguous regions along the X axis that make up the environment.");
            YRegions = Get(true, "XRegions", 1, "The number of contiguous regions along the Y axis that make up the environment.");
            HeightmapDefault = Get(true, "HeightmapDefault", 0f, "The default heightmap height. Any square that does not have heightmap data set will revert to this.");
        }
    }
}
