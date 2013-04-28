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
using System.Diagnostics;
using Chimera.Util;
using System.Threading;

namespace Chimera.Launcher {
    class Program {
        private static string sServerExe;
        private static string sProxyExe;
        private static Process sServerProcess;
        private static Process sProxyProcess;

        static void Main(string[] args) {
            LauncherConfig cfg = new LauncherConfig();

            if (cfg.LaunchServer) {
                sServerExe = cfg.ServerExe;
                sServerProcess = ProcessWrangler.InitProcess(sServerExe);
                //if (cfg.AutoRestart)
                    //sServerProcess.Exited += sServerProcess_Exited;
                sServerProcess.Start();

                Console.WriteLine("Launcher started.");

                Thread.Sleep(15 * 1000);
            }

            //sProxyExe = typeof(ChimeraLauncher).Assembly.Location;
            sProxyExe = cfg.ProxyExe;
            sProxyProcess = ProcessWrangler.InitProcess(sProxyExe);
                //if (cfg.AutoRestart)
                    //sProxyProcess.Exited += sProxyProcess_Exited;
            sProxyProcess.Start();

            Thread input = new Thread(() => {
                Console.WriteLine("Type Exit to quit.");
                string line = Console.ReadLine();
                while (!line.ToUpper().Equals("EXIT")) {
                    line = Console.ReadLine();
                    if (line.ToUpper().Equals("CLOSE PROXY"))
                        ProcessWrangler.PressKey(sProxyProcess, "{F4}", false, true, false);
                }

                ProcessWrangler.PressKey(sProxyProcess, "{F4}", false, true, false);
                if (sServerProcess != null)
                    ProcessWrangler.PressKey(sServerProcess, "q{ENTER}");
            });
            input.Name = "Launcher input thread.";
            input.Start();
        }

        static void sProxyProcess_Exited(object sender, EventArgs e) {
            Process proxy = ProcessWrangler.InitProcess(sProxyExe);
            proxy.Exited += sProxyProcess_Exited;
            proxy.Start();
        }

        static void sServerProcess_Exited(object sender, EventArgs e) {
            Process server = ProcessWrangler.InitProcess(sServerExe);
            server.Exited += sServerProcess_Exited;
            server.Start();
        }
    }
}
