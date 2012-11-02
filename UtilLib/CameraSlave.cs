using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse.Packets;
using OpenMetaverse;

namespace UtilLib {
    public class CameraSlave : ProxyManager {

        public CameraSlave() {
            OffsetPosition = Vector3.Zero;
            OffsetRotation = new Rotation();

            SourcePosition = new Vector3(128f, 128f, 24f);
            SourceRotation = new Rotation();

            InterProxyClient = new InterProxyClient();
            OnClientLoggedIn += (source, args) => InjectPacket();
        }

        /// <summary>
        /// Vector to be added to any position vector to get the new camera position.
        /// </summary>
        public Vector3 OffsetPosition {
            get;
            set;
        }

        /// <summary>
        /// Rotation to rotate the camera rotation around.
        /// </summary>
        public Rotation OffsetRotation {
            get;
            set;
        }

        /// <summary>
        /// Vector supplied for the camera position.
        /// </summary>
        public Vector3 SourcePosition {
            get;
            set;
        }

        /// <summary>
        /// Rotation supplied for the camera rotation.
        /// </summary>
        public Rotation SourceRotation {
            get;
            set;
        }

        public InterProxyClient InterProxyClient {
            get;
            set;
        }

        public void Connect(int port) {
            InterProxyClient.OnPacketReceived += (p, ep) => {
                if (p.Type == PacketType.AgentUpdate) {
                    AgentUpdatePacket ap = (AgentUpdatePacket)p;
                    SourcePosition = ap.AgentData.CameraCenter;
                    SourceRotation.LookAtVector = ap.AgentData.CameraAtAxis;
                    InjectPacket();
                }
                return p;
            };
            InterProxyClient.Connect(port);
        }

        public void Stop() {
            StopProxy();
            if (InterProxyClient.Running)
                InterProxyClient.Stop();
        }
    
        protected override OpenMetaverse.Packets.Packet ReceiveOutgoingPacket(OpenMetaverse.Packets.Packet p, System.Net.IPEndPoint ep) {
            return p;
        }

        protected override OpenMetaverse.Packets.Packet ReceiveIncomingPacket(OpenMetaverse.Packets.Packet p, System.Net.IPEndPoint ep) {
            return p;
        }

        private void InjectPacket() {
            SetFollowCamPropertiesPacket packet = new SetFollowCamPropertiesPacket();
            packet.CameraProperty = new SetFollowCamPropertiesPacket.CameraPropertyBlock[22];
            for (int i = 0; i < 22; i++) {
                packet.CameraProperty[i] = new SetFollowCamPropertiesPacket.CameraPropertyBlock();
                packet.CameraProperty[i].Type = i + 1;
            }

            Rotation finalRotation = new Rotation();
            finalRotation.Yaw = SourceRotation.Yaw + OffsetRotation.Yaw;
            finalRotation.Pitch = SourceRotation.Pitch + OffsetRotation.Pitch;
            Vector3 position = SourcePosition + (OffsetPosition * SourceRotation.Rot);
            Vector3 focus = position + finalRotation.LookAtVector;

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
            packet.CameraProperty[13].Value = position.X;
            packet.CameraProperty[14].Value = position.Y;
            packet.CameraProperty[15].Value = position.Z;
            packet.CameraProperty[16].Value = 0f;
            packet.CameraProperty[17].Value = focus.X;
            packet.CameraProperty[18].Value = focus.Y;
            packet.CameraProperty[19].Value = focus.Z;
            packet.CameraProperty[20].Value = 1f;
            packet.CameraProperty[21].Value = 1f;

            clientProxy.InjectPacket(packet, GridProxy.Direction.Incoming);
        }
    }
}
