using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;
using Chimera.Interfaces;
using System.IO;
using Chimera.Plugins;

namespace Chimera.Kinect.Axes {
    public class KinectAxisConfig : AxisBasedDelta.AxisConfig {
        public KinectAxisConfig()
            : base("Kinect") {
        }
    }
}
