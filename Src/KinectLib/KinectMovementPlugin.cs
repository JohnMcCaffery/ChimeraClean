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
                new StandAxis(true),
                new StandAxis(false),
                new PushAxis(true),
                new PushAxis(false),
                new PushAxis(),
                new TAxis(true),
                new TAxis(false),
                new CrouchAxis(),
                new TwistAxis(),
                new LeanAxis(),
                new ArmYawAxis(true),
                new ArmYawAxis(false),
                new ArmPitchAxis(true),
                new ArmPitchAxis(false)
                ) {
        }

    }
}
