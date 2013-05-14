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
        public double IdleFadeTime;

        public string MonitorBase;
        public int NumWindows;
        public int FirstWindow;

        public bool BackwardsCompatible;

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
            IdleFadeTime = Get(true, "IdleFadeTime", 3000, "How long the fade between the splash screen and the idle screen should take");

            ButtonFolder = Get(true, "ButtonFolder", "../Images/", "The folder where all button images are kept.");

            GUI = Get(true, "GUI", true, "Whether to launch the GUI when the system starts.");

            MonitorBase = Get(true, "MonitorBase", "MainWindow", "The name of the window. If NumWindows > 1 then X will be appended to NumWindow to get the name of the window. X goes from 'FirstWindow' to 'FirstWindow + NumWindows'");
            NumWindows = Get(true, "NumWindows", 1, "The number of windows to launch.");
            FirstWindow = Get(true, "FirstWindow", 1, "The first window to launch. If NumWindows > 1 then an index will be appended to MonitorBase to get the window names.");

            BackwardsCompatible = Get(true, "BackwardsCompatible", false, "If true, no unusual packets will be injected into the viewer. This will disable remote control and frustum control.");
        }
    }
}
