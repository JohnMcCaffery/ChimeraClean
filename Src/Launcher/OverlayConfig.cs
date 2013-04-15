using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;

namespace Chimera.Launcher {
    class LauncherConfig : ConfigBase {
        public string ServerExe;
        public bool LaunchServer;
        public string ProxyExe;
        public bool AutoRestart;

        public LauncherConfig(params string[] args) : base("appSettings", args) { }

        public override string Group {
            get { return "Launcher"; }
        }

        protected override void InitConfig() {
            AddCommandLineKey(false, "ServerExe", "s");
            AddCommandLineKey(false, "LaunchServer", "l");
            AddCommandLineKey(false, "AutoRestart", "r");

            ServerExe = Get(false, "ServerExe", "C:\\OpenSim\\bin\\OpenSim.exe", "The executable file that will run the OpenSim server.");
            ProxyExe = Get(false, "ProxyExe", "C:\\TARDIS\\Chimera\\Bin\\Chimera.exe", "The executable file that will run the OpenSim server.");
            //ProxyExe = Get(false, "ProxyExe", typeof(Chimera.ChimeraLauncher).Assembly.Location, "The executable file that will run the proxy server.");
            LaunchServer = Get(false, "LaunchServer", false, "Whether the launcher should start up an OpenSim server.");
            AutoRestart = Get(false, "AutoRestart", true, "Whether the launcher should automatically restart any proxies it launches when they close.");
        }
    }
}
