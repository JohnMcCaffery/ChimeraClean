using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse.Packets;
using OpenMetaverse;

namespace Chimera.OpenSim {
    public class BackwardCompatibleController : ProxyControllerBase {
        public BackwardCompatibleController(Frame frame)
            : base(frame) {
        }
        private SetFollowCamPropertiesPacket MakePacket(bool enable) {
            SetFollowCamPropertiesPacket cameraPacket = new SetFollowCamPropertiesPacket();
            cameraPacket.CameraProperty = new SetFollowCamPropertiesPacket.CameraPropertyBlock[22];
            for (int i = 0; i < 22; i++) {
                cameraPacket.CameraProperty[i] = new SetFollowCamPropertiesPacket.CameraPropertyBlock();
                cameraPacket.CameraProperty[i].Type = i + 1;
            }

            Vector3 focus = Frame.Coordinator.Position + Frame.Coordinator.Orientation.LookAtVector;
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
            cameraPacket.CameraProperty[11].Value = enable ? 1f : 0f; //enable
            cameraPacket.CameraProperty[12].Value = 0f;
            cameraPacket.CameraProperty[13].Value = Frame.Coordinator.Position.X;
            cameraPacket.CameraProperty[14].Value = Frame.Coordinator.Position.Y;
            cameraPacket.CameraProperty[15].Value = Frame.Coordinator.Position.Z;
            cameraPacket.CameraProperty[16].Value = 0f;
            cameraPacket.CameraProperty[17].Value = focus.X;
            cameraPacket.CameraProperty[18].Value = focus.Y;
            cameraPacket.CameraProperty[19].Value = focus.Z;
            cameraPacket.CameraProperty[20].Value = 1f;
            cameraPacket.CameraProperty[21].Value = 1f;
            return cameraPacket;
        }
        protected override Packet ActualSetCamera() {
            return MakePacket(true);
        }

        protected override Packet ActualSetCamera(OpenMetaverse.Vector3 positionDelta, Util.Rotation orientationDelta) {
            return MakePacket(true);
        }

        public override void SetFrustum(bool setPosition) {
            //throw new NotImplementedException();
        }

        public override void Move(OpenMetaverse.Vector3 positionDelta, Util.Rotation orientationDelta, float scale) {
            InjectPacket(MakePacket(true));
        }

        public override void ClearCamera() {
            //throw new NotImplementedException();
            InjectPacket(MakePacket(false));
        }

        public override void ClearFrustum() {
            //throw new NotImplementedException();
        }

        public override void ClearMovement() {
            //throw new NotImplementedException();
        }
    }
}
