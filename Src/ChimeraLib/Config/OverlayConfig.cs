using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;

namespace Chimera.Config {
    public class OverlayConfig : ConfigFolderBase {
        public string IdleState;
        public string HomeState;
        public bool InitOverlay;
        public bool UseClicks;
        public int IdleTimeoutMs;

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
            InitOverlay = Get(true, "InitOverlay", true, "If true the overlay will be initialised. If false, no overlay will be loaded.");
            UseClicks = Get(true, "UseClicks", false, "Whether to use click triggers rather than hover triggers for selecting items.");
            HomeState = Get(true, "HomeState", "None", "The state which the system should start in and return to whenever the idle state ceases.");
            IdleState = Get(true, "IdleState", "None", "The state which the system should launch when idle. If not set no idle state will be configured.");
            IdleTimeoutMs = Get(true, "IdleTimeout", 30000, "How long the system should be left alone for before it's considered idle.");

            LaunchOverlay = Get(false, "LaunchOverlay", false, "Whether to launch an overlay for this window at startup.");
            Fullscreen = Get(false, "Fullscreen", false, "Whether to launch the overlay fullscreen.");
            ControlPointer = Get(false, "ControlPointer", false, "Whether the overlay should take control of the pointer and move it when the pointer is over the window.");
            AlwaysOnTop = Get(false, "AlwaysOnTop", true, "Whether the overlay window should force itself to stay on top at all times or let it's order in the Z buffer be freely decided.");
        }
    }
}
