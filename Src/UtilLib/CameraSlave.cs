using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse.Packets;
using OpenMetaverse;
using log4net;

namespace UtilLib {
    public class CameraSlave : ProxyManager {
        private static int SlaveCount = 0;
        private bool controlCamera = true;
        private int injectedPackets = 0;

        /// <summary>
        /// Triggered whenever a camera update is received from the master.
        /// </summary>
        public event Action<Vector3, Vector3> OnUpdateReceivedFromMaster;

        /// <summary>
        /// Triggered whenever a camera update is sent to the client.
        /// </summary>
        public event System.Action<Vector3, Vector3> OnUpdateSentToClient;

        /// <summary>
        /// Triggered whenever the master signals for the client to disconnect.
        /// </summary>
        public event Action OnMasterDisconnected;

        private Vector3 masterPosition;
        private Rotation masterRotation;
        private Vector3 offsetPosition;
        private Rotation offsetRotation;
        private Vector3 finalPosition;
        private readonly Rotation finalRotation;
        private readonly InterProxyClient interProxyClient;

        public CameraSlave() : this("Slave " + (SlaveCount + 1)) { }

        public CameraSlave(string name) : this (name, new InterProxyClient(name)) { }

        public CameraSlave(string name, Init.Config config) : this(name, new InterProxyClient(name), config) { }

        public CameraSlave(InterProxyClient client) : this ("Slave " + (SlaveCount + 1), client)  { }

        public CameraSlave(string name, InterProxyClient client) : this (name, client, null) { }

        public CameraSlave(string name, InterProxyClient client, Init.Config config) : base (config, LogManager.GetLogger(name)) { 
            client.Name = name;

            finalPosition = MasterPosition;
            finalRotation = new Rotation();            
            
            offsetPosition = Vector3.Zero;
            OffsetRotation = new Rotation();

            masterPosition = new Vector3(128f, 128f, 24f);
            MasterRotation = new Rotation();

            controlCamera = config.ControlCamera;

            OnClientLoggedIn += (source, args) => {
                if (controlCamera)
                    InjectPacket();
            };

            interProxyClient = client;
            interProxyClient.OnPacketReceived += (p, ep) => {
                if (p.Type == PacketType.AgentUpdate) {
                    AgentUpdatePacket ap = (AgentUpdatePacket)p;
                    masterPosition = ap.AgentData.CameraCenter;
                    masterRotation.LookAtVector = ap.AgentData.CameraAtAxis;
                    Recalculate();
                    if (OnUpdateReceivedFromMaster != null)
                        OnUpdateReceivedFromMaster(ap.AgentData.CameraCenter, ap.AgentData.CameraAtAxis);
                }
                return p;
            };
            interProxyClient.OnDisconnected += (source, args) => {
                if (OnMasterDisconnected != null)
                    OnMasterDisconnected();
            };
            SlaveCount++;
        }

        /// <summary>
        /// How many packets the slave has received from the master.
        /// </summary>
        public int PacketsReceived {
            get { return interProxyClient.ReceivedPackets; }
        }

        /// <summary>
        /// How many packets the slave has sent to the client.
        /// </summary>
        public int PacketsInjected {
            get { return injectedPackets; }
        }

        /// <summary>
        /// True if the client is connected to the server.
        /// </summary>
        public bool ConnectedToMaster {
            get { return interProxyClient.Connected; }
        }

        /// <summary>
        /// The name that this proxy will show up as to the master.
        /// </summary>
        public string Name {
            get { return interProxyClient.Name; }
            set { 
                interProxyClient.Name = value;
                Logger = LogManager.GetLogger(value);
            }
        }

        /// <summary>
        /// Vector supplied for the camera position.
        /// </summary>
        public Vector3 MasterPosition {
            get { return masterPosition; }
            set {
                masterPosition = value;
                Recalculate();
            }
        }

        /// <summary>
        /// MasterRotation supplied for the camera rotation.
        /// </summary>
        public Rotation MasterRotation {
            get { return masterRotation; }
            set {
                if (masterRotation != null)
                    masterRotation.OnChange -= RotationChanged;
                masterRotation = value;
                masterRotation.OnChange += RotationChanged;
            }
        }

        /// <summary>
        /// Vector to be added to any position vector to get the new camera position.
        /// </summary>
        public Vector3 OffsetPosition {
            get { return offsetPosition; 
            }
            set {
                offsetPosition = value;
                Recalculate();
            }
        }

