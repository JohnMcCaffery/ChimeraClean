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
using System.Windows.Forms;
using Chimera.Util;
using Chimera.GUI.Controls.Plugins;
using System.Drawing;
using OpenMetaverse;
using Chimera.Config;

namespace Chimera.Plugins {
    public class MousePlugin : ISystemPlugin {
        private Coordinator mCoordinator;
        private MousePluginPanel mPanel;
        private Point mLastMouse;
        private bool mEnabled;

        public event Action<int, int> MouseMoved;

        public MousePlugin() {
            PluginConfig cfg = new PluginConfig();
        }

        void coordinator_Tick() {
            if (!mEnabled)
                return;

            if (MouseMoved != null)
                MouseMoved(Cursor.Position.X, Cursor.Position.Y);

            foreach (var window in mCoordinator.Windows) {
                Rectangle bounds = window.Monitor.Bounds;
                if (bounds.Contains(Cursor.Position)) {
                    if (mLastMouse.X != Cursor.Position.X || mLastMouse.Y != Cursor.Position.Y) {
                        Update(window, bounds, Cursor.Position.X - bounds.Left, Cursor.Position.Y - bounds.Top);
                        mLastMouse = Cursor.Position;
                    } 
                    return;
                } 
            }
        }

        private void Update(Window window, Rectangle bounds, int x, int y) {
            //window.OverlayManager.UpdateCursor((double)x / (double)bounds.Width, (double)y / (double)bounds.Height);
        }

        #region ISystemPluginMembers

        public event Action<IPlugin, bool> EnabledChanged;

        public void Init(Coordinator coordinator) {
            mCoordinator = coordinator;
            coordinator.Tick += new Action(coordinator_Tick);
        }

        public UserControl ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new MousePluginPanel(this);
                return mPanel; 
            }
        }

        #endregion

        #region IPlugin Members

        public virtual bool Enabled {
            get { return mEnabled; }
            set { 
                mEnabled = value;
                if (EnabledChanged != null)
                    EnabledChanged(this, value);
            }
        }

        public string Name {
            get { return "MouseCursor"; }
        }

        public string State {
            get { 
                string dump = "-Mouse Plugin-" + Environment.NewLine;
                dump += "LastPosition" + mLastMouse + Environment.NewLine;
                return dump;
            }
        }

        public ConfigBase Config {
            get { throw new NotImplementedException(); }
        }

        public void Close() { }

        public void Draw(Func<Vector3, Point> to2D, System.Drawing.Graphics graphics, Action redraw, Perspective perspective) {
            //Do nothing
        }


        #endregion
    }
}
