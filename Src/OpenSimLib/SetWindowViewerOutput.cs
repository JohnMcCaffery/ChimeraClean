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

            /*
            Vector3 upperRight = new Vector3(0f, (float)(window.Width / 2.0), (float)(window.Height / 2.0));
            Vector3 lowerLeft = new Vector3(0f, (float)(window.Width / -2.0), (float)(window.Height / -2.0));
            Vector3 diff = Window.ScreenPosition - Window.Coordinator.EyePosition;

            diff *= -window.RotationOffset.Quaternion;
            //diff *= input.RotationOffset.Quaternion;

            upperRight += diff;
            lowerLeft += diff;

            //upperRight /= (Math.Abs(diff.X) - .01f);
            //lowerLeft /= (Math.Abs(diff.X) - .01f);
            upperRight /= (float) (diff.X * 10.0);
            lowerLeft /= (float) (diff.X * 10.0);

            float x1 = Math.Min(upperRight.Y, lowerLeft.Y);
            float x2 = Math.Max(upperRight.Y, lowerLeft.Y);
            float y1 = Math.Max(upperRight.Z, lowerLeft.Z);
            float y2 = Math.Min(upperRight.Z, lowerLeft.Z);
            float dn = (diff.Length() / diff.X) * .1f;
            float df = (512f * 100f) * dn;

		    Matrix4 matrix = new Matrix4(
    			(2*dn) / (x2-x1),   0,              (x2+x1)/(x2-x1),   0,
    			0,                  (2*dn)/(y1-y2), (y1+y2)/(y1-y2),   0,
    			0,                  0,              -(df+dn)/(df-dn),   -(2.0f*df*dn)/(df-dn),
    			0,                  0,              -1.0f,              0);


            /*
            SetFrustumPacket fp = input.CreateFrustumPacket(512f);
            float x1 = fp.Frustum.x1;
            float x2 = fp.Frustum.x2;
            float y1 = fp.Frustum.y1;
            float y2 = fp.Frustum.y2;
            float dn = fp.Frustum.dn;
            float df = fp.Frustum.df;
            */

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
