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
using Chimera;
using Nini.Config;

namespace UtilLib {
    public class CameraMaster : Master {
        /// <summary>
        /// How often updates are to be generated (ms).
        /// </summary>
        public static uint UPDATE_FREQ = 20;
        private int packetsReceived;
        private int packetsForwarded;
        private int packetsGenerated;
        private int packetsProccessed;
        private uint localID;
        private bool processingPacket;
        private bool viewerControl;
        private Vector3 mWorldPosition;
        private Rotation mWorldRotation;
        private Vector3 mPositionOffset;
        private Rotation mRotationOffset = new Rotation();
        private Vector3 mPositionDelta;
        private Vector3 mRotationDelta;
        private Vector3 cameraOffset = new Vector3(0f, 0f, 0f);

        private readonly HashSet<PacketType> packetTypesToForward = new HashSet<PacketType>();

        /// <summary>
        /// Selected whenever a packet is received from the connected proxyAddress.
        /// </summary>
        public event PacketDelegate OnPacketReceived;

        /// <summary>
        /// Selected whenever a packet is received and processed from the connected proxyAddress.
        /// </summary>
        public event EventHandler OnPacketProcessed;

        /// <summary>
        /// Selected whenever a packet is created and sent to all connected slaves.
        /// </summary>
        public event EventHandler OnSlavesUpdated;

        /// <summary>
        /// Selected whenever a packet from the connected proxyAddress is forwarded to all connected slaves.
        /// </summary>
        public event EventHandler OnPacketForwarded;

        /// <summary>
        /// Selected whenever the camera is updated, either by a received packet or by a direct change in value.
        /// </summary>
        public event EventHandler OnCameraUpdated;

