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
using Chimera.Config;

namespace Chimera.ConfigurationTool.Controls {
    public partial class AxisConfigurationPanel : UserControl {
        private BindingsControlPanel mBindings;
        private Type mConfigType;
        private ConfigurationObjectControlPanel mPanel;

        public AxisConfigurationPanel() {
            InitializeComponent();
        }

        public AxisConfigurationPanel(AxisConfig config, BindingsControlPanel bindings) : this() {
            mBindings = bindings;
            mConfigType = config.GetType();

            Init(config);
        }

        private void Init(AxisConfig config) {

            foreach (var axis in mBindings.GetBoundClasses<IAxis>().Where(ax => !ax.IsAbstract)) {
                if (IsConstrained(axis)) {
                    config.GetDeadzone(axis.Name);
                    config.GetScale(axis.Name);
                }
                config.GetBinding(axis.Name);
            }

            mPanel = new ConfigurationObjectControlPanel(config);
            mPanel.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            mPanel.Location = new Point(0, reloadButton.Height);
            mPanel.Width = Width;
            mPanel.Height = Height - mPanel.Location.Y;

            Controls.Add(mPanel);
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

        private void reloadButton_Click(object sender, EventArgs e) {
            AxisConfig config = (AxisConfig)mConfigType.Assembly.CreateInstance(mConfigType.FullName);

            Controls.Remove(mPanel);
            mPanel.Dispose();

            Init(config);
        }
    }
}
