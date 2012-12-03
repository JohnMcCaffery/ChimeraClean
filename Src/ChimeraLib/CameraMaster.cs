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
using GridProxy;
using OpenMetaverse.Packets;
using System.Net;
using OpenMetaverse;

namespace UtilLib {
    public class CameraMaster : Master {
        private int packetsReceived;
        private int packetsForwarded;
        private int packetsGenerated;
        private int packetsProccessed;
        private bool processingPacket;
        private Vector3 position;
        private Rotation rotation;

        private readonly HashSet<PacketType> packetTypesToForward = new HashSet<PacketType>();

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
        /// Triggered whenever the camera is updated, either by a received packet or by a direct change in value.
        /// </summary>
        public event EventHandler OnCameraUpdated;

        /// <summary>
        /// Creates a master with no specified masterAddress and masterPort. Address is localhost. Port will be generated when the master is started.
        /// </summary>
        public CameraMaster(Init.Config config) : base(config) {
            Rotation = new Rotation();
            Position = new Vector3(128f, 128f, 24f);
            OnSlaveConnected += (slave) => {
                slave.OnChange += (source, args) => CreatePacket();
                CreatePacket();
            };
        }

        public CameraMaster() : this(null) { }

        /// <summary>
        /// How many packets have been received from the connected proxyAddress.
        /// </summary>
        public int PacketsReceived {
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
            get { return packetsGenerated; }
        }

        /// <summary>
        /// How many packets were recived and processed from the connected proxyAddress.
        /// </summary>
        public int PacketsProcessed {
            get { return packetsProccessed; }
        }

        /// <summary>
        /// Positon of the camera.
        /// </summary>
        public Vector3 Position {
            get { return position; }
            set {
                position = value;
                if (!processingPacket)
                    CreatePacket();
            }
        }

        /// <summary>
        /// The rotation of the camera.
        /// </summary>
        public Rotation Rotation {
            get { return rotation; }
            set {
                if (rotation != null)
                    rotation.OnChange -= RotationChanged;
                rotation = value;
                rotation.OnChange += RotationChanged;
                RotationChanged(rotation, null);
            }
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

            foreach (var slave in slaves.Values) {
                Rotation rot = new Rotation(Rotation.Pitch + slave.Window.RotationOffset.Pitch, Rotation.Yaw + slave.Window.RotationOffset.Yaw);
                p.AgentData.CameraAtAxis = rot.LookAtVector;
                p.AgentData.CameraCenter = Position + (slave.Window.EyeOffset * Rotation.Quaternion);
                masterServer.Send(p, slave.TargetEP);
            }

            Logger.Debug("Master created AgentUpdatePacket and broadcast it to " + slaves.Count + " slaves.");

            packetsGenerated++;
            if (OnPacketGenerated != null)
                OnPacketGenerated(p, null);
            if (OnCameraUpdated != null)
                OnCameraUpdated(this, null);
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
                ProcessAgentUpdatePacket(p);
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

        private void ProcessAgentUpdatePacket(Packet p) {
            AgentUpdatePacket packet = (AgentUpdatePacket)p;

            processingPacket = true;
            Position = packet.AgentData.CameraCenter;
            Rotation.LookAtVector = packet.AgentData.CameraAtAxis;
            processingPacket = false;

            //packetsForwarded++;
            CreatePacket();
            if (OnPacketForwarded != null)
                OnPacketForwarded(this, null);
            if (OnCameraUpdated != null)
                OnCameraUpdated(this, null);
        }

    }
}
