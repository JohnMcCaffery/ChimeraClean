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
                InjectPacket(new ClearWindowPacket());
        }

        public override void SetCamera() {
            if (ProxyRunning && ControlCamera)
                InjectPacket(MakePacket(Window.Coordinator.Position, Vector3.Zero, Window.Coordinator.Orientation, Rotation.Zero));
        }

        protected override void ProcessChange (Coordinator coordinator, CameraUpdateEventArgs args) {
            if (ProxyRunning && ControlCamera)
                InjectPacket(MakePacket(args.position, args.positionDelta, args.rotation, args.rotationDelta));
        }

        protected override void ProcessEyeUpdate(Coordinator coordinator, EventArgs args) { }

        private Packet MakePacket(Vector3 position, Vector3 positionDelta, Rotation rotation, Rotation rotationDelta) {
            //Vector3 focus = Window.Coordinator.Position + Window.Coordinator.Orientation.LookAtVector;
            Vector3 lookAt = (rotation + Window.Orientation).LookAtVector;
            Vector3 eyePos = new Vector3(Window.Coordinator.EyePosition.Y, Window.Coordinator.EyePosition.X, -Window.Coordinator.EyePosition.Z);

            SetWindowPacket p = new SetWindowPacket();
            p.Window.Position = position - (eyePos / 1000f);
            p.Window.PositionDelta = positionDelta;
            p.Window.LookAt = lookAt;
            p.Window.LookAtDelta = rotationDelta.LookAtVector;
            p.Window.TickLength = (uint) Window.Coordinator.TickLength * 1000;
            p.Window.ProjectionMatrix = Window.ProjectionMatrix;
            return p;
        }
    }
}
