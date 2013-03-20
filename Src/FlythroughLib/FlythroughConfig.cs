using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;

namespace Chimera.Flythrough {
    class FlythroughConfig : ConfigBase {
        public bool Enabled;

        public override string Group {
            get { return "Flythrough"; }
        }

        protected override void InitConfig() {
            Enabled = Get(true, "Enabled", true, "Whether to allow the flythrough manager to control the camera at the start.");
        }
    }
}
