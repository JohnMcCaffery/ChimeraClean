using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces;
using OpenMetaverse;
using Chimera.Util;
using System.Windows.Forms;
using Chimera.GUI.Controls.Inputs;
using System.Drawing;

namespace Chimera.Inputs {
    public class AxisBasedDelta : IDeltaInput {
        private readonly List<IAxis> mAxes = new List<IAxis>();
        private readonly string mName;

        private AxisBasedDeltaPanel mPanel;
        private bool mEnableX;
        private bool mEnableY;
        private bool mEnableZ;
        private bool mEnablePitch;
        private bool mEnableYaw;
        private bool mEnabled;
        private float mScale;

        public event Action<IAxis> AxisAdded;

        public float Scale {
            get { return mScale; }
            set {
                mScale = value;
                if (Change != null)
                    Change(this);
            }
        }

        public IEnumerable<IAxis> Axes {
            get { return mAxes; }
        }

        public AxisBasedDelta(string name, params IAxis[] axes) {
            mName = name;

            int i = 0;
            if (axes.Length > i && axes[i++] != null)
                axes[i - 1].Binding = AxisBinding.X;
            if (axes.Length > i && axes[i++] != null)
                axes[i - 1].Binding = AxisBinding.Y;
            if (axes.Length > i && axes[i++] != null)
                axes[i - 1].Binding = AxisBinding.Z;
            if (axes.Length > i && axes[i++] != null)
                axes[i - 1].Binding = AxisBinding.Pitch;
            if (axes.Length > i && axes[i++] != null)
                axes[i - 1].Binding = AxisBinding.Yaw;

            foreach (var axis in mAxes)
                AddAxis(axis);
        }

        public void AddAxis(IAxis axis) {
            mAxes.Add(axis);
            axis.Changed += new Action(axis_Changed);
            if (AxisAdded != null)
                AxisAdded(axis);
        }

        void axis_Changed() {
            if (Change != null)
                Change(this);
        }

        #region IDeltaInput Members

        public event Action<IDeltaInput> Change;

        public Vector3 PositionDelta {
            get {
                float x = mEnableX ? mAxes.Where(a => a.Binding == AxisBinding.X).Sum(a => a.Delta) : 0f;
                float y = mEnableY ? mAxes.Where(a => a.Binding == AxisBinding.Y).Sum(a => a.Delta) : 0f;
                float z = mEnableZ ? mAxes.Where(a => a.Binding == AxisBinding.Z).Sum(a => a.Delta) : 0f;
                return new Vector3(x, y, z) * mScale;
            }
        }

        public Rotation OrientationDelta {
            get { 
                float p = mEnablePitch ? mAxes.Where(a => a.Binding == AxisBinding.Pitch).Sum(a => a.Delta) : 0f;
                float y = mEnableYaw ? mAxes.Where(a => a.Binding == AxisBinding.Yaw).Sum(a => a.Delta) : 0f;
                return new Rotation(p * mScale, y * mScale);
            }
        }

        public bool WalkEnabled {
            get { return mEnableX; }
            set {
                if (value != mEnableX) {
                    mEnableX = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public bool StrafeEnabled {
            get { return mEnableY; }
            set {
                if (value != mEnableY) {
                    mEnableY = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public bool FlyEnabled {
            get { return mEnableZ; }
            set {
                if (value != mEnableZ) {
                    mEnableZ = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public bool YawEnabled {
            get { return mEnableYaw; }
            set {
                if (value != mEnableYaw) {
                    mEnableYaw = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public bool PitchEnabled {
            get { return mEnablePitch; }
            set {
                if (value != mEnablePitch) {
                    mEnablePitch = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public void Init(IInputSource input) { }

        #endregion

        #region IInput Members

        public event Action<IInput, bool> EnabledChanged;

        public UserControl ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new AxisBasedDeltaPanel(this);
                return mPanel;
            }
        }

        public bool Enabled {
            get { return mEnabled; }
            set {
                if (mEnabled != value) {
                    mEnabled = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public string Name {
            get { return mName; }
        }

        public string State {
            get { throw new NotImplementedException(); }
        }

        public ConfigBase Config {
            get { throw new NotImplementedException(); }
        }

        public void Close() { }

        public void Draw(Func<Vector3, Point> to2D, Graphics graphics, Action redraw) { }

        #endregion
    }
}
