using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Plugins;
using Chimera.Interfaces;
using NuiLibDotNet;
using Chimera.Kinect.Interfaces;
using System.Windows.Forms;
using Chimera.Kinect.GUI.Axes;

namespace Chimera.Kinect.Axes {
    /// <summary>
    /// Takes the signed dot product of two vectors as the final axis
    /// </summary>
    public abstract class DotAxis : KinectAxis {
        private Scalar mRaw;
        private Scalar mValue;
        private DotAxisPanel mPanel;

        public abstract Vector A { get; }
        public abstract Vector B { get; }

        public override UserControl ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new DotAxisPanel(this);
                return mPanel;
            }
        }

        protected abstract Scalar Sign {
            get;
        }

        public DotAxis(string name)
            : this(name, AxisBinding.None) {
        }

        public DotAxis(string name, AxisBinding binding)
            : base(name, binding) {

            Init();
        }

        protected Scalar MakeSign(Perspective perspective) {
            Vector a = A;
            Vector b = B;
            Vector cross = Nui.cross(A, B);
            switch (perspective) {
                case Perspective.X: return Nui.ifScalar(Nui.x(cross) > 0f, 1f, -1f);
                case Perspective.Y: return Nui.ifScalar(Nui.y(cross) > 0f, 1f, -1f);
                case Perspective.Z: return Nui.ifScalar(Nui.z(cross) > 0f, 1f, -1f);
            }
            return Scalar.Create(1f);
        }

        /// <summary>
        /// Will be called from constructor. To re-set up after setting up constructor variables call again.
        /// </summary>
        protected void Init() {
            Scalar dot = Nui.acos(Nui.dot(Nui.normalize(A), Nui.normalize(B)));
            mRaw = dot * Sign * (180f / (float)Math.PI);
            //mRaw = Nui.acos(Nui.dot(A, B)) * (180f / (float) Math.PI);

            mValue = Nui.ifScalar(Active, mRaw, 0f);

            Nui.Tick += new ChangeDelegate(Nui_Tick);
        }

        void Nui_Tick() {
            SetRawValue(mValue.Value);
        }

        #region IKinectAxis Members

        public override ConstrainedAxis Axis {
            get { return this; }
        }

        /*
        public override Scalar Raw {
            get { return mRaw; }
        }
        */

        #endregion
    }
}
