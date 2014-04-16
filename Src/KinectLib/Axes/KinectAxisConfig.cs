using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;
using Chimera.Interfaces;
using System.IO;
using Chimera.Plugins;
using Chimera.Config;

namespace Chimera.Kinect.Axes {
    public class KinectAxisConfig : AxisConfig {
        public KinectAxisConfig()
            : base("KinectMovement") {
        }
    }
}
