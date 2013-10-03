using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Config;

namespace Chimera.Experimental {
    public class ExperimentalConfig : ConfigFolderBase {
        public string ExperimentFile;

        public ExperimentalConfig()
            : base("Experiments") { }

        public override string Group {
            get { return "Experiments"; }
        }

        protected override void InitConfig() {
            ExperimentFile = Get(true, "File", null, "The xml file which defines the experiment.");
        }
    }
}
