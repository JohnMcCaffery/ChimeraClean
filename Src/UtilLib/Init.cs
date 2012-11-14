using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nini.Config;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace UtilLib {
    /// <summary>
    /// Static class for initialising values.
    /// 
    /// The following flags are used (section is Master or the name of the Slave):
    /// General UseGrid ug
    /// General ProxyGrid g
    /// General LoginURI u
    /// General MasterPort mp
    /// General MasterAddress ma
    /// General ClientExe e
    /// 
    /// section AutoStartMaster am
    /// section AutoConnectSlave as
    /// section AutoStartProxy ap
    /// section AutoStartClient ac
    /// section ProxyPort pp
    /// section FirstName f
    /// section LastName l
    /// section Password pw
    /// </summary>
    public static class Init {
        public static readonly int SECTION_WIDTH = 8;
        public static readonly int LONG_WIDTH = 20;
        public static readonly int SHORT_WIDTH = 5;
        public static readonly int DEFAULT_WIDTH = 25;
        public static readonly int EXPLANATION_WIDTH= 100;

        public static string MakeHelpLine(object section, object lng, object shrt, object explanation, object deflt) {
            string sectionCol = "{0,-" + SECTION_WIDTH + "} ";
            string longCol = "{1,-" + LONG_WIDTH + "} ";
            string shortCol = "{2,-" + SHORT_WIDTH + "} ";
            string defaultCol = "{3,-" + DEFAULT_WIDTH + "} ";
            string explanationCol = "{4,-" + EXPLANATION_WIDTH + "} ";
            string format = longCol + explanationCol + sectionCol + shortCol + defaultCol;
            return String.Format(format, section, lng, shrt, deflt, explanation);
        }

        public static string HelpHeaders {
            get { return MakeHelpLine("Section", "Long", "Short", "Explanation", "Default"); }
        }

        public static string Help(string section) {
            string ret = "";
            ret += MakeHelpLine("General", "UseGrid", "ug", "Whether to use the '--grid' flag when launching the client", "true") + "\n";
            ret += MakeHelpLine("General", "ProxyGrid", "g", "The name of the grid to specify using '--grid' when launching the client.", "The proxy port") + "\n";
            ret += MakeHelpLine("General", "LoginURI", "u", "The LoginURI of the actual servr being proxied", Config.DEFAULT_LOGINURI) + "\n";
            ret += MakeHelpLine("General", "MasterPort", "mp", "The port the master is running/will be run on for distributing packets between master/slaves", Config.DEFAULT_MASTER_PORT) + "\n";
            ret += MakeHelpLine("General", "MasterAddress", "ma", "The address the master is running at for distributing packets between master/slaves", "localhost") + "\n";
            ret += MakeHelpLine("General", "ClientExe", "e", "The file to be executed to launch the client", Path.GetFileName(Config.DEFAULT_CLIENT_EXE)) + "\n";
            ret += MakeHelpLine(section, "AutoStartMaster", "am", "Whether to automatically start the master running", "true") + "\n";
            ret += MakeHelpLine(section, "AutoConnectSlave", "as", "Whether to automatically connect the slave to the master", "true") + "\n";
            ret += MakeHelpLine(section, "AutoStartProxy", "ap", "Whether to autostart a proxy", "true") + "\n";
            ret += MakeHelpLine(section, "AutoStartClient", "ac", "Whether to auto launch the client executable to connect to the proxy", "false") + "\n";
            ret += MakeHelpLine(section, "ProxyPort", "p", "The port the proxy is to run on", Config.DEFAULT_PROXY_PORT) + "\n";
            ret += MakeHelpLine(section, "FirstName", "f", "The first name to log in with", "Not Set") + "\n";
            ret += MakeHelpLine(section, "LastName", "l", "The last name to log in with", "Not Set") + "\n";
            ret += MakeHelpLine(section, "Password", "pw", "The password to log in with", "Not Set") + "\n";
            return ret;
        }
        public class Config {
            public static readonly string DEFAULT_LOGINURI = "http://localhost:9000";
            public static readonly string DEFAULT_CLIENT_EXE = "C:\\Program Files (x86)\\Firestorm-Release\\Firestorm-Release.exe";
            public static readonly string DEFAULT_MASTER_ADDRESS = "127.0.0.1";
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
            private string masterAddress = DEFAULT_MASTER_ADDRESS;

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
            /// The address of the master server.
            /// </summary>
            public string MasterAddress {
                get { return masterAddress; }
                set { masterAddress = value; }
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
                ArgvConfigSource config = new ArgvConfigSource(args);
                config.AddSwitch("General", "ClientExe", "e");
                config.AddSwitch("General", "UseGrid", "ug");
                config.AddSwitch("General", "ProxyGrid", "g");
                config.AddSwitch("General", "LoginURI", "u");
                config.AddSwitch("General", "MasterAddress", "ma");
                config.AddSwitch("General", "MasterPort", "mp");
                config.AddSwitch(section, "ProxyPort", "p");
                config.AddSwitch(section, "FirstName", "f");
                config.AddSwitch(section, "LastName", "l");
                config.AddSwitch(section, "Password", "pw");

                if (File.Exists(file)) {
                    IConfigSource dotnet = new DotNetConfigSource(file);
                    config.Merge(dotnet);
                }
                IConfig mainConfig = config.Configs[section];
                IConfig generalConfig = config.Configs["General"];

                clientExe = Get(generalConfig, "ClientExe", DEFAULT_CLIENT_EXE);
                loginURI = Get(generalConfig, "LoginURI", DEFAULT_LOGINURI);
                masterPort = Get(generalConfig, "MasterPort", DEFAULT_MASTER_PORT);
                masterAddress = Get(generalConfig, "MasterAddress", DEFAULT_MASTER_ADDRESS);

                firstName = Get(mainConfig, "FirstName", null);
                lastName = Get(mainConfig, "LastName", null);
                password = Get(mainConfig, "Password", null);
                proxyPort = Get(mainConfig, "ProxyPort", CURRENT_PORT++);

                if (Get(generalConfig, "UseGrid", true))
                    LoginGrid = Get(generalConfig, "ProxyGrid", proxyPort.ToString());
            }
        }

        public static CameraMaster InitCameraMaster(string[] args, string configFile) {
            ArgvConfigSource config = new ArgvConfigSource(args);

            config.AddSwitch("Master", "AutoStartMaster", "am");
            config.AddSwitch("Master", "AutoStartProxy", "ap");
            config.AddSwitch("Master", "AutoStartClient", "ac");

            if (File.Exists(configFile)) {
                IConfigSource dotnet = new DotNetConfigSource(configFile);
                config.Merge(dotnet);
            }

            IConfig masterConfig = config.Configs["Master"];
            IConfig generalConfig = config.Configs["General"];

            Config proxyConfig = new Config(args, "Master", AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            CameraMaster m = new CameraMaster(proxyConfig);

            bool autostartProxy = Get(generalConfig, "AutoStartProxy", false);
            bool autostartClient = Get(generalConfig, "AutoStartClient", false);
            bool autostartMaster = Get(generalConfig, "AutoStartMaster", true);
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
            ArgvConfigSource config = new ArgvConfigSource(args);

            config.AddSwitch(slave, "AutoConnectSlave", "as");
            config.AddSwitch(slave, "AutoStartProxy", "ap");
            config.AddSwitch(slave, "AutoStartClient", "ac");

            if (File.Exists(configFile)) {
                IConfigSource dotnet = new DotNetConfigSource(configFile);
                config.Merge(dotnet);
            }
            
            IConfig slaveConfig = config.Configs[slave];
            IConfig generalConfig = config.Configs["General"];

            Config proxyConfig = new Config(args, slave, AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            CameraSlave s = new CameraSlave(slave, proxyConfig);

            bool autostartProxy = Get(generalConfig, "AutoStartProxy", false);
            bool autostartClient = Get(generalConfig, "AutoStartClient", false);
            bool autostartSlave = Get(generalConfig, "AutoConnectSlave", true);

            if (autostartProxy || autostartClient)
                while (!s.StartProxy())
                    proxyConfig.ProxyPort++;
            if (autostartClient)
                s.StartClient();
            if (autostartSlave)
                s.Connect();
            return s;
        }

        public static bool Get(IConfig cfg, string key, bool defalt) {
            return cfg == null ? defalt : cfg.GetBoolean(key, defalt);
        }
        public static string Get(IConfig cfg, string key, string defalt) {
            return cfg == null ? defalt : cfg.Get(key, defalt);
        }
        public static int Get(IConfig cfg, string key, int defalt) {
            return cfg == null ? defalt : cfg.GetInt(key, defalt);
        }
        public static void StartGui(Func<Form> createForm) {
                Thread t = new Thread(() => {
                    Form f = createForm();
                    f.ShowDialog();
                });
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
                //Application.EnableVisualStyles();
                //Application.Run(new SlaveForm(s));
        }
    }
}
