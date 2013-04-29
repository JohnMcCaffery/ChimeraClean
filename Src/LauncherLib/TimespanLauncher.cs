using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay.Triggers;
using System.Drawing;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay;
using Chimera.Overlay.States;
using Chimera.Kinect.Overlay;
using Chimera.Flythrough.Overlay;
using Chimera.Overlay.Transitions;
using Chimera.Overlay.Drawables;
using Chimera.OpenSim;
using Chimera.Kinect;
using Chimera.Inputs;
using Chimera.Kinect.Interfaces;
using Joystick;

namespace Chimera.Launcher {
    public class TimespanLauncher : Launcher{
        private SetWindowViewerOutput mMainWindowProxy = new SetWindowViewerOutput("MainWindow");

        protected override ISystemInput[] GetInputs() {
            TimespanMovementInput timespan = new TimespanMovementInput();
            DolphinMovementInput dolphin = new DolphinMovementInput();
            RaiseArmHelpTrigger trigger = new RaiseArmHelpTrigger();
            SimpleCursorFactory simpleFactory = new SimpleCursorFactory();
            PointCursorFactory pointFactory = new PointCursorFactory();

            //IOutput output = new SetFollowCamPropertiesViewerOutput("MainWindow");
            ISystemInput joystick = new DeltaBasedInput(new JoystickInput());
            ISystemInput kbMouseInput = new DeltaBasedInput(new KBMouseInput());
            ISystemInput flythrough = new Chimera.Flythrough.Flythrough();
            ISystemInput mouse = new MouseInput();
            ISystemInput heightmap = new HeightmapInput();
            ISystemInput kinectInput = new KinectInput(
                new IDeltaInput[] { 
                    timespan, 
                    dolphin 
                }, 
                new IHelpTrigger[] { 
                    trigger 
                }, 
                simpleFactory, 
                pointFactory
                );

            return new ISystemInput[] { 
                joystick,
                kbMouseInput, 
                kinectInput, 
                mouse, 
                heightmap, 
                flythrough, 
                mMainWindowProxy 
            };
        }

        protected override Window[] GetWindows() {
            return new Window[] { new Window("MainWindow", mMainWindowProxy)};
        }

