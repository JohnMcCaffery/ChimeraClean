using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Plugins;
using Chimera.Interfaces;
using NuiLibDotNet;
using Chimera.Kinect.Interfaces;

namespace Chimera.Kinect.Axes {
    public class LeanAxis : DotAxis {
        private static readonly float DEADZONE = 10f;
        private static readonly float SCALE = .01f;

        private Vector mA = Vector.Create("Y", 0f, 1f, 0f);
        private Vector mB = 
                Nui.limit(Nui.joint(Nui.Head), true, true, false) -
                Nui.limit(Nui.joint(Nui.Hip_Centre), true, true, false);

        private Condition mActive = Condition.Create("LeanActive", true);

        public override Vector A {
            get { return mA; }
        }

        public override Vector B {
            get { return mB; }
        }

        protected override Scalar Sign {
            get { return MakeSign(Perspective.Z); }
        }

        public LeanAxis(AxisBinding binding)
            : base("Lean") {
        }

        public LeanAxis()
            : base("Lean") {
        }

        public override Condition Active {
            get { return mActive; }
        }
    }
}
