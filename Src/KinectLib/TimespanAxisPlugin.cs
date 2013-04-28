using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Plugins;
using Chimera.Kinect.Axes;

namespace Chimera.Kinect {
    public class TimespanDeltaPlugin : DeltaBasedInput {
        public TimespanDeltaPlugin()
            : base(new TimespanAxisPlugin()) {
        }
    }
    public class TimespanAxisPlugin : AxisBasedDelta {
        public TimespanAxisPlugin()
            : base("Kinect Move - Timespan",
                new PushAxis(true, Chimera.Interfaces.AxisBinding.X),
                new PushAxis(false, Chimera.Interfaces.AxisBinding.X)
                ) {
        }

    }
}
