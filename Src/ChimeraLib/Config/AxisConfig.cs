using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces;
using Chimera.Plugins;

namespace Chimera.Config {
    public class AxisConfig : ConfigFolderBase {
        private string mName;

        public AxisConfig(string name)
            : base(name, new string[0]) {
            mName = name;
        }

        public override string Group {
            get { return mName; }
        }

        protected override void InitConfig() { }

        public float GetDeadzone(string name) {
            return Get("Deadzones", name, .1f, "");
        }

        public float GetScale(string name) {
            return Get("Scales", name, 1f, "");
        }

        public AxisBinding GetBinding(string name) {
            return (AxisBinding)Enum.Parse(typeof(AxisBinding), Get("Bindings", name, "None", ""));
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
