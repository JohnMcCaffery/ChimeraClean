using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chimera {
    public abstract class WindowState : IWindowState {
        public Chimera.IDrawable[] Features {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        /// <summary>
        /// How seethrough the window on which this state is being drawn should be.
        /// </summary>
        public double Opacity {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        /// <summary>
        /// Force the window this state is being drawn on to redraw.
        /// </summary>
        public void Redraw() {
            throw new System.NotImplementedException();
        }
    }
}
