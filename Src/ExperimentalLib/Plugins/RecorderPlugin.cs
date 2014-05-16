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
using System.Windows.Forms;
using OpenMetaverse;
using Chimera.OpenSim.GUI;
using System.Threading;
using System.Drawing;
using Chimera.Config;
using log4net;
using Chimera.OpenSim.Interfaces;
using Chimera.Experimental.Plugins;
using Chimera.Experimental;
using System.IO;
using System.Globalization;
using Chimera.Experimental.GUI;
using Routrek.SSHCV2;
using Routrek.SSHC;
using System.Net;
using System.Net.Sockets;
using System.Data.SQLite;

namespace Chimera.OpenSim {
    public class RecorderPlugin : OpensimBotPlugin {
        const string LOG_TIMESTAMP_FORMAT = "yyyy-MM-ddTHH:mm:ssZ";
        private ILog Logger = LogManager.GetLogger("StatsRecorder");

        private RecorderControl mPanel;
        private AvatarMovementPlugin mMovementPlugin;
        private ExperimentalConfig mConfig;
        private List<ServerStats> mServerStats = new List<ServerStats>();
        private List<ClientStats> mClientStats = new List<ClientStats>();
        private ServerStats mLastStat;
        private Action mTickListener;
        private bool mRecording;
        private bool mCopyDone = false;
        private UUID mSessionID = UUID.Zero;

        private ServerStats NearestServerStat(DateTime timestamp) {
            return mServerStats.FirstOrDefault(s => s.TimeStamp > timestamp);
        }

        private ClientStats NearestClientStat(DateTime timestamp) {
            return mClientStats.FirstOrDefault(c => c.TimeStamp > timestamp);
        }

        public ServerStats this[DateTime timestamp] {
            get { return NearestServerStat(timestamp); }
        }

        public string this[DateTime ts, string key] {
            get { 
                if (key == "CFPS" || key == "Polygons" || key == "Ping")
                    return NearestClientStat(ts).Get(key);
                else
                    return NearestServerStat(ts).Get(key);
            }
        }
        internal bool HasStat(DateTime ts, string key) {
            if (key == "CFPS" || key == "Polygons" || key == "Ping")
                return mClientStats.Last().TimeStamp > ts;
            else
                return mServerStats.Last().TimeStamp > ts;
        }

        public IEnumerable<String> CombinedStats {
            get {
                if (mServerStats.Count != 0 && mClientStats.Count != 0) {
                    DateTime start = mConfig.Timestamp;

                    DateTime cEnd = mClientStats.Last().TimeStamp;
                    DateTime sEnd = mServerStats.Last().TimeStamp;
                    DateTime end = cEnd < sEnd ? cEnd : sEnd;

                    IEnumerator<ServerStats> server =
                            mServerStats.
                            SkipWhile(s => s.TimeStamp < start).
                            TakeWhile(s => s.TimeStamp <= end).
                            GetEnumerator();

                    IEnumerator<ClientStats> client =
                            mClientStats.
                            SkipWhile(c => c.TimeStamp < start).
                            TakeWhile(c => c.TimeStamp <= end).
                            GetEnumerator();

                    List<string> lines = new List<string>();

                    bool moreClients = server.MoveNext();
                    bool moreServers = client.MoveNext();

                    while (moreClients || moreServers) {
                        while (moreClients && moreServers && server.Current.TimeStamp == client.Current.TimeStamp) {
                            lines.Add(CombineStats(mConfig, server.Current, client.Current));
                            moreClients = client.MoveNext();
                            moreServers = server.MoveNext();
                        }

                        while (moreClients && (!moreServers || server.Current.TimeStamp > client.Current.TimeStamp)) {
                            lines.Add(client.Current.ToString(mConfig));
                            moreClients = client.MoveNext();
                        }

                        while (moreServers && (!moreClients || server.Current.TimeStamp < client.Current.TimeStamp)) {
                            lines.Add(server.Current.ToString(mConfig));
                            moreServers = server.MoveNext();
                        }
                    }

                    return lines;

                } else if (mServerStats.Count > 0)
                    return mServerStats.SkipWhile(s => s.TimeStamp < mConfig.Timestamp).Select(s => s.TimeStamp.ToString(s.ToString(mConfig)));
                else if (mClientStats.Count > 0)
                    return mClientStats.SkipWhile(c => c.TimeStamp < mConfig.Timestamp).Select(c => c.TimeStamp.ToString(c.ToString(mConfig)));

                return new string[0];
            }
        }

