using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.GUI.Controls.Plugins;
using Chimera.Util;
using OpenMetaverse;
using System.Drawing;

namespace Chimera.Plugins {
    public class PluginSelector : ISystemPlugin {
        private readonly List<ISystemPlugin> mPlugins = new List<ISystemPlugin>();
        private readonly string mName;

        private ISystemPlugin mCurrentInput;
        private PluginSelectorPanel mControlPanel;
        private Coordinator mCoordinator;
        private bool mEnabled;

        public event Action<ISystemPlugin> PluginAdded;

        public ISystemPlugin CurrentPlugin {
            get { return mCurrentInput; }
            set {
                if (mCurrentInput != null)
                    mCurrentInput.Enabled = false;
                mCurrentInput = value;
                mCurrentInput.Enabled = mEnabled;
            }
        }

        public IEnumerable<ISystemPlugin> Plugins {
            get { return mPlugins; }
        }

        public PluginSelector(string name, params ISystemPlugin[] inputs) {
            mName = name;

            foreach (var input in inputs)
                AddInput(input);
        }

        public void AddInput(ISystemPlugin input) {
            if (mCurrentInput == null) {
                mCurrentInput = input;
                mCurrentInput.Enabled = mEnabled;
            }
            mPlugins.Add(input);
            if (PluginAdded != null)
                PluginAdded(input);
        }

        #region ISystemPlugin Members

        public void Init(Coordinator coordinator) {
            mCoordinator = coordinator;
            foreach (var plugin in mPlugins)
                plugin.Init(coordinator);
        }

        #endregion

        #region IInput Members

        public event Action<IPlugin, bool> EnabledChanged;

        public System.Windows.Forms.UserControl ControlPanel {
            get {
                if (mControlPanel == null)
                    mControlPanel = new PluginSelectorPanel(this);
                return mControlPanel;
            }
        }

        public bool Enabled {
            get { return mEnabled; }
            set {
                if (mEnabled != value) {
                    mEnabled = value;
                    if (mCurrentInput != null)
                        mCurrentInput.Enabled = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public string Name {
            get { return mName; }
        }

        public string State {
            get {
                string ret = mName + " -- combined input. Current Input: " + mCurrentInput.Name + Environment.NewLine;
                if (mCurrentInput != null) {
                    ret += mCurrentInput.State;
                    ret += "---------------------------" + Environment.NewLine;
                }
                foreach (var input in mPlugins) {
                    if (input != mCurrentInput) {
                        ret += input.State;
                        ret += "---------------------------" + Environment.NewLine;
                    }
                }
                return ret;
            }
        }

        public ConfigBase Config {
            get { throw new NotImplementedException(); }
        }

        public void Close() {
            foreach (var input in mPlugins)
                input.Close();
        }

        public void Draw(Func<Vector3, Point> to2D, Graphics graphics, Action redraw) {
            if (mCurrentInput != null)
                mCurrentInput.Draw(to2D, graphics, redraw);
        }

        #endregion
    }
}
