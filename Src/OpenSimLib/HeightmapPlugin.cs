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

namespace Chimera.OpenSim {
    public class HeightmapPlugin : ISystemPlugin {
        private readonly Dictionary<ulong, HashSet<string>> mMappedParcels = new Dictionary<ulong, HashSet<string>>();
        private readonly HashSet<ulong> mFinishedRegions = new HashSet<ulong>();

        private  GridClient Client = new GridClient();
        private Coordinator mCoordinator;
        private HeightmapConfig mConfig;
        private HeightmapInputPanel mPanel;
        private bool mLoggedIn;
        private bool mEnabled;
        private bool mLoggingOut;
        private Point mStartLocation;
        private object mGridLayerWait = new object();
        private string mCurrentRegion;


        public event Action<bool> LoggedInChanged;

        public event Action LoginFailed;

        public bool LoggedIn {
            get { return mLoggedIn; }
        }

        public string LoginFailMessage {
            get { return Client.Network.LoginErrorKey; }
        }

        public HeightmapPlugin() {
            mConfig = new HeightmapConfig();
        }

        void Network_LoggedOut(object sender, LoggedOutEventArgs e) {
            mLoggedIn = false;
            mLoggingOut = false;
        }

        void Terrain_LandPatchReceived(object sender, LandPatchReceivedEventArgs e) {
            DateTime start = DateTime.Now;
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

            while (!mLoggedIn)
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
                    Console.WriteLine("Finished mapping " + e.Simulator.Name);
                } if (mFinishedRegions.Count == numRegions && mConfig.AutoLogout)
                    Logout();
            }
        }

        public void Login() {
            if (!mEnabled)
                return;
            Thread t = new Thread(() => {
                string startLocation = NetworkManager.StartLocation(mConfig.StartIsland, (int)mConfig.StartLocation.X, (int)mConfig.StartLocation.Y, (int)mConfig.StartLocation.Z);
                Client.Settings.LOGIN_SERVER = new ViewerConfig().ProxyLoginURI;
                Client.Terrain.LandPatchReceived += Terrain_LandPatchReceived;
                Client.Network.LoggedOut += Network_LoggedOut;
                if (Client.Network.Login(mConfig.FirstName, mConfig.LastName, mConfig.Password, "Monitor", startLocation, "1.0")) {
                    mCurrentRegion = Client.Network.CurrentSim.Name;
                    uint globalX, globalY;
                    Utils.LongToUInts(Client.Network.CurrentSim.Handle, out globalX, out globalY);
                    mStartLocation = new Point((int)globalX, (int)globalY);

                    mLoggedIn = true;
                    if (LoggedInChanged != null)
                        LoggedInChanged(true);
                } else {
                    if (LoginFailed != null)
                        LoginFailed();
                }
            });
            t.Name = "Heightmap Bot Login Thread";
            t.Start();
        }

        public void Logout() {
            if (mLoggedIn && !mLoggingOut) {
                mLoggingOut = true;
                Client.Network.Logout();
                Client.Terrain.LandPatchReceived -= Terrain_LandPatchReceived;
                Client.Network.LoggedOut -= Network_LoggedOut;
                if (LoggedInChanged != null)
                    LoggedInChanged(false);
            }
        }

        #region IInput Members

        public event Action<IPlugin, bool> EnabledChanged;

        public UserControl ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new HeightmapInputPanel(this);
                return mPanel;
            }
        }

        public bool Enabled {
            get { return mEnabled; }
            set {
                if (value != mEnabled) {
                    mEnabled = value;
                    if (value)
                        if (mConfig.AutoLogin)
                            Login();
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public string Name {
            get { return "HeightmapBot"; }
        }

        public string State {
            get { throw new NotImplementedException(); }
        }

        public ConfigBase Config {
            get { return mConfig; }
        }

        public void Close() {
            Logout();
        }

        public void Draw(System.Drawing.Graphics graphics, Func<Vector3, Point> to2D, Action redraw, Perspective perspective) {
            //Do nothing
        }

        #endregion

        #region ISystemPlugin Members

        public void Init(Coordinator coordinator) {
            mCoordinator = coordinator;
        }

        #endregion

        #region ISystemPlugin Members


        public void SetForm(Form form) {
        }

        #endregion
    }
}
