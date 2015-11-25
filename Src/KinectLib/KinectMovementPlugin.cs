using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Plugins;
using Chimera.Kinect.Axes;
using Chimera.Interfaces;
using Chimera.Config;

namespace Chimera.Kinect {
    public class KinectMovementPlugin : AxisBasedDelta {
        public KinectMovementPlugin()
            : base("KinectMovement",
                new KinectMovementConfig(),
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

    public class KinectMovementConfig : AxisConfig {
        public KinectMovementConfig()
            : base("KinectMovement") {
        }

        protected override void InitConfig() {

            GetDeadzone("PushLeft+");
            GetDeadzone("PushRight+");
            GetDeadzone("PushLeft-");
            GetDeadzone("PushRight-");
            GetDeadzone("T-Right");
            GetDeadzone("T-Left");
            GetDeadzone("Crouch");
            GetDeadzone("Twist");
            GetDeadzone("Lean");
            GetDeadzone("ArmYawRight");
            GetDeadzone("ArmYawLeft");
            GetDeadzone("ArmPitchRight");
            GetDeadzone("ArmPitchLeft");
            GetDeadzone("StandX");
            GetDeadzone("StandY");

            GetScale("PushLeft+");
            GetScale("PushRight+");
            GetScale("PushLeft-");
            GetScale("PushRight-");
            GetScale("T-Right");
            GetScale("T-Left");
            GetScale("Crouch");
            GetScale("Twist");
            GetScale("Lean");
            GetScale("ArmYawRight");
            GetScale("ArmYawLeft");
            GetScale("ArmPitchRight");
            GetScale("ArmPitchLeft");
            GetScale("StandX");
            GetScale("StandY");

            GetBinding("PushLeft+");
            GetBinding("PushRight+");
            GetBinding("PushLeft-");
            GetBinding("PushRight-");
            GetBinding("T-Right");
            GetBinding("T-Left");
            GetBinding("Crouch");
            GetBinding("Twist");
            GetBinding("Lean");
            GetBinding("ArmYawRight");
            GetBinding("ArmYawLeft");
            GetBinding("ArmPitchRight");
            GetBinding("ArmPitchLeft");
            GetBinding("StandX");
            GetBinding("StandY");
        }
    }
}
