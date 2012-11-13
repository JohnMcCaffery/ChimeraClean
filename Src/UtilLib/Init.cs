using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nini.Config;

namespace UtilLib {
    public static class Init {
        public class Config {
            public static readonly string DEFAULT_LOGINURI = "http://localhost:9000";
            public static readonly string DEFAULT_CLIENT_EXE = "C:\\Program Files (x86)\\Firestorm-Release\\Firestorm-Release.exe";
            public static readonly int DEFAULT_MASTER_PORT = 8090;
            public static readonly int DEFAULT_PROXY_PORT = 8080;
            public static int CURRENT_PORT = DEFAULT_PROXY_PORT;

            //Client
            private string clientExe = null;
            private string firstName = null;
            private string lastName = null;
            private string password = null;
            private string grid = null;

            //Proxy
            private string loginURI = DEFAULT_LOGINURI;
            private int proxyPort;

            //Master
            private int masterPort = DEFAULT_MASTER_PORT;

            /// <summary>
            /// True if the client should automatically log in whilst starting.
            /// </summary>
            public bool AutoLoginClient {
                get { return firstName != null && lastName != null && password != null; }
            }

            /// <summary>
            /// True if a '--grid' argument rather than a '--loginuri' argument should be specified when launching the client.
            /// </summary>
            public bool UseGrid {
                get { return grid != null; }
            }

            /// <summary>
            /// Connection string for the server the proxy is proxying.
            /// </summary>
            public string ProxyLoginURI {
                get { return loginURI; }
                set { loginURI = value; }
            }

            /// <summary>
            /// Port that clients should use to connect to the proxy.
            /// </summary>
            public int ProxyPort {
                get { return proxyPort; }
                set { proxyPort = value; }
            }

            /// <summary>
            /// The port the master server is running on.
            /// </summary>
            public int MasterPort {
                get { return masterPort; }
                set { masterPort = value; }
            }

            /// <summary>
            /// The executable that will run the client.
            /// </summary>
            public string ClientExecutable {
                get { return clientExe; }
                set { clientExe = value; }
            }

            /// <summary>
            /// The first name to login with.
            /// </summary>
            public string LoginFirstName {
                get { return firstName; }
                set { firstName = value; }
            }

            /// <summary>
            /// The last name to login with.
            /// </summary>
            public string LoginLastName {
                get { return lastName; }
                set { lastName = value; }
            }

            /// <summary>
            /// The password to login with.
            /// </summary>
            public string LoginPassword {
                get { return password; }
                set { password = value; }
            }

            /// <summary>
            /// The name of the grid the client should connect to.
            /// </summary>
            public string LoginGrid {
                get { return grid; }
                set { grid = value; }
            }

            public Config() {
                proxyPort = CURRENT_PORT++;
            }

            public Config(string[] args, string section, string file) {
                IConfigSource config = new DotNetConfigSource(file);
                ArgvConfigSource argConfig = new ArgvConfigSource(args);
                argConfig.AddSwitch(section, "ProxyPort", "pp");
                argConfig.AddSwitch(section, "ClientExe", "e");
                argConfig.AddSwitch(section, "FirstName", "f");
                argConfig.AddSwitch(section, "LastName", "l");
                argConfig.AddSwitch(section, "Password", "pw");
                argConfig.AddSwitch(section, "UseGrid", "ug");
                argConfig.AddSwitch(section, "ProxyGrid", "g");
                argConfig.AddSwitch("General", "LoginURI", "s");

                IConfig mainConfig = config.Configs[section];
                config.Merge(argConfig);

                clientExe = mainConfig.Get("ClientExe", DEFAULT_CLIENT_EXE);
                firstName = mainConfig.Get("FirstName", null);
                lastName = mainConfig.Get("LastName", null);
                password = mainConfig.Get("Password", null);
                proxyPort = mainConfig.GetInt("ProxyPort", CURRENT_PORT++);
                loginURI = config.Configs["General"].Get("LoginURI", DEFAULT_LOGINURI);
                masterPort = config.Configs["General"].GetInt("MasterPort", DEFAULT_MASTER_PORT);

                if (config.Configs["General"].GetBoolean("UseGrid", true))
                    LoginGrid = mainConfig.Get("ProxyGrid", proxyPort.ToString());
            }
        }

        public static CameraMaster InitCameraMaster(string[] args, string configFile) {
            IConfigSource config = new DotNetConfigSource(configFile);
            ArgvConfigSource argConfig = new ArgvConfigSource(args);

            argConfig.AddSwitch("Master", "AutoStartMaster", "am");
            argConfig.AddSwitch("Master", "AutoStartProxy", "ap");
            argConfig.AddSwitch("Master", "AutoStartClient", "ac");

            config.Merge(argConfig);
            IConfig masterConfig = config.Configs["Master"];
            IConfig generalConfig = config.Configs["General"];

            Config proxyConfig = new Config(args, "Master", AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            CameraMaster m = new CameraMaster(proxyConfig);

            bool autostartProxy = generalConfig.GetBoolean("AutoStartProxy", false);
            bool autostartClient = generalConfig.GetBoolean("AutoStartClient", false);
            bool autostartMaster = generalConfig.GetBoolean("AutoStartMaster", false);
            if (autostartProxy || autostartClient)
                while (!m.StartProxy())
                    proxyConfig.ProxyPort++;
            if (autostartClient)
                m.StartClient();
            if (autostartMaster)
                m.StartMaster();
            return m;
        }

        public static CameraSlave InitCameraSlave(string[] args, string configFile, string slave) {
            IConfigSource config = new DotNetConfigSource(configFile);
            ArgvConfigSource argConfig = new ArgvConfigSource(args);

            argConfig.AddSwitch(slave, "AutoConnectSlave", "as");
            argConfig.AddSwitch(slave, "AutoStartProxy", "ap");
            argConfig.AddSwitch(slave, "AutoStartClient", "ac");

            config.Merge(argConfig);
            IConfig slaveConfig = config.Configs[slave];
            IConfig generalConfig = config.Configs["General"];

            Config proxyConfig = new Config(args, slave, AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            CameraSlave s = new CameraSlave(slave, proxyConfig);

            bool autostartProxy = generalConfig.GetBoolean("AutostartProxy", false);
            bool autostartClient = generalConfig.GetBoolean("AutoStartClient", false);
            bool autostartSlave = generalConfig.GetBoolean("AutoConnectSlave", false);

            if (autostartProxy || autostartClient)
                while (!s.StartProxy())
                    proxyConfig.ProxyPort++;
            if (autostartClient)
                s.StartClient();
            if (autostartSlave)
                s.Connect();
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
