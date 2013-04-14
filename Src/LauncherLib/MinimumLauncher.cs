using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Kinect.Interfaces;
using Chimera.Kinect;
using Chimera.Inputs;
using Chimera.OpenSim;
using Chimera.Overlay.Triggers;
using System.Drawing;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay.Drawables;
using Chimera.Overlay.Transitions;
using Chimera.Overlay.States;
using Chimera.Overlay;
using Chimera.Flythrough.Overlay;

namespace Chimera.Launcher {
    public class MinimumLauncher : Launcher {
        private SetWindowViewerOutput mMainWindowProxy = new SetWindowViewerOutput("MainWindow");

        protected override ISystemInput[] GetInputs() {
            /*
            TimespanMovementInput timespan = new TimespanMovementInput();
            DolphinMovementInput dolphin = new DolphinMovementInput();
            RaiseArmHelpTrigger trigger = new RaiseArmHelpTrigger();
            SimpleCursorFactory simpleFactory = new SimpleCursorFactory();
            PointCursorFactory pointFactory = new PointCursorFactory();
            */

            //IOutput output = new SetFollowCamPropertiesViewerOutput("MainWindow");
            ISystemInput kbMouseInput = new DeltaBasedInput(new KBMouseInput());
            /*
            ISystemInput flythrough = new Chimera.Flythrough.Flythrough();
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
            */

            ISystemInput mouse = new MouseInput();

            return new ISystemInput[] { 
                kbMouseInput, 
                //kinectInput, 
                mouse, 
                //heightmap, 
                //flythrough, 
                mMainWindowProxy 
            };
        }

        protected override Window[] GetWindows() {
            return new Window[] { new Window("MainWindow", mMainWindowProxy)};
        }

