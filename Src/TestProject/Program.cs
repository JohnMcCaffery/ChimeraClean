using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Chimera.GUI.Forms;
using Chimera;
using Chimera.OpenSim;
using Chimera.Inputs;
using Chimera.Util;
using Chimera.FlythroughLib;

namespace TestProject {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {

            Application.SetCompatibleTextRenderingDefault(false);

            IOutput output = new SetFollowCamPropertiesViewerOutput("Main Window");
            IInput kbMouseInput = new KBMouseInput();
            IInput flythrough = new Flythrough();
            Window[] windows = new Window[] { new Window("Main Window", output) };
            Coordinator coordinator = new Coordinator(windows, kbMouseInput, flythrough);
            CoordinatorForm form = new CoordinatorForm(coordinator);

            CoordinatorConfig cfg = new CoordinatorConfig();
            ProcessWrangler.BlockingRunForm(form, coordinator, cfg.AutoRestart);
        }
    }
}