using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nini.Config;
using System.IO;
using UtilLib;

namespace ArmadilloProxy {
    class ArmadilloProxy {
        static void Main(string[] args) {
            ArgvConfigSource argConfig = new ArgvConfigSource(args);

            argConfig.AddSwitch("General", "Master", "m");
            argConfig.AddSwitch("General", "Slave", "s");
            argConfig.AddSwitch("General", "SlaveCount", "sc");
            argConfig.AddSwitch("General", "FirstSlave", "fs");
            argConfig.AddSwitch("General", "File", "f");
            argConfig.AddSwitch("General", "Help", "h");
            argConfig.AddSwitch("General", "Name", "n");

            string file;
            IConfigSource config = Init.AddFile(argConfig, out file);
            
            bool help = Init.Has(config.Configs["General"], "Help");
            if (help) {
                Console.WriteLine("Proxy Help");
                Console.WriteLine(Init.HelpHeaders);
                string[] list = new string[] {
                    Init.MakeHelpLine("General", "File", "f", "The file to load the general configuration from.", "AppDomain ConfigFile"),
                    Init.MakeHelpLine("General", "Help", "h", "Display this help. Use with -m or -s to see help for master or slave.", "Not Set"),
                    Init.MakeHelpLine("General", "Master", "m", "Launch as a master. Can be used with -s or -sc.", "Not Set"),
                    Init.MakeHelpLine("General", "Slave", "s", "Launch as a slave. Can be used with -m.", "Not Set"),
                    Init.MakeHelpLine("General", "SlaveCount", "sc", "The number of slaves to create. If < 1, ignored. Can be used with -m. Overrides -s", "-1"),
                    Init.MakeHelpLine("General", "FirstSlave", "fs", "The name of the first slave when using the -sc flag. Specified as an integer. When using -fc the first slave created will be called 'Slave<fs>', the second Slave'(<fs>+1)' etc.", 1)
                };
                foreach (string line in list.OrderBy(l => l))
                    Console.WriteLine(line);
                Console.WriteLine("");
            }

            IConfig general = config.Configs["General"];

            List<ProxyManager> consoleInstances = new List<ProxyManager>();

            if (Init.Has(general, "Master")) {
                ProxyManager m = Init.InitCameraMaster(args);
                if (!Init.Get(config.Configs["Master"], "GUI", true))
                    consoleInstances.Add(m);
            }

            if (help && (Init.Has(general, "Slave") || Init.Has(general, "SlaveCount"))) {
                if (Init.Has(general, "Master"))
                    Console.WriteLine("");
                Init.InitCameraSlave(args);
            } else {
                int sc = Init.Get(general, "SlaveCount", -1);
                if (Init.Has(general, "Slave") || Init.Get(general, "Name", null) != null) {
                    CameraSlave s = Init.InitCameraSlave(args);
                    if (!Init.Get(config.Configs[s.Name], "GUI", true))
                        consoleInstances.Add(s);
                } else if (sc > 0) {
                    int slave = Init.Get(general, "FirstSlave", 1);
                    for (int i = 1; i <= sc; i++) {
                        string slaveName = "Slave" + (slave++);
                        CameraSlave s = Init.InitCameraSlave(new string[] { "-n", slaveName }.Concat(args).ToArray());
                        if (!Init.Get(config.Configs[slaveName], "GUI", true))
                            consoleInstances.Add(s);
                    }
                }
            }

            if (consoleInstances.Count > 0) {
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                foreach (var instance in consoleInstances)
                    instance.Stop();
            }
        }
    }
}
