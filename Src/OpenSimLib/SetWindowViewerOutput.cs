using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse.Packets;
using Chimera.Util;
using OpenMetaverse;
using Chimera;

namespace Chimera.OpenSim {
    public class SetWindowViewerOutput : ViewerProxy {
        public SetWindowViewerOutput(params string[] args)
            : base(args) {
        }
        public SetWindowViewerOutput(string name, string file, params string[] args)
            : base(name, args) {
        }

        public override void ClearCamera() {
            if (ProxyRunning)
                InjectPacket(new ClearCameraPacket());
        }

        public override void SetCamera() {
            if (ProxyRunning && ControlCamera)
                InjectPacket(MakePacket(Window.Coordinator.Position, Vector3.Zero, Window.Coordinator.Orientation, Rotation.Zero));
        }

        public override void SetWindow() {
            if (ProxyRunning && ControlCamera)
                InjectPacket(new SetWindowPacket(Window.ProjectionMatrix));
        }

        protected override void ProcessCameraUpdate (Coordinator coordinator, CameraUpdateEventArgs args) {
            if (ProxyRunning && ControlCamera)
                InjectPacket(MakePacket(args.position, args.positionDelta, args.rotation, args.rotationDelta));
        }

        private Packet MakePacket(Vector3 position, Vector3 positionDelta, Rotation rotation, Rotation rotationDelta) {
            //Vector3 focus = Window.Coordinator.Position + Window.Coordinator.Orientation.LookAtVector;
            Vector3 lookAt = (rotation + Window.Orientation).LookAtVector;
            Vector3 eyePos = new Vector3(Window.Coordinator.EyePosition.Y, Window.Coordinator.EyePosition.X, -Window.Coordinator.EyePosition.Z);

            SetCameraPacket p = new SetCameraPacket();
            p.Camera.Position = position - (eyePos / 1000f);
            p.Camera.PositionDelta = positionDelta;
            p.Camera.LookAt = lookAt;
            p.Camera.LookAtDelta = rotationDelta.LookAtVector;
            p.Camera.TickLength = (uint) Window.Coordinator.TickLength * 1000;
            return p;
        }
    }
}
