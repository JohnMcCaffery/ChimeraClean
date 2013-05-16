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
using OpenMetaverse.Packets;
using Chimera.Util;
using OpenMetaverse;
using Chimera;

namespace Chimera.OpenSim {
    public class SetWindowViewerOutput : ViewerProxy {
        public SetWindowViewerOutput(string name, params string[] args)
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
            if (ProxyRunning && ControlCamera) {
                InjectPacket(new SetWindowPacket(Window.ProjectionMatrix));
                SetCamera();
            }
        }

        protected override void ProcessCameraUpdate (Coordinator coordinator, CameraUpdateEventArgs args) {
            if (ProxyRunning && ControlCamera)
                InjectPacket(MakePacket(args.position, args.positionDelta, args.rotation, args.rotationDelta));
        }

        private Packet MakePacket(Vector3 position, Vector3 positionDelta, Rotation rotation, Rotation rotationDelta) {
            //Vector3 focus = Window.Coordinator.Position + Window.Coordinator.Orientation.LookAtVector;
            Vector3 lookAt = (rotation - Window.Orientation).LookAtVector;
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
