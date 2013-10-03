using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;
using Chimera.Config;
using System.IO;

namespace Chimera.Launcher {
    public class LauncherConfig : ConfigFolderBase {
        public bool GUI;
        public bool BasicGUI;

        public bool BackwardsCompatible;
        public String BindingsFile;

        public override string Group {
            get { return "Launch"; }
        }

        public LauncherConfig(params string[] args)
            : base("Launch", args) { }

        protected override void InitConfig() {
            BasicGUI = Get(true, "BasicGUI", true, "Whether to use a simple GUI. If not the system will either use the full GUI or no GUI depending on the GUI flag.");
            GUI = Get(true, "GUI", true, "Whether to launch the full GUI or no GUI if not using the basic GUI.");
            BindingsFile = Get(true, "BindingsFile", null, "The XML file describing the dependency injection bindings used to instantiate the system. Relative paths are specified relative to the folder the launch config file is in.");
            if (!Path.IsPathRooted(BindingsFile))
                BindingsFile = Path.Combine(Folder, BindingsFile);
        }
    }
}
