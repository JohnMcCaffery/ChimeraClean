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

namespace Chimera.OpenSim {
    public class RecorderPlugin : OpensimBotPlugin {
        private AvatarMovementPlugin mMovementPlugin;
        private ExperimentalConfig mConfig;
        private Dictionary<string, Stats> mStats = new Dictionary<string, Stats>();
        private Stats mLastStat;

        public Stats LastStat {
            get { return mLastStat; }
        }

        protected override IOpensimBotConfig BotConfig {
            get { return mConfig != null ? mConfig : mMovementPlugin.Config as ExperimentalConfig; }
        }

        protected override void OnLoggedIn() {
        }

        protected override void OnLoggingOut() {
        }

        protected override void OnLoggedOut() {
        }

        public override string Name {
            get { return "Recorder"; }
        }

        public override void Init(Core coordinator) {
            base.Init(coordinator);
            if (Core.HasPlugin<AvatarMovementPlugin>())
                mMovementPlugin = Core.GetPlugin<AvatarMovementPlugin>();
            else
                mConfig = new ExperimentalConfig();

            Core.Tick += new Action(Core_Tick);
        }

        void Core_Tick() {
            mLastStat = new Stats(Sim.Stats, Core.Frames.Count());
            mStats.Add(mLastStat.TimeStamp.ToString("yyyy.MM.dd.HH.mm.ss"), mLastStat);
        }

        public struct Stats {
            public int[] CFPS;
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

            public string Get(string key) {
                switch (key.ToUpper()) {
                    case "FPS": return CFPS.Aggregate("", (s, v) => s + v + ",", f => f.TrimEnd(','));
                    case "PINGTIME": return PingTime.Aggregate("", (s, v) => s + v + ",", f => f.TrimEnd(','));
                    case "FT": return FrameTime.ToString();
                    case "SFPS": return SFPS.ToString();
                }
                return "UnknownKey(" + key + ")";
            }

            public Stats(Simulator.SimStats stats, int frames) {
                CFPS = new int[frames];
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
        }
    }
}
