using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Plugins;
using Chimera.Interfaces;
using NuiLibDotNet;
using System.Windows.Forms;
using Chimera.Kinect.GUI.Axes;

namespace Chimera.Kinect.Axes {
    /// <summary>
    /// Takes the signed dot product of two vectors as the final axis
    /// </summary>
    public abstract class DotAxis : KinectAxis {
        private Scalar mRaw;
        //private Scalar mValue;
        private DotAxisPanel mPanel;
        private Perspective mPerspective;
        private Vector mCross;

        public abstract Vector A { get; }
        public abstract Vector B { get; }

        public override UserControl ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new DotAxisPanel(this);
                return mPanel;
            }
        }

        public DotAxis(string name, Perspective perspective)
            : this(name, AxisBinding.NotSet, perspective) {
        }

        public DotAxis(string name, AxisBinding binding, Perspective perspective)
            : base(name, binding) {

            mPerspective = perspective;
            Init();
        }

        /// <summary>
        /// Will be called from constructor. To re-set up after setting up constructor variables call again.
        /// </summary>
        protected void Init() {
            mCross = Nui.cross(A, B);
            mRaw = Nui.acos(Nui.dot(Nui.normalize(A), Nui.normalize(B)));
            //mRaw = dot * Sign * (180f / (float)Math.PI);
            //mRaw = Nui.acos(Nui.dot(A, B)) * (180f / (float) Math.PI);
            //mValue = Nui.ifScalar(Active, mRaw, 0f);

            AddListener();
        }

        #region IKinectAxis Members

        public override ConstrainedAxis Axis {
            get { return this; }
        }

        public override float RawValue {
            get {
                return mRaw.Value * Sign * TO_DEG;
            }
        }

        public virtual float Sign {
            get {
                switch (mPerspective) {
                    case Perspective.X: return mCross.X > 0f ? 1f : -1f;
                    case Perspective.Y: return mCross.Y > 0f ? 1f : -1f;
                    case Perspective.Z: return mCross.Z > 0f ? 1f : -1f;
                };
                return 1f;
            }
        }

        private const float TO_DEG = (float)(180.0 / Math.PI);


        #endregion
    }
}
