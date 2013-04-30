using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Plugins;
using Chimera.Interfaces;
using G = Chimera.Kinect.GlobalConditions;
using NuiLibDotNet;
using Chimera.Kinect.GUI;

namespace Chimera.Kinect.Axes {
    public abstract class KinectAxis : ConstrainedAxis {
        private static Scalar sAnchor = Nui.smooth(Nui.magnitude(Nui.joint(Nui.Shoulder_Centre) - Nui.joint(Nui.Hip_Centre)), 50);
        private static Scalar sTmpDZScale = Scalar.Create(1f);
        private static Scalar sTmpSScale = Scalar.Create(1f);

        private Scalar mDeadzoneScale = Scalar.Create(1f);
        private Scalar mScaleScale = Scalar.Create(1f);

        private static Scalar MakeDZ() {
            sTmpDZScale = Scalar.Create(1f);
            return sTmpDZScale;
        }

        private static Scalar MakeScale() {
            sTmpSScale = Scalar.Create(1f);
            return sTmpSScale;
        }

        public Scalar DeadzoneScale {
            get { return mDeadzoneScale; }
        }

        public Scalar ScaleScale {
            get { return mScaleScale; }
        }

        public abstract ConstrainedAxis Axis { get; }
        public abstract Scalar Raw { get; }
        public abstract Condition Active { get; }


        public KinectAxis(string name, AxisBinding binding)
            : base(name, new ScalarUpdater(sAnchor * MakeDZ()), new ScalarUpdater(sAnchor * MakeScale()), binding) {

            mDeadzoneScale = sTmpDZScale;
            mScaleScale = sTmpSScale;

            mDeadzoneScale.Value = G.Cfg.GetDeadzone(name);
            mScaleScale.Value = G.Cfg.GetScale(name);
        }
    }
}
