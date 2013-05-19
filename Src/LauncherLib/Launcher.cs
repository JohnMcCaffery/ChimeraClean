﻿/*************************************************************************
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
using Ninject;
using Ninject.Parameters;
using Ninject.Extensions.Xml;
using Chimera.Interfaces;

namespace Chimera.Launcher {
    public class Launcher {
        private readonly List<Window> mWindows = new List<Window>();
        private readonly Coordinator mCoordinator;
        private LauncherConfig mConfig;
        private CoordinatorForm mForm;
        private IHoverSelectorRenderer mRenderer;
        private string mButtonFolder = "../Images/Examples/";
        protected IOutput mFirstWindowOutput;

        protected IOutput MakeOutput(string name) {
            return new OpenSimController();
        }

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

            var settings = new NinjectSettings { LoadExtensions = false };
            IKernel k = new StandardKernel(settings, new XmlExtensionModule());
            k.Load(mConfig.BindingsFile);

            mCoordinator = k.Get<Coordinator>();
            IOutputFactory outputFactory = k.Get<IOutputFactory>();

            foreach (string windowName in mConfig.Windows.Split(',')) {
                Coordinator.AddWindow(new Window(windowName, outputFactory.Create()));
            }

            mButtonFolder = Path.GetFullPath(mConfig.ButtonFolder);

            if (mConfig.InterfaceMode != StateManager.CLICK_MODE)
                mRenderer = new DialCursorRenderer();
        }


        public static Launcher Create() {
            return new Launcher();
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
