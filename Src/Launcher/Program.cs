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
