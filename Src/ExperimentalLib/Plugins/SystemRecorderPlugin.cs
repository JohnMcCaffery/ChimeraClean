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
using System.Diagnostics;
using System.Management;

namespace Chimera.OpenSim {
    public class SystemRecorderPlugin : ISystemPlugin {
        const string LOG_TIMESTAMP_FORMAT = "yyyy-MM-ddTHH:mm:ssZ";
        private ILog Logger = LogManager.GetLogger("SystemRecorder");

        private SystemRecorderControl mPanel;
        private ExperimentalConfig mConfig;
        private List<SystemStats> mStats = new List<SystemStats>();
        private SystemStats mLastStat;
        private Action mTickListener;
        private bool mRecording;
        private bool mEnabled = true;
        private UUID mSessionID = UUID.Zero;
        private int mLastServerStatSec = -1;
        private Core mCore;

        private SystemStats NearestServerStat(DateTime timestamp) {
            return mStats.FirstOrDefault(s => s.TimeStamp > timestamp);
        }

        public SystemStats this[DateTime timestamp] {
            get { return NearestServerStat(timestamp); }
        }

        public string this[DateTime ts, string key] {
            get { return NearestServerStat(ts).Get(key); }
        }
        internal bool HasStat(DateTime ts, string key) {
            return mStats.Last().TimeStamp > ts;
        }

        public IEnumerable<string> Stats {
            get { return mStats.Select(s => s.ToString()); }
        }

        public SystemStats LastStat {
            get { return mLastStat; }
        }

        public bool Recording {
            get { return mRecording; }
        }

        public Control ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new SystemRecorderControl(this);
                return mPanel;
            }
        }

        public string Name {
            get { return "SystemRecorder"; }
        }

        public void Init(Core coordinator) {
            SystemStats.InitialiseCounters();
            mCore = coordinator;
            mTickListener = new Action(mCore_Tick);

            mConfig = ExperimentalConfig.Instance;
            /*
            if (mCore.HasPlugin<AvatarMovementPlugin>()) {
                mMovementPlugin = mCore.GetPlugin<AvatarMovementPlugin>();
                mConfig = mMovementPlugin.Config as ExperimentalConfig;
            } else
                mConfig = new ExperimentalConfig();
            */

            mCore.Tick += mTickListener;
        }

        public void Close() {
            /*
            if (mConfig.ProcessOnFinish && mStats.Count > 0) {
                Logger.Warn("Writing out stats on close.");
                WriteCSV(GetCSVName());
            }
            */
        }

        public void WriteCSV(string file) {
            if (mStats.Count <= 0) {
                Logger.Warn("Not writing stats. No client or server stats in memory to write. Exiting with code " + mConfig.RepeatCode + ".");
                mCore.ExitCode = mConfig.RepeatCode;
                return;
            }

            string ids = mConfig.IDS;

            File.Delete(file);
            try {
                File.Create(file).Close();
            } catch (Exception ex) {
                Logger.Warn("Unable to create " + file + ".", ex);
            }

            string headers = "Timestamp,Second,Sample#,";
            headers += mConfig.OutputKeys.Aggregate((a, k) => a + "," + k);
            headers += ids;
            headers += Environment.NewLine;


            File.AppendAllText(file, headers);
            File.AppendAllLines(file, Stats);

            Logger.Info("Written statistics to: " + file);
        }

        public DateTime LoadCSV(string file) {
            mStats.Clear();

            bool skip = true;
            foreach (var line in File.ReadAllLines(file)) {
                if (skip) {
                    skip = false;
                    string[] split = line.Split(',');
                    mSessionID = UUID.Parse(split[split.Length - 1]);
                    continue;
                }

                SystemStats s = new SystemStats(line, mConfig);
                mStats.Add(s);
            }

            string[] splitFilename = Path.GetFileNameWithoutExtension(file).Split(new char[] { '-' }, 2);
            mConfig.RunInfo = splitFilename[0];
            DateTime ret = DateTime.ParseExact(splitFilename[1], mConfig.TimestampFormat, new DateTimeFormatInfo());

            Logger.Info("Loaded pre recorded statistics from: " + file + ".");

            return ret;
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

        void mCore_Tick() {
            mRecording = true;
            mLastStat = new SystemStats(mConfig);
            lock (mStats) {
                if (!mConfig.OneSecMininum || mLastServerStatSec != mLastStat.TimeStamp.Second) {
                    mStats.Add(mLastStat);
                    mLastServerStatSec = mLastStat.TimeStamp.Second;
                }
            }
        }

        public class SystemStats : Stats {
            private static PerformanceCounter sProcessorUsage;
            private static List<PerformanceCounter> sProcessorsUsage = new List<PerformanceCounter>();

            public static void InitialiseCounters() {
                //PerformanceCounterCategory.GetCategories()
                sProcessorUsage = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");

                for (int i = 0; i < Environment.ProcessorCount; i++)
                    sProcessorsUsage.Add(new PerformanceCounter("Processor Information", "% Processor Time", "0," + i));

            }

            private float mProcessorUsage;
            private List<float> mProcessorsUsage = new List<float>();

            public string Get(string key) {
                switch (key.ToUpper()) {
                    case "CPU": return mProcessorUsage.ToString();
                    case "CORE": return mProcessorsUsage.Aggregate("", (s, v) => s + v + ",", f => f.TrimEnd(','));
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

            public SystemStats(ExperimentalConfig config)
                : base(DateTime.Now, config) {

                mProcessorUsage = sProcessorUsage.NextValue();
                mProcessorsUsage.AddRange(sProcessorsUsage.Select(s => s.NextValue()));
            }

            public SystemStats(string line, ExperimentalConfig config)
                : base(line, config) {
                string[] s = line.Split(',');
                int processors = Environment.ProcessorCount;

                for (int i = 0; i < mConfig.OutputKeys.Length; i++) {
                    switch (mConfig.OutputKeys[i]) {
                        case "CPU": mProcessorUsage = float.Parse(s[i + 2]); break;
                        case "Core": mProcessorsUsage.AddRange(s.Skip(i + 2).Take(processors).Select(polys => polys != "-" ? float.Parse(polys) : 0f)); i += processors-1; break;
                    }
                }
            }

            public override string ToString() {
                return TimeStamp.ToString(mConfig.TimestampFormat);
            }

            public string ToString(ExperimentalConfig config, int count) {
                string line = base.ToString(count);

                foreach (var key in config.OutputKeys)
                    line += Get(key) + ",";

                return line.TrimEnd(',');
            }
        }

        internal void StartLogging() {
            mSessionID = UUID.Parse(mConfig.IDS.Split(',')[0]);
        }

        internal string GetCSVName() {
            string file = Path.GetFullPath(Path.Combine("Experiments", mConfig.ExperimentName, mConfig.RunInfo));
            if (mConfig.IncludeTimestamp)
                file += "-" + mConfig.Timestamp.ToString(mConfig.TimestampFormat);
            return file + ".csv";
        }


        public void SetForm(Form form) { }

        public event Action<IPlugin, bool> EnabledChanged;

        public bool Enabled {
            get { return mEnabled; }
            set {
                if (mEnabled != value) {
                    mEnabled = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public string State {
            get { throw new NotImplementedException(); }
        }

        public ConfigBase Config {
            get { return mConfig; }
        }

        public void Draw(Graphics graphics, Func<Vector3, Point> to2D, Action redraw, Perspective perspective) { }
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