        protected override void InitOverlay() {
            Window mainWindow = Coordinator["MainWindow"];

            DialRenderer dialRenderer = new DialRenderer();
            CursorRenderer cursorRenderer = new DialCursorRenderer(dialRenderer, mainWindow.OverlayManager);
            //CursorTrigger t = new CursorTrigger(new CircleRenderer(100), mainWindow);

            Font font = new Font("Verdana", 52, FontStyle.Bold);
            Rectangle clip = new Rectangle(0, 0, 1920, 1080);

            /*
            ITrigger slideshowNext = new TextHoverTrigger(mainWindow.OverlayManager, cursorRenderer, 
                new StaticText("Next", mainWindow.Name, font, Color.DarkBlue, new PointF(.85f, .85f)), clip);
            ITrigger slideshowPrev = new TextHoverTrigger(mainWindow.OverlayManager, cursorRenderer, 
                new StaticText("Prev", mainWindow.Name, font, Color.DarkBlue, new PointF(.25f, .9f)), clip);
            */
            ITrigger slideshowNext = new ImageHoverTrigger(mainWindow.OverlayManager, cursorRenderer, 
                new OverlayImage(new Bitmap("../Images/Caen/Buttons/NextTrans.png"), .85f, .85f, mainWindow.Name));
            ITrigger slideshowPrev = new ImageHoverTrigger(mainWindow.OverlayManager, cursorRenderer, 
                new OverlayImage(new Bitmap("../Images/Caen/Buttons/PrevTrans.png"), .05f, .85f, mainWindow.Name));
            ITrigger flythroughNext = new ImageHoverTrigger(mainWindow.OverlayManager, cursorRenderer, 
                new OverlayImage(new Bitmap("../Images/Caen/Buttons/Next.png"), .85f, .85f, mainWindow.Name));

            IImageTransitionFactory fade = new FadeFactory();
            CutWindowTransitionFactory cutTransition = new CutWindowTransitionFactory();
            BitmapFadeTransitionFactory fadeTransition = new BitmapFadeTransitionFactory(fade, 1500.0);
            OpacityFadeOutTransitionFactory fadeOutTransition = new OpacityFadeOutTransitionFactory(1500.0);
            OpacityFadeInTransitionFactory fadeInTransition = new OpacityFadeInTransitionFactory(1500.0);

            State splash = new ImageBGState("SplashScreen", Coordinator.StateManager, "../Images/Caen/MenuBGs/Caen-Splash.png");
            State kinectAvatar = new KinectControlState("KinectControlAvatar", Coordinator.StateManager, true);
            State kinectFlycam = new KinectControlState("KinectControlFlycam", Coordinator.StateManager, false);
            State helpAvatar = new KinectHelpState("KinectHelpAvatar", Coordinator.StateManager, mainWindow.Name, mainWindow.Name);
            State helpFlycam = new KinectHelpState("KinectHelpFlycam", Coordinator.StateManager, mainWindow.Name, mainWindow.Name);
            State idleFlythrough = new FlythroughState("IdleFlythrough", Coordinator.StateManager, "../Flythroughs/Caen-long.xml");
            State slideshow = new SlideshowState("Slideshow", Coordinator.StateManager, "../Images/Caen/TodaySlideshow", slideshowNext, slideshowPrev, fade, 1500);
            State structuredFlythrough = new FlythroughState("StructuredFlythrough", Coordinator.StateManager, "../Flythroughs/Caen-Guided.xml", splash, fadeTransition, flythroughNext);

            State confirmDog = new ImageBGState("Story1Confirm", Coordinator.StateManager, "../Images/Caen/StorytellingVideos/ConfirmDogs.png");
            State confirmGartymore = new ImageBGState("Story2Confirm", Coordinator.StateManager, "../Images/Caen/StorytellingVideos/ConfirmGartymore.png");
            State confirmFrakkok = new ImageBGState("Story3Confirm", Coordinator.StateManager, "../Images/Caen/StorytellingVideos/ConfirmFrakkok.png");
            State confirmFisherman = new ImageBGState("Story4Confirm", Coordinator.StateManager, "../Images/Caen/StorytellingVideos/ConfirmFisherman.png");
            State confirmWolf = new ImageBGState("Story5Confirm", Coordinator.StateManager, "../Images/Caen/StorytellingVideos/ConfirmWolf.png");

            State storyDog = new OverlayVideoState("Story1", mainWindow.OverlayManager, "../../Videos/dogs.mpg", splash, fadeTransition);
            State storyGartymore = new OverlayVideoState("Story2", mainWindow.OverlayManager, "../../Videos/garyore.mpg", splash, fadeTransition);
            State storyFrakkok = new OverlayVideoState("Story3", mainWindow.OverlayManager, "../../Videos/orkney.mpg", splash, fadeTransition);
            State storyFisherman = new OverlayVideoState("Story4", mainWindow.OverlayManager, "../../Videos/fisherman.mpg", splash, fadeTransition);
            State storyWolf = new OverlayVideoState("Story5", mainWindow.OverlayManager, "../../Videos/wolf.mpg", splash, fadeTransition);
            /*
            State storyDog = new VideoState("Story1", Coordinator.StateManager, mainWindow.Name, "../../Videos/dogs.mpg", splash, fadeTransition);
            State storyGartymore = new VideoState("Story2", Coordinator.StateManager, mainWindow.Name, "../../Videos/garyore.mpg", splash, fadeTransition);
            State storyFrakkok = new VideoState("Story3", Coordinator.StateManager, mainWindow.Name, "../../Videos/orkney.mpg", splash, fadeTransition);
            State storyFisherman = new VideoState("Story4", Coordinator.StateManager, mainWindow.Name, "../../Videos/fisherman.mpg", splash, fadeTransition);
            State storyWolf = new VideoState("Story5", Coordinator.StateManager, mainWindow.Name, "../../Videos/wolf.mpg", splash, fadeTransition);
            */

            ImgTrans(helpAvatar,    kinectAvatar,           "HelpToWorld",          .85f, .15f, .1f, mainWindow, cursorRenderer, fadeOutTransition);
            ImgTrans(helpFlycam,    kinectFlycam,           "HelpToWorld",          .85f, .15f, .1f, mainWindow, cursorRenderer, fadeOutTransition);
            ImgTrans(helpAvatar,    splash,                 "MainMenu",             .85f, .35f, .1f, mainWindow, cursorRenderer, fadeTransition);
            ImgTrans(helpFlycam,    splash,                 "MainMenu",             .85f, .35f, .1f, mainWindow, cursorRenderer, fadeTransition);

            //TxtTrans(structuredFlythrough,  splash,         "Back",                 .05f, .85f, font, Color.DarkBlue, clip, mainWindow, cursorRenderer, fadeTransition);
            ImgTrans(structuredFlythrough,  splash,         "Back",                 .05f, .85f, mainWindow, cursorRenderer, fadeTransition);
            ImgTrans(slideshow,  splash,                    "BackTrans",            .45f, .85f, mainWindow, cursorRenderer, fadeTransition);

            InvisTrans(splash, confirmDog,                new Point(115,395), new Point(835,485), clip, mainWindow, cursorRenderer, fadeTransition);
            InvisTrans(splash, confirmGartymore,          new Point(115,510), new Point(835,600), clip, mainWindow, cursorRenderer, fadeTransition);
            InvisTrans(splash, confirmFrakkok,            new Point(115,625), new Point(835,720), clip, mainWindow, cursorRenderer, fadeTransition);
            InvisTrans(splash, confirmFisherman,          new Point(115,745), new Point(835,825), clip, mainWindow, cursorRenderer, fadeTransition);
            InvisTrans(splash, confirmWolf,               new Point(115,860), new Point(835,955), clip, mainWindow, cursorRenderer, fadeTransition);
            
            InvisTrans(confirmDog, splash,               new Point(320,380), new Point(770,665), clip, mainWindow, cursorRenderer, fadeTransition);
            InvisTrans(confirmDog, storyWolf,            new Point(960,380), new Point(1410,665), clip, mainWindow, cursorRenderer, fadeTransition);
            InvisTrans(confirmGartymore, splash,         new Point(320,380), new Point(770,665), clip, mainWindow, cursorRenderer, fadeTransition);
            InvisTrans(confirmGartymore, storyGartymore, new Point(960,380), new Point(1410,665), clip, mainWindow, cursorRenderer, fadeTransition);
            InvisTrans(confirmFrakkok, splash,           new Point(320,380), new Point(770,665), clip, mainWindow, cursorRenderer, fadeTransition);
            InvisTrans(confirmFrakkok, storyFrakkok,     new Point(960,380), new Point(1410,665), clip, mainWindow, cursorRenderer, fadeTransition);
            InvisTrans(confirmFisherman, splash,         new Point(320,380), new Point(770,665), clip, mainWindow, cursorRenderer, fadeTransition);
            InvisTrans(confirmFisherman, storyFisherman, new Point(960,380), new Point(1410,665), clip, mainWindow, cursorRenderer, fadeTransition);
            InvisTrans(confirmWolf, splash,              new Point(320,380), new Point(770,665), clip, mainWindow, cursorRenderer, fadeTransition);
            InvisTrans(confirmWolf, storyWolf,           new Point(960,380), new Point(1410,665), clip, mainWindow, cursorRenderer, fadeTransition);

            InvisTrans(splash, structuredFlythrough,    new Point(1080,415), new Point(1755,500), clip, mainWindow, cursorRenderer, fadeOutTransition);
            InvisTrans(splash, kinectAvatar,            new Point(1080,535), new Point(1755,620), clip, mainWindow, cursorRenderer, fadeOutTransition);
            InvisTrans(splash, kinectFlycam,            new Point(1080,645), new Point(1755,735), clip, mainWindow, cursorRenderer, fadeOutTransition);
            InvisTrans(splash, slideshow,               new Point(1080,765), new Point(1755,865), clip, mainWindow, cursorRenderer, fadeTransition);

            ITrigger customTriggerHelp = new CustomTriggerTrigger(Coordinator.StateManager, "Help");

            StateTransition kinectHelpAvatarTransition = new StateTransition(Coordinator.StateManager, kinectAvatar, helpAvatar, customTriggerHelp, fadeInTransition);
            StateTransition kinectHelpFlycamTransition = new StateTransition(Coordinator.StateManager, kinectFlycam, helpFlycam, customTriggerHelp, fadeInTransition);
           

            kinectAvatar.AddTransition(kinectHelpAvatarTransition);
            kinectFlycam.AddTransition(kinectHelpFlycamTransition);

            SkeletonFeature splashSkeleton = new SkeletonFeature(940, 1470, 1000, 100f, mainWindow.Name, clip);
            splash.AddFeature(splashSkeleton);

            FadingText raiseHandText = new FadingText("Stretch your arm above your head for help", 30000, 30000, mainWindow.Name, Color.Red, font, 150, 10, clip);
            kinectAvatar.AddFeature(raiseHandText);
            kinectFlycam.AddFeature(raiseHandText);

            Coordinator.StateManager.AddState(splash);
            Coordinator.StateManager.AddState(kinectAvatar);
            Coordinator.StateManager.AddState(kinectFlycam);
            Coordinator.StateManager.AddState(helpAvatar);
            Coordinator.StateManager.AddState(helpFlycam);
            Coordinator.StateManager.AddState(slideshow);
            Coordinator.StateManager.AddState(structuredFlythrough);
            Coordinator.StateManager.AddState(idleFlythrough);
            Coordinator.StateManager.AddState(storyDog);
            Coordinator.StateManager.AddState(storyGartymore);
            Coordinator.StateManager.AddState(storyFrakkok);
            Coordinator.StateManager.AddState(storyFisherman);
            Coordinator.StateManager.AddState(storyWolf);
            Coordinator.StateManager.CurrentState = splash;
            //Coordinator.StateManager.CurrentState = structuredFlythrough;
            //Coordinator.StateManager.CurrentState = helpFlycam;
            //Coordinator.StateManager.CurrentState = kinectFlycam;

            OverlayConfig cfg = new OverlayConfig();
            if (cfg.IdleState)
                InitIdle(idleFlythrough, splash, fadeInTransition, fadeOutTransition, cfg.IdleTimeoutMs);

            /*
            ImgTrans(splash,        explore,                "SeeTheTownship",       .10f, .35f, .25f, mainWindow, cursorRenderer, fadeTransition);
            ImgTrans(splash,        learn,                  "Learn",                .65f, .4f, mainWindow, cursorRenderer, fadeTransition);
            ImgTrans(explore,       kinectAvatar,           "ExploreWithAvatar",    .15f, .4f, mainWindow, cursorRenderer, fadeOutTransition);
            ImgTrans(explore,       kinectFlycam,           "ExploreFreely",        .45f, .4f, mainWindow, cursorRenderer, fadeOutTransition);
            TxtTrans(explore,       structured,             "Guide Me",             .75f, .4f, font, Color.DarkBlue, clip, mainWindow, cursorRenderer, fadeTransition);
            ImgTrans(learn,         slideshow,              "Slideshow",            .65f, .4f, mainWindow, cursorRenderer, fadeTransition);
            TxtTrans(learn,         infoVideo,              "Show me about the township", .25f, .4f, font, Color.DarkBlue, clip, mainWindow, cursorRenderer, fadeTransition);
            TxtTrans(learn,         splash,                 "Back",                 .45f, .9f, font, Color.DarkBlue, clip, mainWindow, cursorRenderer, fadeTransition);
            TxtTrans(structured,    structuredFlythrough,   "Guided Tour",          .35f, .4f, font, Color.DarkBlue, clip, mainWindow, cursorRenderer, fadeTransition);
            */

            /*
            double opacity = new CoordinatorConfig().OverlayOpacity;
            IState cathedralOverlay = new ImageBGState("CathedralOverlay", Coordinator.StateManager, "../Images/CathedralSplashScreen.png");
            Coordinator.StateManager.CurrentState = cathedralOverlay;
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
