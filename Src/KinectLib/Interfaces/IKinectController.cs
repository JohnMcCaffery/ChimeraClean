using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using Chimera.Util;

namespace Chimera.Kinect.Interfaces {
    public interface IKinectController {
        Vector3 Position { get; }
        Rotation Orientation { get; }

        event Action<Vector3> PositionChanged;
        event Action<Rotation> OrientationChanged;
    }
}
