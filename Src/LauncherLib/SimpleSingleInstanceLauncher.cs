using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Kinect;
using Chimera.Inputs;
using Chimera;
using Chimera.GUI.Forms;
using Chimera.Kinect.Interfaces;
using Chimera.Flythrough;
using Chimera.OpenSim;
using Chimera.Overlay.States;
using Chimera.Overlay.Triggers;
using Chimera.Overlay;

namespace Chimera.Launcher {
    public class SimpleSingleInstanceLauncher {
        private readonly Coordinator mCoordinator;
        private readonly KinectInput mKinect;
        private CoordinatorForm mForm;

        public Coordinator Coordinator {
            get { return mCoordinator; }
        }
        public CoordinatorForm Form {
            get {
                if (mForm == null) {
                    mForm = new CoordinatorForm(mCoordinator);
                    mKinect.FlyEnabled = true;
                    mKinect.WalkEnabled = true;
                    mKinect.YawEnabled = true;
                }
                return mForm;
            }
        }
        public SimpleSingleInstanceLauncher(params string[] args) {
            TimespanMovementInput timespan = new TimespanMovementInput();
            DolphinMovementInput dolphin = new DolphinMovementInput();
            RaiseArmHelpTrigger trigger = new RaiseArmHelpTrigger();
            SimpleCursorFactory simpleFactory = new SimpleCursorFactory();
            PointCursorFactory pointFactory = new PointCursorFactory();

            //IOutput output = new SetFollowCamPropertiesViewerOutput("Main Window");
            IOutput output = new SetWindowViewerOutput("Main Window");
            ISystemInput kbMouseInput = new DeltaBasedInput(new KBMouseInput());
            ISystemInput flythrough = new Chimera.Flythrough.Flythrough();
            ISystemInput mouse = new MouseInput();
            ISystemInput heightmap = new HeightmapInput();
            mKinect = new KinectInput(new IDeltaInput[] { timespan, dolphin }, new IHelpTrigger[] { trigger }, simpleFactory, pointFactory);
            //ISystemInput kinectDolphin = new DeltaBasedInput(dolphin);

            Window[] windows = new Window[] { new Window("Main Window", output) };
            //ImageHoverTrigger mOverlay = new ImageHoverTrigger("../Select1.jpg", .1f, .1f, .3f, .3f);
            //From mState = new TestState();
            //MainMenuItem item1 = new MainMenuItem(mState, mOverlay);
            mCoordinator = new Coordinator(windows, kbMouseInput, mKinect, mouse, heightmap, flythrough);

            ImageBGState splashScreen = new ImageBGState("SplashScreen", mCoordinator.StateManager, "../Images/CathedralSplashScreen.png");
            ImageBGState helpScreen = new ImageBGState("HelpScreen", mCoordinator.StateManager, "../Images/CathedralHelp.png");

            DialRenderer renderer;
            //InvisibleHoverTrigger splashHelpTrigger = new InvisibleHoverTrigger(mGoInWorldRender, 155f / 1920, 220f / 1080, (555f - 155f) / 1920, (870f - 220f) / 1080);
            InvisibleHoverTrigger splashHelpTrigger = new InvisibleHoverTrigger(mCoordinator.StateManager, renderer, 265f / 1920, 255f / 1080, (675f - 255f) / 1920, (900f - 255f) / 1080f);

            //InvisibleHoverTrigger helpSplashTrigger = new InvisibleHoverTrigger(mCoordinator.StateManager, renderer, 70f / 1920, 65f / 1080, (490f - 70f) / 1920, (300f - 65f) / 1080);
            InvisibleHoverTrigger helpSplashTrigger = new InvisibleHoverTrigger(mCoordinator.StateManager, renderer, 70f / 1920, 65f / 1080, (490f - 70f) / 1920, (300f - 65f) / 1080);

            //InvisibleHoverTrigger helpWorldTrigger = new InvisibleHoverTrigger(mCoordinator.StateManager, renderer, 55f / 1920, 515f / 1080, (330f - 55f) / 1920, (950f - 515f) / 1080);
            //InvisibleHoverTrigger helpWorldTrigger = new InvisibleHoverTrigger(mCoordinator.StateManager, renderer, 60f / 1920, 520f / 1080, (335f - 60f) / 1920, (945f - 520f) / 1080);

            CutStateTransitionFactory cutTransition = new CutStateTransitionFactory();
            StateTransition splashHelpTransition = new StateTransition(mCoordinator.StateManager, splashScreen, helpScreen, helpSplashTrigger, cutTransition);
            StateTransition helpSplashTransition = new StateTransition(mCoordinator.StateManager, splashScreen, helpScreen, helpSplashTrigger, cutTransition);

            splashScreen.AddTransition(splashHelpTransition);
            helpScreen.AddTransition(helpSplashTransition);

            mCoordinator.StateManager.AddState(splashScreen);
            mCoordinator.StateManager.AddState(helpScreen);

            mCoordinator.StateManager.CurrentState = splashScreen;

            //Window[] windows = new Window[] { new Window("Main Window") };
            //Chimera.Overlay.MainMenu mainMenu = new Chimera.Overlay.MainMenu();
            //Coordinator input = new Coordinator(windows, mainMenu, kinect);
        }
    }
}
