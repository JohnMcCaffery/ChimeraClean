using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using Chimera.Util;
using Chimera.OpenSim.Packets;

namespace Chimera.OpenSim {
    public class FullController : ProxyControllerBase {
        public FullController(Window window)
            : base(window) {
        }        
        protected override void ActualSetCamera() {
            InjectPacket(new SetCameraPacket(MakeCameraBlock()));
        }

        protected override void ActualSetCamera(OpenMetaverse.Vector3 positionDelta, Util.Rotation orientationDelta) {
            InjectPacket(new SetCameraPacket(MakeCameraBlock()));
        }

        public override void SetFrustum(bool setPosition) {
            if (setPosition)
                InjectPacket(new SetWindowPacket(Window.ProjectionMatrix, MakeCameraBlock()));
            else
                InjectPacket(new SetFrustumPacket(Window.ProjectionMatrix));
        }

        public override void Move(Vector3 positionDelta, Rotation orientationDelta, float deltaScale) {
            RemoteControlPacket packet = new RemoteControlPacket();
            packet.Delta.Position = positionDelta * deltaScale;
            packet.Delta.Pitch = (float)(orientationDelta.Pitch * (Math.PI / 45.0));
            packet.Delta.Yaw = (float)(orientationDelta.Yaw * (Math.PI / 45.0));
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
            return MakeCameraBlock(Window.Coordinator.Position, Vector3.Zero, Window.Coordinator.Orientation, Rotation.Zero);
        }
        private SetCameraPacket.CameraBlock MakeCameraBlock(Vector3 position, Vector3 positionDelta, Rotation rotation, Rotation rotationDelta) {
            //Vector3 focus = Window.Coordinator.Position + Window.Coordinator.Orientation.LookAtVector;
            Vector3 lookAt = (rotation - Window.Orientation).LookAtVector;
            Vector3 eyePos = new Vector3(Window.Coordinator.EyePosition.Y, Window.Coordinator.EyePosition.X, -Window.Coordinator.EyePosition.Z);

            SetCameraPacket.CameraBlock block = new SetCameraPacket.CameraBlock();
            block.Position = position - (eyePos / 1000f);
            block.PositionDelta = positionDelta;
            block.LookAt = lookAt;
            block.LookAtDelta = rotationDelta.LookAtVector;
            block.TickLength = (uint) Window.Coordinator.TickLength * 1000;
            return block;
        }
    }
}
