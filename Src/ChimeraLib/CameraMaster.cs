using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GridProxy;
using OpenMetaverse.Packets;
using System.Net;
using OpenMetaverse;
using ChimeraLib;

namespace UtilLib {
    public class CameraMaster : Master {
        private int packetsReceived;
        private int packetsForwarded;
        private int packetsGenerated;
        private int packetsProccessed;
        private uint localID;
        private bool processingPacket;
        private bool viewerControl;
        private Vector3 position;
        private Rotation rotation;
        private Vector3 cameraOffset = new Vector3(0f, 0f, 0f);

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

            Window.OnChange += (source, args) => {
                SetCameraPropertiesPacket screenPacket = new SetCameraPropertiesPacket();
                screenPacket.CameraProperty.CameraAngle = (float)Window.FieldOfView;
                screenPacket.CameraProperty.FrustumOffsetH = (float)(Window.ScreenPosition.X / Window.Width);
                screenPacket.CameraProperty.FrustumOffsetV = (float)(Window.ScreenPosition.Y / Window.Height);


                //Byte[] bytes = screenPacket.ToBytes();
                //int end = bytes.Length - 1;
                //SetCameraPropertiesPacket testPacket = (SetCameraPropertiesPacket) Packet.BuildPacket(bytes, ref end, new byte[1000]);
                foreach (var slave in slaves.Values) 
                    masterServer.Send(screenPacket, slave.TargetEP);

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
        /// If true then input from the proxied viewer software will be used to control position in the virtual world.
        /// </summary>
        public bool ViewerControl {
            get { return viewerControl; }
            set { viewerControl = value; }
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
            SetCameraPropertiesPacket screenPacket = new SetCameraPropertiesPacket();
            screenPacket.CameraProperty.CameraAngle = (float) Window.FieldOfView;
            screenPacket.CameraProperty.FrustumOffsetH = (float) (Window.ScreenPosition.X / Window.Width);
            screenPacket.CameraProperty.FrustumOffsetV = (float) (Window.ScreenPosition.Y / Window.Height);

            AgentUpdatePacket cameraPacket = (AgentUpdatePacket) Packet.BuildPacket(PacketType.AgentUpdate);
            cameraPacket.AgentData.AgentID = UUID.Random();
            cameraPacket.AgentData.BodyRotation = Quaternion.Identity;
            cameraPacket.AgentData.CameraLeftAxis = Vector3.Cross(Vector3.UnitZ, Rotation.LookAtVector);
            cameraPacket.AgentData.CameraUpAxis = Vector3.UnitZ;
            cameraPacket.AgentData.HeadRotation = Quaternion.Identity;
            cameraPacket.AgentData.SessionID = UUID.Random();

            foreach (var slave in slaves.Values) {
                Rotation rot = new Rotation(Rotation.Pitch + slave.Window.RotationOffset.Pitch, Rotation.Yaw + slave.Window.RotationOffset.Yaw);
                cameraPacket.AgentData.CameraAtAxis = rot.LookAtVector;
                cameraPacket.AgentData.CameraCenter = Position + (slave.Window.EyePosition * Rotation.Quaternion);
                masterServer.Send(cameraPacket, slave.TargetEP);
                masterServer.Send(screenPacket, slave.TargetEP);
            }

            Logger.Debug("Master created AgentUpdatePacket and broadcast it to " + slaves.Count + " slaves.");

            GenerateMasterPacket();

            packetsGenerated++;
            if (OnPacketGenerated != null)
                OnPacketGenerated(cameraPacket, null);
            if (OnCameraUpdated != null)
                OnCameraUpdated(this, null);
        }

        private bool updateRotation;
        private bool updateEyeOffset;
        private bool updateFrustumH;
        private bool updateFrustumV;
        private bool updateFrustumNear;
        private bool updateFoV;
        private bool updateAspectRatio;
        private bool updatingCamera;

        public bool UpdateFrustumH {
            get { return updateFrustumH; }
            set { 
                updateFrustumH = value; 
                GenerateMasterPacket();
            }
        }
        public bool UpdateFrustumV {
            get { return updateFrustumV; }
            set { 
                updateFrustumV = value; 
                GenerateMasterPacket();
            }
        }
        public bool UpdateFrustumNear {
            get { return updateFrustumNear; }
            set { 
                updateFrustumNear = value; 
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
        public bool UpdateEyeOffset {
            get { return updateEyeOffset; }
            set {
                updateEyeOffset = value;
                CameraChange();
            }
        }
        private void CameraChange() {
            if (!updateRotation && !updateEyeOffset && updatingCamera) {
                clientProxy.InjectPacket(new ClearFollowCamPropertiesPacket(), Direction.Incoming);
                updatingCamera = false;
            } else {
                updatingCamera = true;
                GenerateMasterPacket();
            }
        }
        private void GenerateMasterPacket() {
            if (!ProxyRunning)
                return;
            if (updatingCamera) {
                SetFollowCamPropertiesPacket cameraPacket = new SetFollowCamPropertiesPacket();
                cameraPacket.CameraProperty = new SetFollowCamPropertiesPacket.CameraPropertyBlock[22];
                for (int i = 0; i < 22; i++) {
                    cameraPacket.CameraProperty[i] = new SetFollowCamPropertiesPacket.CameraPropertyBlock();
                    cameraPacket.CameraProperty[i].Type = i + 1;
                }

                Rotation finalRot = rotation;
                if (updateRotation)
                    finalRot = new Rotation(rotation.Pitch + Window.RotationOffset.Pitch, rotation.Yaw + Window.RotationOffset.Yaw);
                Vector3 finalPos = position;
                if (updateEyeOffset) {
                    Vector3 rotatatedLookAt = ((Window.EyePosition * new Vector3(1f, -1f, 1f)) / 1000f) * rotation.Quaternion;
                    finalPos += rotatatedLookAt;
                }
                //Vector3 lookAt = finalPos + master.Rotation.LookAtVector;
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
                cameraPacket.CameraProperty[11].Value = 1f; //enable
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
                clientProxy.InjectPacket(cameraPacket, Direction.Incoming);
            }

            if (updateFrustumH || updateFrustumV || UpdateFrustumNear || updateFoV || updateAspectRatio) {
                /*
                Vector3 upperRight = new Vector3(0f, (float)(Window.Width / 2.0), (float)(Window.Height / 2.0));
                Vector3 lowerLeft = new Vector3(0f, (float)(Window.Width / -2.0), (float)(Window.Height / -2.0));
                Vector3 diff = Window.ScreenPosition - Window.EyePosition;

                upperRight += diff;
                lowerLeft += diff;

                upperRight /= diff.X;
                lowerLeft /= diff.X;

                float x1 = Math.Min(upperRight.Y, lowerLeft.Y);
                float x2 = Math.Max(upperRight.Y, lowerLeft.Y);
                float y1 = Math.Max(upperRight.Z, lowerLeft.Z);
                float y2 = Math.Min(upperRight.Z, lowerLeft.Z);

                double fovH = Math.Atan2(x2 - x1, 2 * Window.AspectRatio) * 2.0;
                double fovV = Math.Atan2(y1 - y2, 2) * 2.0;

                double frustumOffsetH = (x2 + x1) / (x2 - x1);
                double frustumOffsetV = (y1 + y2) / (y1 - y2);

                SetCameraPropertiesPacket screenPacket = new SetCameraPropertiesPacket();
                screenPacket.CameraProperty = new SetCameraPropertiesPacket.CameraPropertyBlock();
                screenPacket.CameraProperty.FrustumOffsetH = (float) frustumOffsetH;
                screenPacket.CameraProperty.FrustumOffsetV = (float) frustumOffsetV;
                screenPacket.CameraProperty.CameraAngle = (float) fovH;

                screenPacket.CameraProperty.AspectSet = false;
                screenPacket.CameraProperty.SetNear = false;
                */

                SetCameraPropertiesPacket screenPacket = new SetCameraPropertiesPacket();
                screenPacket.CameraProperty = new SetCameraPropertiesPacket.CameraPropertyBlock();
                screenPacket.CameraProperty.FrustumOffsetH = 0f;
                screenPacket.CameraProperty.FrustumOffsetV = 0f;
                screenPacket.CameraProperty.CameraAngle = 1f;

                screenPacket.CameraProperty.AspectRatio = (float)(Window.Width / Window.Height);
                screenPacket.CameraProperty.AspectSet = updateAspectRatio;

                screenPacket.CameraProperty.FrustumNear = 0f;
                screenPacket.CameraProperty.SetNear = false;

                if (updateFrustumH)
                    screenPacket.CameraProperty.FrustumOffsetH = (float)((2 * Window.FrustumOffsetH) / Window.Width);
                if (updateFrustumV)
                    screenPacket.CameraProperty.FrustumOffsetV = (float)((2 * Window.FrustumOffsetV) / Window.Height);
                if (updateFoV)
                    screenPacket.CameraProperty.CameraAngle = (float)Window.FieldOfView;

                clientProxy.InjectPacket(screenPacket, Direction.Incoming);
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
            if (p.Type == PacketType.AgentUpdate) {
                //ProcessAgentUpdatePacket(p);
            } else if (p.Type == PacketType.ObjectUpdate) {
                ProcessObjectUpdatePacket(p);
            } else if (p.Type == PacketType.ImprovedTerseObjectUpdate) {
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
                // State
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
                new Vector3(
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
                new Vector3(
  Utils.UInt16ToFloat(block.Data, i, -64.0f, 64.0f),
  Utils.UInt16ToFloat(block.Data, i + 2, -64.0f, 64.0f),
  Utils.UInt16ToFloat(block.Data, i + 4, -64.0f, 64.0f));


                processingPacket = true;

                newPosition += cameraOffset * newRotation;
                if (!Equal(newPosition, position) && !Equal(newPosition, ModifiedPosition))
                    Position = newPosition;

                Vector3 lookAt = Vector3.UnitX * newRotation;
                if (!Equal(lookAt, rotation.LookAtVector) && !Equal(lookAt, ModifiedRotation.LookAtVector))
                    Rotation.LookAtVector = lookAt;

                processingPacket = false;

                CreatePacket();
            }
        }

        private void ProcessAgentUpdatePacket(Packet p) {
            AgentUpdatePacket packet = (AgentUpdatePacket)p;

            processingPacket = true;
            bool posEq = Equal(position, packet.AgentData.CameraCenter);
            bool modEq = Equal(packet.AgentData.CameraCenter, ModifiedPosition);
            if (!Equal(position,packet.AgentData.CameraCenter) && !Equal(packet.AgentData.CameraCenter, ModifiedPosition))
                Position = packet.AgentData.CameraCenter;
            if (!Equal(rotation.LookAtVector, packet.AgentData.CameraAtAxis) && !Equal(packet.AgentData.CameraAtAxis, Rotation.LookAtVector))
                Rotation.LookAtVector = packet.AgentData.CameraAtAxis;
            processingPacket = false;

            //packetsForwarded++;
            CreatePacket();
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
            get { return Position + ((Window.EyePosition * Rotation.Quaternion) / 1000f); }
        }

        public Rotation ModifiedRotation {
            get { return new Rotation(Rotation.Pitch + Window.RotationOffset.Pitch, Rotation.Yaw + Window.RotationOffset.Yaw); }
        }

        /// <summary>
        /// Where the camera should be located relative to the avatar before any transformations are applied.
        /// Measured in virtual world units (m).
        /// </summary>
        public Vector3 CameraOffset {
            get { return cameraOffset; }
            set {
                position -= cameraOffset * rotation.Quaternion;
                cameraOffset = value;
                position += cameraOffset * rotation.Quaternion;
                CreatePacket();
            }
        }
    }
}
