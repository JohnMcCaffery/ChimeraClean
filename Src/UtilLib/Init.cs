using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nini.Config;

namespace UtilLib {
    public static class Init {
        public static CameraMaster InitCameraMaster(string[] args, string configFile) {
            IConfigSource config = new DotNetConfigSource(configFile);
            ArgvConfigSource argConfig = new ArgvConfigSource(args);
            argConfig.AddSwitch("General", "MasterPort", "p");
            argConfig.AddSwitch("General", "LoginURI", "s");
            argConfig.AddSwitch("Master", "AutostartMaster", "am");
            argConfig.AddSwitch("Master", "AutostartProxy", "ap");
            argConfig.AddSwitch("Master", "AutostartClient", "ac");
            argConfig.AddSwitch("Master", "ProxyPort", "pp");
            argConfig.AddSwitch("Master", "ClientExe", "e");
            argConfig.AddSwitch("Master", "FirstName", "f");
            argConfig.AddSwitch("Master", "LastName", "l");
            argConfig.AddSwitch("Master", "Password", "pw");
            argConfig.AddSwitch("Master", "UseGrid", "ug");
            argConfig.AddSwitch("Master", "ProxyGrid", "g");

            config.Merge(argConfig);
            IConfig masterConfig = config.Configs["Master"];
            IConfig appConfig = config.Configs["General"];

            int masterPort = appConfig.GetInt("MasterPort", 8090);
            string loginURI = appConfig.Get("LoginURI", "http://diana.cs.st-andrews.ac.uk:8002");

            bool autostartProxy = masterConfig.GetBoolean("AutostartProxy", false);
            bool autostartClient = masterConfig.GetBoolean("AutostartClient", false);
            bool autostartMaster = masterConfig.GetBoolean("AutostartMaster", false);

            int proxyPort = masterConfig.GetInt("ProxyPort", 8080);

            string clientExe = masterConfig.Get("ClientExe", "C:\\Program Files (x86)\\Firestorm-Release\\Firestorm-Release.exe");
            string clientFirstName = masterConfig.Get("FirstName", "Routing");
            string clientLastName = masterConfig.Get("LastName", "God");
            string clientPassword = masterConfig.Get("Password", "1245");
            bool useGrid = masterConfig.GetBoolean("UseGrid", true);
            string proxyGrid = masterConfig.Get("ProxyGrid", proxyPort.ToString());

            CameraMaster m = new CameraMaster();

            m.ProxyPort = proxyPort;
            m.ProxyLoginURI = loginURI;
            m.MasterPort = masterPort;
            m.ClientExecutable = clientExe;
            m.FirstName = clientFirstName;
            m.LastName = clientLastName;
            m.Password = clientPassword;
            if (useGrid)
                m.Grid = proxyGrid;

            if (autostartProxy || autostartClient) {
                while (!m.StartProxy())
                    m.ProxyPort++;
            }
            if (autostartClient) {
                m.StartClient();
            }
            if (autostartMaster) {
                m.StartMaster();
            }
            return m;
        }

        public static CameraSlave InitCameraSlave(string args) {
            bool autostartProxy;
            bool autoConnect;
            bool autostartClient;
            string clientExe;
            string clientFirstName;
            string clientLastName;
            string clientPassword;
            string name;
            int masterPort;
            string masterAddress;
            string configFile;
            CameraSlave s = new CameraSlave();
            return s;
        }



    private class CommandLineConfig : ConfigSourceBase, IConfigSource {
        private List<string> _args = new List<string>();

        public CommandLineConfig(string argstring, bool includesProgram, params string[] splitCharacters) {
            int i = 0;
            foreach (var arg in argstring.Split(new string[] { "\"" }, StringSplitOptions.RemoveEmptyEntries)) {
                if (i % 2 == 0)
                    _args.AddRange(arg.Split(splitCharacters, StringSplitOptions.RemoveEmptyEntries));
                else
                    _args.Add(arg);
                i++;
            }

            if (includesProgram)
                _args.Remove(_args[0]);
        }

        #region IConfigSource Members

        public void AddSetting(string configName, string longName, string shortName) {
            IConfig config = GetConfig(configName);
            for (int i = 0; i < _args.Count - 1; i++) {
                string arg = _args[i];
                if (
                    ((arg.StartsWith("-") || arg.StartsWith("/")) && arg.Substring(1).Equals(shortName)) ||
                    (arg.StartsWith("--") && arg.Substring(2).Equals(longName))) {
                    config.Set(longName, _args[i + 1]);
                    _args.RemoveRange(i, 2);
                    break;
                }
            }
        }

        public void AddFlag(string configName, string longName, string shortName, bool value) {
            IConfig config = GetConfig(configName);
            for (int i = 0; i < _args.Count; i++) {
                string arg = _args[i];
                if ((arg.StartsWith("-") || arg.StartsWith("/")) && arg.Substring(1).Equals(shortName)) {
                    config.Set(longName, value);
                    _args.RemoveRange(i, 1);
                    break;
                }
            }
        }

        public string Argument {
            get {
                return _args.Count > 0 ? _args.Aggregate((sum, current) => sum + " " + current).Trim() : "";
            }
        }

        public IConfig GetConfig(string configName) {
            IConfig config = Configs[configName];
            if (config == null) {
                AddConfig(configName);
                config = Configs[configName];
            }
            return config;
        }

        #endregion
    }
    }
}
