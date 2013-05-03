using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;

namespace Chimera.Overlay {
    public class OverlayConfig : ConfigBase {
        public string IdleState;
        public string HomeState;
        public bool InitOverlay;
        public int IdleTimeoutMs;

        public override string Group {
            get { return "Overlay"; }
        }

        public OverlayConfig(params string[] args)
            : base(args) { }

        protected override void InitConfig() {
            IdleState = Get(true, "IdleState", "None", "The state which the system should launch when idle. If not set no idle state will be configured.");
            HomeState = Get(true, "HomeState", "None", "The state which the system should start in and return to whenever the idle state ceases.");
            InitOverlay = Get(true, "InitOverlay", true, "If true the overlay will be initialised. If false, no overlay will be loaded.");
            IdleTimeoutMs = Get(true, "IdleTimeout", 30000, "How long the system should be left alone for before it's considered idle.");
        }
    }
}
