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
using OpenMetaverse;
using Chimera.Config;

namespace Chimera.OpenSim {
    public class HeightmapConfig : ConfigFolderBase {
        public string FirstName;
        public string LastName;
        public string Password;
        public Vector3 StartLocation;
        public string StartIsland;
        public bool AutoLogin;
        public bool AutoLogout;

        public override string Group {
            get { return "OpenSimHeightmapBot"; }
        }

        protected override void InitConfig() {
            FirstName = Get("FirstName", "Heightmap", "The first name for the heightmap listening bot to login with.");
            LastName = Get("LastName", "Bot", "The last name for the heightmap listening bot to login with.");
            Password = Get("Password", "botPassword", "The password for the heightmap listening bot to login with.");
            AutoLogin = Get("AutoLogin", true, "Whether the bot should automatically login when the system starts up.");
            AutoLogout = Get("AutoLogout", true, "Whether the bot should automatically logout when the system starts up.");
            StartLocation = GetV("StartLocation", new Vector3(128f, 128f, 60f), "Where on the island the bot should login to.");
            StartIsland = Get("StartIsland", "Cathedral 1", "What island the bot should login to.");
        }

        public HeightmapConfig(params string[] args)
            : base("Heightmap", args) {
        }
    }
}
