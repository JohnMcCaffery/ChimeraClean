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
using Chimera.Util;
using System.Windows.Forms;
using Chimera.Overlay.Drawables;
using System.Drawing;
using OpenMetaverse;

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
            SetWindowViewerOutput mainWindowProxy = new SetWindowViewerOutput("Main Window");
            ISystemInput kbMouseInput = new DeltaBasedInput(new KBMouseInput());
            ISystemInput flythrough = new Chimera.Flythrough.Flythrough();
            ISystemInput mouse = new MouseInput();
            ISystemInput heightmap = new HeightmapInput();
            mKinect = new KinectInput(new IDeltaInput[] { timespan, dolphin }, new IHelpTrigger[] { trigger }, simpleFactory, pointFactory);

            Window mainWindow = new Window("Main Window", mainWindowProxy);
            Window[] windows = new Window[] { mainWindow };
            mCoordinator = new Coordinator(windows, kbMouseInput, mKinect, mouse, heightmap, flythrough, mainWindowProxy);




            State splash = new SeeThroughMenuState("SplashScreen", mCoordinator.StateManager, new Vector3(128f, 128f, 55f), new Rotation(0.0, 90.0));
            State explore = new SeeThroughMenuState("InWorldChoice", mCoordinator.StateManager, new Vector3(512f, 512f, 60f), new Rotation(0.0, 40.0));
            State kinectAvatar = new KinectControlState("KinectHelpAvatar", mCoordinator.StateManager, true);
            State kinectFlycam = new KinectControlState("KinectHelpFlycam", mCoordinator.StateManager, false);
            State helpAvatar = new KinectHelpState("KinectControlAvatar", mCoordinator.StateManager, mainWindow.Name, mainWindow.Name);
            State helpFlycam = new KinectHelpState("KinectControlFlycam", mCoordinator.StateManager, mainWindow.Name, mainWindow.Name);
            State flythroughState = new FlythroughState("Flythrough", mCoordinator.StateManager, "../CathedralFlythrough-LookAt.xml");

            DialRenderer dialRenderer = new DialRenderer();
            CursorRenderer cursorRenderer = new DialCursorRenderer(dialRenderer, mainWindow.OverlayManager);
            CursorTrigger t = new CursorTrigger(new CircleRenderer(100), mainWindow);

            OverlayImage helpKinectButton = new OverlayImage(new Bitmap("../Images/HelpToWorld.png"), .85f, .5f, mainWindow.Name);
            OverlayImage exploreButton = new OverlayImage(new Bitmap("../Images/SeeTheTownship.png"), .85f, .5f, mainWindow.Name);
            OverlayImage kinectAvatarButton = new OverlayImage(new Bitmap("../Images/ExploreWithAvatar.png"), .85f, .5f, mainWindow.Name);
            OverlayImage kinectFlycamButton = new OverlayImage(new Bitmap("../Images/ExploreFreely.png"), .85f, .5f, mainWindow.Name);
            OverlayImage splashButton = new OverlayImage(new Bitmap("../Images/MainMenu.png"), .85f, .5f, mainWindow.Name);

            Rectangle clip = new Rectangle(0, 0, 1920, 1080);
            ImageHoverTrigger helpSplashTrigger = new ImageHoverTrigger(mainWindow.OverlayManager, cursorRenderer, splashButton);
            ImageHoverTrigger helpKinectTrigger = new ImageHoverTrigger(mainWindow.OverlayManager, cursorRenderer, helpKinectButton);
            ImageHoverTrigger splashExploreTrigger = new ImageHoverTrigger(mainWindow.OverlayManager, cursorRenderer, exploreButton);
            ImageHoverTrigger exploreAvatarTrigger = new ImageHoverTrigger(mainWindow.OverlayManager, cursorRenderer, kinectAvatarButton);
            ImageHoverTrigger exploreFlycamTrigger = new ImageHoverTrigger(mainWindow.OverlayManager, cursorRenderer, kinectFlycamButton);

            ITrigger customTriggerHelp = new CustomTriggerTrigger(mCoordinator.StateManager, "Help");
            ITrigger skeletonLost = new SkeletonLostTrigger(mCoordinator, 15000.0);
            ITrigger skeletonFound = new SkeletonFoundTrigger();

            CutWindowTransitionFactory cutTransition = new CutWindowTransitionFactory();
            BitmapFadeTransitionFactory fadeTransition = new BitmapFadeTransitionFactory(1500.0);
            OpacityFadeOutTransitionFactory fadeOutTransition = new OpacityFadeOutTransitionFactory(1500.0);
            OpacityFadeInTransitionFactory fadeInTransition = new OpacityFadeInTransitionFactory(1500.0);

            StateTransition splashExploreTransition = new StateTransition(mCoordinator.StateManager, splash, explore, splashExploreTrigger, fadeTransition);

            StateTransition exploreAvatarTransition = new StateTransition(mCoordinator.StateManager, explore, kinectAvatar, exploreAvatarTrigger, fadeOutTransition);
            StateTransition exploreFlycamTransition = new StateTransition(mCoordinator.StateManager, explore, kinectFlycam, exploreFlycamTrigger, fadeOutTransition);

            StateTransition helpAvatarSplashTransition = new StateTransition(mCoordinator.StateManager, helpAvatar, splash, helpSplashTrigger, fadeTransition);
            StateTransition helpAvatarKinectTransition = new StateTransition(mCoordinator.StateManager, helpAvatar, kinectAvatar, helpKinectTrigger, fadeOutTransition);

            StateTransition helpFlycamSplashTransition = new StateTransition(mCoordinator.StateManager, helpFlycam, splash, helpSplashTrigger, fadeTransition);
            StateTransition helpFlycamKinectTransition = new StateTransition(mCoordinator.StateManager, helpFlycam, kinectFlycam, helpKinectTrigger, fadeOutTransition);

            StateTransition kinectHelpAvatarTransition = new StateTransition(mCoordinator.StateManager, kinectAvatar, helpAvatar, customTriggerHelp, fadeInTransition);
            StateTransition kinectHelpFlycamTransition = new StateTransition(mCoordinator.StateManager, kinectFlycam, helpFlycam, customTriggerHelp, fadeInTransition);

            StateTransition splashFlythroughTransition = new StateTransition(mCoordinator.StateManager, splash, flythroughState, skeletonLost, fadeOutTransition);
            StateTransition kinectFlythroughTransition = new StateTransition(mCoordinator.StateManager, kinectAvatar, flythroughState, skeletonLost, fadeOutTransition);
            StateTransition flythroughSplashTransition = new StateTransition(mCoordinator.StateManager, flythroughState, splash, skeletonFound, fadeInTransition);
            StateTransition helpAvatarFlythroughTransition = new StateTransition(mCoordinator.StateManager, helpAvatar, flythroughState, skeletonLost, fadeOutTransition);
            StateTransition helpFlycamFlythroughTransition = new StateTransition(mCoordinator.StateManager, helpFlycam, flythroughState, skeletonLost, fadeOutTransition);

            SkeletonFeature helpSkeleton = new SkeletonFeature(1650, 1650, 800, 225f, mainWindow.Name, clip);

            splash.AddTransition(splashExploreTransition);

            explore.AddTransition(exploreAvatarTransition);
            explore.AddTransition(exploreFlycamTransition);

            kinectAvatar.AddTransition(kinectHelpAvatarTransition);

            kinectFlycam.AddTransition(kinectHelpFlycamTransition);

            helpAvatar.AddTransition(helpAvatarKinectTransition);
            helpAvatar.AddTransition(helpAvatarSplashTransition);
            helpAvatar.AddFeature(helpSkeleton);

            helpFlycam.AddTransition(helpFlycamKinectTransition);
            helpFlycam.AddTransition(helpFlycamSplashTransition);
            helpFlycam.AddFeature(helpSkeleton);

            flythroughState.AddTransition(flythroughSplashTransition);

            mCoordinator.StateManager.AddState(splash);
            mCoordinator.StateManager.AddState(kinectAvatar);
            mCoordinator.StateManager.AddState(kinectFlycam);
            mCoordinator.StateManager.AddState(helpAvatar);
            mCoordinator.StateManager.AddState(helpFlycam);

            mCoordinator.StateManager.CurrentState = splash;

            /*
            double opacity = new CoordinatorConfig().OverlayOpacity;
            IState cathedralOverlay = new ImageBGState("CathedralOverlay", mCoordinator.StateManager, "../Images/CathedralSplashScreen.png");
            mCoordinator.StateManager.CurrentState = cathedralOverlay;
            mainWindow.OverlayManager.Opacity = opacity;
            customTriggerHelp.Triggered += () => mainWindow.OverlayManager.Opacity = mainWindow.OverlayManager.Opacity > 0.0 ? 0.0 : opacity;
            customTriggerHelp.Active = true;
            */

            //Window[] windows = new Window[] { new Window("Main Window") };
            //Chimera.Overlay.MainMenu mainMenu = new Chimera.Overlay.MainMenu();
            //Coordinator input = new Coordinator(windows, mainMenu, kinect);
        }
    }
}
