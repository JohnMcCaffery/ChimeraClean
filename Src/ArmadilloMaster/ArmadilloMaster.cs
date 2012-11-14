using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilLib;
using ConsoleTest;
using Nini.Config;

namespace ArmadilloMaster {
    class ArmadilloMaster {
        static void Main(string[] args) {
            ArgvConfigSource argConfig = new ArgvConfigSource(args);

            argConfig.AddSwitch("Master", "File", "f");
            argConfig.AddSwitch("Master", "GUI", "g");
            argConfig.AddSwitch("Master", "Help", "h");

            if (argConfig.Configs["Master"].Get("Help", null) != null) {
                Console.WriteLine(Init.HelpHeaders);
                Console.WriteLine(Init.Help("Master"));
                Console.WriteLine(Init.MakeHelpLine("Master", "File", "f", "The config file to use", "AppDomain ConfigFile"));
                Console.WriteLine(Init.MakeHelpLine("Master", "GUI", "h", "Whether to launch a GUI", true));
                Console.WriteLine(Init.MakeHelpLine("Master", "Help", "h", "Display this help", "Not Set"));
                return;
            }

            string file = argConfig.Configs["Master"].Get("File", AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            DotNetConfigSource fileConfig = new DotNetConfigSource(file);
            fileConfig.Merge(argConfig);

            CameraMaster m = Init.InitCameraMaster(args, file);
            if (fileConfig.Configs["Master"].GetBoolean("GUI", true))
                Init.StartGui(() => new MasterForm(m));
            else {
                Console.WriteLine("Type 'Exit' to quit.");
                while (!Console.ReadLine().ToUpper().Equals("EXIT"))
                    Console.WriteLine("Type 'Exit' to quit.");
            }
        }
    }
}
