using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UtilLib;
using ConsoleTest;
using Nini.Config;

namespace ArmadilloMasterGUI {
    static class ArmadilloMasterGUI {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ArgvConfigSource config = new ArgvConfigSource(args);
            config.AddSwitch("General", "File", "f");
            string file = Init.AddFile(config);

            CameraMaster m = Init.InitCameraMaster(args, file);
            Application.Run(new MasterForm(m));
        }
    }
}
