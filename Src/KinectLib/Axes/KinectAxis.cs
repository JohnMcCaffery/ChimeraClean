using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Plugins;
using System.Windows.Forms;
using Chimera.Interfaces;
using NuiLibDotNet;
using G = Chimera.Kinect.GlobalConditions;

namespace Chimera.Kinect.Axes {
    public abstract class KinectAxis : ConstrainedAxis {
        public abstract Condition Active { get; }
        public abstract ConstrainedAxis Axis { get; }

        public abstract float KinectRawValue { get; }

        public KinectAxis(string name, IUpdater<float> deadzone, IUpdater<float> scale, AxisBinding binding)
            : base(name, deadzone, scale, binding) {
            Init();
        }

        public KinectAxis(string name, AxisBinding binding)
            : base(name, binding) {
            Init();
        }

        private void Init() {
            Deadzone.Value = G.Cfg.GetDeadzone(Name);
            Scale.Value = G.Cfg.GetScale(Name);
        }

        protected override float RawValue {
            get { return Nui.HasSkeleton && Active.Value ? KinectRawValue : 0f; }
        }
    }
}
