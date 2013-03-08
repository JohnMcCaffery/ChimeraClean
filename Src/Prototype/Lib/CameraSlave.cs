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
using Chimera;
using GridProxy;
using System.Net;

namespace UtilLib {
    public class CameraSlave : ProxyManager, IScreenListener {
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
        public event Action<Vector3, Vector3> OnUpdateSentToViewer;

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
        private bool useSetFollowCam;
        private bool enableWindow;
        private readonly InterProxyClient interProxyClient;

        public CameraSlave() : this("Slave " + (SlaveCount + 1)) { }

        public CameraSlave(string name) : this (name, new InterProxyClient(name)) { }

        public CameraSlave(string name, Init.Config config) : this(name, new InterProxyClient(name), config) { }

        public CameraSlave(InterProxyClient client) : this ("Slave " + (SlaveCount + 1), client)  { }

        public CameraSlave(string name, InterProxyClient client) : this (name, client, null) { }

        public CameraSlave(string name, InterProxyClient client, Init.Config config) : base (config, LogManager.GetLogger(name)) { 
            client.Name = name;

            position = new Vector3(128f, 128f, 24f);
            WorldRotation = new Rotation();
            Window = new Window(name);

            controlCamera = config.ControlCamera;
            useSetFollowCam = config.UseSetFollowCamPackets;
            enableWindow = config.EnableWindowPackets;

            OnClientLoggedIn += (source, args) => {
                if (controlCamera)
                    InjectPacket();
            };

            interProxyClient = client;
            interProxyClient.AddDataListener(this);
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

        private bool clearFollowCam;
        private bool clearWindow;

        public bool EnableWindowPackets {
            get { return enableWindow; }
            set {
                if (!value) {
                    useSetFollowCam = true;
                    clearWindow = false;
                }
                enableWindow = value;
                InjectPacket();
            }
        }

        public bool UseSetFollowCamPackets {
            get { return useSetFollowCam; }
            set { 
                clearWindow = !clearWindow && useSetFollowCam != value && value;
                clearFollowCam = !clearFollowCam && useSetFollowCam != value && !value;
                useSetFollowCam = value;
                InjectPacket();
            }
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
        public Vector3 WorldPosition {
            get { return position; }
            set { 
                position = value;
                Update();
            }
        }

        /// <summary>
        /// The avatar rotation in virtual space.
        /// </summary>
        public Rotation WorldRotation {
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
            if (window == null || clientProxy == null || !ProxyRunning)
                return;

            if (enable == 0f) {
                clientProxy.InjectPacket(new ClearFollowCamPropertiesPacket(), Direction.Incoming);
                if (enableWindow)
                    clientProxy.InjectPacket(new ClearWindowPacket(), Direction.Incoming);
            } else {
                if (useSetFollowCam) {
                    if (clearWindow && enableWindow)
                        clientProxy.InjectPacket(new ClearWindowPacket(), Direction.Incoming);
                    clientProxy.InjectPacket(window.CreateSetFollowCamPropertiesPacket(WorldPosition, WorldRotation), Direction.Incoming);
                } else {
                    if (clearFollowCam)
                        clientProxy.InjectPacket(new ClearFollowCamPropertiesPacket(), Direction.Incoming);
                    if (enableWindow)
                        clientProxy.InjectPacket(window.CreateWindowPacket(WorldPosition, WorldPositionDelta, WorldRotation, WorldRotationDelta, CameraMaster.UPDATE_FREQ), Direction.Incoming);
                }
            }

            if (OnUpdateSentToViewer != null)
                OnUpdateSentToViewer(WorldPosition, WorldRotation.LookAtVector);
        }


        public Vector3 WorldPositionDelta {
            get; set;
        }

        public Vector3 WorldRotationDelta {
            get; set;
        }
    }
}
