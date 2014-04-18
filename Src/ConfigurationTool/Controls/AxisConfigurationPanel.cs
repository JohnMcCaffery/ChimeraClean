using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ConfigurationTool.Controls;
using Chimera.Interfaces;
using Chimera.Plugins;

namespace Chimera.ConfigurationTool.Controls {
    public partial class AxisConfigurationPanel : UserControl {
        private BindingsControlPanel mBindings;

        public AxisConfigurationPanel() {
            InitializeComponent();
        }

        public AxisConfigurationPanel(Type clazz, BindingsControlPanel bindings) : this() {
            //Chimera.Plugins.AxisBasedDelta.AxisConfig config = new Plugins.AxisBasedDelta.AxisConfig("blah");
            mBindings = bindings;

            Chimera.Plugins.AxisBasedDelta.AxisConfig config = (Chimera.Plugins.AxisBasedDelta.AxisConfig)clazz.Assembly.CreateInstance(clazz.Name);
            foreach (var axis in bindings.GetBoundClasses<IAxis>()) {
                if (IsConstrained(axis)) {
                    config.GetDeadzone(axis.Name);
                    config.GetScale(axis.Name);
                }
                config.GetBinding(axis.Name);
            }
        }

        private bool IsConstrained(Type clazz) {
            Type c = typeof(ConstrainedAxis);
            Type p = clazz.BaseType;

            while (p != null) {
                if (p == c)
                    return true;
                p = p.BaseType;
            }
            return false;

        }
    }
}
