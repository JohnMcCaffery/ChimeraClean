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

namespace Chimera.OpenSim {
    public class HeightmapPlugin : OpensimBotPlugin {
        private readonly ILog ThisLogger = LogManager.GetLogger("Heightmap");
        private readonly Dictionary<ulong, HashSet<string>> mMappedParcels = new Dictionary<ulong, HashSet<string>>();
        private readonly HashSet<ulong> mFinishedRegions = new HashSet<ulong>();

        private Core mCoordinator;
        private HeightmapConfig mConfig = new HeightmapConfig();
        private Point mStartLocation;
        private object mGridLayerWait = new object();
        private string mCurrentRegion;

        public HeightmapPlugin() {
            mConfig = new HeightmapConfig();
        }

        protected override IOpensimBotConfig BotConfig {
            get { return mConfig; }
        }

        protected override void OnLoggedIn() {
            Client.Terrain.LandPatchReceived += Terrain_LandPatchReceived;
            mCurrentRegion = Client.Network.CurrentSim.Name;
        }

        protected override void OnLoggingOut() {
            Client.Terrain.LandPatchReceived -= Terrain_LandPatchReceived;
        }

        protected override void OnLoggedOut() {
        }

        void Terrain_LandPatchReceived(object sender, LandPatchReceivedEventArgs e) {
            DateTime start = DateTime.UtcNow;
            if (e.X >= 16 || e.Y >= 16) {
                Logger.Log(String.Format("Bad patch coordinates, x = {0}, y = {1}", e.X, e.Y), Helpers.LogLevel.Warning);
                return;
            }

            if (e.PatchSize != 16) {
                Logger.Log(String.Format("Unhandled patch size {0} x {1}", e.PatchSize, +e.PatchSize), Helpers.LogLevel.Warning);
                return;
            }

            string simName = e.Simulator.Name;

            UpdateHeightmap(e);
        }

        private void UpdateHeightmap(LandPatchReceivedEventArgs e) {
            ulong handle = e.Simulator.Handle;

            lock (mMappedParcels) {
                if (!mMappedParcels.ContainsKey(handle))
                    mMappedParcels.Add(handle, new HashSet<string>());
                mMappedParcels[handle].Add(e.X + "," + e.Y);
            }

            int x = e.X * 16;
            int y = e.Y * 16;

            uint globalX, globalY;
            Utils.LongToUInts(handle, out globalX, out globalY);


            float[,] terrainHeight = new float[16, 16];
            for (int j = 0; j < 16; j++) {
                for (int i = 0; i < 16; i++) {
                    terrainHeight[i, j] = e.HeightMap[j * 16 + i];
                }
            }

            while (!LoggedIn)
                Thread.Sleep(500);

            x += (int) globalX - mStartLocation.X;
            y += (int) globalY - mStartLocation.Y;
            mCoordinator.SetHeightmapSection(terrainHeight, x, y, mMappedParcels[handle].Count > 250);

            int w = mCoordinator.Heightmap.GetLength(0) / 256;
            int h = mCoordinator.Heightmap.GetLength(1) / 256;
            int numRegions = w * h;

            lock (mMappedParcels) {
                if (mMappedParcels[handle].Count == 256) {
                    mFinishedRegions.Add(handle);
                    ThisLogger.Info("Finished mapping " + e.Simulator.Name);
                } if (mFinishedRegions.Count == numRegions && mConfig.AutoLogout)
                    Logout();
            }
        }

        #region IPlugin Members

        public override string Name {
            get { return "HeightmapBot"; }
        }

        #endregion
    }
}
