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
        private bool mAllowFly = true;

        public FullController(Frame frame)
            : base(frame) {
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
            //Vector3 focus = Window.Core.Position + Window.Core.Orientation.LookAtVector;
            //Vector3 lookAt = (rotation - Frame.Orientation).LookAtVector;
            Quaternion offset = new Rotation(Frame.Orientation.Pitch, -Frame.Orientation.Yaw).Quaternion;
            Vector3 lookAt = rotation.LookAtVector * offset;
            Vector3 eyePos = new Vector3(Frame.Core.EyePosition.Y, Frame.Core.EyePosition.X, -Frame.Core.EyePosition.Z);

            SetCameraPacket.CameraBlock block = new SetCameraPacket.CameraBlock();
            block.Position = position - (eyePos / 1000f);
            block.PositionDelta = positionDelta;
            block.LookAt = lookAt;
            block.LookAtDelta = rotationDelta.LookAtVector;
            block.Up = (Vector3.UnitZ * rotation.Quaternion) * offset;
            block.TickLength = (uint) Frame.Core.TickLength * 1000;
            return block;
        }
    }
}
