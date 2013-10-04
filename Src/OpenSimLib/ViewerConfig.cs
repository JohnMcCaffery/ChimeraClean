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
using Chimera.Config;

namespace Chimera.OpenSim {
    internal class ViewerConfig : ConfigFolderBase {
        public static readonly string DEFAULT_LOGINURI = "http://localhost:9000";
        public static readonly string DEFAULT_CLIENT_EXE = "C:\\Program Files (x86)\\Firestorm-Release\\Firestorm-Release.exe";
        public static readonly string DEFAULT_MASTER_ADDRESS = "127.0.0.1";
        public static readonly int DEFAULT_MASTER_PORT = 8090;
        public static readonly int DEFAULT_PROXY_PORT = 8080;
        public static int CURRENT_PORT = DEFAULT_PROXY_PORT;

        public string ProxyLoginURI;
        public string ViewerExecutable;
        public string LoginFirstName;
        public string LoginLastName;
        public string LoginPassword;
        public string LoginGrid;
        public string ViewerWorkingDirectory;
        public string CrashLogFile;
        public string ViewerArguments;
        public string ViewerToggleHUDKey;
        public bool UseGrid;
        public bool AutoLoginClient;
        public bool AutoStartProxy;
        public bool AutoStartViewer;
        public bool AutoRestartViewer;
        public bool ControlCamera;
        public bool Fullscreen;
        public int ProxyPort;

        public bool BackwardsCompatible;
        public string StartupKeyPresses;
        public float DeltaScale;
        public bool ControlFrustum;
        public bool UseThread;
        public bool CheckForPause;
        public bool AllowFly;
        public bool ControlCameraPosition;
        public int StartStagger;

        public override string Group {
            get { return "SecondLifeViewer"; }
        }

        public ViewerConfig(params string[] args)
            : base("OpenSim", args) {
        }

        public ViewerConfig(string windowName, params string[] args)
            : base(windowName, "OpenSim", args) {
        }

        protected override void InitConfig() {
            /*
            AddCommandLineParam("General", "ViewerExe", "v");
            AddCommandLineParam("General", "WorkingDirectory", "d");
            AddCommandLineParam("General", "ViewerArguments", "a");
            AddCommandLineParam("General", "ViewerToggleHUDKey", "k");
            AddCommandLineParam("General", "UseGrid", "ug");
            AddCommandLineParam("General", "UseSetFollowCamPackets", "uf");
            AddCommandLineParam("General", "EnableWindowPackets", "ew");
            AddCommandLineParam("General", "ProxyGrid", "g");
            AddCommandLineParam("General", "LoginURI", "u");
            AddCommandLineParam("General", "MasterAddress", "ma");
            AddCommandLineParam("General", "MasterPort", "mp");
            AddCommandLineParam("General", "WorldPosition", "cw");
            AddCommandLineParam("General", "WorldPitch", "pw");
            AddCommandLineParam("General", "WorldYaw", "yw");
            AddCommandLineParam("General", "AutoRestart", "r");
            AddCommandLineParam("General", "CrashLogFile", "l");
            AddCommandLineParam(Name, "ControlCamera", "c");
            AddCommandLineParam(Name, "AutoStartProxy", "ap");
            AddCommandLineParam(Name, "AutoStartViewer", "av");
            AddCommandLineParam(Name, "ProxyPort", "p");
            AddCommandLineParam(Name, "FirstName", "fn");
            AddCommandLineParam(Name, "LastName", "l");
            AddCommandLineParam(Name, "Password", "pw");
            AddCommandLineParam(Name, "Fullscreen", "pw");
            */
            
            ViewerExecutable = Path.GetFullPath(Get(true, "ViewerExe", DEFAULT_CLIENT_EXE, "The executable that runs the viewer."));
            ViewerWorkingDirectory = Get(true, "WorkingDirectory", Path.GetDirectoryName(ViewerExecutable), "The workign directory for the viewer executable.");
            ViewerArguments = Get(true, "ViewerArguments", "", "Any arguments to be passed to the viewer when it starts.");
            ViewerToggleHUDKey= Get(true, "ViewerToggleHUDKey", "%^{F1}", "The key press that will toggle the HUD on and off in the viewer.");
            ProxyLoginURI = Get(true, "LoginURI", DEFAULT_LOGINURI, "The URI of the server the proxy should proxy.");
            UseGrid = Get(true, "UseGrid", false, "Whether to login using the --grid or --loginuri command line parameter to specify the login target.");
            DeltaScale = Get(true, "DeltaScale", .25f, "How much to scale delta values by when using remote control.");

            ControlCameraPosition = Get(true, "ControlCameraOffset", false, "Whether to use SetFollowCamProperties packets to control the camera position.");
            AllowFly = Get(true, "AllowFly", false, "Whether to allow the avatar to fly in delta mode.");

            CheckForPause = Get(true, "CheckForPause", false, "Whether the proxy controller should check to see whether the updates have been received which correspond to the updates sent out.");

            UseThread = Get(true, "UseThread", false, "If true then each proxy will spawn a thread to deliver camera updates to the viewer at a constant rate. If false packets will be injected whenever CameraUpdate events are triggered.");
            StartStagger = Get(true, "StartStagger", 60, "How many seconds to way between starting each viewer if multiple viewers are being launched.");
            BackwardsCompatible = Get(true, "BackwardsCompatible", false, "If true, no unusual packets will be injected into the viewer. This will disable remote control and frustum control.");

            CrashLogFile = Get(true, "CrashLogFile", "CrashLog.log", "The log file to record crashes to.");

            LoginFirstName = Get(false, "FirstName", null, "The first name to log the viewer in with.");
            LoginLastName = Get(false, "LastName", null, "The last name to log the viewer in with.");
            LoginPassword = Get(false, "Password", null, "The password to log the viewer in with.");
            ProxyPort = Get(false, "ProxyPort", CURRENT_PORT++, "The port to run the proxy on.");
            LoginGrid = Get(false, "ProxyGrid", ProxyPort.ToString(), "The name of the grid the proxy will appear as.");
            AutoLoginClient = LoginFirstName != null && LoginLastName != null && LoginPassword != null;

            AutoStartProxy = Get(false, "AutoStartProxy", false, "Whether to automatically start the proxy when the system start.");
            AutoStartViewer = Get(false, "AutoStartViewer", false, "Whether to automatically start the viewer when the system starts.");
            AutoRestartViewer = Get(true, "AutoRestart", false, "Whether to automatically restart the viewer if the process exits.");
            ControlCamera = Get(false, "ControlCamera", true, "Whether to control the position of the camera on the viewer.");
            ControlFrustum = Get(false, "ControlFrustum", true, "Whether to control the viewing frustum on the viewer.");
            Fullscreen = Get(false, "Fullscreen", true, "Whether to start the viewer fullscreen.");

            StartupKeyPresses = Get(true, "StartupKeyPresses", "", "A series of key presses, using SendKeys syntax, which will be pressed when the viewer logs in. Separate sequences with commas.");

            //EnableWindowPackets = Init.Get(generalConfig, "EnableWindowPackets", true);
            //UseSetFollowCamPackets = !enableWindowPackets || Get(generalConfig, "UseSetFollowCamPackets", false);
            //ControlCamera = Init.Get(sectionConfig, "ControlCamera", true);
        }
    }
}
