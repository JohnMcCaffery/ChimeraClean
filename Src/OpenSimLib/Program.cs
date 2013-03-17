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
using Chimera.Kinect.Interfaces;

namespace ChimeraOutput {
    public static class RunChimera {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main() {
            Application.SetCompatibleTextRenderingDefault(false);

            DolphinMovementInput dolphin = new DolphinMovementInput();
            RaiseArmHelpTrigger trigger = new RaiseArmHelpTrigger();
            SimpleCursorFactory simpleFactory = new SimpleCursorFactory();
            PointCursorFactory pointFactory = new PointCursorFactory();

            //IOutput output = new SetFollowCamPropertiesViewerOutput("Main Window");
            IOutput output = new SetWindowViewerOutput("Main Window");
            ISystemInput kbMouseInput = new KBMouseInput();
            ISystemInput flythrough = new Flythrough();
            ISystemInput mouse = new MouseInput();
            KinectInput kinect = new KinectInput(new IDeltaInput[] { dolphin }, new IHelpTrigger[] { trigger }, simpleFactory, pointFactory);
            //ISystemInput kinectDolphin = new DeltaBasedInput(dolphin);

            Window[] windows = new Window[] { new Window("Main Window", output) };
            windows[0].Overlay.SetOverlayWindowFactory(new SimpleOverlayWindowFactory());
            //ImageSelection mOverlay = new ImageSelection("../Select1.jpg", .1f, .1f, .3f, .3f);
            //IOverlayState mState = new TestState();
            //MainMenuItem item1 = new MainMenuItem(mState, mOverlay);
            Chimera.Overlay.MainMenu mainMenu = new Chimera.Overlay.MainMenu();
            Coordinator coordinator = new Coordinator(windows, mainMenu, kinect, kbMouseInput, mouse, flythrough);

            //Window[] windows = new Window[] { new Window("Main Window") };
            //Chimera.Overlay.MainMenu mainMenu = new Chimera.Overlay.MainMenu();
            //Coordinator input = new Coordinator(windows, mainMenu, kinect);

            CoordinatorForm form = new CoordinatorForm(coordinator);
            CoordinatorConfig cfg = new CoordinatorConfig();
            kinect.FlyEnabled = true;
            kinect.WalkEnabled = true;
            kinect.YawEnabled = true;
            ProcessWrangler.BlockingRunForm(form, coordinator, cfg.AutoRestart);
        }
    }
}
