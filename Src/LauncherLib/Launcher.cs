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
using Chimera.OpenSim;

namespace Chimera.Launcher {
    public class Launcher {
        private readonly Coordinator mCoordinator;
        private LauncherConfig mConfig;
        private CoordinatorForm mForm;
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
            if (mConfig.BindingsFile == null) {
                Console.WriteLine("Unable to launch. No bindings file specified.");
                return;
            }
            try {
                k.Load(mConfig.BindingsFile);
            } catch (Exception e) {
                Console.WriteLine("Unable to launch. Problem loading bindings. " + e.Message);
            }
            if (k.TryGet<IMediaPlayer>() == null)
                k.Bind<IMediaPlayer>().To<DummyPlayer>().InSingletonScope();

            try {
                mCoordinator = k.Get<Coordinator>();
            } catch (Exception e) {
                Console.WriteLine("Unable to launch. Problem instantiating coordinator. " + (e.InnerException != null ? e.InnerException.Message :e.Message));
            }
        }


        public static Launcher Create() {
            return new Launcher();
        }

        public void Launch() {
            if (mCoordinator == null)
                return;
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
