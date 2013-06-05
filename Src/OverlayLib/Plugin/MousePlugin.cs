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
using Chimera.Overlay;

namespace Chimera.Overlay.Plugins {
    public class MousePlugin : ISystemPlugin {
        private MousePluginPanel mPanel;
        private OverlayPlugin mOverlayPlugin;
        private Point mLastMouse;
        private PointF mLastCursor;
        private bool mEnabled;

        public event Action<int, int> MouseMoved;

        public MousePlugin() {
            PluginConfig cfg = new PluginConfig();
        }

        public PointF LastCursor {
            get { return mLastCursor; }
        }

        void coordinator_Tick() {
            if (!mEnabled)
                return;

            if (MouseMoved != null)
                MouseMoved(Cursor.Position.X, Cursor.Position.Y);

            foreach (var manager in mOverlayPlugin.OverlayManagers) {
                Rectangle bounds = manager.Window.Monitor.Bounds;
                if (bounds.Contains(Cursor.Position)) {
                    if (mLastMouse.X != Cursor.Position.X || mLastMouse.Y != Cursor.Position.Y) {
                        Update(manager, bounds, Cursor.Position.X - bounds.Left, Cursor.Position.Y - bounds.Top);
                        mLastMouse = Cursor.Position;
                        mLastCursor = manager.CursorPosition;
                    } 
                    return;
                } 
            }
        }

        private void Update(WindowOverlayManager manager, Rectangle bounds, int x, int y) {
            mOverlayPlugin[manager.Name].UpdateCursor((double)x / (double)bounds.Width, (double)y / (double)bounds.Height);
        }

        #region ISystemPluginMembers

        public event Action<IPlugin, bool> EnabledChanged;

        public void Init(Coordinator coordinator) {
            if (!coordinator.HasPlugin<OverlayPlugin>())
                throw new ArgumentException("Unable to initialise MousePlugin. No OverlayPlugin registered with the coordinator.");
            mOverlayPlugin = coordinator.GetPlugin<OverlayPlugin>();
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

        public void Draw(System.Drawing.Graphics graphics, Func<Vector3, Point> to2D, Action redraw, Perspective perspective) {
            //Do nothing
        }


        #endregion
    }
}
