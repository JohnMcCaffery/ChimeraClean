using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Chimera.GUI.Forms;
using Chimera;
using Chimera.OpenSim;
using Chimera.Util;
using Chimera.Inputs;
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
            MainMenuItem item1 = new MainMenuItem("C:\\Users\\Iain\\Desktop\\Helmsdale Demo - 18-2-2013\\100_2344.JPG", .1, .1, .3, .3);
            Chimera.MainMenu mainMenu = new Chimera.MainMenu(item1);
            Coordinator coordinator = new Coordinator(windows, mainMenu, kbMouseInput, flythrough);
            CoordinatorForm form = new CoordinatorForm(coordinator);

            CoordinatorConfig cfg = new CoordinatorConfig();
            ProcessWrangler.BlockingRunForm(form, coordinator, cfg.AutoRestart);
        }
    }
}
