using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nini.Config;
using Chimera.Util;
using System.IO;

namespace Chimera.OpenSim {
    internal class ProxyConfig {
        public static readonly string DEFAULT_LOGINURI = "http://localhost:9000";
        public static readonly string DEFAULT_CLIENT_EXE = "C:\\Program Files (x86)\\Firestorm-Release\\Firestorm-Release.exe";
        public static readonly string DEFAULT_MASTER_ADDRESS = "127.0.0.1";
        public static readonly int DEFAULT_MASTER_PORT = 8090;
        public static readonly int DEFAULT_PROXY_PORT = 8080;
        public static int CURRENT_PORT = DEFAULT_PROXY_PORT;

        public string ProxyLoginURI;
        public string Name;
        public string ViewerExecutable;
        public string LoginFirstName;
        public string LoginLastName;
        public string LoginPassword;
        public string LoginGrid;
        public string ViewerWorkingDirectory;
        public string CrashLogFile;
        public bool UseGrid;
        public bool AutoLoginClient;
        public bool AutoStartProxy;
        public bool AutoStartViewer;
        public bool AutoRestartViewer;
        public bool ControlCamera;
        public bool Border;
        public int ProxyPort;

        public ProxyConfig(params string[] args) {
            ArgvConfigSource argConfig = Init.InitArgConfig(args);
            argConfig.AddSwitch("General", "Name", "n");
            argConfig.AddSwitch("General", "File", "f");

            string file;
            IConfigSource config = Init.AddFile(argConfig, out file);
            Name = Init.Get(config.Configs["General"], "Name", "MainWindow");

            InitConfig(file, args);
        }

        public ProxyConfig(string name, string file, string[] args) {
            Name = name;
            InitConfig(file, args);
        }

        private void InitConfig(string file, string[] args) {
            ArgvConfigSource argConfig = Init.InitArgConfig(args);
            argConfig.AddSwitch("General", "ViewerExe", "v");
            argConfig.AddSwitch("General", "WorkingDirectory", "d");
            argConfig.AddSwitch("General", "UseGrid", "ug");
            argConfig.AddSwitch("General", "UseSetFollowCamPackets", "uf");
            argConfig.AddSwitch("General", "EnableWindowPackets", "ew");
            argConfig.AddSwitch("General", "ProxyGrid", "g");
            argConfig.AddSwitch("General", "LoginURI", "u");
            argConfig.AddSwitch("General", "MasterAddress", "ma");
            argConfig.AddSwitch("General", "MasterPort", "mp");
            argConfig.AddSwitch("General", "WorldPosition", "cw");
            argConfig.AddSwitch("General", "WorldPitch", "pw");
            argConfig.AddSwitch("General", "WorldYaw", "yw");
            argConfig.AddSwitch("General", "AutoRestart", "r");
            argConfig.AddSwitch("General", "CrashLogFile", "l");
            argConfig.AddSwitch(Name, "ControlCamera", "c");
            argConfig.AddSwitch(Name, "AutoStartProxy", "ap");
            argConfig.AddSwitch(Name, "AutoStartViewer", "av");
            argConfig.AddSwitch(Name, "ProxyPort", "p");
            argConfig.AddSwitch(Name, "FirstName", "fn");
            argConfig.AddSwitch(Name, "LastName", "l");
            argConfig.AddSwitch(Name, "Password", "pw");
            argConfig.AddSwitch(Name, "Border", "pw");
            
            IConfigSource config = Init.AddFile(argConfig, file);
            IConfig sectionConfig = config.Configs[Name];
            IConfig generalConfig = config.Configs["General"];

            ViewerExecutable = Init.Get(generalConfig, "ViewerExe", DEFAULT_CLIENT_EXE);
            ViewerWorkingDirectory = Init.Get(generalConfig, "WorkingDirectory", Path.GetDirectoryName(ViewerExecutable));
            ProxyLoginURI = Init.Get(generalConfig, "LoginURI", DEFAULT_LOGINURI);
            UseGrid = Init.Get(generalConfig, "UseGrid", false);

            CrashLogFile = Init.Get(generalConfig, "CrashLogFile", "CrashLog.log");

            LoginFirstName = Init.Get(sectionConfig, "FirstName", null);
            LoginLastName = Init.Get(sectionConfig, "LastName", null);
            LoginPassword = Init.Get(sectionConfig, "Password", null);
            ProxyPort = Init.Get(sectionConfig, "ProxyPort", CURRENT_PORT++);
            LoginGrid = Init.Get(sectionConfig, "ProxyGrid", ProxyPort.ToString());
            AutoLoginClient = LoginFirstName != null && LoginLastName != null && LoginPassword != null;

            AutoStartProxy = Init.Get(sectionConfig, "AutoStartProxy", false);
            AutoStartViewer = Init.Get(sectionConfig, "AutoStartViewer", false);
            AutoRestartViewer = Init.Get(generalConfig, "AutoRestart", false);
            ControlCamera = Init.Get(sectionConfig, "ControlCamera", true);
            Border = Init.Get(sectionConfig, "Border", false);

            //EnableWindowPackets = Init.Get(generalConfig, "EnableWindowPackets", true);
            //UseSetFollowCamPackets = !enableWindowPackets || Get(generalConfig, "UseSetFollowCamPackets", false);
            //ControlCamera = Init.Get(sectionConfig, "ControlCamera", true);
        }
    }
}
