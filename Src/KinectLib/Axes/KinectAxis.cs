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
        public abstract Scalar Raw { get; }
        public abstract ConstrainedAxis Axis { get; }

        private ChangeDelegate mTickListener;

        public KinectAxis(string name, IUpdater<float> deadzone, IUpdater<float> scale, AxisBinding binding)
            : base(name, deadzone, scale, binding) {
        }

        public KinectAxis(string name, AxisBinding binding)
            : base(name, binding) {

            Deadzone.Value = G.Cfg.GetDeadzone(name);
            Scale.Value = G.Cfg.GetScale(name);

            mTickListener = new ChangeDelegate(Nui_Tick);
            Nui.SkeletonFound += new SkeletonTrackDelegate(Nui_SkeletonFound);
            Nui.SkeletonLost += new SkeletonTrackDelegate(Nui_SkeletonLost);

            if (Nui.HasSkeleton)
                Nui.Tick += mTickListener;
        }

        void Nui_SkeletonFound() {
            Nui.Tick += mTickListener;
        }

        void Nui_SkeletonLost() {
            Nui.Tick -= mTickListener;
            SetRawValue(0f);
        }

        void Nui_Tick() {
            SetRawValue(Nui.HasSkeleton && Active.Value ? Raw.Value : 0f);
        }   
    }
}
