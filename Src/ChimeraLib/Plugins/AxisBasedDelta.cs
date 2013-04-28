/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces;
using OpenMetaverse;
using Chimera.Util;
using System.Windows.Forms;
using Chimera.GUI.Controls.Plugins;
using System.Drawing;

namespace Chimera.Plugins {
    public class AxisBasedDelta : IDeltaInput {
        private readonly List<IAxis> mAxes = new List<IAxis>();
        private readonly string mName;
        private ITickSource mSource;

        private AxisBasedDeltaPanel mPanel;
        private bool mEnableX = true;
        private bool mEnableY = true;
        private bool mEnableZ = true;
        private bool mEnablePitch = true;
        private bool mEnableYaw = true;
        private bool mEnabled = false;
        private float mScale = 1f;
        private float mRotXMove = 3f;

        public event Action<IAxis> AxisAdded;

        public float Scale {
            get { return mScale; }
            set {
                mScale = value;
                if (Change != null)
                    Change(this);
            }
        }

        public float RotXMove { 
            get { return mRotXMove; }
            set {
                mRotXMove = value;
                if (Change != null)
                    Change(this);
            }
        }

        public IEnumerable<IAxis> Axes {
            get { return mAxes; }
        }

        /// <summary>
        /// Input axes will automatically be assigned to camera axes if no axis is specified.
        /// The ordering is as follows:
        /// 1st axis: x
        /// 2nd axis: y
        /// 3rd axis: z
        /// 4th axis: pitch
        /// 5th axis : yaw
        /// Specify null if you do not which to assign that axis.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="axes"></param>
        public AxisBasedDelta(string name, params IAxis[] axes) {
            mName = name;

            int i = 0;
            if (axes.Length > i && axes[i] != null && axes[i++].Binding == AxisBinding.None)
                axes[i - 1].Binding = AxisBinding.X;
            if (axes.Length > i && axes[i] != null && axes[i++].Binding == AxisBinding.None)
                axes[i - 1].Binding = AxisBinding.Y;
            if (axes.Length > i && axes[i] != null && axes[i++].Binding == AxisBinding.None)
                axes[i - 1].Binding = AxisBinding.Z;
            if (axes.Length > i && axes[i] != null && axes[i++].Binding == AxisBinding.None)
                axes[i - 1].Binding = AxisBinding.Pitch;
            if (axes.Length > i && axes[i] != null && axes[i++].Binding == AxisBinding.None)
                axes[i - 1].Binding = AxisBinding.Yaw;

            foreach (var axis in axes)
                if (axis != null)
                    AddAxis(axis);
        }

        public void AddAxis(IAxis axis) {
            mAxes.Add(axis);
            if (mSource != null && axis is ITickListener)
                (axis as ITickListener).Init(mSource);
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
                return new Rotation(p * mScale * mRotXMove, y * mScale * mRotXMove);
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

        public void Init(ITickSource input) {
            mSource = input;
            foreach (var axis in mAxes)
                if (axis is ITickListener)
                    (axis as ITickListener).Init(input);

            input.Tick += new Action(input_Tick);
        }

        void input_Tick() {
            if (Change != null)
                Change(this);
        }

        #endregion

        #region IInput Members

        public event Action<IPlugin, bool> EnabledChanged;

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
