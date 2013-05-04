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
using Nini.Config;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using log4net;
using OpenMetaverse;

namespace Chimera.Util {
    /// <summary>
    /// Static class for initialising values.
    /// 
    /// The following flags are used (section is Master or the name of the Slave):
    /// General UseGrid ug
    /// General ProxyGrid g
    /// General LoginURI u
    /// General MasterPort mp
    /// General MasterAddress ma
    /// General ViewerExe v
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

        public static string[] Help(string section) {
            return new string[] {
                MakeHelpLine("General", "ViewerExe", "e", "The file to be executed to launch the client.", Path.GetFileName(Config.DEFAULT_CLIENT_EXE)),
                MakeHelpLine("General", "LoginURI", "u", "The LoginURI of the actual server being proxied.", Config.DEFAULT_LOGINURI),
                MakeHelpLine("General", "MasterAddress", "ma", "The address the master is running at for distributing packets between master/slaves.", "localhost"),
                MakeHelpLine("General", "MasterPort", "mp", "The port the master is running/will be run on for distributing packets between master/slaves.", Config.DEFAULT_MASTER_PORT),
                MakeHelpLine("General", "UseGrid", "ug", "Whether to use the '--grid' flag when launching the client. Will only work with OpenSim enabled viewers.", "false"),
                MakeHelpLine("General", "UseSetFollowCamPackets", "uf", "Whether to use SetFollowCamProperties packets to control the client or SetWindowPackets. Set false to allow the view frame to be dictated by the size and shape of the input.", "false"),
                MakeHelpLine("General", "EnableWindowPackets", "ew", "Whether to allow the proxy to inject Window packets. Set false for backward compatibility with unmodified viewers.", "true"),
                MakeHelpLine("General", "WorldPosition", "cw", "The position the camera should start at in the world.", "true"),
                MakeHelpLine("General", "WorldPitch", "pw", "The pitch the camera should start at.", "true"),
                MakeHelpLine("General", "WorldYaw", "yw", "The yaw the camera should start at.", "true"),
                MakeHelpLine(section, "AutoConnectSlave", "as", "Whether to automatically connect the slave to the master.", "true"),
                MakeHelpLine(section, "AutoStartClient", "ac", "Whether to auto launch the client executable to connect to the proxy.", "false"),
                MakeHelpLine(section, "AutoStartMaster", "am", "Whether to automatically start the master running.", "true"),
                MakeHelpLine(section, "AutoStartProxy", "ap", "Whether to autostart a proxy.", "true"),
                MakeHelpLine(section, "FirstName", "fn", "The first name to log in with.", "Not Set"),
                MakeHelpLine(section, "LastName", "l", "The last name to log in with.", "Not Set"),
                MakeHelpLine(section, "Password", "pw", "The password to log in with.", "Not Set"),
                MakeHelpLine(section, "ControlCamera", "c", "True if the proxy is allowed to control the camera of its client.", true),
                MakeHelpLine(section, "ProxyGrid", "g", "The name of the grid to specify using '--grid' when launching the client.", "The proxy port"),
                MakeHelpLine(section, "ProxyPort", "p", "The port the proxy is to run on.", Config.DEFAULT_PROXY_PORT),
                MakeHelpLine(section, "GUI", "g", "Whether to launch a GUI.", true),
                MakeHelpLine(section, "Help", "h", "Display this help.", "Not Set"),
                MakeHelpLine(section, "File", "f", "The config file to use.", "AppDomain ConfigFile"),
            };
        }
        public class Config {
            public static readonly string DEFAULT_LOGINURI = "http://localhost:9000";
            public static readonly string DEFAULT_CLIENT_EXE = "C:\\Program Files (x86)\\Firestorm-Release\\Firestorm-Release.exe";
            public static readonly string DEFAULT_MASTER_ADDRESS = "127.0.0.1";
            public static readonly int DEFAULT_MASTER_PORT = 8090;
            public static readonly int DEFAULT_PROXY_PORT = 8080;
            public static int CURRENT_PORT = DEFAULT_PROXY_PORT;

