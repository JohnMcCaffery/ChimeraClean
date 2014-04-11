using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay.GUI.Triggers;
using System.Xml;

namespace Chimera.Overlay.Triggers {
    public abstract class TriggerBase : OverlayXmlLoader, ITrigger {
        private TriggerPanel mPanel;

        public TriggerBase()
            : base() {
        }
        public TriggerBase(XmlNode node)
            : base(node) {
        }

        public virtual void Trigger() {
            if (Active && Triggered != null)
                Triggered(this);
        }

        public virtual event Action<ITrigger> Triggered;

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
