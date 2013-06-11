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
using Chimera.GUI.Controls.Plugins;
using Chimera.Util;
using OpenMetaverse;
using System.Drawing;
using Chimera.Config;

namespace Chimera.Plugins {
    public class PluginSelector : ISystemPlugin {
        private readonly List<ISystemPlugin> mPlugins = new List<ISystemPlugin>();
        private readonly string mName;

        private ISystemPlugin mCurrentInput;
        private PluginSelectorPanel mControlPanel;
        private Core mCoordinator;
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

        public void Init(Core coordinator) {
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

        public void Draw(Graphics graphics, Func<Vector3, Point> to2D, Action redraw, Perspective perspective) {
            if (mCurrentInput != null)
                mCurrentInput.Draw(graphics, to2D, redraw, perspective);
        }

        #endregion

        #region ISystemPlugin Members


        public void SetForm(System.Windows.Forms.Form form) {
        }

        #endregion
    }
}
