using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuiLibDotNet;
using C = NuiLibDotNet.Condition;
using Chimera.Interfaces;

namespace Chimera.Kinect.Axes {
    public class ArmPitchAxis : DotAxis {
        private bool mRight;
        private Vector mA = Vector.Create("Z", 0f, 0f, 1f);
        private Vector mB = 
            Nui.limit(Nui.joint(Nui.Hand_Right), false, true, true) -
            Nui.limit(Nui.joint(Nui.Shoulder_Right), false, true, true);

        private Condition mActive = Condition.Create("ArmPitchActive", true);

        public override NuiLibDotNet.Vector A {
            get { return mA; }
        }

        public override NuiLibDotNet.Vector B {
            get { return mB; }
        }

        public override float Sign {
            get {
                return base.Sign * -1f;
            }
        }

        public ArmPitchAxis(bool right, AxisBinding binding)
            : base("ArmPitch" + (right ? "Right" : "Left"), binding, Perspective.X) {

            mRight = right;

            Vector shoulders = Nui.joint(Nui.Shoulder_Left) - Nui.joint(Nui.Shoulder_Right);
            Vector diff = 
                Nui.joint(mRight ? Nui.Shoulder_Right : Nui.Shoulder_Left) -
                Nui.joint(mRight ? Nui.Hand_Right : Nui.Hand_Left);

            mActive = Condition.And(
                mRight ? GlobalConditions.ActiveR : GlobalConditions.ActiveL,
                /*Condition.And(
                    Nui.z(diff) > Nui.abs(Nui.x(diff)),
                    Nui.z(diff) > Nui.abs(Nui.y(diff)))*/
                Nui.z(diff) > Nui.abs(Nui.magnitude(shoulders))
                    );

            mB = Nui.limit(diff, false, true, true);

            Init();
        }

        public ArmPitchAxis(bool right)
            : this(right, AxisBinding.NotSet) { }

        public override Condition Active {
            get { return mActive; }
        }
    }
}
