using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse.Packets;
using OpenMetaverse;

namespace UtilLib {
    public class CameraSlave : ProxyManager {
        private static int SlaveCount = 0;
        /// <summary>
        /// Triggered whenever a camera update is received from the master.
        /// </summary>
        public event Action<Vector3, Vector3> OnUpdateReceivedFromMaster;

        /// <summary>
        /// Triggered whenever a camera update is sent to the client.
        /// </summary>
        public event System.Action<Vector3, Vector3> OnUpdateSentToClient;

        private Vector3 masterPosition;
        private Rotation masterRotation;
        private Vector3 offsetPosition;
        private Rotation offsetRotation;
        private Vector3 finalPosition;
        private readonly Rotation finalRotation;
        private readonly InterProxyClient interProxyClient;

        public CameraSlave() : this("Slave " + (SlaveCount + 1), new InterProxyClient()) { }

        public CameraSlave(string name) : this (name, new InterProxyClient()) { }

        public CameraSlave(InterProxyClient client) : this ("Slave " + (SlaveCount + 1), client)  { }
        public CameraSlave(string name, InterProxyClient client) {
            client.Name = name;

            finalPosition = MasterPosition;
            finalRotation = new Rotation();            
            
            offsetPosition = Vector3.Zero;
            OffsetRotation = new Rotation();

            masterPosition = new Vector3(128f, 128f, 24f);
            MasterRotation = new Rotation();

            interProxyClient = client;
            OnClientLoggedIn += (source, args) => InjectPacket();
            SlaveCount++;
        }

        /// <summary>
        /// The name that this proxy will show up as to the master.
        /// </summary>
        public string Name {
            get { return interProxyClient.Name; }
            set { interProxyClient.Name = value; }
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

        public InterProxyClient InterProxyClient {
            get { return interProxyClient; }
        }

        private void RotationChanged(object source, EventArgs args) {
            Recalculate();
        }

        public bool Connect(int port) {
            InterProxyClient.OnPacketReceived += (p, ep) => {
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
            return InterProxyClient.Connect(port);
        }

        public void Stop() {
            StopProxy();
            InterProxyClient.Stop();
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
            InjectPacket();
        }

        private void InjectPacket() {
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
            packet.CameraProperty[11].Value = 1f;
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

            if (clientProxy != null)
                clientProxy.InjectPacket(packet, GridProxy.Direction.Incoming);

            if (OnUpdateSentToClient != null)
                OnUpdateSentToClient(FinalPosition, FinalRotation.LookAtVector);
        }
    }
}