        /// <summary>
        /// Rotation to rotate the camera rotation around.
        /// </summary>
        public Rotation OffsetRotation {
            get { return offsetRotation; }
            set {
                if (offsetRotation != null)
                    offsetRotation.OnChange -= RotationChanged;
                offsetRotation = value;
                offsetRotation.OnChange += RotationChanged;
            }
        }

        /// <summary>
        /// The postion after the offset has been applied to the source value.
        /// </summary>
        public Vector3 FinalPosition {
            get { return finalPosition; }
        }

        /// <summary>
        /// The rotation after the offset has been applied to the source value.
        /// </summary>
        public Rotation FinalRotation {
            get { return finalRotation; }
        }

        /// <summary>
        /// Whether to send packets to the client to control the camera.
        /// </summary>
        public bool ControlCamera {
            get { return controlCamera; }
            set {
                controlCamera = value;
                InjectPacket(value ? 1f : 0f);
            }
        }

        private void RotationChanged(object source, EventArgs args) {
            Recalculate();
        }

        public bool Connect(int port) {
            ProxyConfig.MasterPort = port;
            return Connect();
        }
        public bool Connect() {
            return interProxyClient.Connect(ProxyConfig.MasterAddress, ProxyConfig.MasterPort);
        }

        public void Disconnect() {
            interProxyClient.Disconnect();
        }

        public void StopSlave() {
            interProxyClient.Stop();
        }

        public override void Stop() {
            base.Stop();
            StopSlave();
        }
    
        protected override OpenMetaverse.Packets.Packet ReceiveOutgoingPacket(OpenMetaverse.Packets.Packet p, System.Net.IPEndPoint ep) {
            return p;
        }

        protected override OpenMetaverse.Packets.Packet ReceiveIncomingPacket(OpenMetaverse.Packets.Packet p, System.Net.IPEndPoint ep) {
            return p;
        }

        private void Recalculate() {
            FinalRotation.Yaw = MasterRotation.Yaw + OffsetRotation.Yaw;
            FinalRotation.Pitch = MasterRotation.Pitch + OffsetRotation.Pitch;
            finalPosition = MasterPosition + (OffsetPosition * MasterRotation.Quaternion);
            //FinalPosition = SourcePosition;
            if (controlCamera)
                InjectPacket();
        }

        private void InjectPacket() {
            InjectPacket(1f);
        }

        private void InjectPacket(float enable) {
            SetFollowCamPropertiesPacket packet = new SetFollowCamPropertiesPacket();
            packet.CameraProperty = new SetFollowCamPropertiesPacket.CameraPropertyBlock[22];
            for (int i = 0; i < 22; i++) {
                packet.CameraProperty[i] = new SetFollowCamPropertiesPacket.CameraPropertyBlock();
                packet.CameraProperty[i].Type = i + 1;
            }

            Vector3 focus = FinalPosition + FinalRotation.LookAtVector;
            packet.CameraProperty[0].Value = 0;
            packet.CameraProperty[1].Value = 0f;
            packet.CameraProperty[2].Value = 0f;
            packet.CameraProperty[3].Value = 0f;
            packet.CameraProperty[4].Value = 0f;
            packet.CameraProperty[5].Value = 0f;
            packet.CameraProperty[6].Value = 0f;
            packet.CameraProperty[7].Value = 0f;
            packet.CameraProperty[8].Value = 0f;
            packet.CameraProperty[9].Value = 0f;
            packet.CameraProperty[10].Value = 0f;
            packet.CameraProperty[11].Value = enable;
            packet.CameraProperty[12].Value = 0f;
            packet.CameraProperty[13].Value = FinalPosition.X;
            packet.CameraProperty[14].Value = FinalPosition.Y;
            packet.CameraProperty[15].Value = FinalPosition.Z;
            packet.CameraProperty[16].Value = 0f;
            packet.CameraProperty[17].Value = focus.X;
            packet.CameraProperty[18].Value = focus.Y;
            packet.CameraProperty[19].Value = focus.Z;
            packet.CameraProperty[20].Value = 1f;
            packet.CameraProperty[21].Value = 1f;

            if (clientProxy != null) {
                clientProxy.InjectPacket(packet, GridProxy.Direction.Incoming);
                injectedPackets++;
            }

            if (OnUpdateSentToClient != null)
                OnUpdateSentToClient(FinalPosition, FinalRotation.LookAtVector);
        }
    }
}
