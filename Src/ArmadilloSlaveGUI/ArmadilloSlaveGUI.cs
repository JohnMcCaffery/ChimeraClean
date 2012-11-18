using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UtilLib;
using System.IO;
using Nini.Config;
using ConsoleTest;

namespace ArmadilloSlaveGUI {
    static class ArmadilloSlaveGUI {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ArgvConfigSource config = new ArgvConfigSource(args);

            config.AddSwitch("Slave", "Name", "n");
            config.AddSwitch("General", "File", "f");

            string file = Init.AddFile(config);

            string name = Init.Get(config.Configs["Slave"], "Name", "Slave1");

            CameraSlave s = Init.InitCameraSlave(args, file, name);
            Application.Run(new SlaveForm(s));
        }
    }
}
