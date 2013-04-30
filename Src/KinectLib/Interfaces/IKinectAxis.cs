using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuiLibDotNet;
using Chimera.Plugins;

namespace Chimera.Kinect.Interfaces {
    public interface IKinectAxis {
        ConstrainedAxis Axis {
            get;
        }
        Scalar Raw {
            get;
        }
        Condition Active {
            get;
        }
    }
}
