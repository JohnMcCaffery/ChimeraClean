using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Plugins;
using Chimera.Kinect.Axes;
using Chimera.Interfaces;

namespace Chimera.Kinect {
    public class KinectMovementPlugin : AxisBasedDelta {
        public KinectMovementPlugin()
            : base("KinectMovement",
                new PushAxis(true, AxisBinding.X),
                new PushAxis(false, AxisBinding.X),
                new TAxis(true, AxisBinding.Z),
                new TAxis(false, AxisBinding.Z),
                new CrouchAxis(AxisBinding.Z),
                new TwistAxis(AxisBinding.Yaw),
                new LeanAxis(AxisBinding.Yaw),
                new ArmYawAxis(true, AxisBinding.Yaw),
                new ArmYawAxis(false, AxisBinding.Yaw)
                ) {
        }

    }
}
