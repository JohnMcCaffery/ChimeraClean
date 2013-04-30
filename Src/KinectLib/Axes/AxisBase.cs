using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Plugins;
using Chimera.Interfaces;
using NuiLibDotNet;
using Chimera.Kinect.Interfaces;

namespace Chimera.Kinect.Axes {
    public class AxisBase : ConstrainedAxis, IKinectAxis {
        private static readonly float DZ = 10f;
        private static readonly float SCALE = .01f;
        private Scalar mRaw;
        private Scalar mValue;
        private Condition mActive;

        public AxisBase()
            : this(AxisBinding.None) {
        }

        public AxisBase(AxisBinding binding)
            : base("Twist", DZ, SCALE, binding) {
        }

        #region IKinectAxis Members

        public ConstrainedAxis Axis {
            get { return this; }
        }

        public Scalar Raw {
            get { return mRaw; }
        }

        public Condition Active {
            get { return mActive; }
        }

        #endregion
    }
}
