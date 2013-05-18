/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.GUI.Forms;
using Chimera.Overlay.Drawables;
using Chimera.Overlay.Triggers;
using Chimera.Overlay;
using System.Drawing;
using Chimera.Interfaces.Overlay;
using Chimera.Kinect.Overlay;
using Chimera.Overlay.Transitions;
using Chimera.Config;
using Touchscreen;
using Chimera.Plugins;
using Joystick;
using Chimera.Flythrough;
using Chimera.OpenSim;
using Chimera.Kinect.GUI;
using Chimera.Kinect;
using System.IO;
using System.Reflection;
using Chimera.Util;
using System.Threading;
using Joystick.Overlay;

namespace Chimera.Launcher {
    public abstract class Launcher {
        private readonly Coordinator mCoordinator;
        private LauncherConfig mConfig;
        private CoordinatorForm mForm;
        private IHoverSelectorRenderer mRenderer;
        private string mButtonFolder = "../Images/Examples/";
        protected SetWindowViewerOutput mMainWindowProxy = new SetWindowViewerOutput("MainWindow");

        public Coordinator Coordinator {
            get { return mCoordinator; }
        }
        public CoordinatorForm Form {
            get {
                if (mForm == null)
                    mForm = new CoordinatorForm(mCoordinator);
                return mForm;
            }
        }
        protected LauncherConfig Config {
            get { return mConfig; }
        }

        public Launcher(params string[] args) {
            mConfig = new LauncherConfig(args);
            mCoordinator = new Coordinator(GetWindows(), GetInputs());
            mButtonFolder = Path.GetFullPath(mConfig.ButtonFolder);

            if (mConfig.InterfaceMode != StateManager.CLICK_MODE)
                mRenderer = new DialCursorRenderer();

            if (mConfig.InitOverlay) {
                InitOverlay();
                State home = mCoordinator.StateManager.States.FirstOrDefault(s => s.Name == mConfig.HomeState);
                if (mConfig.IdleState != "None") {
                    Coordinator.StateManager.IdleEnabled = true;
                    State idle = mCoordinator.StateManager.States.FirstOrDefault(s => s.Name == mConfig.IdleState);
                    if (idle != null && home != null)
                        InitIdle(idle, home, new OpacityFadeInWindowTransitionFactory(mConfig.IdleFadeTime), new OpacityFadeOutWindowTransitionFactory(mConfig.IdleFadeTime), mConfig.IdleTimeoutMs);
                    else
                        Console.WriteLine("Unable to create idle state. The idle state specified (" + mConfig.IdleState + ") was not found.");
                }

                if (home != null)
                    mCoordinator.StateManager.CurrentState = home;
            }
        }

        protected virtual Window[] GetWindows() {
            return new Window[] { new Window("MainWindow", mMainWindowProxy)};
        }

        protected virtual ISystemPlugin[] GetInputs() {
            List<ISystemPlugin> plugins = new List<ISystemPlugin>();
            //Control
            //if (Config.InterfaceMode == StateManager.CLICK_MODE)
                //plugins.Add(new TouchscreenPlugin());
            plugins.Add(new KBMousePlugin());
            plugins.Add(new XBoxControllerPlugin());
            plugins.Add(mMainWindowProxy);

            //Flythrough
            plugins.Add(new FlythroughPlugin());

            //Overlay
            plugins.Add(new MousePlugin());

            //Heightmap
            plugins.Add(new HeightmapPlugin());

            //Kinect
            if (Config.InterfaceMode != StateManager.CLICK_MODE) {
                plugins.Add(new KinectCamera());
                plugins.Add(new KinectMovementPlugin());
                plugins.Add(new SimpleKinectCursor());
                plugins.Add(new RaiseArmHelpTrigger());
            }

            return plugins.ToArray();
        }

        protected abstract void InitOverlay();

        protected void InvisTrans(State from, State to, Point topLeft, Point bottomRight, Rectangle clip, Window window, IWindowTransitionFactory transition) {
            ITrigger trigger  = InvisTrigger(topLeft, bottomRight, clip, window);
            StateTransition splashExploreTransition = new StateTransition(window.Coordinator.StateManager, from, to, trigger, transition);
            from.AddTransition(splashExploreTransition);
        }

        protected void ImgTrans(State from, State to, string image, float x, float y, Window window, IWindowTransitionFactory transition) {
            ImgTrans(from, to, image, x, y, -1f, window, transition);
        }
        protected void ImgTrans(State from, State to, string image, float x, float y, float w, Window window, IWindowTransitionFactory transition) {
            ITrigger trigger = ImgTrigger(window, image, x, y, w);
            StateTransition splashExploreTransition = new StateTransition(window.Coordinator.StateManager, from, to, trigger, transition);
            from.AddTransition(splashExploreTransition);
        }

