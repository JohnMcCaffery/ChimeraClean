using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera.Overlay {
    class BoxArea : ISelectable {
        private RectangleF mBounds;
        private bool mVisible;
        private Window mWindow;

        public event Action<ISelectable> Selected;

        public event Action<ISelectable> StaticChanged;

        public event Action<ISelectable> Shown;

        public event Action<ISelectable> Hidden;

        public string DebugState {
            get { throw new NotImplementedException(); }
        }

        public bool CurrentlySelected {
            get { throw new NotImplementedException(); }
        }

        public bool CurrentlyHovering {
            get { throw new NotImplementedException(); }
        }

        public bool Visible {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public ISelectionRenderer SelectionRenderer {
            get { throw new NotImplementedException(); }
        }

        public System.Drawing.RectangleF Bounds {
            get { throw new NotImplementedException(); }
        }

        public virtual Rectangle ScaledBounds {
            get {
                return new Rectangle(
                    (int)(mWindow.Monitor.Bounds.Width * mBounds.X),
                    (int)(mWindow.Monitor.Bounds.Height * mBounds.Y),
                    (int)(mWindow.Monitor.Bounds.Width * mBounds.Width),
                    (int)(mWindow.Monitor.Bounds.Height * mBounds.Height));
                }
        }

        public bool Active {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public void Init(Window window) {
            throw new NotImplementedException();
        }

        public void DrawStatic(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipRectangle) {
            throw new NotImplementedException();
        }

        public void Show() {
            throw new NotImplementedException();
        }

        public void Hide() {
            throw new NotImplementedException();
        }

        public void DrawDynamic(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipRectangle) {
            throw new NotImplementedException();
        }
    }
}
