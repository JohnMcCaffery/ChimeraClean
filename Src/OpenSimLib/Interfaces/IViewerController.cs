using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using Chimera.Util;
using GridProxy;

namespace Chimera.OpenSim.Interfaces {
    interface IViewerController {
        Proxy Proxy { get; }

        string FirstName { get; }
        string LastName { get; }
        UUID SessionID { get; }
        UUID SecureSessionID { get; }
        UUID AgentID { get; }

        void Chat(string msg, int channel);

        void Init(Window window);

        //-----------------------------
        
        void SetCamera();
        void SetCamera(Vector3 positionDelta, Rotation orientationDelta);
        void SetFrustum();
        void Move(Vector3 positionDelta, Rotation orientationDelta);

        void ClearCamera();
        void ClearFrustum();
        void ClearMovement();
    }
}
