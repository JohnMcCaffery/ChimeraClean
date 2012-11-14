using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nini.Config;
using UtilLib;
using ConsoleTest;
using System.IO;

namespace ArmadilloSlave {
    class ArmadilloSlave {
        static void Main(string[] args) {
            ArgvConfigSource config = new ArgvConfigSource(args);

            config.AddSwitch("Slave", "Name", "n");

            string file = config.Configs["Slave"].Get("File", AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            if (File.Exists(file) && Path.GetExtension(file).ToUpper().Equals("config")) {
                DotNetConfigSource dotnet = new DotNetConfigSource(file);
                config.Merge(dotnet);
            }

            string name = config.Configs["Slave"].Get("Name", "Slave1");

            config.AddSwitch(name, "File", "f");
            config.AddSwitch(name, "GUI", "g");
            config.AddSwitch(name, "Help", "h");

            if (config.Configs[name].Get("Help", null) != null) {
                Console.WriteLine(Init.HelpHeaders);
                Console.WriteLine(Init.Help(name));
                Console.WriteLine(Init.MakeHelpLine("Slave", "Name", "h", "The name for this slave", "Slave1"));
                Console.WriteLine(Init.MakeHelpLine(name, "File", "f", "The config file to use", "AppDomain ConfigFile"));
                Console.WriteLine(Init.MakeHelpLine(name, "GUI", "h", "Whether to launch a GUI", true));
                Console.WriteLine(Init.MakeHelpLine(name, "Help", "h", "Display this help", "Not Set"));
                return;
            }

            CameraSlave m = Init.InitCameraSlave(args, file, name);
            if (config.Configs[name].GetBoolean("GUI", true))
                Init.StartGui(() => new SlaveForm(m));
            else {
                Console.WriteLine("Type 'Exit' to quit.");
                while (!Console.ReadLine().ToUpper().Equals("EXIT"))
                    Console.WriteLine("Type 'Exit' to quit.");
            }
        }
    }
}
