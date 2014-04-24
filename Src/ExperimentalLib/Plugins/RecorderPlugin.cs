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

namespace Chimera.OpenSim {
    public class RecorderPlugin : OpensimBotPlugin {
        private AvatarMovementPlugin mMovementPlugin;
        private ExperimentalConfig mConfig;
        private Dictionary<string, Stats> mStats = new Dictionary<string, Stats>();
        private Stats mLastStat;
        private Action mTickListener;
        private int mExitCount = 0;

        public Stats LastStat {
            get { return mLastStat; }
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

            foreach (var output in Core.Frames.Select(f => f.Output))
                output.Process.Exited += new EventHandler(Process_Exited);
        }

        void Process_Exited(object sender, EventArgs e) {
            if (++mExitCount == Core.Frames.Count())
                LoadFPS();
        }

        public void LoadFPS() {
            Dictionary<string, List<float>> fpses = new Dictionary<string, List<float>>();

            string startTS = mConfig.Timestamp;
            foreach (var file in Directory.
                    GetFiles(Path.Combine("Experiments", mConfig.ExperimentName)).
                    Where(f => Path.GetFileName(f).StartsWith(mConfig.Timestamp))) {

                foreach (var line in File.
                        ReadAllLines(file).
                        Where(l => l.Contains("FPS")).
                        SkipWhile(l => !l.StartsWith(startTS))) {

                    string[] s = line.Split(' ');
                    if (!fpses.ContainsKey(s[0]))
                        fpses.Add(s[0], new List<float>());
                    fpses[s[0]].Add(float.Parse(s[6]));
                }
            }

            foreach (var timestamp in mStats.Keys) {
                if (fpses.ContainsKey(timestamp)) {
                    Stats stat = mStats[timestamp];
                    stat.CFPS = fpses[timestamp].ToArray();
                }
            }

            string resultsFile = Path.GetFullPath(Path.Combine("Experiments", mConfig.ExperimentName, mConfig.Timestamp + ".csv"));
            File.Delete(resultsFile);
            File.Create(resultsFile);
            File.AppendAllText(resultsFile, mConfig.OutputKeys.Aggregate((a, k) => a + "," + k) + Environment.NewLine);
            File.AppendAllLines(resultsFile, mStats.Values.Select(s => s.ToString(mConfig.OutputKeys)));
        }

        void Core_Tick() {
            mLastStat = new Stats(Sim.Stats, Core.Frames.Count(), mConfig);
            string ts = mLastStat.ToString();
            if (mStats.ContainsKey(ts))
                mStats[ts] = mLastStat;
            else
                mStats.Add(mLastStat.ToString(), mLastStat);
        }

        public struct Stats {
            public float[] CFPS;
            public int[] PingTime;

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
                    case "CFPS": return CFPS.Aggregate("", (s, v) => s + v + ",", f => f.TrimEnd(','));
                    case "PINGTIME": return PingTime.Aggregate("", (s, v) => s + v + ",", f => f.TrimEnd(','));
                    case "FT": return FrameTime.ToString();
                    case "SFPS": return SFPS.ToString();
                }
                return "UnknownKey(" + key + ")";
            }

            public Stats(Simulator.SimStats stats, int frames, ExperimentalConfig config) {
                mConfig = config;

                CFPS = new float[frames];
                PingTime = new int[frames];

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

            public override string ToString() {
                return TimeStamp.ToString(mConfig.TimestampFormat);
            }

            public string ToString(params string[] keys) {
                string line = "";

                foreach (var key in keys)
                    line += Get(key) + ",";

                return line.TrimEnd(',');
            }
        }
    }
}
