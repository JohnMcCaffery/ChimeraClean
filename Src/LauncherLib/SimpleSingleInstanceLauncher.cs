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
            State structured = new ImageBGState("Structured", mCoordinator.StateManager, "../Images/Caen/MenuBGs/Caen-Structured.png");
            State slideshow = new SlideshowState("Slideshow", mCoordinator.StateManager, "../Images/Caen/Slideshow", slideshowNext, slideshowPrev, fade, 1000.0);
            State kinectAvatar = new KinectControlState("KinectControlAvatar", mCoordinator.StateManager, true);
            State kinectFlycam = new KinectControlState("KinectControlFlycam", mCoordinator.StateManager, false);
            State helpAvatar = new KinectHelpState("KinectHelpAvatar", mCoordinator.StateManager, mainWindow.Name, mainWindow.Name);
            State helpFlycam = new KinectHelpState("KinectHelpFlycam", mCoordinator.StateManager, mainWindow.Name, mainWindow.Name);
            State idleFlythrough = new FlythroughState("IdleFlythrough", mCoordinator.StateManager, "../Flythroughs/Caen-long.xml");
            State structuredFlythrough = new FlythroughState("StructuredFlythrough", mCoordinator.StateManager, "../Flythroughs/Caen-Guided.xml", slideshowNext);

            IImageTransitionFactory fadeFactory = new FadeFactory();
            CutWindowTransitionFactory cutTransition = new CutWindowTransitionFactory();
            BitmapFadeTransitionFactory fadeTransition = new BitmapFadeTransitionFactory(fadeFactory, 1500.0);
            OpacityFadeOutTransitionFactory fadeOutTransition = new OpacityFadeOutTransitionFactory(1500.0);
            OpacityFadeInTransitionFactory fadeInTransition = new OpacityFadeInTransitionFactory(1500.0);

            ImgTrans(splash,        explore,                "SeeTheTownship",       .10f, .35f, .25f, mainWindow, cursorRenderer, fadeTransition);
            ImgTrans(splash,        learn,                  "Learn",                .65f, .4f, mainWindow, cursorRenderer, fadeTransition);
            ImgTrans(helpAvatar,    kinectAvatar,           "HelpToWorld",          .85f, .15f, .1f, mainWindow, cursorRenderer, fadeOutTransition);
            ImgTrans(helpAvatar,    splash,                 "MainMenu",             .85f, .35f, .1f, mainWindow, cursorRenderer, fadeTransition);
            ImgTrans(helpFlycam,    kinectFlycam,           "HelpToWorld",          .85f, .15f, .1f, mainWindow, cursorRenderer, fadeOutTransition);
            ImgTrans(helpFlycam,    splash,                 "MainMenu",             .85f, .35f, .1f, mainWindow, cursorRenderer, fadeTransition);
            ImgTrans(explore,       kinectAvatar,           "ExploreWithAvatar",    .15f, .4f, mainWindow, cursorRenderer, fadeOutTransition);
            ImgTrans(explore,       kinectFlycam,           "ExploreFreely",        .45f, .4f, mainWindow, cursorRenderer, fadeOutTransition);
            TxtTrans(explore,       structured,             "Guide Me",             .75f, .4f, font, Color.DarkBlue, clip, mainWindow, cursorRenderer, fadeTransition);
            ImgTrans(learn,         slideshow,              "Slideshow",            .65f, .5f, mainWindow, cursorRenderer, fadeTransition);
            TxtTrans(learn,         splash,                 "Back",                 .45f, .9f, font, Color.DarkBlue, clip, mainWindow, cursorRenderer, fadeTransition);
            TxtTrans(slideshow,     learn,                  "Back",                 .45f, .9f, font, Color.DarkBlue, clip, mainWindow, cursorRenderer, fadeTransition);
            TxtTrans(structured,    structuredFlythrough,   "Guided Tour",          .35f, .4f, font, Color.DarkBlue, clip, mainWindow, cursorRenderer, fadeTransition);
            TxtTrans(structuredFlythrough,  structured,     "Back",                 .45f, .9f, font, Color.DarkBlue, clip, mainWindow, cursorRenderer, fadeTransition);

            ITrigger customTriggerHelp = new CustomTriggerTrigger(mCoordinator.StateManager, "Help");
            ITrigger skeletonLost = new SkeletonLostTrigger(mCoordinator, 15000.0);
            ITrigger skeletonFound = new SkeletonFoundTrigger();

            StateTransition kinectHelpAvatarTransition = new StateTransition(mCoordinator.StateManager, kinectAvatar, helpAvatar, customTriggerHelp, fadeInTransition);
            StateTransition kinectHelpFlycamTransition = new StateTransition(mCoordinator.StateManager, kinectFlycam, helpFlycam, customTriggerHelp, fadeInTransition);

            StateTransition splashFlythroughTransition = new StateTransition(mCoordinator.StateManager, splash, idleFlythrough, skeletonLost, fadeOutTransition);
            StateTransition kinectFlythroughTransition = new StateTransition(mCoordinator.StateManager, kinectAvatar, idleFlythrough, skeletonLost, fadeOutTransition);
            StateTransition flythroughSplashTransition = new StateTransition(mCoordinator.StateManager, idleFlythrough, splash, skeletonFound, fadeInTransition);
            StateTransition helpAvatarFlythroughTransition = new StateTransition(mCoordinator.StateManager, helpAvatar, idleFlythrough, skeletonLost, fadeOutTransition);
            StateTransition helpFlycamFlythroughTransition = new StateTransition(mCoordinator.StateManager, helpFlycam, idleFlythrough, skeletonLost, fadeOutTransition);

            SkeletonFeature helpSkeleton = new SkeletonFeature(1650, 1650, 800, 225f, mainWindow.Name, clip);

            kinectAvatar.AddTransition(kinectHelpAvatarTransition);
            kinectFlycam.AddTransition(kinectHelpFlycamTransition);

            //TODO - should this be here or inbuilt?
            helpAvatar.AddFeature(helpSkeleton);
            helpFlycam.AddFeature(helpSkeleton);

            idleFlythrough.AddTransition(flythroughSplashTransition);

            mCoordinator.StateManager.AddState(splash);
            mCoordinator.StateManager.AddState(kinectAvatar);
            mCoordinator.StateManager.AddState(kinectFlycam);
            mCoordinator.StateManager.AddState(helpAvatar);
            mCoordinator.StateManager.AddState(helpFlycam);
            mCoordinator.StateManager.AddState(structured);
            mCoordinator.StateManager.AddState(structuredFlythrough);
            mCoordinator.StateManager.AddState(slideshow);
            mCoordinator.StateManager.AddState(learn);
            //mCoordinator.StateManager.CurrentState = splash;

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

        static void ImgTrans(State from, State to, string image, float x, float y, Window window, IHoverSelectorRenderer renderer, IWindowTransitionFactory transition) {
            ImgTrans(from, to, image, x, y, -1f, window, renderer, transition);
        }
        static void ImgTrans(State from, State to, string image, float x, float y, float w, Window window, IHoverSelectorRenderer renderer, IWindowTransitionFactory transition) {
            OverlayImage exploreButton = new OverlayImage(new Bitmap("../Images/Caen/Buttons/" + image + ".png"), x, y, w, window.Name);
            ImageHoverTrigger splashExploreTrigger = new ImageHoverTrigger(window.OverlayManager, renderer, exploreButton);
            StateTransition splashExploreTransition = new StateTransition(window.Coordinator.StateManager, from, to, splashExploreTrigger, transition);
            from.AddTransition(splashExploreTransition);
        }

        static void TxtTrans(State from, State to, string text, float x, float y, Font font, Color colour, Rectangle clip, Window window, IHoverSelectorRenderer renderer, IWindowTransitionFactory transition) {
            Text txt = new StaticText(text, window.Name, font, colour, new PointF(x, y));
            TextHoverTrigger splashExploreTrigger = new TextHoverTrigger(window.OverlayManager, renderer, txt, clip);
            StateTransition splashExploreTransition = new StateTransition(window.Coordinator.StateManager, from, to, splashExploreTrigger, transition);
            from.AddTransition(splashExploreTransition);
        }
    }
}
