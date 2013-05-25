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
        public abstract Condition Active { get; }        public abstract ConstrainedAxis Axis { get; }

        public KinectAxis(string name, AxisBinding binding)
            : base(name, binding) {

            Deadzone.Value = G.Cfg.GetDeadzone(name);
            Scale.Value = G.Cfg.GetScale(name);

            Nui.SkeletonLost += new SkeletonTrackDelegate(Nui_SkeletonLost);
        }

        void Nui_SkeletonLost() {
            SetRawValue(0f);
        }
    }
}
