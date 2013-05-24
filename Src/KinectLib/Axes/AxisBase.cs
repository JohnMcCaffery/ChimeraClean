using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Plugins;
using Chimera.Interfaces;
using NuiLibDotNet;

namespace Chimera.Kinect.Axes {
    public class AxisBase : KinectAxis {
        private static readonly float DZ = 10f;
        private static readonly float SCALE = .01f;
        private Scalar mRaw;
        private Scalar mValue;
        private Condition mActive;

        public AxisBase()
            : this(AxisBinding.NotSet) {
        }

        public AxisBase(AxisBinding binding)
            : base("Twist", binding) {
        }

        #region IKinectAxis Members

        public override ConstrainedAxis Axis {
            get { return this; }
        }

        public override Scalar Raw {
            get { return mRaw; }
        }

        public override Condition Active {
            get { return mActive; }
        }

        #endregion
    }
}
