using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Plugins;
using Chimera.Kinect.Axes;
using Chimera.Interfaces;

namespace Chimera.Kinect {
    public class TimespanAxisPlugin : AxisBasedDelta {
        public TimespanAxisPlugin()
            : base("KinectMove-Timespan",
                new PushAxis(true, AxisBinding.X),
                new PushAxis(false, AxisBinding.X),
                new TAxis(true, AxisBinding.Z),
                new TAxis(false, AxisBinding.Z),
                new CrouchAxis(AxisBinding.None),
                new TwistAxis(AxisBinding.Yaw),
                new LeanAxis(AxisBinding.Yaw),
                new ArmYawAxis(true, AxisBinding.Yaw),
                new ArmYawAxis(false, AxisBinding.Yaw)
                ) {
        }

    }
}
