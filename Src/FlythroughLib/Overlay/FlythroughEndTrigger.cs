using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay;
using log4net;

namespace Chimera.Flythrough.Overlay {
    public class FlythroughEndTrigger : XmlLoader, ITrigger {
        private ILog Logger = LogManager.GetLogger("Flythrough.Overlay");
        private FlythroughPlugin mPlugin;
        private EventHandler mFlythroughEndListener;
        private bool mActive;

        public FlythroughEndTrigger(OverlayPlugin overlayPlugin) {
            if (!overlayPlugin.Core.HasPlugin<FlythroughPlugin>()) {
                Logger.Warn("Unable to instantiate FlythroughEndTrigger FlythroughPlugin is not registered.");
                return;
            }

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
