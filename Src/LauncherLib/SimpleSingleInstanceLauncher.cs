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

            //IOutput output = new SetFollowCamPropertiesViewerOutput("MainWindow");
            SetWindowViewerOutput mainWindowProxy = new SetWindowViewerOutput("MainWindow");
            ISystemInput kbMouseInput = new DeltaBasedInput(new KBMouseInput());
            ISystemInput flythrough = new Chimera.Flythrough.Flythrough();
            ISystemInput mouse = new MouseInput();
            ISystemInput heightmap = new HeightmapInput();
            mKinect = new KinectInput(new IDeltaInput[] { timespan, dolphin }, new IHelpTrigger[] { trigger }, simpleFactory, pointFactory);

            Window mainWindow = new Window("MainWindow", mainWindowProxy);
            Window[] windows = new Window[] { mainWindow };
            mCoordinator = new Coordinator(windows, kbMouseInput, mKinect, mouse, heightmap, flythrough, mainWindowProxy);

            DialRenderer dialRenderer = new DialRenderer();
            CursorRenderer cursorRenderer = new DialCursorRenderer(dialRenderer, mainWindow.OverlayManager);
            //CursorTrigger t = new CursorTrigger(new CircleRenderer(100), mainWindow);

            Font font = new Font("Verdana", 62f, FontStyle.Bold);
            Rectangle clip = new Rectangle(0, 0, 1920, 1080);

            IImageTransitionFactory fade = new FadeFactory();
            ITrigger slideshowNext = new TextHoverTrigger(mainWindow.OverlayManager, cursorRenderer, 
                new StaticText("Next", mainWindow.Name, font, Color.DarkBlue, new PointF(.6f, .9f)), clip);
            ITrigger slideshowPrev = new TextHoverTrigger(mainWindow.OverlayManager, cursorRenderer, 
                new StaticText("Prev", mainWindow.Name, font, Color.DarkBlue, new PointF(.25f, .9f)), clip);


            State splash = new ImageBGState("SplashScreen", mCoordinator.StateManager, "../Images/Caen/MenuBGs/Caen-Splash.png");
            State explore = new ImageBGState("InWorldChoice", mCoordinator.StateManager, "../Images/Caen/MenuBGs/Caen-Explore.png");
            //State splash = new SeeThroughMenuState("SplashScreen", mCoordinator.StateManager, new Vector3(488f, 224f, 80f), new Rotation(23.6, 159.2));
            //State explore = new SeeThroughMenuState("InWorldChoice", mCoordinator.StateManager, new Vector3(437F, 274f, 48f), new Rotation(0.0, -128.0));
            State learn = new ImageBGState("Learn", mCoordinator.StateManager, "../Images/Caen/MenuBGs/Caen-Learn.png");
            State slideshow = new SlideshowState("Slideshow", mCoordinator.StateManager, "../Images/Caen/Slideshow", slideshowNext, slideshowPrev, fade, 1000.0);
            State kinectAvatar = new KinectControlState("KinectControlAvatar", mCoordinator.StateManager, true);
            State kinectFlycam = new KinectControlState("KinectControlFlycam", mCoordinator.StateManager, false);
            State helpAvatar = new KinectHelpState("KinectHelpAvatar", mCoordinator.StateManager, mainWindow.Name, mainWindow.Name);
            State helpFlycam = new KinectHelpState("KinectHelpFlycam", mCoordinator.StateManager, mainWindow.Name, mainWindow.Name);
            State flythroughState = new FlythroughState("Flythrough", mCoordinator.StateManager, "../Flythroughs/Caen-long.xml");

            OverlayImage exploreButton = new OverlayImage(new Bitmap("../Images/Caen/Buttons/SeeTheTownship.png"), .10f, .35f, .25f, mainWindow.Name);
            OverlayImage helpKinectButton = new OverlayImage(new Bitmap("../Images/Caen/Buttons/HelpToWorld.png"), .75f, 05f, mainWindow.Name);
            OverlayImage kinectAvatarButton = new OverlayImage(new Bitmap("../Images/Caen/Buttons/ExploreWithAvatar.png"), .15f, .4f, mainWindow.Name);
            OverlayImage kinectFlycamButton = new OverlayImage(new Bitmap("../Images/Caen/Buttons/ExploreFreely.png"), .65f, .4f, mainWindow.Name);
            OverlayImage splashButton = new OverlayImage(new Bitmap("../Images/Caen/Buttons/MainMenu.png"), .85f, .5f, mainWindow.Name);
            OverlayImage splashLearnButton = new OverlayImage(new Bitmap("../Images/Caen/Buttons/Learn.png"), .65f, .4f, mainWindow.Name);
            OverlayImage learnSlideshowButton = new OverlayImage(new Bitmap("../Images/Caen/Buttons/Slideshow.png"), .85f, .5f, mainWindow.Name);

            ImageHoverTrigger splashExploreTrigger = new ImageHoverTrigger(mainWindow.OverlayManager, cursorRenderer, exploreButton);
            ImageHoverTrigger helpKinectTrigger = new ImageHoverTrigger(mainWindow.OverlayManager, cursorRenderer, helpKinectButton);
            ImageHoverTrigger helpSplashTrigger = new ImageHoverTrigger(mainWindow.OverlayManager, cursorRenderer, splashButton);
            ImageHoverTrigger exploreAvatarTrigger = new ImageHoverTrigger(mainWindow.OverlayManager, cursorRenderer, kinectAvatarButton);
            ImageHoverTrigger exploreFlycamTrigger = new ImageHoverTrigger(mainWindow.OverlayManager, cursorRenderer, kinectFlycamButton);
            ImageHoverTrigger splashLearnTrigger = new ImageHoverTrigger(mainWindow.OverlayManager, cursorRenderer, splashLearnButton);
            ImageHoverTrigger learnSlideshowTrigger = new ImageHoverTrigger(mainWindow.OverlayManager, cursorRenderer, learnSlideshowButton);
            TextHoverTrigger backTrigger = new TextHoverTrigger(mainWindow.OverlayManager, cursorRenderer, 
                new StaticText("Back", mainWindow.Name, font, Color.DarkBlue, new PointF(.45f, .8f)), clip);

            ITrigger customTriggerHelp = new CustomTriggerTrigger(mCoordinator.StateManager, "Help");
            ITrigger skeletonLost = new SkeletonLostTrigger(mCoordinator, 15000.0);
            ITrigger skeletonFound = new SkeletonFoundTrigger();

            IImageTransitionFactory fadeFactory = new FadeFactory();
            CutWindowTransitionFactory cutTransition = new CutWindowTransitionFactory();
            BitmapFadeTransitionFactory fadeTransition = new BitmapFadeTransitionFactory(fadeFactory, 1500.0);
            OpacityFadeOutTransitionFactory fadeOutTransition = new OpacityFadeOutTransitionFactory(1500.0);
            OpacityFadeInTransitionFactory fadeInTransition = new OpacityFadeInTransitionFactory(1500.0);

            StateTransition splashExploreTransition = new StateTransition(mCoordinator.StateManager, splash, explore, splashExploreTrigger, fadeTransition);
            StateTransition splashLearnTransition = new StateTransition(mCoordinator.StateManager, splash, learn, splashLearnTrigger, fadeTransition);

            StateTransition learnSlideshowTransition = new StateTransition(mCoordinator.StateManager, learn, slideshow, learnSlideshowTrigger, fadeTransition);
            StateTransition learnSplashTransition = new StateTransition(mCoordinator.StateManager, learn, splash, backTrigger, fadeTransition);

            StateTransition slideshowLearnTransition = new StateTransition(mCoordinator.StateManager, slideshow, learn, backTrigger, fadeTransition);

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
            splash.AddTransition(splashLearnTransition);

            learn.AddTransition(learnSlideshowTransition);
            learn.AddTransition(learnSplashTransition);

            slideshow.AddTransition(slideshowLearnTransition);

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

            //Window[] windows = new Window[] { new Window("MainWindow") };
            //Chimera.Overlay.MainMenu mainMenu = new Chimera.Overlay.MainMenu();
            //Coordinator input = new Coordinator(windows, mainMenu, kinect);
        }
    }
}
