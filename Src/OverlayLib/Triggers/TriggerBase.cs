using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay.GUI.Triggers;

namespace Chimera.Overlay.Triggers {
    public abstract class TriggerBase : XmlLoader, ITrigger {
        private TriggerPanel mPanel;

        public void Trigger() {
            if (Triggered != null)
                Triggered();
        }

        public event Action Triggered;

        public abstract bool Active { get; set; }

        public override System.Windows.Forms.Control ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new TriggerPanel(this);
                return mPanel;
            }
        }
    }
}
