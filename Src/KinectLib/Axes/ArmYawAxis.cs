using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuiLibDotNet;
using Chimera.Interfaces;

namespace Chimera.Kinect.Axes {
    public class ArmYawAxis : DotAxis {
        private bool mRight;
        private Vector mA = Vector.Create("Z", 0f, 0f, -1f);
        private Vector mB = 
            Nui.limit(Nui.joint(Nui.Hand_Right), true, false, true) -
            Nui.limit(Nui.joint(Nui.Shoulder_Right), true, false, true);

        private Condition mActive = Condition.Create("ArmYawActive", true);

        public override NuiLibDotNet.Vector A {
            get { return mA; }
        }

        public override NuiLibDotNet.Vector B {
            get { return mB; }
        }

        protected override Scalar Sign {
            get { return MakeSign(Perspective.Y) * -1f; }
        }

        public ArmYawAxis(bool right, AxisBinding binding)
            : base("ArmYaw" + (right ? "Right" : "Left"), binding) {

            mRight = right;

            Vector diff = 
                Nui.joint(mRight ? Nui.Shoulder_Right : Nui.Shoulder_Left) -
                Nui.joint(mRight ? Nui.Hand_Right : Nui.Shoulder_Left);

            mActive = Condition.And(
                mRight ? GlobalConditions.ActiveR : GlobalConditions.ActiveL,
                Condition.And(
                    Nui.z(diff) > Nui.abs(Nui.x(diff)),
                    Nui.z(diff) > Nui.abs(Nui.y(diff))));

            mB = Nui.limit(diff, true, false, true);

            Init();
        }

        public ArmYawAxis(bool right)
            : this(right, AxisBinding.None) { }

        public override Condition Active {
            get { return mActive; }
        }
    }
}
