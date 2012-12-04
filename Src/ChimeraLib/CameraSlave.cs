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
        /// Triggered whenever the slave connects to the master.
        /// </summary>
        public event EventHandler OnConnectedToMaster;

        /// <summary>
        /// Triggered if the slave is unable to connect to the master.
        /// </summary>
        public event EventHandler OnUnableToConnectToMaster;

        /// <summary>
        /// Triggered whenever the master signals for the client to disconnect.
        /// </summary>
        public event Action OnDisconnectedFromMaster;

        private Vector3 position;
        private readonly Rotation rotation;
        private readonly InterProxyClient interProxyClient;

        public CameraSlave() : this("Slave " + (SlaveCount + 1)) { }

        public CameraSlave(string name) : this (name, new InterProxyClient(name)) { }

        public CameraSlave(string name, Init.Config config) : this(name, new InterProxyClient(name), config) { }

        public CameraSlave(InterProxyClient client) : this ("Slave " + (SlaveCount + 1), client)  { }

        public CameraSlave(string name, InterProxyClient client) : this (name, client, null) { }

        public CameraSlave(string name, InterProxyClient client, Init.Config config) : base (config, LogManager.GetLogger(name)) { 
            client.Name = name;

            position = new Vector3(128f, 128f, 24f);
            rotation = new Rotation();            

            controlCamera = config.ControlCamera;

            OnClientLoggedIn += (source, args) => {
                if (controlCamera)
                    InjectPacket();
            };

            interProxyClient = client;
            interProxyClient.OnPacketReceived += (p, ep) => {
                if (p.Type == PacketType.AgentUpdate) {
                    AgentUpdatePacket ap = (AgentUpdatePacket)p;
                    position = ap.AgentData.CameraCenter;
                    rotation.LookAtVector = ap.AgentData.CameraAtAxis;
                    Update();
                    if (OnUpdateReceivedFromMaster != null)
                        OnUpdateReceivedFromMaster(ap.AgentData.CameraCenter, ap.AgentData.CameraAtAxis);
                }
                return p;
            };
            interProxyClient.OnDisconnected += (source, args) => {
                if (OnDisconnectedFromMaster != null)
                    OnDisconnectedFromMaster();
            };
            interProxyClient.OnConnected += (source, args) => {
                if (OnConnectedToMaster != null)
                    OnConnectedToMaster(source, args);
            };
            interProxyClient.OnUnableToConnect += (source, args) => {
                if (OnUnableToConnectToMaster != null)
                    OnUnableToConnectToMaster(source, args);
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

        public string MasterAddress {
            get { return interProxyClient.MasterAddress; }
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
        /// The postion after the offset has been applied to the source value.
        /// </summary>
        public Vector3 Position {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// The rotation after the offset has been applied to the source value.
        /// </summary>
        public Rotation Rotation {
            get { return rotation; }
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

        private void Update() {
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

            Vector3 focus = Position + Rotation.LookAtVector;
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
            packet.CameraProperty[13].Value = Position.X;
            packet.CameraProperty[14].Value = Position.Y;
            packet.CameraProperty[15].Value = Position.Z;
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
                OnUpdateSentToClient(Position, Rotation.LookAtVector);
        }
    }
}
