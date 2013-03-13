using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nini.Config;
using Chimera.Util;
using System.IO;

namespace Chimera {
    public class WindowConfig {
        public string Monitor;
        public string Name;
        public bool LaunchOverlay;
        public bool Fullscreen;
        public bool MouseControl;

        public WindowConfig(params string[] args) {
            ArgvConfigSource argConfig = Init.InitArgConfig(args);
            argConfig.AddSwitch("General", "File", "f");
            argConfig.AddSwitch("General", "Name", "n");

            string file;
            IConfigSource config = Init.AddFile(argConfig, out file);
            Name = Init.Get(config.Configs["General"], "Name", "MainWindow");

            InitConfig(file, args);
        }

        public WindowConfig(string name, string file, string[] args) {
            Name = name;
            InitConfig(file, args);
        }

        private void InitConfig(string file, string[] args) {
            ArgvConfigSource argConfig = Init.InitArgConfig(args);
            argConfig.AddSwitch(Name, "Monitor", "m");
            argConfig.AddSwitch(Name, "LaunchOverlay", "l");
            argConfig.AddSwitch(Name, "Fullscreen", "f");
            argConfig.AddSwitch(Name, "MouseControl", "mc");
            
            IConfigSource config = Init.AddFile(argConfig, file);
            IConfig specificConfig = config.Configs[Name];
            IConfig generalConfig = config.Configs["General"];

            Monitor = Init.Get(specificConfig, "Monitor", "CrashLog.log");
            LaunchOverlay = Init.Get(specificConfig, "LaunchOverlay", false);
            Fullscreen = Init.Get(specificConfig, "Fullscreen", false);
            MouseControl = Init.Get(specificConfig, "MouseControl", false);
        }
    }
}
