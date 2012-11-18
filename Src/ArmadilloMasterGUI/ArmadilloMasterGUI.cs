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
            CameraMaster m = Init.InitCameraMaster(args.Concat(new string[] { "-g", "true" }).ToArray());
        }
    }
}
