using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse.Packets;
using Chimera.Util;
using OpenMetaverse;
using Chimera;

namespace Chimera.OpenSim {
    public class SetFollowCamPropertiesViewerOutput : ViewerProxy {
        public SetFollowCamPropertiesViewerOutput(params string[] args)
            : base(args) {
        }
        public SetFollowCamPropertiesViewerOutput(string name, string file, params string[] args)
            : base(name, args) {
        }

        public override void ClearCamera() {
            if (ProxyRunning)
                InjectPacket(new ClearFollowCamPropertiesPacket());
        }

        public override void SetCamera() {
            if (ProxyRunning && ControlCamera)
                InjectPacket(MakePacket());
        }

        protected override void ProcessChange (Coordinator coordinator, CameraUpdateEventArgs args) {
            SetCamera();
        }

        protected override void ProcessEyeUpdate(Coordinator coordinator, EventArgs args) { }

        private SetFollowCamPropertiesPacket MakePacket() {
            SetFollowCamPropertiesPacket cameraPacket = new SetFollowCamPropertiesPacket();
            cameraPacket.CameraProperty = new SetFollowCamPropertiesPacket.CameraPropertyBlock[22];
            for (int i = 0; i < 22; i++) {
                cameraPacket.CameraProperty[i] = new SetFollowCamPropertiesPacket.CameraPropertyBlock();
                cameraPacket.CameraProperty[i].Type = i + 1;
            }

            Vector3 focus = Window.Coordinator.Position + Window.Coordinator.Orientation.LookAtVector;
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
            cameraPacket.CameraProperty[13].Value = Window.Coordinator.Position.X;
            cameraPacket.CameraProperty[14].Value = Window.Coordinator.Position.Y;
            cameraPacket.CameraProperty[15].Value = Window.Coordinator.Position.Z;
            cameraPacket.CameraProperty[16].Value = 0f;
            cameraPacket.CameraProperty[17].Value = focus.X;
            cameraPacket.CameraProperty[18].Value = focus.Y;
            cameraPacket.CameraProperty[19].Value = focus.Z;
            cameraPacket.CameraProperty[20].Value = 1f;
            cameraPacket.CameraProperty[21].Value = 1f;
            return cameraPacket;
        }
    }
}
