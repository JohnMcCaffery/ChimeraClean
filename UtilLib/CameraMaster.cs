using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GridProxy;
using OpenMetaverse.Packets;
using System.Net;
using OpenMetaverse;

namespace UtilLib {
    public class CameraMaster : Master {
        private readonly HashSet<PacketType> packetTypesToForward = new HashSet<PacketType>();
        private int packetsReceived;
        private int packetsForwarded;
        private int packetsCreated;
        private int packetsProccessed;
        private bool processingPacket;

        /// <summary>
        /// Triggered whenever a packet is received from the connected proxyAddress.
        /// </summary>
        public event PacketDelegate OnPacketReceived;

        /// <summary>
        /// Triggered whenever a packet is received and processed from the connected proxyAddress.
        /// </summary>
        public event EventHandler OnPacketProcessed;

        /// <summary>
        /// Triggered whenever a packet is created and sent to all connected slaves.
        /// </summary>
        public event EventHandler OnPacketGenerated;

        /// <summary>
        /// Triggered whenever a packet from the connected proxyAddress is forwarded to all connected slaves.
        /// </summary>
        public event EventHandler OnPacketForwarded;

        /// <summary>
        /// Creates a master with no specified masterAddress and masterPort. Address is localhost. Port will be generated when the master is started.
        /// </summary>
        public CameraMaster() {
            Rotation = new Rotation();
            Position = new Vector3(128f, 128f, 24f);
            Rotation.OnChange += RotationChanged;
            OnSlaveConnected += (source, args) => {
                CreatePacket();
            };
        }

        /// <param name="masterPort">Create a master with the masterPort that slaves can use to connect to this master specified. Address is localhost. Clients can connect on localhost with a random masterPort that is specified when the proxy is started.</param>
        public CameraMaster(int masterPort) : this() {
            StartMaster(masterPort);
        }

        /// <summary>
        /// Create a master with an masterAddress and masterPort specified for clients to connect to. Slaves can connect using localhost and a random masterPort selected when the master is started.
        /// </summary>
        /// <param name="loginURI">The address of the server the proxy is proxying.</param>
        /// <param name="proxyPort">The masterPort that clients should use to connect to this proxy.</param>
        public CameraMaster(string loginURI, int proxyPort) : this() {
            StartProxy(loginURI, proxyPort);
        }

        /// <summary>
        /// Create a master with an masterAddress and masterPort specified for clients to connect to and a masterPort specified for slaves to connect to.
        /// </summary>
        /// <param name="loginURI">The address of the server the proxy is proxying.</param>
        /// <param name="proxyPort">The masterPort that clients should use to connect to this proxy.</param>
        /// <param name="masterPort">The masterPort that slaves should use to connect to this proxy.</param>
        public CameraMaster(string loginURI, int proxyPort, int masterPort) : this() {
            StartProxy(loginURI, proxyPort);
            StartMaster(masterPort);
        }

        /// <summary>
        /// Rotation of the camera.
        /// </summary>
        public Rotation Rotation {
            get;
            set;
        }

        /// <summary>
        /// How many packets have been received from the connected proxyAddress.
        /// </summary>
        public int PacketsReceieved {
            get { return packetsReceived; }
        }

        /// <summary>
        /// How many packets received from the connected proxyAddress were forwarded to all slaves.
        /// </summary>
        public int PacketsForwarded {
            get { return packetsForwarded; }
        }

        /// <summary>
        /// How many packets were generated and sent to all connected slaves.
        /// </summary>
        public int PacketsGenerated {
            get { return PacketsGenerated; }
        }

        /// <summary>
        /// How many packets were recived and processed from the connected proxyAddress.
        /// </summary>
        public int PacketsProcessed {
            get { return packetsProccessed; }
        }

        /// <summary>
        /// Which packet types will be forwarded.
        /// </summary>
        public HashSet<PacketType> PacketTypesToForward {
            get { return packetTypesToForward; }
        }

        /// <summary>
        /// Positon of the camera.
        /// </summary>
        public Vector3 Position {
            get;
            set;
        }

        private void RotationChanged(object source, EventArgs args) {
            if (!processingPacket)
                CreatePacket();
        }

        private void CreatePacket() {
            AgentUpdatePacket p = (AgentUpdatePacket) Packet.BuildPacket(PacketType.AgentUpdate);
            p.AgentData.AgentID = UUID.Random();
            p.AgentData.BodyRotation = Quaternion.Identity;
            p.AgentData.CameraLeftAxis = Vector3.Cross(Vector3.UnitZ, Rotation.LookAtVector);
            p.AgentData.CameraUpAxis = Vector3.UnitZ;
            p.AgentData.HeadRotation = Quaternion.Identity;
            p.AgentData.SessionID = UUID.Random();

            p.AgentData.CameraCenter = Position;
            p.AgentData.CameraAtAxis = Position + Rotation.LookAtVector;
            masterServer.BroadcastPacket(p);
            packetsCreated++;
            if (OnPacketGenerated != null)
                OnPacketGenerated(p, null);
        }

        protected override Packet ReceiveIncomingPacket(Packet p, IPEndPoint ep) {
            return ReceivePacket(p, ep);
        }
        protected override Packet ReceiveOutgoingPacket(Packet p, IPEndPoint ep) {
            return ReceivePacket(p, ep);
        }
        private Packet ReceivePacket(Packet p, IPEndPoint ep) {
            packetsReceived++;
            bool processed = true;
            if (OnPacketReceived != null)
                OnPacketReceived(p, ep);
            if (p.Type == PacketType.AgentUpdate) {
                packetsForwarded++;
                masterServer.BroadcastPacket(p);
                if (OnPacketForwarded != null)
                    OnPacketForwarded(this, null);
            } else if (packetTypesToForward.Contains(p.Type)) {
                packetsForwarded++;
                masterServer.BroadcastPacket(p);
                if (OnPacketForwarded != null)
                    OnPacketForwarded(p, null);
            } else {
                packetsProccessed++;
                processed = false;
            }

            if (processed && OnPacketProcessed != null)
                OnPacketProcessed(this, null);
            return p;
        }
    }
}
