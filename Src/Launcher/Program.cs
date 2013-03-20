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
                if (cfg.AutoRestart)
                    sServerProcess.Exited += new EventHandler(sServerProcess_Exited);
                sServerProcess.Start();

                Thread.Sleep(60 * 1000);
            }

            //sProxyExe = typeof(ChimeraLauncher).Assembly.Location;
            sProxyExe = cfg.ProxyExe;
            sProxyProcess = ProcessWrangler.InitProcess(sProxyExe);
                if (cfg.AutoRestart)
                    sProxyProcess.Exited += new EventHandler(sProxyProcess_Exited);
            sProxyProcess.Start();

            Console.WriteLine("Type Exit to quit.");
            string line = Console.ReadLine();
            while (!line.ToUpper().Equals("EXIT"))
                line = Console.ReadLine();

            ProcessWrangler.PressKey(sProxyProcess, "{F4}", false, true, false);
            if (sServerProcess != null)
                ProcessWrangler.PressKey(sServerProcess, "q{ENTER}");
        }

        static void sProxyProcess_Exited(object sender, EventArgs e) {
            Process proxy = ProcessWrangler.InitProcess(sProxyExe);
            proxy.Exited += new EventHandler(sProxyProcess_Exited);
            proxy.Start();
        }

        static void sServerProcess_Exited(object sender, EventArgs e) {
            Process server = ProcessWrangler.InitProcess(sServerExe);
            server.Exited += new EventHandler(sServerProcess_Exited);
            server.Start();
        }
    }
}
