using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;
using OpenMetaverse;
using Chimera.Util;
using Chimera.Overlay.Drawables;

namespace Chimera.Overlay.States {
    public class SeeThroughMenuState : State {
        private readonly List<SeeThroughMenuWindow> mWindows = new List<SeeThroughMenuWindow>();
        private Vector3 mPosition;
        private Rotation mOrientation;

        public SeeThroughMenuState(string name, StateManager manager, Vector3 position, Rotation orientation)
            : base(name, manager) {

            mPosition = position;
            mOrientation = orientation;
        }

        public override IWindowState CreateWindowState(Window window) {
            SeeThroughMenuWindow stateWindow = new SeeThroughMenuWindow(window);
            mWindows.Add(stateWindow);
            return stateWindow;
        }

        public override void TransitionToStart() {
            Manager.Coordinator.EnableUpdates = true;
            Manager.Coordinator.ControlMode = ControlMode.Absolute;
            Manager.Coordinator.Update(mPosition, Vector3.Zero, mOrientation, Rotation.Zero);
            Manager.Coordinator.EnableUpdates = false;

            foreach (var window in mWindows)
                window.ResetToTransparent();
        }

        protected override void TransitionToFinish() {
            TransitionToStart();
        }

        protected override void TransitionFromStart() {
            foreach (var window in mWindows)
                window.TransitionFromState();
        }

        public override void TransitionFromFinish() {
            foreach (var window in mWindows)
                window.ResetToTransparent();
        }

        private class SeeThroughMenuWindow : WindowState {
            private Bitmap mFadeBG;

            public SeeThroughMenuWindow(Window window)
                : base(window.OverlayManager) {
            }

            internal void TransitionFromState() {
                mFadeBG = new Bitmap(Manager.Window.Monitor.Bounds.Width, Manager.Window.Monitor.Bounds.Height);
                using (Graphics g = Graphics.FromImage(mFadeBG)) {
                    g.CopyFromScreen(Manager.Window.Monitor.Bounds.Location, Point.Empty, Manager.Window.Monitor.Bounds.Size);
                }
            }

            internal void ResetToTransparent() {
                mFadeBG = null;
            }

            protected override void OnActivated() { }

            public override void RedrawStatic(Rectangle clip, Graphics graphics) {
                if (mFadeBG != null)
                    graphics.DrawImage(mFadeBG, Point.Empty);
                else {
                    using (Pen p = new Pen(Color.FromArgb(200, Color.White)))
                        graphics.DrawRectangle(p, clip);
                    base.RedrawStatic(clip, graphics);
                }
            }
        }
    }
}
