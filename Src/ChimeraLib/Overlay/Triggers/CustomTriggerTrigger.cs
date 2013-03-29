using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay.Triggers {
    public class CustomTriggerTrigger : ITrigger {
        private bool mActive;
        private string mKey;

        public event Action Triggered;

        public bool Active {
            get { return mActive; }
            set { mActive = value; }
        }

        public CustomTriggerTrigger(StateManager stateManager, string key) {
            mKey = key;
            stateManager.CustomTrigger += new Action<string>(stateManager_CustomTrigger);
        }

        private void stateManager_CustomTrigger(string key) {
            if (Triggered != null && key.Equals(this.mKey))
                Triggered();
        }
    }
}
