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
using System.IO;
using Chimera.Config;
using log4net;

namespace Chimera.Plugins {
    public class AxisBasedDelta : DeltaBasedPlugin, ISystemPlugin {
        private readonly List<IAxis> mAxes = new List<IAxis>();
        private readonly string mName;
        private readonly Action mTickListener;
        private readonly ILog Logger;

        private Core mCore;
        private AxisConfig mConfig;

        private AxisBasedDeltaPanel mPanel;
        private float mScale = 1f;
        private float mRotXMove = 3f;

#if DEBUG
        private TickStatistics mStatistics = new TickStatistics();
#endif

        public event Action<IAxis> AxisAdded;

        public float Scale {
            get { return mScale; }
            set {
                mScale = value;
                TriggerChange(this);
            }
        }

        public float RotXMove { 
            get { return mRotXMove; }
            set {
                mRotXMove = value;
                TriggerChange(this);
            }
        }

        public IEnumerable<IAxis> Axes {
            get { return mAxes; }
        }

        public override bool Enabled {
            get { return base.Enabled; }
            set {
                if (value != base.Enabled) {
                    base.Enabled = value;

                    if (mCore != null) {
                        if (value)
                            mCore.Tick += mTickListener;
                        else
                            mCore.Tick -= mTickListener;
                    }

                    foreach (var axis in mAxes)
                        axis.Enabled = value;

                    Logger.Debug(value ? "Enabled" : "Disabled");
                }
            }
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
            Logger = LogManager.GetLogger(name);
            mName = name;
            mConfig = new AxisConfig(name);

            mTickListener = new Action(mCore_Tick);

            foreach (var axis in axes)
                if (axis != null)
                    AddAxis(axis);

#if DEBUG
            StatisticsCollection.AddStatistics(mStatistics, name);
#endif
        }

        public void AddAxis(IAxis axis) {
            mAxes.Add(axis);
            if (mCore != null && axis is ITickListener)
                (axis as ITickListener).Init(mCore);
            if (axis is ConstrainedAxis) {
                ConstrainedAxis ax = axis as ConstrainedAxis;
                ax.Deadzone.Value = AxConfig.GetDeadzone(axis.Name);
                ax.Scale.Value  = AxConfig.GetScale(axis.Name);
            }
            if (axis.Binding == AxisBinding.NotSet)
                axis.Binding = AxConfig.GetBinding(axis.Name);
            if (AxisAdded != null)
                AxisAdded(axis);
        }

        void axis_Changed() {
            TriggerChange(this);
        }

        #region IDeltaInput Members

        public override Vector3 PositionDelta {
            get {
                float x = mAxes.Where(a => a.Binding == AxisBinding.X).Sum(a => a.Delta);
                float y = mAxes.Where(a => a.Binding == AxisBinding.Y).Sum(a => a.Delta);
                float z = mAxes.Where(a => a.Binding == AxisBinding.Z).Sum(a => a.Delta);
                return new Vector3(x, y, z) * mScale;
            }
        }

        public override Rotation OrientationDelta {
            get { 
                float p = mAxes.Where(a => a.Binding == AxisBinding.Pitch).Sum(a => a.Delta);
                float y = mAxes.Where(a => a.Binding == AxisBinding.Yaw).Sum(a => a.Delta);
                return new Rotation(p * mScale * mRotXMove, y * mScale * mRotXMove);
            }
        }

        public override Vector2 MouseDelta
        {
            get
            {
                float x = mAxes.Where(a => a.Binding == AxisBinding.MouseX).Sum(a => a.Delta);
                float y = mAxes.Where(a => a.Binding == AxisBinding.MouseY).Sum(a => a.Delta);
                return new Vector2(x, y);
            }
        }

        public override void Init(Core core) {
            base.Init(core);
            mCore = core;
            foreach (var axis in mAxes)
                if (axis is ITickListener)
                    (axis as ITickListener).Init(core);

            if (Enabled)
                core.Tick += mTickListener;
        }

        void mCore_Tick() {
#if DEBUG
            mStatistics.Begin();
#endif
            TriggerChange(this);
#if DEBUG
            mStatistics.End();
#endif
        }

        #endregion

        #region IInput Members

        public override Control ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new AxisBasedDeltaPanel(this);
                return mPanel;
            }
        }

        public override string Name {
            get { return mName; }
        }

        public override string State {
            get { throw new NotImplementedException(); }
        }

        public override ConfigBase Config {
            get { throw new NotImplementedException(); }
        }

        public override void Close() { }

        public override void Draw(Graphics graphics, Func<Vector3, Point> to2D, Action redraw, Perspective perspective) { }

        #endregion

        protected virtual AxisConfig AxConfig {
            get { return mConfig; }
        }

        public class AxisConfig : ConfigFolderBase {
            private string mType;

            public AxisConfig(string type)
                : base(type, new string[0]) {
                mType = type;
            }

            public override string Group {
                get { return mType; }
            }

            protected override void InitConfig() {
            }

            public float GetDeadzone(string name) {
                return Get("Deadzones", name, .1f, "");
            }

            public float GetScale(string name) {
                return Get("Scales", name, 1f, "");
            }

            public AxisBinding GetBinding(string name) {
                //return (AxisBinding) Enum.Parse(typeof(AxisBinding), Get("Bindings", name, "None", ""));
                return GetEnum<AxisBinding>("Bindings", name, AxisBinding.None, "The output axis that " + name + " is bound to.", LogManager.GetLogger(name));
            }
        }

        #region ISystemPlugin Members

        void ISystemPlugin.SetForm(Form form) { }

        #endregion
    }
}
