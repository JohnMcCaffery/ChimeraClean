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

namespace Chimera.OpenSim {
    public class HeightmapInput : ISystemPlugin {
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

        public HeightmapInput() {
            mConfig = new HeightmapConfig();
            mEnabled = mConfig.Enabled;
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
                Client.Settings.LOGIN_SERVER = mConfig.LoginURI;
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
                mEnabled = value;
                if (EnabledChanged != null)
                    EnabledChanged(this, value);
            }
        }

        public string Name {
            get { return "Bot Heightmap Listener"; }
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

        public void Draw(Func<Vector3, Point> to2D, System.Drawing.Graphics graphics, Action redraw) {
            //Do nothing
        }

        #endregion

        #region ISystemInput Members

        public Coordinator Coordinator {
            get { return mCoordinator; }
        }

        public void Init(Coordinator coordinator) {
            mCoordinator = coordinator;
            if (mConfig.AutoLogin)
                Login();
        }

        #endregion
    }
}
