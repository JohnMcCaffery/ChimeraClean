using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Chimera.GUI.Forms;
using Chimera;
using Chimera.OpenSim;

namespace TestProject {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            IOutput output = new SetFollowCamPropertiesViewerOutput("Main Window");
            Window[] windows = new Window[] { new Window("Main Window", output) };
            Coordinator c = new Coordinator(windows);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CoordinatorForm(c));
        }
    }
}