            //Client
            private string viewerExe = null;
            private string workingDir = null;
            private string firstName = null;
            private string lastName = null;
            private string password = null;
            private string grid = null;
            private bool useGrid = false;
            private bool useSetFollowCamPackets = false;
            private bool enableWindowPackets = false;

            //Proxy
            private string loginURI = DEFAULT_LOGINURI;
            private int proxyPort;

            //Master
            private int masterPort = DEFAULT_MASTER_PORT;
            private string masterAddress = DEFAULT_MASTER_ADDRESS;
            private Vector3 worldPosition;
            private float worldPitch;
            private float worldYaw;

            //Slave
            private bool controlCamera = true;

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
                get { return useGrid && grid != null; }
                set { useGrid = value; }
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
            /// The executable that will run the viewer.
            /// </summary>
            public string ViewerExecutable {
                get { return viewerExe; }
                set { viewerExe = value; }
            }

            public string ViewerWorkingDirectory {
                get { return workingDir; }
                set { workingDir = value; }
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

            /// <summary>
            /// Whether the proxy should control the camera of the client which is connected to it.
            /// </summary>
            public bool ControlCamera {
                get { return controlCamera; }
                set { controlCamera = value; }
            }

            /// <summary>
            /// Set this to false to stop the proxy transition ever injecting Window packets into the client.
            /// If false this will mean that using the proxy will not break unmodified viewers.
            /// </summary>
            public bool EnableWindowPackets {
                get { return enableWindowPackets; }
                set { enableWindowPackets = value; }
            }

            /// <summary>
            /// Set true to have the proxy control the viewer via SetFollowCamProperties packets rather than  Window packets.
            /// Using SetFollowCamProperties packets leaves control of the frustum to the viewer.
            /// </summary>
            public bool UseSetFollowCamPackets {
                get { return useSetFollowCamPackets; }
                set { useSetFollowCamPackets = value; }
            }

            /// <summary>
            /// Position of the camera in the world to start with.
            /// </summary>
            public Vector3 WorldPosition {
                get { return worldPosition; }
                set { worldPosition = value; }
            }

            /// <summary>
            /// Pitch of the camera to start with.
            /// </summary>
            public float WorldPitch {
                get { return worldPitch; }
                set { worldPitch = value; }
            }

            /// <summary>
            /// Yaw of the camera to start with.
            /// </summary>
            public float WorldYaw {
                get { return worldYaw; }
                set { worldYaw = value; }
            }

            public Config() {
                proxyPort = CURRENT_PORT++;
            }

            public IConfig GeneralConfig {
                get { return generalConfig; }
            }

            public IConfig SectionConfig {
                get { return sectionConfig; }
            }

            private IConfig generalConfig;
            private IConfig sectionConfig;

            public Config(string[] args, string section, string file) {
                ArgvConfigSource argConfig = InitArgConfig(args);
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
                argConfig.AddSwitch(section, "ProxyPort", "p");
                argConfig.AddSwitch(section, "FirstName", "fn");
                argConfig.AddSwitch(section, "LastName", "l");
                argConfig.AddSwitch(section, "Password", "pw");
                argConfig.AddSwitch(section, "ControlCamera", "c");

                IConfigSource config = AddFile(argConfig, file);
                sectionConfig = config.Configs[section];
                generalConfig = config.Configs["General"];

                masterPort = Get(generalConfig, "MasterPort", DEFAULT_MASTER_PORT);
                masterAddress = Get(generalConfig, "MasterAddress", DEFAULT_MASTER_ADDRESS);
                worldPosition = GetV(generalConfig, "WorldPosition", new Vector3(128, 128, 40));
                worldPitch = Get(generalConfig, "WorldPitch", 0f);
                worldYaw = Get(generalConfig, "WorldYaw", 0f);

                viewerExe = Get(generalConfig, "ViewerExe", DEFAULT_CLIENT_EXE);
                workingDir = Get(generalConfig, "WorkingDirectory", Path.GetDirectoryName(viewerExe));
                loginURI = Get(generalConfig, "LoginURI", DEFAULT_LOGINURI);
                UseGrid = Get(generalConfig, "UseGrid", false);
                EnableWindowPackets = Get(generalConfig, "EnableWindowPackets", true);
                UseSetFollowCamPackets = !enableWindowPackets || Get(generalConfig, "UseSetFollowCamPackets", false);

                firstName = Get(sectionConfig, "FirstName", null);
                lastName = Get(sectionConfig, "LastName", null);
                password = Get(sectionConfig, "Password", null);
                proxyPort = Get(sectionConfig, "ProxyPort", CURRENT_PORT++);
                LoginGrid = Get(sectionConfig, "ProxyGrid", proxyPort.ToString());

                controlCamera = Get(sectionConfig, "ControlCamera", true);
            }
        }

