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
        private int mJitter = 1;

        public MouseInput() {
            InputConfig cfg = new InputConfig();
            mEnabled = cfg.MouseEnabled;
        }

        void coordinator_Tick() {
            if (!mEnabled)
                return;

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
            window.Overlay.UpdateCursor((double)x / (double)bounds.Width, (double)y / (double)bounds.Height);
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
                    mPanel = new MouseInputPanel();
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
            get { throw new NotImplementedException(); }
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
