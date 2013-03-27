using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Util;
using Chimera.GUI.Controls.Inputs;
using System.Drawing;

namespace Chimera.Inputs {
    public class MouseInput : ISystemInput {
        private Coordinator mCoordinator;
        private MouseInputPanel mPanel;
        private Point mLastMouse;
        private bool mEnabled;
        private bool mMouseOnScreen;

        public event Action<int, int> MouseMoved;

        public MouseInput() {
            InputConfig cfg = new InputConfig();
            mEnabled = cfg.MouseEnabled;
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
            //window.Overlay.UpdateCursor((double)x / (double)bounds.Width, (double)y / (double)bounds.Height);
        }

        #region ISystemInputMembers

        public event Action<IInput, bool> EnabledChanged;

        public Coordinator Coordinator {
            get { return mCoordinator; }
        }

        public void Init(Coordinator coordinator) {
            mCoordinator = coordinator;
            coordinator.Tick += new Action(coordinator_Tick);
        }

        public UserControl ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new MouseInputPanel(this);
                return mPanel; 
            }
        }

        #endregion

        #region IInput Members

        public virtual bool Enabled {
            get { return mEnabled; }
            set { 
                mEnabled = value;
                if (EnabledChanged != null)
                    EnabledChanged(this, value);
            }
        }

        public string Name {
            get { return "Mouse Input"; }
        }

        public string State {
            get { 
                string dump = "-KB/Mouse Input-" + Environment.NewLine;
                dump += "LastPosition" + mLastMouse + Environment.NewLine;
                dump += "Onscreen: " + mMouseOnScreen + Environment.NewLine;
                return dump;
            }
        }

        public ConfigBase Config {
            get { throw new NotImplementedException(); }
        }

        public void Close() { }

        public void Draw(Perspective perspective, System.Drawing.Graphics graphics) {
            throw new NotImplementedException();
        }


        #endregion
    }
}
