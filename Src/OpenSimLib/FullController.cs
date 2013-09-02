using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using Chimera.Util;
using Chimera.OpenSim.Packets;
using OpenMetaverse.Packets;

namespace Chimera.OpenSim {
    public class FullController : ProxyControllerBase {
        private bool mAllowFly = false;

        public FullController(Frame frame)
            : base(frame) {

            mAllowFly = new ViewerConfig().AllowFly;
        }        
        protected override Packet ActualSetCamera() {
            return new SetCameraPacket(MakeCameraBlock());
        }

        protected override Packet ActualSetCamera(OpenMetaverse.Vector3 positionDelta, Util.Rotation orientationDelta) {
            return new SetCameraPacket(MakeCameraBlock());
        }

        public override void SetFrustum(bool setPosition) {
            if (setPosition)
                InjectPacket(new SetWindowPacket(Frame.ProjectionMatrix, MakeCameraBlock()));
            else
                InjectPacket(new SetFrustumPacket(Frame.ProjectionMatrix));
        }

        public override void Move(Vector3 positionDelta, Rotation orientationDelta, float deltaScale) {
            RemoteControlPacket packet = new RemoteControlPacket();
            packet.Delta.Position = positionDelta * deltaScale;
            if (!mAllowFly)
                packet.Delta.Position.Z = 0f;
            packet.Delta.Pitch = (float)(orientationDelta.Pitch * (Math.PI / 45.0)) * deltaScale;
            packet.Delta.Yaw = (float)(orientationDelta.Yaw * (Math.PI / 45.0)) * deltaScale;
            InjectPacket(packet);
        }

        public override void ClearCamera() {
            InjectPacket(new ClearCameraPacket());
        }

        public override void ClearFrustum() {
            InjectPacket(new ClearFrustumPacket());
        }

        public override void ClearMovement() {
            InjectPacket(new ClearRemoteControlPacket());
        }




        private SetCameraPacket.CameraBlock MakeCameraBlock() {
            return MakeCameraBlock(Frame.Core.Position, Vector3.Zero, Frame.Core.Orientation, Rotation.Zero);
        }
        private SetCameraPacket.CameraBlock MakeCameraBlock(Vector3 position, Vector3 positionDelta, Rotation rotation, Rotation rotationDelta) {
            Rotation orientationOffset = new Rotation(Frame.Orientation.Pitch, -Frame.Orientation.Yaw);
            Vector3 lookAt = orientationOffset.LookAtVector * rotation.Quaternion;
            //Vector3 eyePos = new Vector3(Frame.Core.EyePosition.Y, Frame.Core.EyePosition.X, -Frame.Core.EyePosition.Z);
            Vector3 eyePos = Frame.Core.EyePosition;
            eyePos *= rotation.Quaternion;

            Vector3 offset = Offset * rotation.Quaternion;
            if (Frame.Core.ControlMode == ControlMode.Absolute)
               offset += eyePos / 1000f;
            
            Vector3 cameraUp = Vector3.UnitZ * rotation.Quaternion;
            Vector3 up = Vector3.UnitZ;
            if (orientationOffset.Yaw != 0.0)
                up = orientationOffset.Yaw < 0.0 ? Vector3.Cross(lookAt, rotation.LookAtVector) : Vector3.Cross(rotation.LookAtVector, lookAt);

            SetCameraPacket.CameraBlock block = new SetCameraPacket.CameraBlock();
            block.Position = position + offset;
            block.PositionDelta = positionDelta;
            block.LookAt = lookAt;
            block.LookAtDelta = rotationDelta.LookAtVector;
            block.Up = up;
            block.TickLength = (uint) Frame.Core.TickLength * 1000;
            return block;
        }
    }
}
