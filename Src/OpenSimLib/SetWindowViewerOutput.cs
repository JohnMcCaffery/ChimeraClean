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
using Chimera.OpenSim.Packets;

namespace Chimera.OpenSim {
    public class SetWindowViewerOutput : ViewerProxy {
        public SetWindowViewerOutput(params string[] args)
            : base() {
        }
        public SetWindowViewerOutput(string name, string file, params string[] args)
            : base() {
        }

        public override void ClearCamera() {
            if (ProxyRunning)
                InjectPacket(new ClearCameraPacket());
        }

        public override void SetCamera() {
            if (ProxyRunning && ControlCamera)
                InjectPacket(new SetCameraPacket(MakeCameraBlock()));
        }

        public override void SetFrame() {
            if (ProxyRunning && ControlCamera)
                InjectPacket(new SetFrustumPacket(Frame.ProjectionMatrix));
        }

        protected override void ProcessCameraUpdate (Core coordinator, CameraUpdateEventArgs args) {
            if (ProxyRunning && ControlCamera)
                InjectPacket(new SetCameraPacket(MakeCameraBlock(args.position, args.positionDelta, args.rotation, args.rotationDelta)));
        }



        private SetCameraPacket.CameraBlock MakeCameraBlock() {
            return MakeCameraBlock(Frame.Coordinator.Position, Vector3.Zero, Frame.Coordinator.Orientation, Rotation.Zero);
        }
        private SetCameraPacket.CameraBlock MakeCameraBlock(Vector3 position, Vector3 positionDelta, Rotation rotation, Rotation rotationDelta) {
            //Vector3 focus = Window.Core.Position + Window.Core.Orientation.LookAtVector;
            Vector3 lookAt = (rotation - Frame.Orientation).LookAtVector;
            Vector3 eyePos = new Vector3(Frame.Coordinator.EyePosition.Y, Frame.Coordinator.EyePosition.X, -Frame.Coordinator.EyePosition.Z);

            SetCameraPacket.CameraBlock block = new SetCameraPacket.CameraBlock();
            block.Position = position - (eyePos / 1000f);
            block.PositionDelta = positionDelta;
            block.LookAt = lookAt;
            block.LookAtDelta = rotationDelta.LookAtVector;
            block.TickLength = (uint)Frame.Coordinator.TickLength * 1000;
            return block;
        }
    }
}
