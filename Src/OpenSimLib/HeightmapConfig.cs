using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;
using OpenMetaverse;

namespace Chimera.OpenSim {
    public class HeightmapConfig : ConfigBase {
        public string FirstName;
        public string LastName;
        public string Password;
        public string LoginURI;
        public Vector3 StartLocation;
        public string StartIsland;
        public bool AutoLogin;
        public bool AutoLogout;
        public bool Enabled;

        public override string Group {
            get { return "OpenSim Heightmap Listener"; }
        }

        protected override void InitConfig() {
            FirstName = Get(true, "HeightmapBotFirstName", "Heightmap", "The first name for the heightmap listening bot to login with.");
            LastName = Get(true, "HeightmapBotLastName", "Bot", "The last name for the heightmap listening bot to login with.");
            Password = Get(true, "HeightmapBotPassword", "botPassword", "The password for the heightmap listening bot to login with.");
            AutoLogin = Get(true, "HeightmapBotAutoLogin", true, "Whether the bot should automatically login when the system starts up.");
            AutoLogout = Get(true, "HeightmapBotAutoLogout", true, "Whether the bot should automatically logout when the system starts up.");
            LoginURI = Get(true, "LoginURI", "http://localhost:9000", "The URI of the server the bot should login to.");
            StartLocation = GetV(true, "HeightmapBotStartLocation", new Vector3(128f, 128f, 60f), "Where on the island the bot should login to.");
            StartIsland = Get(true, "HeightmapBotStartIsland", "Cathedral 1", "What island the bot should login to.");
            Enabled = Get(true, "HeightmapBotEnabled", true, "Whether the bot is enabled to start with. If the bot is not enabled it won't log in.");
        }
    }
}
