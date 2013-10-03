using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuiLibDotNet;
using Chimera.Interfaces;

namespace Chimera.Kinect.Axes {
    public class TwistAxis : DotAxis {
        private static readonly float DEADZONE = 10f;
        private static readonly float SCALE = .01f;
        private Vector mA = Vector.Create("X", 1f, 0f, 0f);
        private Vector mB = 
            Nui.limit(Nui.joint(Nui.Shoulder_Right), true, false, true) -
            Nui.limit(Nui.joint(Nui.Shoulder_Left), true, false, true);

        private Condition mActive = Condition.Create("TwistActive", true);

        public override NuiLibDotNet.Vector A {
            get { return mA; }
        }

        public override NuiLibDotNet.Vector B {
            get { return mB; }
        }

        public override float Sign {
            get { return base.Sign * -1f; }
        }

        public TwistAxis(AxisBinding binding)
            : base("Twist", binding, Perspective.Y) {
        }

        public TwistAxis()
            : base("Twist", Perspective.Y) {
        }

        public override Condition Active {
            get { return mActive; }
        }
    }
}
