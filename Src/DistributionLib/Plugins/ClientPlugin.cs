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
using OpenMetaverse.Packets;
using Chimera.OpenSim.Packets;
using Chimera.Util;
using log4net;

namespace DistributionLib.Plugins {
    public class ClientPlugin : PluginBase<ClientPluginPanel> {
        private ILog Logger = LogManager.GetLogger("ClientPlugin");

        private DistributedConfig mConfig = new DistributedConfig();
        private InterProxyClient mClient;
        private BackChannelPacketDelegate mListener;

        public bool Connected {
            get { return mClient != null; }
        }

        public ClientPlugin()
            : base("ClientPlugin", plugin => new ClientPluginPanel(plugin as ClientPlugin)) {
            mListener = new BackChannelPacketDelegate(Listener);
        }

        private Packet Listener(Packet p, IPEndPoint source) {
            if (Enabled && p is SetCameraPacket) {
                SetCameraPacket.CameraBlock camera = (p as SetCameraPacket).Camera;
                mCore.Update(camera.Position, camera.PositionDelta, new Rotation(camera.LookAt), new Rotation(camera.LookAtDelta));
            }
            return p;
        }

        public void Connect() {
            Logger.Info("Connecting as " + mConfig.ClientName);
            mClient = new InterProxyClient(mConfig.ClientName, mConfig.Port);
            mClient.OnPacketReceived += mListener;
        }

        public void Disconnect() {
            mClient.OnPacketReceived -= mListener;
            mClient.Disconnect();
            mClient = null;
        }

        public override void Init(Core core) {
            base.Init(core);

            if (mConfig.AutoStartClient) {
                Connect();
                Enabled = true;
            }
        }

        public override ConfigBase Config {
            get { return mConfig; }
        }
    }
}
