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
    public abstract class OpensimBotPlugin : ISystemPlugin {
        private readonly ILog ThisLogger = LogManager.GetLogger("Heightmap");
        private readonly Dictionary<ulong, HashSet<string>> mMappedParcels = new Dictionary<ulong, HashSet<string>>();
        private readonly HashSet<ulong> mFinishedRegions = new HashSet<ulong>();

        private  GridClient mClient = new GridClient();
        private Core mCore;
        private OpensimBotPanel mPanel;
        private bool mLoggedIn;
        private bool mEnabled;
        private bool mLoggingOut;
        private Point mStartLocation;

        public event Action<bool> LoggedInChanged;

        public event Action LoginFailed;

        protected GridClient Client {
            get { return mClient; }
        }

        protected Simulator Sim {
            get { return mClient.Network.CurrentSim; }
        }

        protected Core Core {
            get { return mCore; }
        }

        public bool LoggedIn {
            get { return mLoggedIn; }
        }

        public string LoginFailMessage {
            get { return mClient.Network.LoginErrorKey; }
        }

        public OpensimBotPlugin() { }

        protected abstract IOpensimBotConfig BotConfig { get; }

        protected abstract void OnLoggedIn();

        protected abstract void OnLoggingOut();

        protected abstract void OnLoggedOut();

        void Network_LoggedOut(object sender, LoggedOutEventArgs e) {
            mLoggedIn = false;
            mLoggingOut = false;
        }

        public void Login() {
            if (!mEnabled)
                return;
            Thread t = new Thread(() => {
                string startLocation = NetworkManager.StartLocation(BotConfig.StartIsland, (int)BotConfig.StartLocation.X, (int)BotConfig.StartLocation.Y, (int)BotConfig.StartLocation.Z);
                mClient.Settings.LOGIN_SERVER = new ViewerConfig().ProxyLoginURI;
                mClient.Network.LoggedOut += Network_LoggedOut;
                if (mClient.Network.Login(BotConfig.FirstName, BotConfig.LastName, BotConfig.Password, "Monitor", startLocation, "1.0")) {
                    uint globalX, globalY;
                    Utils.LongToUInts(mClient.Network.CurrentSim.Handle, out globalX, out globalY);
                    mStartLocation = new Point((int)globalX, (int)globalY);

                    mLoggedIn = true;
                    if (LoggedInChanged != null)
                        LoggedInChanged(true);
                } else {
                    ThisLogger.Warn("Unable to log in heightmap bot. " + mClient.Network.LoginMessage);
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
                OnLoggingOut();
                mClient.Network.Logout();
                mClient.Network.LoggedOut -= Network_LoggedOut;
                OnLoggedOut();
                if (LoggedInChanged != null)
                    LoggedInChanged(false);
            }
        }

        #region IPlugin Members

        public event Action<IPlugin, bool> EnabledChanged;

        public virtual Control ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new OpensimBotPanel(this);
                return mPanel;
            }
        }

        public bool Enabled {
            get { return mEnabled; }
            set {
                if (value != mEnabled) {
                    mEnabled = value;
                    if (value)
                        if (BotConfig.AutoLogin)
                            Login();
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public abstract string Name {
            get;
        }

        public string State {
            get { throw new NotImplementedException(); }
        }

        public ConfigBase Config {
            get { return BotConfig as ConfigBase; }
        }

        public virtual void Close() {
            Logout();
        }

        public virtual void Draw(System.Drawing.Graphics graphics, Func<Vector3, Point> to2D, Action redraw, Perspective perspective) {
            //Do nothing
        }

        #endregion

        #region ISystemPlugin Members

        public virtual void Init(Core coordinator) {
            mCore = coordinator;
        }

        public virtual void SetForm(Form form) {
        }

        #endregion
    }
}