        /*
        public static Coordinator InitCoordinator(string[] args, out IConfig masterConfig ) {
            ArgvConfigSource argConfig = InitArgConfig(args);
            argConfig.AddSwitch("Master", "AutoStartMaster", "am");
            argConfig.AddSwitch("Master", "AutoStartProxy", "ap");
            argConfig.AddSwitch("Master", "AutoStartClient", "ac");
            argConfig.AddSwitch("Master", "File", "f");
            argConfig.AddSwitch("Master", "GUI", "g");
            argConfig.AddSwitch("Master", "Help", "h");

            string file;
            IConfigSource config = Init.AddFile(argConfig, out file);

            masterConfig = config.Configs["Master"];
            IConfig generalConfig = config.Configs["General"];

            if (Init.Has(masterConfig, "Help")) {
                Console.WriteLine("Master Help");
                Console.WriteLine(Init.HelpHeaders);
                foreach (string line in Init.Help("Master").OrderBy(l => l))
                    Console.WriteLine(line);
                return null;
            }

            AddFile(config, file);

            LogManager.GetLogger("Master").Info("Creating Master.");

            Config proxyConfig = new Config(args, "Master", AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            Coordinator m = new Coordinator(proxyConfig);

            bool autostartMaster = Get(masterConfig, "AutoStartMaster", true);

            if (Get(masterConfig, "GUI", true)) {
                if (autostartMaster)
                    new Thread(() => {
                        Thread.Sleep(500);
                        m.StartMaster();
                    }).Begin();
            } else {
                if (autostartMaster)
                    m.StartMaster();
                if (Get(masterConfig, "AutoStartClient", false) || Get(masterConfig, "AutoStartProxy", false))
                    m.StartProxy();
                if (Get(masterConfig, "AutoStartClient", false))
                    m.StartClient();
            }

            return m;
        }

        public static Window InitWindow(string[] args, out IConfig slaveConfig) {
            ArgvConfigSource argConfig = InitArgConfig(args);
            argConfig.AddSwitch("General", "Name", "n");
            argConfig.AddSwitch("General", "File", "f");

            string file;
            IConfigSource config = AddFile(argConfig, out file);
            string name = Init.Get(config.Configs["General"], "Name", "Slave1");

            argConfig.AddSwitch(name, "AutoConnectSlave", "as");
            argConfig.AddSwitch(name, "AutoStartProxy", "ap");
            argConfig.AddSwitch(name, "AutoStartClient", "ac");
            argConfig.AddSwitch(name, "GUI", "g");
            argConfig.AddSwitch(name, "Help", "h");

            config = AddFile(argConfig, out file);
            
            slaveConfig = config.Configs[name];
            IConfig generalConfig = config.Configs["General"];

            if (Init.Has(slaveConfig, "Help")) {
                Console.WriteLine("Slave Help");
                Console.WriteLine(Init.HelpHeaders);

                IEnumerable<string> list = Init.Help(name);
                list = list.Concat(new string[] {
                    Init.MakeHelpLine("General", "Name", "n", "The name for this slave.", "Slave1")
                });
                foreach (string line in list.OrderBy(l => l))
                    Console.WriteLine(line);

                return null;
            }

            LogManager.GetLogger(name).Info("Creating " + name + ".");

            Config proxyConfig = new Config(args, name, AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            Window s = new Window(name, proxyConfig);

            bool autostartSlave = Get(slaveConfig, "AutoConnectSlave", true);


            if (Init.Get(slaveConfig, "GUI", true)) {
                if (autostartSlave)
                    new Thread(() => {
                        Thread.Sleep(500);
                        s.Connect();
                    }).Begin();
            } else {
                if (autostartSlave)
                    s.Connect();
                if (Get(slaveConfig, "AutoStartClient", false) || Get(slaveConfig, "AutoStartProxy", false))
                    s.StartProxy();
                if (Get(slaveConfig, "AutoStartClient", false))
                    s.StartClient();
            }
            return s;
        }
        */

