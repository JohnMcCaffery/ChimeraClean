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
