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
using log4net;

namespace Chimera.Overlay.Plugins {
    public class MousePlugin : ISystemPlugin {
#if DEBUG
        private readonly TickStatistics mStatistics = new TickStatistics();
#endif
        private MousePluginPanel mPanel;
        private OverlayPlugin mOverlayPlugin;
        private Point mLastMouse;
        private PointF mLastCursor;
        private bool mEnabled;

        private Action mTickListener;
        private Core mCore;

        private ILog Logger = LogManager.GetLogger("MousePlugin");

        public event Action<int, int> MouseMoved;

        public MousePlugin() {
            PluginConfig cfg = new PluginConfig();
            mTickListener = new Action(mCore_Tick);

#if DEBUG
            StatisticsCollection.AddStatistics(mStatistics, "Mouse Plugin");
#endif
        }

        public PointF LastCursor {
            get { return mLastCursor; }
        }

        void mCore_Tick() {
            if (!mEnabled)
                return;

#if DEBUG
            mStatistics.Begin();
#endif
            if (MouseMoved != null)
                MouseMoved(Cursor.Position.X, Cursor.Position.Y);

            foreach (var manager in mOverlayPlugin.OverlayManagers) {
                Rectangle bounds = manager.Frame.Monitor.Bounds;
                if (bounds.Contains(Cursor.Position)) {
                    if (mLastMouse.X != Cursor.Position.X || mLastMouse.Y != Cursor.Position.Y) {
                        Update(manager, bounds, Cursor.Position.X - bounds.Left, Cursor.Position.Y - bounds.Top);
                        mLastMouse = Cursor.Position;
                        mLastCursor = manager.CursorPosition;
                    } 
#if DEBUG
                    mStatistics.End();
#endif
                    return;
                } 
#if DEBUG
                mStatistics.End();
#endif
            }
        }

        private void Update(FrameOverlayManager manager, Rectangle bounds, int x, int y) {
            mOverlayPlugin[manager.Name].UpdateCursor((double)x / (double)bounds.Width, (double)y / (double)bounds.Height);
        }

        #region ISystemPluginMembers

        public void Init(Core core) {
            if (!core.HasPlugin<OverlayPlugin>()) {
                Logger.Warn("Unable to initialise MousePlugin. No OverlayPlugin registered with the coordinator.");
            } else {
                mOverlayPlugin = core.GetPlugin<OverlayPlugin>();
                mCore = core;
            }

            //if (mEnabled)
                //core.Tick += mTickListener;
        }


        public void SetForm(Form form) { }

        #endregion

        #region IPlugin Members

        public event Action<IPlugin, bool> EnabledChanged;

        public Control ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new MousePluginPanel(this);
                return mPanel; 
            }
        }

        public virtual bool Enabled {
            get { return mEnabled; }
            set {
                if (mOverlayPlugin == null) {
                    if (EnabledChanged != null)
                        EnabledChanged(this, false);
                    return;
                }

                mEnabled = value;
                if (EnabledChanged != null)
                    EnabledChanged(this, value);

                if (mOverlayPlugin != null) {
                    if (value)
                        mCore.Tick += mTickListener;
                    else
                        mCore.Tick -= mTickListener;
                }
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
