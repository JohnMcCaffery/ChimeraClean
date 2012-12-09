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
using ChimeraLib;
using GridProxy;
using System.Net;

namespace UtilLib {
    public class CameraSlave : ProxyManager {
        private static int SlaveCount = 0;
        private bool controlCamera = true;
        private int injectedPackets = 0;
        private Window window;

        /// <summary>
        /// Triggered whenever a camera update is received from the master.
        /// </summary>
        public event Action<Vector3, Vector3> OnUpdateReceivedFromMaster;

        /// <summary>
        /// Triggered whenever a camera update is sent to the client.
        /// </summary>
        public event Action<Vector3, Vector3> OnUpdateSentToClient;

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
        private Rotation rotation;
        private readonly InterProxyClient interProxyClient;

        public CameraSlave() : this("Slave " + (SlaveCount + 1)) { }

        public CameraSlave(string name) : this (name, new InterProxyClient(name)) { }

        public CameraSlave(string name, Init.Config config) : this(name, new InterProxyClient(name), config) { }

        public CameraSlave(InterProxyClient client) : this ("Slave " + (SlaveCount + 1), client)  { }

        public CameraSlave(string name, InterProxyClient client) : this (name, client, null) { }

        public CameraSlave(string name, InterProxyClient client, Init.Config config) : base (config, LogManager.GetLogger(name)) { 
            client.Name = name;

            position = new Vector3(128f, 128f, 24f);
            Rotation = new Rotation();
            Window = new Window(name);

            controlCamera = config.ControlCamera;

            OnClientLoggedIn += (source, args) => {
                if (controlCamera)
                    InjectPacket();
            };

            interProxyClient = client;
            interProxyClient.OnPacketReceived += ProcessPacket;
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


        private Packet ProcessPacket(Packet p, IPEndPoint ep) {
            if (p.Type == PacketType.AgentUpdate) {
                AgentUpdatePacket ap = (AgentUpdatePacket)p;
                position = ap.AgentData.CameraCenter;
                rotation.LookAtVector = ap.AgentData.CameraAtAxis;
                Update();
                if (OnUpdateReceivedFromMaster != null)
                    OnUpdateReceivedFromMaster(ap.AgentData.CameraCenter, ap.AgentData.CameraAtAxis);
            } else if (p.Type == PacketType.SetCameraProperties) {
                SetCameraPropertiesPacket cp = (SetCameraPropertiesPacket)p;
                window.ScreenPosition = new Vector3(cp.CameraProperty.FrustumOffsetH, cp.CameraProperty.FrustumOffsetV, window.ScreenPosition.Z) * 100;
                window.Width = cp.CameraProperty.FrustumOffsetH * 100;
                window.Height = cp.CameraProperty.FrustumOffsetV * 100;
                window.FieldOfView = cp.CameraProperty.CameraAngle;
            }
            return p;
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
        /// The avatar position in virtual space.
        /// </summary>
        public Vector3 Position {
            get { return position; }
            set { 
                position = value;
                Update();
            }
        }

        /// <summary>
        /// The avatar rotation in virtual space.
        /// </summary>
        public Rotation Rotation {
            get { return rotation; }
            set {
                if (rotation != null)
                    rotation.OnChange -= ValueChanged;
                rotation = value;
                rotation.OnChange += ValueChanged;
                ValueChanged(rotation, null);
            }
        }

        /// <summary>
        /// The window which defines the position of the screen this slave projects onto in real space.
        /// </summary>
        public Window Window {
            get { return window; }
            set {
                if (window != null)
                    window.OnChange -= ValueChanged;
                window = value;
                window.OnChange += ValueChanged;
                ValueChanged(window, null);
            }
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

        private void ValueChanged (object source, EventArgs args) {
            Update();
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
            if (window == null)
                return;

            if (clientProxy != null) {
                SetFollowCamPropertiesPacket cameraPacket = new SetFollowCamPropertiesPacket();
                cameraPacket.CameraProperty = new SetFollowCamPropertiesPacket.CameraPropertyBlock[22];
                for (int i = 0; i < 22; i++) {
                    cameraPacket.CameraProperty[i] = new SetFollowCamPropertiesPacket.CameraPropertyBlock();
                    cameraPacket.CameraProperty[i].Type = i + 1;
                }

                Rotation finalRot = rotation + window.RotationOffset;
                Vector3 finalPos = position + (window.EyePosition / 1000f);
                Vector3 lookAt = finalPos + finalRot.LookAtVector;
                cameraPacket.CameraProperty[0].Value = 0;
                cameraPacket.CameraProperty[1].Value = 0f;
                cameraPacket.CameraProperty[2].Value = 0f;
                cameraPacket.CameraProperty[3].Value = 0f;
                cameraPacket.CameraProperty[4].Value = 0f;
                cameraPacket.CameraProperty[5].Value = 0f;
                cameraPacket.CameraProperty[6].Value = 0f;
                cameraPacket.CameraProperty[7].Value = 0f;
                cameraPacket.CameraProperty[8].Value = 0f;
                cameraPacket.CameraProperty[9].Value = 0f;
                cameraPacket.CameraProperty[10].Value = 0f;
                cameraPacket.CameraProperty[11].Value = enable;
                cameraPacket.CameraProperty[12].Value = 0f;
                cameraPacket.CameraProperty[13].Value = finalPos.X;
                cameraPacket.CameraProperty[14].Value = finalPos.Y;
                cameraPacket.CameraProperty[15].Value = finalPos.Z;
                cameraPacket.CameraProperty[16].Value = 0f;
                cameraPacket.CameraProperty[17].Value = lookAt.X;
                cameraPacket.CameraProperty[18].Value = lookAt.Y;
                cameraPacket.CameraProperty[19].Value = lookAt.Z;
                cameraPacket.CameraProperty[20].Value = 1f;
                cameraPacket.CameraProperty[21].Value = 1f;

                SetCameraPropertiesPacket screenPacket = new SetCameraPropertiesPacket();
                screenPacket.CameraProperty = new SetCameraPropertiesPacket.CameraPropertyBlock();
                screenPacket.CameraProperty.FrustumOffsetH = (float)(window.FrustumOffsetH / window.Width);
                screenPacket.CameraProperty.FrustumOffsetV = (float)(window.FrustumOffsetV / window.Height);
                screenPacket.CameraProperty.CameraAngle = (float)window.FieldOfView;
                screenPacket.CameraProperty.AspectRatio = (float)(window.Width / window.Height);
                screenPacket.CameraProperty.AspectSet = true;

                clientProxy.InjectPacket(cameraPacket, Direction.Incoming);
                clientProxy.InjectPacket(screenPacket, Direction.Incoming);
                injectedPackets++;
            }

            if (OnUpdateSentToClient != null)
                OnUpdateSentToClient(Position, Rotation.LookAtVector);
        }
    }
}
