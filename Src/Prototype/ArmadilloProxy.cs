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
using Nini.Config;
using System.IO;
using UtilLib;
using ConsoleTest;

namespace ArmadilloProxy {
    class ArmadilloProxy {
        static void Main(string[] args) {
            bool started = false;
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
                    Init.MakeHelpLine("General", "File", "f", "The file to load the general configuration transition.", "AppDomain ConfigFile"),
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
                IConfig masterConfig;
                CameraMaster m = Init.InitCameraMaster(args, out masterConfig);
                if (!Init.Get(config.Configs["Master"], "GUI", true))
                    consoleInstances.Add(m);
                if (Init.Get(masterConfig, "GUI", true))
                    Init.StartGui(masterConfig, m, () => new MasterForm(m));
                started = true;
            }

            IConfig slaveConfig;
            if (help && (Init.Has(general, "Slave") || Init.Has(general, "SlaveCount"))) {
                if (Init.Has(general, "Master"))
                    Console.WriteLine("");
                CameraSlave s = Init.InitCameraSlave(args, out slaveConfig);
                if (Init.Get(slaveConfig, "GUI", true))
                    Init.StartGui(slaveConfig, s, () => new SlaveForm(s));
            } else {
                int sc = Init.Get(general, "SlaveCount", -1);
                if (Init.Has(general, "Slave") || Init.Get(general, "Name", null) != null) {
                    CameraSlave s = Init.InitCameraSlave(args, out slaveConfig);
                    if (!Init.Get(slaveConfig, "GUI", true))
                        consoleInstances.Add(s);
                    else
                        Init.StartGui(slaveConfig, s, () => new SlaveForm(s));
                    started = true;
                } else if (sc > 0) {
                    started = true;
                    int slave = Init.Get(general, "FirstSlave", 1);
                    for (int i = 1; i <= sc; i++) {
                        string slaveName = "Slave" + (slave++);
                        CameraSlave s = Init.InitCameraSlave(new string[] { "-n", slaveName }.Concat(args).ToArray(), out slaveConfig);
                        if (!Init.Get(config.Configs[slaveName], "GUI", true))
                            consoleInstances.Add(s);
                        else
                            Init.StartGui(slaveConfig, s, () => new SlaveForm(s));
                    }
                }
            }

            if (consoleInstances.Count > 0) {
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                foreach (var instance in consoleInstances)
                    instance.Stop();
            }

            if (!started)
                Console.WriteLine("No configuration specified. Exiting.");
        }
    }
}
