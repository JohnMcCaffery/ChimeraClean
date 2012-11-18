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
            ArgvConfigSource config = new ArgvConfigSource(args);

            config.AddSwitch("General", "Master", "m");
            config.AddSwitch("General", "Slave", "s");
            config.AddSwitch("General", "SlaveCount", "sc");
            config.AddSwitch("General", "File", "f");
            config.AddSwitch("General", "Help", "h");

            Init.AddFile(config);
            
            bool help = Init.Has(config.Configs["General"], "Help");
            if (help) {
                Console.WriteLine("Proxy Help");
                Console.WriteLine(Init.HelpHeaders);
                string[] list = new string[] {
                    Init.MakeHelpLine("General", "File", "f", "The file to load the general configuration from.", "AppDomain ConfigFile"),
                    Init.MakeHelpLine("General", "Help", "h", "Display this help. Use with -m or -s to see help for master or slave.", "Not Set"),
                    Init.MakeHelpLine("General", "Master", "m", "Launch as a master. Can be used with -s or -sc.", "Not Set"),
                    Init.MakeHelpLine("General", "Slave", "s", "Launch as a slave. Can be used with -m.", "Not Set"),
                    Init.MakeHelpLine("General", "SlaveCount", "sc", "The number of slaves to create. If < 1, ignored. Can be used with -m. Overrides -s", "-1")
                };
                foreach (string line in list.OrderBy(l => l))
                    Console.WriteLine(line);
                Console.WriteLine("");
            }

            IConfig general = config.Configs["General"];

            if (Init.Has(general, "Master"))
                ArmadilloMaster.ArmadilloMaster.Main(args);

            if (help && (Init.Has(general, "Slave") || Init.Has(general, "SlaveCount"))) {
                if (Init.Has(general, "Master"))
                    Console.WriteLine("");
                ArmadilloSlave.ArmadilloSlave.Main(args);
            } else {
                int sc = Init.Get(general, "SlaveCount", -1);
                if (sc > 0) {
                    for (int i = 1; i <= sc; i++)
                        ArmadilloSlave.ArmadilloSlave.Main(new string[] { "-n", "Slave" + i }.Concat(args).ToArray());
                } else if (Init.Has(general, "Slave"))
                    ArmadilloSlave.ArmadilloSlave.Main(args);
            }
        }
    }
}