        /// <summary>
        /// Creates a master with no specified masterAddress and masterPort. Address is localhost. Port will be generated when the master is started.
        /// </summary>
        public CameraMaster(Init.Config config) : base(config) {
            mPositionOffset = Init.GetV(config.SectionConfig, "PositionOffset", Vector3.Zero);
            mRotationOffset.Pitch = Init.Get(config.SectionConfig, "RotationOffsetPitch", 0f);
            mRotationOffset.Yaw = Init.Get(config.SectionConfig, "RotationOffsetYaw", 0f);

            mWorldRotation = new Rotation(config.WorldPitch, config.WorldYaw);

            Update(config.WorldPosition, Vector3.Zero, mWorldRotation.LookAtVector, Vector3.Zero);
            OnSlaveConnected += (slave) => {
                slave.Window.OnChange += (source, args) => {
                    masterServer.Send(mWorldPosition, mPositionDelta, mWorldRotation, mRotationDelta, slave.Window, slave.EP);
                    if (OnCameraUpdated != null)
                        OnCameraUpdated(this, null);
                };
                NotifySlaves();
            };

            Window.OnChange += (source, args) => {
                lock (slaves)
                    foreach (var slave in slaves.Values) 
                        masterServer.Send(mWorldPosition, mPositionDelta, mWorldRotation, mRotationDelta, slave.Window, slave.EP);

                GenerateMasterPacket();
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
        /// If true then input from the proxied viewer software will be used to control position in the virtual world.
        /// </summary>
        public bool ViewerControl {
            get { return viewerControl; }
            set { viewerControl = value; }
        }

        /// <summary>
        /// Positon of the camera.
        /// </summary>
        public Vector3 Position {
            get { return mWorldPosition; }
            /*set {
                mWorldPosition = value;
                if (!processingPacket)
                    NotifySlaves();
            }*/
        }

        public Vector3 PositionOffset {
            get { return mPositionOffset; }
            set { mPositionOffset = value; }
        }

        public Rotation RotationOffset {
            get { return mRotationOffset; }
        }

        /*
        public void SetPosition(Vector3 position, Vector3 positionDelta) {
            mWorldPosition = position;
            mPositionDelta = positionDelta;
        }*/

        /// <summary>
        /// The rotation of the camera.
        /// </summary>
        public Vector3 LookAt {
            get { return mWorldRotation.LookAtVector; }
            /*set {
                if (worldRotation != null)
                    worldRotation.OnChange -= RotationChanged;
                worldRotation = value;
                worldRotation.OnChange += RotationChanged;
                RotationChanged(worldRotation, null);
            }*/
        }

        public void Update(Vector3 position, Vector3 positionDelta, Vector3 lookAt, Vector3 lookAtDelta) {
            mWorldPosition = position;
            mPositionDelta = positionDelta;
            mWorldRotation.LookAtVector = lookAt;
            mRotationDelta = lookAtDelta;

            NotifySlaves();
        }

        /*
        public void SetRotation(float pitch, float yaw, float pitchDelta, float yawDelta) {
            mWorldRotation.Pitch = pitch;
            mWorldRotation.Yaw = yaw;
            mRotationDelta = new Rotation(pitchDelta, yawDelta).LookAtVector;

        }*/

        /*
        public void SetRotation(Vector3 lookAt, Vector3 lookAtDelta) {
            mWorldRotation.LookAtVector = lookAt;
            mRotationDelta = lookAtDelta;
        }

        public void SetRotation(Quaternion rotation, Quaternion rotationDelta) {
            mWorldRotation.Quaternion = rotation;
            mRotationDelta = new Rotation(rotationDelta).LookAtVector;
        }*/

        /*
        private void RotationChanged(object source, EventArgs args) {
            if (!processingPacket)
                NotifySlaves();
        }*/

        private void NotifySlaves() {
            lock (slaves) {
                foreach (var slave in slaves.Values) {
                    masterServer.Send(mWorldPosition, mPositionDelta, mWorldRotation, mRotationDelta, slave.Window, slave.EP);
                }
            }

            Logger.Debug("Master created Notified all slaves of a change and broadcast it to " + slaves.Count + " slaves.");

            GenerateMasterPacket();

            packetsGenerated++;
            if (OnSlavesUpdated != null)
                OnSlavesUpdated(this, null);
            if (OnCameraUpdated != null)
                OnCameraUpdated(this, null);
        }

        private float vanishingDistance = 512f;
        private bool updateFrustum;
        private bool updateRotation;
        private bool updateEyeOffset;
        private bool updateFoV;
        private bool updateAspectRatio;

        private CameraControlEnum cameraControl = CameraControlEnum.None;

        public enum CameraControlEnum { Individual, Frustum, None }

        public CameraControlEnum CameraControl {
            get { return cameraControl; }
            set { 
                cameraControl = value;
                if (!ProxyRunning)
                    return;

                if (value == CameraMaster.CameraControlEnum.Frustum) {
                    //clientProxy.InjectPacket(new SetCameraPropertiesPacket(false), Direction.Incoming);
                    GenerateMasterPacket();
                } else if (value == CameraControlEnum.Individual) {
                    //clientProxy.InjectPacket(new SetFrustumPacket(false), Direction.Incoming);
                    CameraChange();
                } else if (value == CameraMaster.CameraControlEnum.None) {
                    //clientProxy.InjectPacket(new SetCameraPropertiesPacket(false), Direction.Incoming);
                    //clientProxy.InjectPacket(new SetFrustumPacket(false), Direction.Incoming);
                    clientProxy.InjectPacket(new ClearFollowCamPropertiesPacket(), Direction.Incoming);
                }
            }
        }

        public float VanishingDistance {
            get { return vanishingDistance; }
            set {
                vanishingDistance = value;
                GenerateMasterPacket();
            }
        }
        public bool UpdateFrustum {
            get { return updateFrustum; }
            set { 
                updateFrustum = value; 
                GenerateMasterPacket();
            }
        }
        public bool UpdateFoV {
            get { return updateFoV; }
            set { 
                updateFoV = value;
                GenerateMasterPacket();
            }
        }
        public bool UpdateAspectRatio {
            get { return updateAspectRatio; }
            set { 
                updateAspectRatio = value; 
                GenerateMasterPacket();
            }
        }
        public bool UpdateRotation {
            get { return updateRotation; }
            set {
                updateRotation = value;
                CameraChange();
            }
        }
        public bool UpdateCameraPosition {
            get { return updateEyeOffset; }
            set {
                updateEyeOffset = value;
                CameraChange();
            }
        }

        private void CameraChange() {
            if (!ProxyRunning)
                return;

            if (!updateEyeOffset && !updateRotation)
                clientProxy.InjectPacket(new ClearFollowCamPropertiesPacket(), Direction.Incoming);

            GenerateMasterPacket();
        }

        private void GenerateMasterPacket() {
            if (!ProxyRunning)
                return;


            if (cameraControl == CameraControlEnum.Frustum) {
                //clientProxy.InjectPacket(Window.CreateSetFollowCamPropertiesPacket(mWorldPosition, mWorldRotation), Direction.Incoming);
                clientProxy.InjectPacket(Window.CreateWindowPacket(Position, Vector3.Zero, mWorldRotation, Vector3.Zero, UPDATE_FREQ), Direction.Incoming);
            }

            if (cameraControl == CameraControlEnum.Individual) {
                if (updateEyeOffset || updateRotation)
                    clientProxy.InjectPacket(Window.CreateSetFollowCamPropertiesPacket(mWorldPosition, mWorldRotation, updateEyeOffset, updateRotation), Direction.Incoming);
                //clientProxy.InjectPacket(Window.CreateWindowPacket(Position, Vector3.Zero, mWorldRotation, Vector3.Zero, UPDATE_FREQ), Direction.Incoming);
            }
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
            if (p.Type == PacketType.AgentUpdate && cameraControl == CameraControlEnum.None) {
                ProcessAgentUpdatePacket(p);
            } else if (p.Type == PacketType.ObjectUpdate) {
                ProcessObjectUpdatePacket(p);
            } else if (p.Type == PacketType.ImprovedTerseObjectUpdate && cameraControl != CameraControlEnum.None) {
                ProcessImprovedTerseObjectUpdatePacket(p);
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
        
        private void ProcessObjectUpdatePacket(Packet p) {
            if (!viewerControl)
                return;
            ObjectUpdatePacket packet = (ObjectUpdatePacket)p;
            foreach (var block in packet.ObjectData) {
                if (block.FullID == AgentID)
                    localID = block.ID;
            }
        }
        private void ProcessImprovedTerseObjectUpdatePacket(Packet p) {
            if (!viewerControl)
                return;
            ImprovedTerseObjectUpdatePacket packet = (ImprovedTerseObjectUpdatePacket)p;

            foreach (var block in packet.ObjectData) {
                int i = 4;
                // LocalID
                uint localID = Utils.BytesToUInt(block.Data, 0);
                if (localID != this.localID)
                    continue;
                // DebugState
                byte point = block.Data[i++];
                // Avatar boolean
                bool isAvatar = (block.Data[i++] != 0);
                if (!isAvatar)
                    continue;
                // Collision normal for avatar
                Vector4 collisionNormal = new Vector4(block.Data, i);
                if (isAvatar) {
                    collisionNormal = new Vector4(block.Data, i);
                    i += 16;
                }
                // Position
                Vector3 newPosition = new Vector3(block.Data, i);
                i += 12;
                // Velocity
                Vector3 positionDelta = new Vector3(
                    Utils.UInt16ToFloat(block.Data, i, -128.0f, 128.0f),
                    Utils.UInt16ToFloat(block.Data, i + 2, -128.0f, 128.0f),
                    Utils.UInt16ToFloat(block.Data, i + 4, -128.0f, 128.0f));
                i += 6;
                // Acceleration
                new Vector3(
                    Utils.UInt16ToFloat(block.Data, i, -64.0f, 64.0f),
                    Utils.UInt16ToFloat(block.Data, i + 2, -64.0f, 64.0f),
                    Utils.UInt16ToFloat(block.Data, i + 4, -64.0f, 64.0f));
                i += 6;
                // Rotation (theta)
                Quaternion newRotation = new Quaternion(
                    Utils.UInt16ToFloat(block.Data, i, -1.0f, 1.0f),
                    Utils.UInt16ToFloat(block.Data, i + 2, -1.0f, 1.0f),
                    Utils.UInt16ToFloat(block.Data, i + 4, -1.0f, 1.0f));
                i += 8;
                // Angular velocity (omega)
                    Vector3 lookAtDelta = new Vector3(
                    Utils.UInt16ToFloat(block.Data, i, -64.0f, 64.0f),
                    Utils.UInt16ToFloat(block.Data, i + 2, -64.0f, 64.0f),
                    Utils.UInt16ToFloat(block.Data, i + 4, -64.0f, 64.0f));


                processingPacket = true;

                newPosition += cameraOffset * newRotation;
                Vector3 lookAt = Vector3.UnitX * newRotation;
                Update(newPosition, positionDelta, lookAt, lookAtDelta);

                //if (!Equal(newPosition, mWorldPosition) && !Equal(newPosition, ModifiedPosition))
                //if (!Equal(lookAt, mWorldRotation.LookAtVector) && !Equal(lookAt, ModifiedRotation.LookAtVector))

                processingPacket = false;

                NotifySlaves();
            }
        }

        private void ProcessAgentUpdatePacket(Packet p) {
            if (!viewerControl)
                return;
            AgentUpdatePacket packet = (AgentUpdatePacket)p;

            processingPacket = true;
            Rotation rot = new Rotation(packet.AgentData.CameraAtAxis);
            rot.Yaw += mRotationOffset.Yaw;
            rot.Pitch += mRotationOffset.Pitch;
            Vector3 pos = packet.AgentData.CameraCenter + (mPositionOffset * rot.Quaternion);
            Update(pos, Vector3.Zero, rot.LookAtVector, Vector3.Zero);
            //bool posEq = Equal(worldPosition, packet.AgentData.CameraCenter);
            //bool modEq = Equal(packet.AgentData.CameraCenter, ModifiedPosition);
            //if (!Equal(worldPosition,packet.AgentData.CameraCenter) && !Equal(packet.AgentData.CameraCenter, ModifiedPosition))
                //SetPosition(packet.AgentData.CameraCenter, Vector3.Zero);
            //if (!Equal(worldRotation.LookAtVector, packet.AgentData.CameraAtAxis) && !Equal(packet.AgentData.CameraAtAxis, Rotation.LookAtVector))
                //SetRotation(packet.AgentData.CameraAtAxis, Vector3.Zero);
            processingPacket = false;

            //packetsForwarded++;
            NotifySlaves();
            if (OnPacketForwarded != null)
                OnPacketForwarded(this, null);
            if (OnCameraUpdated != null)
                OnCameraUpdated(this, null);
        }

        private bool Equal(Vector3 v1, Vector3 v2) {
            return Math.Abs(v1.X - v2.X) < TOLERANCE && Math.Abs(v1.Y - v2.Y) < TOLERANCE && Math.Abs(v1.Z - v2.Z) < TOLERANCE;
        }

        private static readonly double TOLERANCE = .001f;


        internal void InjectPacket(Packet packet, Direction direction) {
            if (ProxyRunning)
                clientProxy.InjectPacket(packet, direction);
        }

        public Vector3 ModifiedPosition {
            get { return Position + ((Window.EyePosition * mWorldRotation.Quaternion) / 1000f); }
        }

        public Rotation ModifiedRotation {
            get { return new Rotation(mWorldRotation.Pitch + Window.RotationOffset.Pitch, mWorldRotation.Yaw + Window.RotationOffset.Yaw); }
        }

        /// <summary>
        /// Where the camera should be located relative to the avatar before any transformations are applied.
        /// Measured in virtual world units (m).
        /// </summary>
        public Vector3 CameraOffset {
            get { return cameraOffset; }
            set {
                mWorldPosition -= cameraOffset * mWorldRotation.Quaternion;
                cameraOffset = value;
                mWorldPosition += cameraOffset * mWorldRotation.Quaternion;
                NotifySlaves();
            }
        }
    }
}
