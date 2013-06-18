using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay;
using log4net;
using System.Xml;

namespace Chimera.Flythrough.Overlay {
    public class FlythroughEndTriggerFactory : ITriggerFactory {
        public SpecialTrigger Special {
            get { return SpecialTrigger.None; }
        }

        public string Mode {
            get { return "None"; }
        }

        public ITrigger Create(OverlayPlugin manager, System.Xml.XmlNode node) {
            return new FlythroughEndTrigger(manager, node);
        }

        public ITrigger Create(OverlayPlugin manager, System.Xml.XmlNode node, System.Drawing.Rectangle clip) {
            return Create(manager, node);
        }

        public string Name {
            get { return "FlythroughEnd"; }
        }
    }


    public class FlythroughEndTrigger : XmlLoader, ITrigger {
        private ILog Logger = LogManager.GetLogger("Flythrough.Overlay");
        private FlythroughPlugin mPlugin;
        private EventHandler mFlythroughEndListener;
        private bool mActive;

        public FlythroughEndTrigger(OverlayPlugin overlayPlugin, XmlNode node)
            : base(node) {
            if (!overlayPlugin.Core.HasPlugin<FlythroughPlugin>()) {
                Logger.Warn("Unable to instantiate FlythroughEndTrigger FlythroughPlugin is not registered.");
                return;
            }

            mFlythroughEndListener = new EventHandler(mPlugin_SequenceFinished);
            mPlugin = overlayPlugin.Core.GetPlugin<FlythroughPlugin>();
        }

        void mPlugin_SequenceFinished(object source, EventArgs args) {
            if (Triggered != null)
                Triggered();
        }

        #region ITrigger Members

        public event Action Triggered;

        public bool Active {
            get { return mActive; }
            set {
                if (mActive != value) {
                    mActive = value;
                    if (value)
                        mPlugin.SequenceFinished += mFlythroughEndListener;
                    else
                        mPlugin.SequenceFinished -= mFlythroughEndListener;
                }
            }
        }

        #endregion
    }
}
