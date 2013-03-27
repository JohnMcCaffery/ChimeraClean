using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;

namespace Chimera.Launcher {
    public class SimpleOverlayConfig : ConfigBase {
        public bool EnableFlythrough;
        public bool EnableMenus;

        public override string Group {
            get { return "Simple Overlay"; }
        }

        protected override void InitConfig() {
            EnableFlythrough = Get(true, "MenuEnableFlythrough", true, "If true then when no user is present the system will revert to a flythrough.");
            EnableMenus = Get(true, "MenuEnableOverlay", true, "If true then menus will appear. Otherwise the overlay will go straight into kinect movement mode.");
        }
    }
}
