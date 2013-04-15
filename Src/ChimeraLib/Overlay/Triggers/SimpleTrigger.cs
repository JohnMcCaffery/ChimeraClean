using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay.Triggers {
    public class SimpleTrigger : ITrigger {
        public event Action Triggered;

        public bool Active {
            get { return true; }
            set { }
        }

        public void Trigger() {
            if (Triggered != null)
                Triggered();
        }
    }
}
