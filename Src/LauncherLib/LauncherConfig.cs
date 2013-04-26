using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;

namespace Chimera.Launcher {
    class OverlayConfig : ConfigBase {
        public bool InitOverlay;
        public bool IdleState;
        public int IdleTimeoutMs;

        public OverlayConfig(params string[] args) : base("appSettings", args) { }

        public override string Group {
            get { return "Overlay"; }
        }

        protected override void InitConfig() {
            InitOverlay = Get(true, "InitOverlay", true, "If true the overlay will be initialised. If false, no overlay will be loaded.");
            IdleState = Get(true, "EnableIdleState", false, "Whether the system should jump to a pre-specified state whenever the system is idle for a specified length of time.");
            IdleTimeoutMs = Get(true, "IdleTimeout", 30000, "How long the system should be left alone for before it's considered idle.");
        }
    }
}
