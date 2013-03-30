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
using Chimera.Overlay.Transitions;
using Chimera.Interfaces.Overlay;
using Chimera.Kinect.Overlay;
using Chimera.Flythrough.Overlay;

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

            Window mainWindow = new Window("Main Window", output);
            Window[] windows = new Window[] { mainWindow };
            //ImageHoverTrigger mOverlay = new ImageHoverTrigger("../Select1.jpg", .1f, .1f, .3f, .3f);
            //From mState = new TestState();
            //MainMenuItem item1 = new MainMenuItem(mState, mOverlay);
            mCoordinator = new Coordinator(windows, kbMouseInput, mKinect, mouse, heightmap, flythrough);

            IState splashScreen = new ImageBGState("SplashScreen", mCoordinator.StateManager, "../Images/CathedralSplashScreen.png");
            IState helpScreen = new ImageBGState("HelpScreen", mCoordinator.StateManager, "../Images/CathedralHelp.png");
            IState kinectControl = new KinectControlState("KinectControL", mCoordinator.StateManager);
            IState flythroughState = new FlythroughState("Flythrough", mCoordinator.StateManager, "../CathedralFlythrough-LookAt.xml");

            IHoverSelectorRenderer renderer = new DialRenderer();

            InvisibleHoverTrigger splashHelpTrigger = new InvisibleHoverTrigger(mainWindow.OverlayManager, renderer, 265f / 1920f, 255f / 1080f, (675f - 255f) / 1920f, (900f - 255f) / 1080f);
            InvisibleHoverTrigger helpSplashTrigger = new InvisibleHoverTrigger(mainWindow.OverlayManager, renderer, 70f / 1920f, 65f / 1080f, (490f - 70f) / 1920f, (300f - 65f) / 1080f);
            InvisibleHoverTrigger helpKinectTrigger = new InvisibleHoverTrigger(mainWindow.OverlayManager, renderer, 60f / 1920f, 520f / 1080f, (335f - 60f) / 1920f, (945f - 520f) / 1080f);

            //InvisibleHoverTrigger splashHelpTrigger = new InvisibleHoverTrigger(mainWindow.OverlayManager, renderer, 155f / 1920f, 220f / 1080f, (555f - 155f) / 1920f, (870f - 220f) / 1080);
            //InvisibleHoverTrigger helpSplashTrigger = new InvisibleHoverTrigger(mainWindow.OverlayManager, renderer, 70f / 1920f, 65f / 1080f, (490f - 70f) / 1920f, (300f - 65f) / 1080f);
            //InvisibleHoverTrigger helpKinectTrigger = new InvisibleHoverTrigger(mainWindow.OverlayManager, renderer, 55f / 1920f, 515f / 1080f, (330f - 55f) / 1920f, (950f - 515f) / 1080f);

            ITrigger customTriggerHelp = new CustomTriggerTrigger(mCoordinator.StateManager, "Help");
            ITrigger skeletonLost = new SkeletonLostTrigger(mCoordinator, 15000.0);
            ITrigger skeletonFound = new SkeletonFoundTrigger();

            CutWindowTransitionFactory cutTransition = new CutWindowTransitionFactory();
            BitmapFadeTransitionFactory fadeTransition = new BitmapFadeTransitionFactory(1500.0);
            OpacityFadeOutTransitionFactory fadeOutTransition = new OpacityFadeOutTransitionFactory(1500.0);
            OpacityFadeInTransitionFactory fadeInTransition = new OpacityFadeInTransitionFactory(1500.0);

            StateTransition splashHelpTransition = new StateTransition(mCoordinator.StateManager, splashScreen, helpScreen, splashHelpTrigger, fadeTransition);
            StateTransition helpSplashTransition = new StateTransition(mCoordinator.StateManager, helpScreen, splashScreen, helpSplashTrigger, cutTransition);
            StateTransition helpKinectTransition = new StateTransition(mCoordinator.StateManager, helpScreen, kinectControl, helpKinectTrigger, fadeOutTransition);
            StateTransition kinectHelpTransition = new StateTransition(mCoordinator.StateManager, kinectControl, helpScreen, customTriggerHelp, fadeInTransition);
            StateTransition splashFlythroughTransition = new StateTransition(mCoordinator.StateManager, splashScreen, flythroughState, skeletonLost, fadeOutTransition);
            StateTransition helpFlythroughTransition = new StateTransition(mCoordinator.StateManager, helpScreen, flythroughState, skeletonLost, fadeOutTransition);
            StateTransition kinectFlythroughTransition = new StateTransition(mCoordinator.StateManager, kinectControl, flythroughState, skeletonLost, fadeOutTransition);
            StateTransition flythroughSplashTransition = new StateTransition(mCoordinator.StateManager, flythroughState, splashScreen, skeletonFound, fadeInTransition);

            SkeletonFeature helpSkeleton = new SkeletonFeature(1650f / 1920f, 0f, 800f / 1080f, 225f);

            splashScreen.AddTransition(splashHelpTransition);
            splashScreen.AddTransition(splashFlythroughTransition);
            splashScreen.AddFeature(mainWindow.Name, splashHelpTrigger);

            helpScreen.AddTransition(helpSplashTransition);
            helpScreen.AddTransition(helpKinectTransition);
            helpScreen.AddTransition(helpFlythroughTransition);
            helpScreen.AddFeature(mainWindow.Name, helpSplashTrigger);
            helpScreen.AddFeature(mainWindow.Name, helpKinectTrigger);
            helpScreen.AddFeature(mainWindow.Name, helpSkeleton);

            kinectControl.AddTransition(kinectFlythroughTransition);
            kinectControl.AddTransition(kinectHelpTransition);

            flythroughState.AddTransition(flythroughSplashTransition);

            mCoordinator.StateManager.AddState(splashScreen);
            mCoordinator.StateManager.AddState(helpScreen);
            mCoordinator.StateManager.AddState(kinectControl);
            mCoordinator.StateManager.AddState(flythroughState);
            //mCoordinator.StateManager.CurrentState = splashScreen;

            //Window[] windows = new Window[] { new Window("Main Window") };
            //Chimera.Overlay.MainMenu mainMenu = new Chimera.Overlay.MainMenu();
            //Coordinator input = new Coordinator(windows, mainMenu, kinect);
        }
    }
}