        protected override void InitOverlay() {
            Coordinator.ControlMode = ControlMode.Delta;
            return;
            Window mainWindow = Coordinator["MainWindow"];

            DialRenderer dialRenderer = new DialRenderer();
            CursorRenderer cursorRenderer = new DialCursorRenderer(dialRenderer, mainWindow.OverlayManager);

            Font font = new Font("Verdana", 62f, FontStyle.Bold);
            Rectangle clip = new Rectangle(0, 0, 1920, 1080);

            IImageTransitionFactory fade = new FadeFactory();
            ITrigger slideshowNext = new TextHoverTrigger(mainWindow.OverlayManager, cursorRenderer, 
                new StaticText("Next", mainWindow.Name, font, Color.DarkBlue, new PointF(.6f, .9f)), clip);
            ITrigger slideshowPrev = new TextHoverTrigger(mainWindow.OverlayManager, cursorRenderer, 
                new StaticText("Prev", mainWindow.Name, font, Color.DarkBlue, new PointF(.25f, .9f)), clip);

            IImageTransitionFactory fadeFactory = new FadeFactory();
            CutWindowTransitionFactory cutTransition = new CutWindowTransitionFactory();
            BitmapFadeTransitionFactory fadeTransition = new BitmapFadeTransitionFactory(fadeFactory, 1500.0);
            OpacityFadeOutTransitionFactory fadeOutTransition = new OpacityFadeOutTransitionFactory(1500.0);
            OpacityFadeInTransitionFactory fadeInTransition = new OpacityFadeInTransitionFactory(1500.0);

            State splash = new ImageBGState("SplashScreen", Coordinator.StateManager, "../Images/Caen/MenuBGs/Caen-Splash.png");
            State slideshow = new SlideshowState("Slideshow", Coordinator.StateManager, "../Images/Caen/Slideshow", slideshowNext, slideshowPrev, fade, 1000.0);
            /*
            State kinectAvatar = new KinectControlState("KinectControlAvatar", Coordinator.StateManager, true);
            State kinectFlycam = new KinectControlState("KinectControlFlycam", Coordinator.StateManager, false);
            State helpAvatar = new KinectHelpState("KinectHelpAvatar", Coordinator.StateManager, mainWindow.Name, mainWindow.Name);
            State helpFlycam = new KinectHelpState("KinectHelpFlycam", Coordinator.StateManager, mainWindow.Name, mainWindow.Name);
            */
            State idleFlythrough = new FlythroughState("IdleFlythrough", Coordinator.StateManager, "../Flythroughs/Caen-long.xml");
            State structuredFlythrough = new FlythroughState("StructuredFlythrough", Coordinator.StateManager, "../Flythroughs/Caen-Guided.xml", splash, fadeTransition, slideshowNext);
            State infoVideo = new VideoState("Video", Coordinator.StateManager, mainWindow.Name, "../Videos/Vid1.flv", splash, fadeTransition);
            State storyWolf = new VideoState("Story1", Coordinator.StateManager, mainWindow.Name, "../Videos/Wolf.flv", splash, fadeTransition);

            /*
            ImgTrans(helpAvatar,    kinectAvatar,           "HelpToWorld",          .85f, .15f, .1f, mainWindow, cursorRenderer, fadeOutTransition);
            ImgTrans(helpFlycam,    kinectFlycam,           "HelpToWorld",          .85f, .15f, .1f, mainWindow, cursorRenderer, fadeOutTransition);
            ImgTrans(helpAvatar,    splash,                 "MainMenu",             .85f, .35f, .1f, mainWindow, cursorRenderer, fadeTransition);
            ImgTrans(helpFlycam,    splash,                 "MainMenu",             .85f, .35f, .1f, mainWindow, cursorRenderer, fadeTransition);
            */

            TxtTrans(slideshow,     splash,                 "Back",                 .45f, .9f, font, Color.DarkBlue, clip, mainWindow, cursorRenderer, fadeTransition);
            TxtTrans(structuredFlythrough,  splash,         "Back",                 .45f, .9f, font, Color.DarkBlue, clip, mainWindow, cursorRenderer, fadeTransition);
            TxtTrans(infoVideo,     splash,                 "Back",                 .45f, .9f, font, Color.DarkBlue, clip, mainWindow, cursorRenderer, fadeTransition);

            InvisTrans(splash, infoVideo, new Point(116,197), new Point(397,346), clip, mainWindow, cursorRenderer, fadeTransition);
            InvisTrans(splash, slideshow, new Point(464,197), new Point(741,346), clip, mainWindow, cursorRenderer, fadeTransition);
            InvisTrans(splash, structuredFlythrough, new Point(494,585), new Point(768,730), clip, mainWindow, cursorRenderer, fadeOutTransition);
            //InvisTrans(splash, kinectAvatar, new Point(147,585), new Point(421,730), clip, mainWindow, cursorRenderer, fadeOutTransition);
            //InvisTrans(splash, kinectFlycam, new Point(312,748), new Point(582,899), clip, mainWindow, cursorRenderer, fadeOutTransition);
            InvisTrans(splash, storyWolf, new Point(1068,670), new Point(1340,790), clip, mainWindow, cursorRenderer, fadeTransition);

            /*
            ITrigger customTriggerHelp = new CustomTriggerTrigger(Coordinator.StateManager, "Help");
            ITrigger skeletonLost = new SkeletonLostTrigger(Coordinator, 15000.0);
            ITrigger skeletonFound = new SkeletonFoundTrigger();

            StateTransition kinectHelpAvatarTransition = new StateTransition(Coordinator.StateManager, kinectAvatar, helpAvatar, customTriggerHelp, fadeInTransition);
            StateTransition kinectHelpFlycamTransition = new StateTransition(Coordinator.StateManager, kinectFlycam, helpFlycam, customTriggerHelp, fadeInTransition);

            StateTransition splashFlythroughTransition = new StateTransition(Coordinator.StateManager, splash, idleFlythrough, skeletonLost, fadeOutTransition);
            StateTransition kinectFlythroughTransition = new StateTransition(Coordinator.StateManager, kinectAvatar, idleFlythrough, skeletonLost, fadeOutTransition);
            StateTransition flythroughSplashTransition = new StateTransition(Coordinator.StateManager, idleFlythrough, splash, skeletonFound, fadeInTransition);
            StateTransition helpAvatarFlythroughTransition = new StateTransition(Coordinator.StateManager, helpAvatar, idleFlythrough, skeletonLost, fadeOutTransition);
            StateTransition helpFlycamFlythroughTransition = new StateTransition(Coordinator.StateManager, helpFlycam, idleFlythrough, skeletonLost, fadeOutTransition);

            kinectAvatar.AddTransition(kinectHelpAvatarTransition);
            kinectFlycam.AddTransition(kinectHelpFlycamTransition);

            idleFlythrough.AddTransition(flythroughSplashTransition);

            Coordinator.StateManager.AddState(splash);
            Coordinator.StateManager.AddState(kinectAvatar);
            Coordinator.StateManager.AddState(kinectFlycam);
            Coordinator.StateManager.AddState(helpAvatar);
            Coordinator.StateManager.AddState(helpFlycam);
            */
            Coordinator.StateManager.AddState(structuredFlythrough);
            Coordinator.StateManager.AddState(slideshow);
            Coordinator.StateManager.AddState(infoVideo);
            Coordinator.StateManager.AddState(storyWolf);
            Coordinator.StateManager.CurrentState = splash;
        }
    }
}
