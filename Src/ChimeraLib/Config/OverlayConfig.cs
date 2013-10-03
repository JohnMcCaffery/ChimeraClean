using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;

namespace Chimera.Config {
    public class OverlayConfig : ConfigFolderBase {
        public bool LaunchOverlay;
        public bool Fullscreen;
        public bool ControlPointer;
        public bool AlwaysOnTop;

        public override string Group {
            get { return "Overlay"; }
        }

        public OverlayConfig(params string[] args)
            : base("Overlay", args) { }

        protected override void InitConfig() {
            LaunchOverlay = Get(true, "LaunchOverlay", false, "Whether to launch an overlay for this window at startup.");
            Fullscreen = Get(true, "Fullscreen", false, "Whether to launch the overlay fullscreen.");
            ControlPointer = Get(true, "ControlPointer", false, "Whether the overlay should take control of the pointer and move it when the pointer is over the window.");
            AlwaysOnTop = Get(true, "AlwaysOnTop", true, "Whether the overlay window should force itself to stay on top at all times or let it's order in the Z buffer be freely decided.");
        }
    }
}
