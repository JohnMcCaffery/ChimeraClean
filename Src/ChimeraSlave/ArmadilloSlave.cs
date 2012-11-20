using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UtilLib;
using System.IO;
using Nini.Config;
using ConsoleTest;

namespace ArmadilloSlaveGUI {
    static class ArmadilloSlave {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CameraSlave s = Init.InitCameraSlave(args.Concat(new string[] { "-g", "true" }).ToArray());
        }
    }
}
