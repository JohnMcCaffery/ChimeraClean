using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuiLibDotNet;
using C = NuiLibDotNet.Condition;
using G = Chimera.Kinect.GlobalConditions;
using Chimera.Interfaces;

namespace Chimera.Kinect.Axes {
    public class TAxis : DotAxis {
        private Condition mActive = Condition.Create(true);
        private Vector mB;
        private bool mRight;

        public override Vector A {
            get { return Vector.Create(mRight ? "X" : "-X", mRight ? 1f : -1f, 0f, 0f); }
        }

        public override Vector B {
            get {
                if (mB == null)
                    return A;
                return mB;
            }
        }

        protected override Scalar Sign {
            get { 
                if (mRight)
                    return MakeSign(Perspective.Z);
                return MakeSign(Perspective.Z) * -1f;
            }
        }

        public TAxis(bool right, AxisBinding binding)
            : base((right ? "T-Right" : "T-Left"), binding) {

            mRight = right;
            Vector h = Nui.joint(mRight ? Nui.Hand_Right : Nui.Hand_Left);
            Vector s = Nui.joint(mRight ? Nui.Shoulder_Right : Nui.Shoulder_Left);
            mB = h - s;

            mActive = C.And(mRight ? G.ActiveR : G.ActiveL, Nui.abs(Nui.x(B)) > Nui.abs(Nui.z(B)) * 2f); 

            mB = Nui.limit(mB, true, true, false);

            Init();
        }

        public TAxis(bool right)
            : this(right, AxisBinding.NotSet) {
        }

        public override Condition Active {
            get { return mActive; }
        }
    }
}
