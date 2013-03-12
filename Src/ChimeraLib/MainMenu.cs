using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chimera {
    public class MainMenu : IOverlayState {
        #region IOverlayState Members

        public string State {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public string Name {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public string Type {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
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

        public IOverlayArea[] OverlayAreas {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public void Init(Coordinator coordinator) {
            throw new NotImplementedException();
        }

        public void Draw(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipRectangle, System.Drawing.Color transparentColour, Window window) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
