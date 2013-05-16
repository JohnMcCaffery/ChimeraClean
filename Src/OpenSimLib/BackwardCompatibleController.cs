using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chimera.OpenSim {
    class BackwardCompatibleController : ProxyControllerBase {
        public override void SetCamera() {
            //throw new NotImplementedException();
        }

        public override void SetCamera(OpenMetaverse.Vector3 positionDelta, Util.Rotation orientationDelta) {
            //throw new NotImplementedException();
        }

        public override void SetFrustum() {
            //throw new NotImplementedException();
        }

        public override void Move(OpenMetaverse.Vector3 positionDelta, Util.Rotation orientationDelta) {
            //throw new NotImplementedException();
        }

        public override void ClearCamera() {
            //throw new NotImplementedException();
        }

        public override void ClearFrustum() {
            //throw new NotImplementedException();
        }

        public override void ClearMovement() {
            //throw new NotImplementedException();
        }
    }
}
