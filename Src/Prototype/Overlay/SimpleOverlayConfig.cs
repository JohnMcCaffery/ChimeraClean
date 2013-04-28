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
