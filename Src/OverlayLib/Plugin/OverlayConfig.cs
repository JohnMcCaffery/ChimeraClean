using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;
using Chimera.Config;

namespace Chimera.Overlay {
    public class OverlayConfig : ConfigFolderBase {
        public string Launcher;

        public string InterfaceMode;
        public string OverlayFile;
        public bool ControlPointer;
        public bool AlwaysOnTop;
        public bool LaunchOverlay;
        public bool Fullscreen;
        public bool IdleEnabled;
        public string DefaultCursor;

        public override string Group {
            get { return "Overlay"; }
        }

        public OverlayConfig(params string[] args)
            : base("Overlay", args) { }

        protected override void InitConfig() {
            Launcher = GetStr("Launcher", "Launcher.MinimumLauncher", "The launcher which will run chimera.");

            Fullscreen = Get("Fullscreen", true, "Whether overlay windows should start full screen.");
            LaunchOverlay = Get("LaunchOverlay", true, "Whether to create the overlay windows when the system starts up.");
            AlwaysOnTop = Get("AlwaysOnTop", true, "Whether to force overlay windows to always be on top.");
            ControlPointer = Get("ControlPointer", true, "Whether to allow the overlay to move the system pointer.");
            InterfaceMode = GetStr("InterfaceMode", "HoverBased", "The mode the overlay is in. Use this to choose between different configs embedded in the same Overlay config file.");
            OverlayFile = GetFile("OverlayFile", null, "The overlay file to load.");
            IdleEnabled = Get("IdleEnabled", true, "Whether the idle triggers should be activated.");

            DefaultCursor = GetFile("DefaultCursor", "Cursors/cursor.cur", "The default cursor file to use on the overlay as the overlay cursor.");
        }
    }
}
