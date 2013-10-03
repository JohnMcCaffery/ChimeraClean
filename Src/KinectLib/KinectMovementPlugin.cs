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
                new PushAxis(true),
                new PushAxis(false),
                new TAxis(true),
                new TAxis(false),
                new CrouchAxis(),
                new TwistAxis(),
                new LeanAxis(),
                new ArmYawAxis(true),
                new ArmYawAxis(false)
                ) {
        }

    }
}
