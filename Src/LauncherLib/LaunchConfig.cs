using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;
using Chimera.Config;

namespace Chimera.Launcher {
    public class LauncherConfig : ConfigFolderBase {
        public string Launcher;

        public string IdleState;
        public string HomeState;
        public bool InitOverlay;
        public bool UseClicks;
        public int IdleTimeoutMs;

        public string ButtonFolder;
        public bool GUI;

        public override string Group {
            get { return "Launch"; }
        }

        public LauncherConfig(params string[] args)
            : base("Overlay", args) { }

        protected override void InitConfig() {
            Launcher = Get(true, "Launcher", "Launcher.MinimumLauncher", "The launcher which will run chimera.");

            InitOverlay = Get(true, "InitOverlay", true, "If true the overlay will be initialised. If false, no overlay will be loaded.");
            UseClicks = Get(true, "UseClicks", false, "Whether to use click triggers rather than hover triggers for selecting items.");
            HomeState = Get(true, "HomeState", "None", "The state which the system should start in and return to whenever the idle state ceases.");
            IdleState = Get(true, "IdleState", "None", "The state which the system should launch when idle. If not set no idle state will be configured.");
            IdleTimeoutMs = Get(true, "IdleTimeout", 30000, "How long the system should be left alone for before it's considered idle.");

            ButtonFolder = Get(true, "ButtonFolder", "../Images/", "The folder where all button images are kept.");

            GUI = Get(true, "GUI", true, "Whether to launch the GUI when the system starts.");
        }
    }
}
