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
using Chimera.Overlay;
using Chimera.Kinect;

namespace ChimeraOutput {
    public static class RunChimera {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main() {
            Application.SetCompatibleTextRenderingDefault(false);

            IOutput output = new SetFollowCamPropertiesViewerOutput("Main Window");
            IInput kbMouseInput = new KBMouseInput();
            IInput flythrough = new Flythrough();
            IInput kinect = new KinectInput();

            Window[] windows = new Window[] { new Window("Main Window", output) };
            ImageArea mOverlay = new ImageArea("../Select1.jpg", .1, .1, .3, .3);
            IOverlayState mState = new TestState();
            MainMenuItem item1 = new MainMenuItem(mState, mOverlay);
            Chimera.Overlay.MainMenu mainMenu = new Chimera.Overlay.MainMenu(item1);
            Coordinator coordinator = new Coordinator(windows, mainMenu, kinect, kbMouseInput, flythrough);

            //Window[] windows = new Window[] { new Window("Main Window") };
            //Chimera.Overlay.MainMenu mainMenu = new Chimera.Overlay.MainMenu();
            //Coordinator coordinator = new Coordinator(windows, mainMenu, kinect);

            CoordinatorForm form = new CoordinatorForm(coordinator);
            CoordinatorConfig cfg = new CoordinatorConfig();
            ProcessWrangler.BlockingRunForm(form, coordinator, cfg.AutoRestart);
        }
    }
}
