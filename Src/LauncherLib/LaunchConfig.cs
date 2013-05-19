using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;
using Chimera.Config;

namespace Chimera.Launcher {
    public class LauncherConfig : ConfigFolderBase {
        public bool GUI;

        public bool BackwardsCompatible;
        public String BindingsFile;

        public override string Group {
            get { return "Launch"; }
        }

        public LauncherConfig(params string[] args)
            : base("Launch", args) { }

        protected override void InitConfig() {
            GUI = Get(true, "GUI", true, "Whether to launch the GUI when the system starts.");
            BindingsFile = Get(true, "BindingsFile", "../Config/FullSetup.xml", "The XML file describing the dependency injection bindings used to instantiate the system.");
        }
    }
}
