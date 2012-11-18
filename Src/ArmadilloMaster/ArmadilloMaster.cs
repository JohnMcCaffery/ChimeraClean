using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilLib;
using ConsoleTest;
using Nini.Config;
using System.IO;

namespace ArmadilloMaster {
    public class ArmadilloMaster {
        public static void Main(string[] args) {
            ArgvConfigSource config = new ArgvConfigSource(args);

            config.AddSwitch("Master", "File", "f");
            config.AddSwitch("Master", "GUI", "g");
            config.AddSwitch("Master", "Help", "h");

            if (Init.Has(config.Configs["Master"], "Help")) {
                Console.WriteLine("Master Help");
                Console.WriteLine(Init.HelpHeaders);
                IEnumerable<string> list = Init.Help("Master");
                list = list.Concat(new string[] {
                    Init.MakeHelpLine("Master", "File", "f", "The config file to use.", "AppDomain ConfigFile"),
                    Init.MakeHelpLine("Master", "GUI", "g", "Whether to launch a GUI.", true),
                    Init.MakeHelpLine("Master", "Help", "h", "Display this help.", "Not Set"),
                });
                foreach (string line in list.OrderBy(l => l))
                    Console.WriteLine(line);
                return;
            }

            string file = Init.AddFile(config);

            CameraMaster m = Init.InitCameraMaster(args, file);
            if (config.Configs["Master"].GetBoolean("GUI", true))
                Init.StartGui(() => new MasterForm(m));
            //else {
                //Console.WriteLine("Type 'Exit' to quit.");
                //while (!Console.ReadLine().ToUpper().Equals("EXIT"))
                    //Console.WriteLine("Type 'Exit' to quit.");
            //}
        }
    }
}