        protected void TxtTrans(State from, State to, string text, float x, float y, Font font, Color colour, Rectangle clip, Window window, IHoverSelectorRenderer renderer, IWindowTransitionFactory transition) {
            Text txt = new StaticText(text, window.OverlayManager, font, colour, new PointF(x, y));
            ITrigger trigger = TxtTrigger(text, x, y, font, colour, clip, window);
            StateTransition splashExploreTransition = new StateTransition(window.Coordinator.StateManager, from, to, trigger, transition);
            from.AddTransition(splashExploreTransition);
        }

        protected void InitIdle(State idle, State home, IWindowTransitionFactory fadeInTransition, IWindowTransitionFactory fadeOutTransition, int timeout) {
            List<ITrigger> inactiveTriggers = new List<ITrigger>();
            List<ITrigger> activeTriggers = new List<ITrigger>();

            if (Config.InterfaceMode != StateManager.CLICK_MODE) {
                activeTriggers.Add(new SkeletonFoundTrigger());
                inactiveTriggers.Add(new SkeletonLostTrigger(Coordinator, timeout));
            }
            if (Coordinator.HasPlugin<XBoxControllerPlugin>()) {
                activeTriggers.Add(new JoystickActivatedTrigger(Coordinator));
                inactiveTriggers.Add(new JoystickInactiveTrigger(timeout, Coordinator));
            }

            foreach (var inactiveTrigger in inactiveTriggers) {
                foreach (var state in Coordinator.StateManager.States) {
                    if (state != idle) {
                        StateTransition trans = new StateTransition(Coordinator.StateManager, state, idle, inactiveTrigger, fadeOutTransition);
                        state.AddTransition(trans);
                    }
                }
            }

            foreach (var activeTrigger in activeTriggers) {
                StateTransition back = new StateTransition(Coordinator.StateManager, idle, home, activeTrigger, fadeInTransition);
                idle.AddTransition(back);
            }
        }

        protected ITrigger ImgTrigger(Window window, string image, float x, float y) {
            return ImgTrigger(window, image, x, y, -1f);
        }
        protected ITrigger ImgTrigger(Window window, string image, float x, float y, float w) {
            OverlayImage img = new OverlayImage(new Bitmap(Path.Combine(mButtonFolder, image + ".png")), x, y, w, window.Name);
            return mConfig.InterfaceMode == StateManager.CLICK_MODE ? 
                (ITrigger) new ImageClickTrigger(window.OverlayManager, img) :
                (ITrigger) new ImageHoverTrigger(window.OverlayManager, mRenderer, img);
        }

        protected ITrigger InvisTrigger(Point topLeft, Point bottomRight, Rectangle clip, Window window) {
            return mConfig.InterfaceMode == StateManager.CLICK_MODE ?
                (ITrigger) new ClickTrigger(window.OverlayManager, topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y, clip) :
                (ITrigger) new HoverTrigger(
                window.OverlayManager, 
                mRenderer, 
                topLeft.X, 
                topLeft.Y, 
                bottomRight.X - topLeft.X, 
                bottomRight.Y - topLeft.Y, 
                clip);
        }

        protected ITrigger TxtTrigger(string text, float x, float y, Font font, Color colour, Rectangle clip, Window window) {
            Text txt = new StaticText(text, window.OverlayManager, font, colour, new PointF(x, y));
            return mConfig.InterfaceMode == StateManager.CLICK_MODE ?
                (ITrigger) new TextClickTrigger(window.OverlayManager, txt, clip) :
                (ITrigger) new TextHoverTrigger(window.OverlayManager, mRenderer, txt, clip);
        }

        public static Launcher Create() {
            //Assembly ass = typeof(Launcher).Assembly;
            //return (Launcher) ass.CreateInstance(new LauncherConfig().Launcher);
            switch (new LauncherConfig().Launcher) {
                case "Chimera.Launcher.ExampleOverlayLauncher": return new ExampleOverlayLauncher();
                case "Chimera.Launcher.FlythroughLauncher": return new FlythroughLauncher();
                case "Chimera.Launcher.XmlOverlayLauncher": return new XmlOverlayLauncher();
            }

            return new MinimumLauncher();
        }

        public void Launch() {
            if (mConfig.GUI)
                ProcessWrangler.BlockingRunForm(Form, Coordinator);
            else {
                //Thread t = new Thread(() => {
                while (!Console.ReadLine().ToUpper().StartsWith("Q")) ;
                mCoordinator.Close();
                //});
                //t.Name = "Input Thread";
                //t.Start();
            }
        }
    }
}
