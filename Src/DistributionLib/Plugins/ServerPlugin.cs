using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera;
using DistributionLib.GUI;
using Chimera.Plugins;
using Chimera.Config;
using DistributionLib.Config;
using System.Net.Sockets;
using UtilLib;
using System.Net;
using log4net;
using Chimera.OpenSim.Packets;

namespace DistributionLib.Plugins {
    public class ServerPlugin : PluginBase<ServerPluginPanel> {
        private ILog Logger = LogManager.GetLogger("ClientPlugin");

        private DistributedConfig mConfig = new DistributedConfig();
        private InterProxyServer mServer;

        private Action<Core, CameraUpdateEventArgs> mUpdateListener;

        private event Action<string, IPEndPoint> mClientConnectedListener;
        private event Action<string> mClientDisconnectedListener;

        public event Action<string, IPEndPoint> ClientConnected;
        public event Action<string> ClientDisconnected;

        public bool Started {
            get { return mServer != null; }
        }

        public ServerPlugin()
            : base("ServerPlugin", plugin => new ServerPluginPanel(plugin as ServerPlugin)) {
            mClientConnectedListener = ConnectedListener;
            mClientDisconnectedListener = DisconnectedListener;
            mUpdateListener = UpdateListener;

	    EnabledChanged += new Action<IPlugin,bool>(EnabledChangedListener);
        }

        private void UpdateListener(Core core, CameraUpdateEventArgs args) {
            if (Started)
                mServer.BroadcastPacket(new SetCameraPacket(mCore.Position, mCore.Orientation.LookAtVector));
        }

        private void EnabledChangedListener(IPlugin p, bool enabled) {
            if (enabled)
                mCore.CameraUpdated += mUpdateListener;
            else
                mCore.CameraUpdated -= mUpdateListener;
        }

        private void ConnectedListener(string name, IPEndPoint source) {
            if (ClientConnected != null)
                ClientConnected(name, source);
        }

        private void DisconnectedListener(string name) {
            if (ClientDisconnected != null)
                ClientDisconnected(name);
        }

        public void Start() {
            mServer = new InterProxyServer(mConfig.Port);
            mServer.OnSlaveConnected += mClientConnectedListener;
            mServer.OnSlaveDisconnected += mClientDisconnectedListener;
        }

        public void Stop() {
            lock (mServer) {
                mServer.Stop();
                mServer = null;
            }
        }

        public override void Init(Core core) {
            base.Init(core);

            if (mConfig.AutoStartServer) {
                Start();
                Enabled = true;
            }

        }

        public override ConfigBase Config {
            get { return mConfig; }
        }

        public override void Close() {
            base.Close();
            if (mServer != null) {
                Stop();
            }
        }
    }
}
