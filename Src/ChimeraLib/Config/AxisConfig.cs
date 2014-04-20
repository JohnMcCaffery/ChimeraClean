using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces;
using Chimera.Plugins;
using log4net;

namespace Chimera.Config {
    /// <summary>
    /// Must be subclassed with a zero argument constructor that passes the name up. If this is not done config util will not work.
    /// </summary>
    public abstract class AxisConfig : ConfigFolderBase {
        private string mName;

        public AxisConfig(string name)
            : base(name, new string[0]) {
            mName = name;
        }

        public override string Group {
            get { return mName; }
        }

        public virtual bool LoadBoundAxes {
            get { return false; }
        }

        protected override void InitConfig() { }

        public float GetDeadzone(string name) {
            return Get("Deadzones", name, .1f, "");
        }

        public float GetScale(string name) {
            return Get("Scales", name, 1f, "");
        }

        public AxisBinding GetBinding(string name) {
            return GetEnum<AxisBinding>("Bindings", name, AxisBinding.None, "What output axis " + name + " is bound to.", LogManager.GetLogger(Group + "AxisBinding"));
        }

        public void ConfigureAxis(IAxis axis, Core core) {
            if (core != null && axis is ITickListener)
                (axis as ITickListener).Init(core);

            if (axis is ConstrainedAxis) {
                ConstrainedAxis ax = axis as ConstrainedAxis;
                ax.Deadzone.Value = GetDeadzone(axis.Name);
                ax.Scale.Value  = GetScale(axis.Name);
            }
            if (axis.Binding == AxisBinding.NotSet)
                axis.Binding = GetBinding(axis.Name);
        }
 
    }
}
