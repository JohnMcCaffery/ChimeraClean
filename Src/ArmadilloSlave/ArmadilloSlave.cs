using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nini.Config;
using UtilLib;
using ConsoleTest;
using System.IO;

namespace ArmadilloSlave {
    public class ArmadilloSlave {
        public static void Main(string[] args) {
            ArgvConfigSource config = new ArgvConfigSource(args);

            config.AddSwitch("Slave", "Name", "n");
            config.AddSwitch("General", "File", "f");

            string file = Init.AddFile(config);

            string name = Init.Get(config.Configs["Slave"], "Name", "Slave1");

            config.AddSwitch(name, "GUI", "g");
            config.AddSwitch(name, "Help", "h");

            if (Init.Has(config.Configs[name], "Help")) {
                Console.WriteLine("Slave Help");
                Console.WriteLine(Init.HelpHeaders);

                IEnumerable<string> list = Init.Help(name);
                list = list.Concat(new string[] {
                    Init.MakeHelpLine(name, "GUI", "g", "Whether to launch a GUI.", true),
                    Init.MakeHelpLine(name, "Help", "h", "Display this help.", "Not Set"),
                    Init.MakeHelpLine("Slave", "File", "f", "The config file to use.", "AppDomain ConfigFile"),
                    Init.MakeHelpLine("Slave", "Name", "n", "The name for this slave.", "Slave1")
                });
                foreach (string line in list.OrderBy(l => l))
                    Console.WriteLine(line);

                return;
            }

            CameraSlave s = Init.InitCameraSlave(args, file, name);
            if (Init.Get(config.Configs[name], "GUI", true))
                Init.StartGui(() => new SlaveForm(s));
            //else {
                //Console.WriteLine("Type 'Exit' to quit.");
                //while (!Console.ReadLine().ToUpper().Equals("EXIT"))
                    //Console.WriteLine("Type 'Exit' to quit.");
            //}
        }
    }
}
