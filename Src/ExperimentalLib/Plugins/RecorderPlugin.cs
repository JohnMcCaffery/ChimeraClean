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
        private Dictionary<string, ServerStats> mServerStats = new Dictionary<string, ServerStats>();
        private Dictionary<string, ClientStats> mClientStats = new Dictionary<string, ClientStats>();
        private ServerStats mLastStat;
        private Action mTickListener;
        private bool mRecording;
        private bool mCopyDone = false;
        private UUID mSessionID = UUID.Zero;

        public ServerStats this[string ts] {
            get { return mServerStats[ts]; }
        }

        public string this[string ts, string key] {
            get { 
                if (key == "CFPS" || key == "Polygons")
                    return mClientStats[ts].Get(key);
                else
                    return mServerStats[ts].Get(key);
            }
        }
        internal bool HasStat(string ts, string key) {
            if (key == "CFPS" || key == "Polygons")
                return mClientStats.ContainsKey(ts);
            else
                return mServerStats.ContainsKey(ts);
        }

        public IEnumerable<ServerStats> StatsList {
            get { return mServerStats.Values; }
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

        public void WriteCSV(string resultsFile) {            if (mServerStats.Count > 0) {

                string ids = mConfig.IDS;

                if (mSessionID != UUID.Zero)
                    ids = "," + mSessionID;

                File.Delete(resultsFile);
                try {
                    File.Create(resultsFile).Close();
                } catch (Exception ex) {
                    Logger.Warn("Unable to create " + resultsFile + ".", ex);
                }

                string headers = "Timestamp,Second,";
                headers += mConfig.OutputKeys.Aggregate((a, k) => a + "," + k);
                headers += ids;
                headers += Environment.NewLine;

                File.AppendAllText(resultsFile, headers);

                IEnumerable<string> lines = null;
                int count = 0;
                if (mServerStats.Count != 0 && mClientStats.Count != 0)
                    lines = mServerStats.Keys.Intersect(mClientStats.Keys).Select(ts => CombineStats(++count, mConfig, mServerStats[ts], mClientStats[ts]));
                else if (mServerStats.Count > 0)
                    lines = mServerStats.Values.Select(s => s.TimeStamp.ToString(s.ToString(mConfig, ++count)));
                else if (mClientStats.Count > 0)
                    lines = mClientStats.Values.Select(c => c.TimeStamp.ToString(c.ToString(mConfig, ++count)));

                if (lines != null)
                    File.AppendAllLines(resultsFile, lines);

                /*
                lock (mServerStats) {
                    File.AppendAllLines(resultsFile, mServerStats.
                        Values.
                        Where(s => s.TimeStamp > mConfig.Timestamp).
                        Select(s => s.ToString(mConfig.OutputKeys)));
                }
                */
            }
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
                mServerStats.Add(s.ToString(), s);

                ClientStats c = new ClientStats(line, mConfig);
                mClientStats.Add(c.ToString(), c);
            }

            string[] splitFilename = Path.GetFileNameWithoutExtension(file).Split(new char[] { '-' }, 2);
            mConfig.RunInfo = splitFilename[0];
            return DateTime.ParseExact(splitFilename[1], mConfig.TimestampFormat, new DateTimeFormatInfo());
        }

        public void LoadClientStats() {
            Dictionary<string, List<float>> fpses = new Dictionary<string, List<float>>();

            string startTS = mConfig.Timestamp.ToString(LOG_TIMESTAMP_FORMAT);
            int i = 0;
            foreach (var file in Directory.
                    GetFiles(Path.Combine("Experiments", mConfig.ExperimentName)).
                    Where(f => 
                        Path.GetExtension(f) == ".log" &&
                        Path.GetFileName(f).StartsWith(mConfig.Timestamp.ToString(mConfig.TimestampFormat)))) {

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

        public DateTime LoadViewerLog(string file, int frame) {
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

            foreach (var line in lines.Where(l => l.Contains("FPS") || l.Contains("POLYGONS"))) {
                DateTime ts = DateTime.ParseExact(line.Split(' ')[0], LOG_TIMESTAMP_FORMAT, new DateTimeFormatInfo());
                string time = ts.ToString(mConfig.TimestampFormat);

                if (!retSet) {
                    retSet = true;
                    ret = ts;
                }

                if (!mClientStats.ContainsKey(time))
                    mClientStats.Add(time, new ClientStats(line, mConfig, ts));
                else
                    mClientStats[time].AddLine(line, frame);

            }
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
                string ts = mLastStat.ToString();
                if (mServerStats.ContainsKey(ts))
                    mServerStats[ts] = mLastStat;
                else
                    mServerStats.Add(ts, mLastStat);
            }
        }

        public static string CombineStats(int count, ExperimentalConfig config, ServerStats server, ClientStats client) {
            DateTime ts = server.TimeStamp;
            string line = client.TimeStamp.ToString(config.TimestampFormat) + "," + count + ",";

            foreach (var key in config.OutputKeys) {
                switch (key.ToUpper()) {
                    case "CFPS": line += client.CFPS.Aggregate("", (s, v) => s + v + ",", f => f); break;
                    case "POLYGONS": line += client.Polys.Aggregate("", (s, v) => s + v + ",", f => f); break;
                    case "FT": line += server.FrameTime.ToString() + ","; break;
                    case "SFPS": line += server.SFPS.ToString() + ","; break;
                }
            }

            return line;
        }

        public struct ServerStats {
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
            public DateTime TimeStamp;

            private ExperimentalConfig mConfig;

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

            public ServerStats(Simulator.SimStats stats, int frames, ExperimentalConfig config) {
                mConfig = config;

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
                TimeStamp = DateTime.Now;
            }

            public ServerStats(string line, ExperimentalConfig config) {
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
                mConfig = config;

                TimeStamp = DateTime.ParseExact(s[0], mConfig.TimestampFormat, new DateTimeFormatInfo());

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

            public string ToString(ExperimentalConfig config, int count) {
                string line = TimeStamp.ToString(mConfig.TimestampFormat) + "," + count + ",";

                foreach (var key in config.OutputKeys)
                    line += Get(key) + ",";

                return line.TrimEnd(',');
            }
        }

        public struct ClientStats {
            public float[] CFPS;
            public float[] Polys;

            public DateTime TimeStamp;

            private ExperimentalConfig mConfig;

            public string Get(string key) {
                switch (key.ToUpper()) {
                    case "CFPS": return CFPS.Aggregate("", (s, v) => s + v + ",", f => f.TrimEnd(','));
                    case "POLYGONS": return Polys.Aggregate("", (s, v) => s + v + ",", f => f.TrimEnd(','));
                }
                return "-";
            }

            public ClientStats(string line, ExperimentalConfig config) {
                string[] s = line.Split(',');
                int frames = new CoreConfig().Frames.Length;

                CFPS = new float[frames];
                Polys = new float[frames];

                mConfig = config;

                TimeStamp = DateTime.ParseExact(s[0], mConfig.TimestampFormat, new DateTimeFormatInfo());

                for (int i = 0; i < mConfig.OutputKeys.Length; i++) {
                    switch (mConfig.OutputKeys[i]) {
                        case "CFPS": CFPS = s.Skip(i + 2).Take(frames).Select(cfps => cfps != "-" ? float.Parse(cfps) : 0f).ToArray(); i += frames-1; break;
                        case "Polygons": Polys = s.Skip(i + 2).Take(frames).Select(polys => polys != "-" ? float.Parse(polys) : 0f).ToArray(); i += frames-1; break;
                    }
                }
            }

            public ClientStats(string line, ExperimentalConfig config, DateTime timestamp) {
                string[] s = line.Split(',');
                int frames = new CoreConfig().Frames.Length;

                CFPS = new float[frames];
                Polys = new float[frames];

                mConfig = config;

                TimeStamp = timestamp;

                AddLine(line, 0);
            }

            public void AddLine(string line, int frame) {                string[] s = line.Split(' ');

                switch (s[5]) {
                    case "FPS:": CFPS[frame] = float.Parse(s[6]); break;
                    case "POLYGONS:": Polys[frame] = float.Parse(s[6]); break;
                }
            }

            public override string ToString() {
                return TimeStamp.ToString(mConfig.TimestampFormat);
            }

            public string ToString(ExperimentalConfig config, int count) {
                string line = TimeStamp.ToString(mConfig.TimestampFormat) + "," + count + ",";

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
            return Path.GetFullPath(Path.Combine("Experiments", mConfig.ExperimentName, mConfig.RunInfo + "-" + mConfig.Timestamp.ToString(mConfig.TimestampFormat) + ".csv"));
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