        public static bool Has(IConfig cfg, string key) {
            return cfg != null && cfg.Contains(key);
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
        public static float Get(IConfig cfg, string key, float defalt) {
            return cfg == null ? defalt : cfg.GetFloat(key, defalt);
        }
        public static double Get(IConfig cfg, string key, double defalt) {
            return cfg == null ? defalt : cfg.GetDouble(key, defalt);
        }
        public static Vector3 GetV(IConfig cfg, string key, Vector3 defalt) {
            string raw = Get(cfg, key, null);
            Vector3 ret;
            if (raw == null || !Vector3.TryParse(raw, out ret))
                return defalt;
            return ret;
        }

        public static ArgvConfigSource InitArgConfig(string[] args) {
            try {
                return new ArgvConfigSource(args);
            } catch (Exception e) {
                Console.WriteLine("Unable to load argument config." + e.Message + "\n" + e.StackTrace);
                return new ArgvConfigSource(new string[0]);
            }
        }

        /*
        public static Form StartGui(IConfig config, ViewerProxy form, Func<Form> createForm) {
            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA) {
                Form f = createForm();
                StartProxyClient(config, form, f);
                f.ShowDialog();
                return f;
            } else {
                object createLock = new object();
                Form f = null;
                bool started = false;
                Thread t = new Thread(() => {
                    f = createForm();
                    lock (createLock)
                        MonitorChanged.PulseAll(createLock);
                    started = true;
                    StartProxyClient(config, form, f);
                    f.ShowDialog();
                });
                t.SetApartmentState(ApartmentState.STA);
                t.Begin();
                if (!started)
                    lock (createLock)
                        MonitorChanged.Wait(createLock);
                return f;
            }
        }

        private static void StartProxyClient(IConfig config, ViewerProxy form, Form f) {
            f.VisibleChanged += (source, args) => {
                if (!f.Visible)
                    return;
                bool autostartProxy = Get(config, "AutoStartProxy", false);
                bool autostartClient = Get(config, "AutoStartClient", false);

                if (autostartProxy || autostartClient)
                    while (!form.StartProxy())
                        form.ProxyConfig.ProxyPort++;
                if (autostartClient)
                    form.StartClient();
            };
        }
        */

        public static IConfigSource AddFile(IConfigSource config, string file) {
            if (File.Exists(file) && Path.GetExtension(file).ToUpper().Equals(".CONFIG")) {
                try {
                    DotNetConfigSource dotnet = new DotNetConfigSource(file);
                    //config.Merge(dotnet);
                    dotnet.Merge(config);
                    return dotnet;
                } catch (Exception e) {
                    Console.WriteLine("Unable to load app configuration file " + file + "'." + e.Message + ".\n" + e.StackTrace);
                }
            } else if (File.Exists(file) && Path.GetExtension(file).ToUpper().Equals(".INI")) {
                try {
                    IniConfigSource ini = new IniConfigSource(file);
                    //config.Merge(ini);
                    ini.Merge(config);
                    return ini;
                } catch (Exception e) {
                    Console.WriteLine("Unable to load ini configuration file " + file + "'." + e.Message + ".\n" + e.StackTrace);
                }
            }
            return config;
        }
    }
}
