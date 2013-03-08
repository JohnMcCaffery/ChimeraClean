using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nini.Config;
using Chimera.Util;
using System.IO;

namespace Chimera {
    public class CoordinatorConfig {
        public string CrashLogFile;
        public bool AutoRestart;

        public CoordinatorConfig(params string[] args) {
            ArgvConfigSource argConfig = Init.InitArgConfig(args);
            argConfig.AddSwitch("General", "File", "f");

            string file;
            IConfigSource config = Init.AddFile(argConfig, out file);

            InitConfig(file, args);
        }

        public CoordinatorConfig(string file, string[] args) {
            InitConfig(file, args);
        }

        private void InitConfig(string file, string[] args) {
            ArgvConfigSource argConfig = Init.InitArgConfig(args);
            argConfig.AddSwitch("General", "AutoRestart", "r");
            argConfig.AddSwitch("General", "CrashLogFile", "l");
            
            IConfigSource config = Init.AddFile(argConfig, file);
            IConfig coordinatorConfig = config.Configs["Coordinator"];
            IConfig generalConfig = config.Configs["General"];

            CrashLogFile = Init.Get(generalConfig, "CrashLogFile", "CrashLog.log");
            AutoRestart = Init.Get(generalConfig, "AutoRestart", false);

            //EnableWindowPackets = Init.Get(generalConfig, "EnableWindowPackets", true);
            //UseSetFollowCamPackets = !enableWindowPackets || Get(generalConfig, "UseSetFollowCamPackets", false);
            //ControlCamera = Init.Get(sectionConfig, "ControlCamera", true);
        }
    }
}
