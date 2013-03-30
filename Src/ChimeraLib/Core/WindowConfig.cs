using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;
using System.IO;
using OpenMetaverse;

namespace Chimera {
    public class WindowConfig : ConfigBase {
        public string Monitor;
        public bool LaunchOverlay;
        public bool Fullscreen;
        public bool ControlPointer;
        public double Width;
        public double Height;
        public Vector3 TopLeft;
        public double Pitch;
        public double Yaw;


        public WindowConfig(params string[] args) : base(args) { }

        public WindowConfig(string name, string file, string[] args)
            : base(name, file, args) {
        }

        public override string Group {
            get { return "Window"; }
        }

        protected override void InitConfig() {
            AddCommandLineKey(false, "Monitor", "m");
            AddCommandLineKey(false, "LaunchOverlay", "l");
            AddCommandLineKey(false, "Fullscreen", "f");
            AddCommandLineKey(false, "MouseControl", "mc");
           

            Monitor = Get(false, "Monitor", "CrashLog.log", "The monitor on which this window should render.");
            LaunchOverlay = Get(false, "LaunchOverlay", false, "Whether to launch an overlay for this window at startup.");
            Fullscreen = Get(false, "Fullscreen", false, "Whether to launch the overlay fullscreen.");
            ControlPointer = Get(false, "ControlPointer", false, "Whether the overlay should take control of the pointer and move it when the pointer is over the window.");

            TopLeft = GetV(false, "TopLeft", new Vector3(1000f, 0f, 0f), "The position of the top left corner of the window in real world coordinates (mm).");
            Yaw = Get(false, "Yaw", 0.0, "The yaw for the direction the monitor faces in the real world.");
            Pitch = Get(false, "Pitch", 0.0, "The pitch for the direction the monitor faces in the real world.");
            Width = Get(false, "Width", 0.0, "The width of the window in the real world (mm).");
            Height = Get(false, "Height", 0.0, "The height of the window in the real world (mm).");
        }
    }
}