        public ServerStats LastStat {
            get { return mLastStat; }
        }

        public bool Recording {
            get { return mRecording; }
        }

        protected override IOpensimBotConfig BotConfig {
            get { return mConfig != null ? mConfig : mMovementPlugin.Config as ExperimentalConfig; }
        }

        protected override void OnLoggedIn() {
            mConfig.Timestamp = DateTime.Now;
        }

        protected override void OnLoggingOut() {
            Core.Tick -= mTickListener;
        }

        protected override void OnLoggedOut() { }

        public override Control ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new RecorderControl(this);
                return mPanel;
            }
        }

        public override string Name {
            get { return "Recorder"; }
        }

        public override void Init(Core coordinator) {
            mTickListener = new Action(Core_Tick);
            base.Init(coordinator);

            if (Core.HasPlugin<AvatarMovementPlugin>()) {
                mMovementPlugin = Core.GetPlugin<AvatarMovementPlugin>();
                mConfig = mMovementPlugin.Config as ExperimentalConfig;
            } else
                mConfig = new ExperimentalConfig();

            LoggedInChanged += new Action<bool>(RecorderPlugin_LoggedInChanged);
        }

        void RecorderPlugin_LoggedInChanged(bool loggedIn) {
            Core.Tick += mTickListener;
        }

        public override void Close() {
            base.Close();
        }

        public override void SetForm(Form form) {
            base.SetForm(form);
            form.FormClosed += new FormClosedEventHandler(form_FormClosed);
        }

        void form_FormClosed(object sender, FormClosedEventArgs e) {
            if (mConfig.ProcessOnFinish)
                LoadClientStats();
                WriteCSV(GetCSVName());
        }

        public void WriteCSV(string file) {            if (mServerStats.Count <= 0 && mClientStats.Count <= 0)
                return;

            string ids = mConfig.IDS;

            File.Delete(file);
            try {
                File.Create(file).Close();
            } catch (Exception ex) {
                Logger.Warn("Unable to create " + file + ".", ex);
            }

            string headers = "Timestamp,Second,";
            headers += mConfig.OutputKeys.Aggregate((a, k) => a + "," + k);
            headers += ids;
            headers += Environment.NewLine;


            File.AppendAllText(file, headers);
            File.AppendAllLines(file, CombinedStats);

            Logger.Info("Written statistics to: " + file);
        }

        public DateTime LoadCSV(string file) {
            mServerStats.Clear();

            bool skip = true;
            foreach (var line in File.ReadAllLines(file)) {
                if (skip) {
                    skip = false;
                    string[] split = line.Split(',');
                    mSessionID = UUID.Parse(split[split.Length - 1]);
                    continue;
                }

                ServerStats s = new ServerStats(line, mConfig);
                mServerStats.Add(s);

                ClientStats c = new ClientStats(line, mConfig);
                mClientStats.Add(c);
            }

            string[] splitFilename = Path.GetFileNameWithoutExtension(file).Split(new char[] { '-' }, 2);
            mConfig.RunInfo = splitFilename[0];
            DateTime ret = DateTime.ParseExact(splitFilename[1], mConfig.TimestampFormat, new DateTimeFormatInfo());

            Logger.Info("Loaded pre recorded statistics from: " + file + ".");

            return ret;
        }

        public void LoadClientStats() {
            Dictionary<string, List<float>> fpses = new Dictionary<string, List<float>>();

            string startTS = mConfig.Timestamp.ToString(LOG_TIMESTAMP_FORMAT);
            int i = 0;
            foreach (var file in Core.Frames.Select(f => mConfig.GetLogFileName(f.Name))) {
                /*
                Directory.
                    GetFiles(Path.Combine("Experiments", mConfig.ExperimentName)).
                    Where(f => 
                        Path.GetExtension(f) == ".log" &&
                        Path.GetFileName(f).StartsWith(mConfig.Timestamp.ToString(mConfig.TimestampFormat)))) {
                */

                            LoadViewerLog(file, i++);
            }
        }

        public DateTime LoadViewerLog(string file) {
            Dictionary<string, List<float>> fpses = new Dictionary<string, List<float>>();
            DateTime ret = LoadViewerLog(file, 0);
            if (mSessionID == UUID.Zero)
                GetMostRecentSessionID();
            return ret;
        }

        public DateTime LoadViewerLog(string file, int frame) {            if (!File.Exists(file)) {
                Logger.Warn("Unable to load viewer log from '" + file + "'. File does not exist.");
                return DateTime.Now;
            }
            string[] lines = null;
            int wait = 500;
            bool retSet = false;
            DateTime ret = DateTime.Now;

            while (lines == null) {
                try {
                    lines = File.ReadAllLines(file);
                } catch (IOException e) {
                    if (wait > 60000)
                        return ret;
                    Logger.Debug("Problem loading log file. Waiting " + wait + "MS then trying again.");
                    //Logger.Debug("Problem loading log file. Waiting " + wait + "MS then trying again.", e);
                    Thread.Sleep(wait);
                    wait = (int)(wait * 1.5);
                }
            }

            foreach (var line in lines.Where(l => l.Contains("FPS") && l.Contains("POLYGONS") && l.Contains("PING"))) {
                DateTime ts = DateTime.ParseExact(line.Split(' ')[0], LOG_TIMESTAMP_FORMAT, new DateTimeFormatInfo());
                string time = ts.ToString(mConfig.TimestampFormat);

                if (!retSet) {
                    retSet = true;
                    ret = ts;
                }

                mClientStats.Add(new ClientStats(line, mConfig, ts));

            }

            Logger.Info("Loaded viewer log file from: " + file + ".");

            return ret;
        }

        private void CopyWebStatsDB() {
            string dbFolder = Path.Combine(Environment.CurrentDirectory, "Experiments", mConfig.ExperimentName);
            string file = "LocalUserStatistics.db";

            if (!mCopyDone) {
                mCopyDone = true;

                string local = dbFolder.Substring(2);
                //string remote = "/home/opensim/opensim-0.7.3.1/bin/LocalUserStatistics.db";
                string remote = "/home/opensim/opensim-0.7.6.1/bin/" + file;
                string server = "mimuve.cs.st-andrews.ac.uk";
                string pass = "P3ngu1n!";
                string username = "jm726";

                ProcessController p = new ProcessController("cmd.exe", "C:\\Windows\\System32", "");

                p.Start();

                Thread.Sleep(500);

                p.PressKey("E:{ENTER}");
                p.SendString("scp " + username + "@" + server + ":" + remote + " " + local);
                p.PressKey("{ENTER}");
                p.SendString(pass);
                p.PressKey("{ENTER}");
                p.SendString(pass);
                p.PressKey("{ENTER}");

                p.Process.Close();

                Thread.Sleep(5000);
            }
        }

        public void GetMostRecentSessionID() {
            string dbFolder = Path.Combine(Environment.CurrentDirectory, "Experiments", mConfig.ExperimentName);
            string file = "LocalUserStatistics.db";
            CopyWebStatsDB();

            var viewerCfg = new ViewerConfig("MainWindow");

            string dbFile = Path.Combine(dbFolder, file);
            var connection = new SQLiteConnection("Data Source=" + dbFile + ";Version=3");
            connection.Open();

            SQLiteDataReader reader;

            //var dataCommand = new SQLiteCommand("SELECT avg_ping FROM stats_session_data WHERE name_f == '" + viewerCfg.LoginFirstName + "' AND name_l == '" + viewerCfg.LoginLastName + "';", connection);
            var dataCommand = new SQLiteCommand("SELECT session_id FROM stats_session_data WHERE name_f == '" + viewerCfg.LoginFirstName + "' AND name_l == '" + viewerCfg.LoginLastName + "';", connection);
            reader = dataCommand.ExecuteReader();
            object[] lastLine = new object[1];
            while (reader.Read()) {
                int columns = reader.GetValues(lastLine);
            }

            mSessionID = UUID.Parse(lastLine[0].ToString());

            connection.Close();

        }

        public void LoadPingTime() {
            string dbFolder = Path.Combine(Environment.CurrentDirectory, "Experiments", mConfig.ExperimentName);
            string file = "LocalUserStatistics.db";

            CopyWebStatsDB();

            var viewerCfg = new ViewerConfig("MainWindow");

            string dbFile = Path.Combine(dbFolder, file);
            var connection = new SQLiteConnection("Data Source=" + dbFile + ";Version=3");
            connection.Open();

            SQLiteDataReader reader;

            //var dataCommand = new SQLiteCommand("SELECT avg_ping FROM stats_session_data WHERE name_f == '" + viewerCfg.LoginFirstName + "' AND name_l == '" + viewerCfg.LoginLastName + "';", connection);
            var dataCommand = new SQLiteCommand("SELECT * FROM stats_session_data WHERE name_f == '" + viewerCfg.LoginFirstName + "' AND name_l == '" + viewerCfg.LoginLastName + "';", connection);
            reader = dataCommand.ExecuteReader();
            while (reader.Read()) {
                object[] row = new object[100];
                int columns = reader.GetValues(row);
                for (int i = 0; i < columns; i++)
                    Console.Write(row[i] + ", ");
                Console.WriteLine();
            }

            connection.Close();

        }

        public void ProcessFolder() {
            Dictionary<string, List<string[]>> lines = new Dictionary<string, List<string[]>>();
            Dictionary<string, List<UUID>> session_ids = new Dictionary<string, List<UUID>>();

            string folder = Path.Combine(Environment.CurrentDirectory, "Experiments", mConfig.ExperimentName);
            foreach (var file in Directory.GetFiles(folder).Where(f => Path.GetExtension(f) == ".csv")) {
                string configuration = file.Split('-')[0];
                if (!lines.ContainsKey(configuration)) {
                    lines.Add(configuration, new List<string[]>());
                    session_ids.Add(configuration, new List<UUID>());
                }

                string[] linesSet = File.ReadAllLines(file);
                lines[configuration].Add(linesSet);

                string[] headers = linesSet[0].Split(',');
                for (int i = linesSet[1].Split(',').Length; i < headers.Length; i++)
                    session_ids[configuration].Add(UUID.Parse(headers[i]));
            }

            for (int i = 0; i < mConfig.OutputKeys.Length; i++) {
                string key = mConfig.OutputKeys[i];
            }
        }

        void Core_Tick() {
            mRecording = true;
            mLastStat = new ServerStats(Sim.Stats, Core.Frames.Count(), mConfig);
            lock (mServerStats) {
                mServerStats.Add(mLastStat);
            }
        }

        public static string CombineStats(ExperimentalConfig config, ServerStats server, ClientStats client) {
            DateTime ts = server.TimeStamp;
            string line = client.TimeStamp.ToString(config.TimestampFormat) + ",";
            line += (config.Timestamp - ts).TotalMilliseconds + ",";

            foreach (var key in config.OutputKeys) {
                switch (key.ToUpper()) {
                    case "CFPS": line += client.CFPS.Aggregate("", (s, v) => s + v + ",", f => f); break;
                    case "POLYGONS": line += client.Polys.Aggregate("", (s, v) => s + v + ",", f => f); break;
                    case "PING": line += client.Ping.Aggregate("", (s, v) => s + v + ",", f => f); break;
                    case "FT": line += server.FrameTime.ToString() + ","; break;
                    case "SFPS": line += server.SFPS.ToString() + ","; break;
                }
            }

            return line + "," + ((server.TimeStamp - client.TimeStamp).TotalMilliseconds);
        }
        public class Stats {
            public readonly DateTime TimeStamp;
            protected readonly ExperimentalConfig mConfig;

            public Stats(DateTime timestamp, ExperimentalConfig config) {
                mConfig = config;

                TimeStamp = timestamp;
            }

            public Stats(string line, ExperimentalConfig config) {
                string[] s = line.Split(',');
                int frames = new CoreConfig().Frames.Length;

                mConfig = config;
                TimeStamp = DateTime.ParseExact(line.Split(',')[0], mConfig.TimestampFormat, new DateTimeFormatInfo());
            }

            public override string ToString() {
                return TimeStamp.ToString(mConfig.TimestampFormat) + "," + (TimeStamp - mConfig.Timestamp).TotalMilliseconds + ",";
            }
        }

        public class ServerStats : Stats {
            public readonly float Dilation;
            public readonly int SFPS;
            public readonly int Agents;
            public readonly int IncomingBPS;
            public readonly int OutgoingBPS;
            public readonly int ResentPackets;
            public readonly int ReceivedResends;
            public readonly float PhysicsFPS;
            public readonly float AgentUpdates;
            public readonly int Objects;
            public readonly int ScriptedObjects;
            public readonly float FrameTime;
            public readonly float NetTime;
            public readonly float ImageTime;
            public readonly float PhysicsTime;
            public readonly float ScriptTime;
            public readonly float OtherTime;
            public readonly int ChildAgents;
            public readonly int ActiveScripts;

            public string Get(string key) {
                switch (key.ToUpper()) {
                    case "FT": return FrameTime.ToString();
                    case "SFPS": return SFPS.ToString();
                }
                return "-";
            }

            private string ArrayStr(Array ar) {
                string ret = "";
                foreach (var item in ar)
                    ret += item + ",";
                Console.WriteLine("From array: " + ar.GetValue(0));
                return ret.TrimEnd(',');
            }

            public ServerStats(Simulator.SimStats stats, int frames, ExperimentalConfig config) : base (DateTime.Now, config) {
                Dilation = stats.Dilation;
                SFPS = stats.FPS;
                Agents = stats.Agents;
                IncomingBPS = stats.IncomingBPS;
                OutgoingBPS = stats.OutgoingBPS;
                ResentPackets = stats.ResentPackets;
                ReceivedResends = stats.ReceivedResends;
                PhysicsFPS = stats.PhysicsFPS;
                AgentUpdates = stats.AgentUpdates;
                Objects = stats.Objects;
                ScriptedObjects = stats.ScriptedObjects;
                FrameTime = stats.FrameTime;
                NetTime = stats.NetTime;
                ImageTime = stats.ImageTime;
                PhysicsTime = stats.PhysicsTime;
                ScriptTime = stats.ScriptTime;
                OtherTime = stats.OtherTime;
                ChildAgents = stats.ChildAgents;
                ActiveScripts = stats.ActiveScripts;
            }

            public ServerStats(string line, ExperimentalConfig config)
                : base(line, config) {
                string[] s = line.Split(',');
                int frames = new CoreConfig().Frames.Length;

                Dilation = 0f;
                SFPS = 0;
                Agents = 0;
                IncomingBPS = 0;
                OutgoingBPS = 0;
                ResentPackets = 0;
                ReceivedResends = 0;
                PhysicsFPS = 0f;
                AgentUpdates = 0f;
                Objects = 0;
                ScriptedObjects = 0;
                FrameTime = 0f;
                NetTime = 0f;
                ImageTime = 0f;
                PhysicsTime = 0f;
                ScriptTime = 0f;
                OtherTime = 0f;
                ChildAgents = 0;
                ActiveScripts = 0;

                for (int i = 0; i < mConfig.OutputKeys.Length; i++) {
                    switch (mConfig.OutputKeys[i]) {
                        case "FT": FrameTime = float.Parse(s[i + 2]); break;
                        case "SFPS": SFPS = int.Parse(s[i + 2]); break;
                    }
                }
            }

            public override string ToString() {
                return TimeStamp.ToString(mConfig.TimestampFormat);
            }

            public string ToString(ExperimentalConfig config) {
                string line = base.ToString();

                foreach (var key in config.OutputKeys)
                    line += Get(key) + ",";

                return line.TrimEnd(',');
            }
        }

        public class ClientStats : Stats {
            public float[] CFPS;
            public float[] Polys;
            public float[] Ping;


            public string Get(string key) {
                switch (key.ToUpper()) {
                    case "CFPS": return CFPS.Aggregate("", (s, v) => s + v + ",", f => f.TrimEnd(','));
                    case "POLYGONS": return Polys.Aggregate("", (s, v) => s + v + ",", f => f.TrimEnd(','));
                    case "PING": return Ping.Aggregate("", (s, v) => s + v + ",", f => f.TrimEnd(','));
                }
                return "-";
            }

            public ClientStats(string line, ExperimentalConfig config) : base (line, config) {
                string[] s = line.Split(',');
                int frames = new CoreConfig().Frames.Length;

                CFPS = new float[frames];
                Polys = new float[frames];
                Ping = new float[frames];

                for (int i = 0; i < mConfig.OutputKeys.Length; i++) {
                    switch (mConfig.OutputKeys[i]) {
                        case "CFPS": CFPS = s.Skip(i + 2).Take(frames).Select(cfps => cfps != "-" ? float.Parse(cfps) : 0f).ToArray(); i += frames-1; break;
                        case "Polygons": Polys = s.Skip(i + 2).Take(frames).Select(polys => polys != "-" ? float.Parse(polys) : 0f).ToArray(); i += frames-1; break;
                        case "Ping": Polys = s.Skip(i + 2).Take(frames).Select(ping => ping != "-" ? float.Parse(ping) : 0f).ToArray(); i += frames-1; break;
                    }
                }
            }


            public ClientStats(string line, ExperimentalConfig config, DateTime timestamp)
                : base(timestamp, config) {
                string[] s = line.Split(',');
                int frames = new CoreConfig().Frames.Length;

                CFPS = new float[frames];
                Polys = new float[frames];
                Ping = new float[frames];

                AddLine(line, 0);
            }

            public void AddLine(string line, int frame) {                string[] s = line.Split(' ');

                CFPS[frame] = float.Parse(s[6]);
                Polys[frame] = float.Parse(s[10]);
                Ping[frame] = float.Parse(s[8]);
            }

            public override string ToString() {
                return TimeStamp.ToString(mConfig.TimestampFormat);
            }

            public string ToString(ExperimentalConfig config) {
                string line = base.ToString();

                foreach (var key in mConfig.OutputKeys)
                    line += Get(key) + ",";

                return line.TrimEnd(',');
            }
        }

        internal void StartLogging() {
            mConfig.SetupFPSLogs(Core, Logger);
            mSessionID = UUID.Parse(mConfig.IDS.Split(',')[0]);
        }

        internal string GetCSVName() {
            string file = Path.GetFullPath(Path.Combine("Experiments", mConfig.ExperimentName, mConfig.RunInfo));
            if (mConfig.IncludeTimestamp)
                file += "-" + mConfig.Timestamp.ToString(mConfig.TimestampFormat);
            return file + ".csv";
        }
    }
}
            /*
            var param = new SSHConnectionParameter();
            param.UserName = "jm726";
            param.Password = "P3ngu1n!";
            param.Protocol = SSHProtocol.SSH2;
            param.AuthenticationType = AuthenticationType.Password;
            param.WindowSize = 0x1000;
            param.PreferableCipherAlgorithms = new CipherAlgorithm[] { 
                CipherAlgorithm.Blowfish, 
                CipherAlgorithm.TripleDES, 
                CipherAlgorithm.AES128, 
            };

            var reader = new Reader();
            var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Connect(new IPEndPoint(Dns.GetHostAddresses("mimuve.cs.st-andrews.ac.uk")[0], 22));

            SSHConnection _conn = SSHConnection.Connect(param, reader, sock);
            reader._conn = _conn;
            SSHChannel ch = _conn.OpenShell(reader);
            reader._pf = ch;
            SSHConnectionInfo info = _conn.ConnectionInfo;

             
             
             
           
        private class Reader : ISSHChannelEventReceiver, ISSHConnectionEventReceiver {
            public SSHConnection _conn;
            public SSHChannel _pf;

            public void OnChannelClosed() { }

            public void OnChannelEOF() { }

            public void OnChannelError(Exception error, string msg) { }

            public void OnChannelReady() { }

            public void OnData(byte[] data, int offset, int length) { }

            public void OnExtendedData(int type, byte[] data) { }

            public void OnMiscPacket(byte packet_type, byte[] data, int offset, int length) { }

            public PortForwardingCheckResult CheckPortForwardingRequest(string remote_host, int remote_port, string originator_ip, int originator_port) {
                throw new NotImplementedException();
            }

            public void EstablishPortforwarding(ISSHChannelEventReceiver receiver, SSHChannel channel) { }

            public void OnAuthenticationPrompt(string[] prompts) { }

            public void OnConnectionClosed() { }

            public void OnDebugMessage(bool always_display, byte[] msg) { }

            public void OnError(Exception error, string msg) { }

            public void OnIgnoreMessage(byte[] msg) { }

            public void OnUnknownMessage(byte type, byte[] data) { }
        } 
            */



