using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay;
using Chimera.Interfaces.Overlay;
using Chimera;
using System.Xml;

namespace Touchscreen.Overlay {
    public class TouchscreenState : State {
        private TouchscreenPlugin mPlugin;
        private bool mAvatar;

        public TouchscreenState(string name, bool avatar, OverlayPlugin manager, XmlNode node)
            : base(name, manager, node) {

            mPlugin = manager.Core.GetPlugin<TouchscreenPlugin>();
            mPlugin.Enabled = false;
            mAvatar = avatar;
        }

        public override IFrameState CreateFrameState(FrameOverlayManager manager) {
            if (manager.Name.Equals(mPlugin.Frame.Name))
                return new TouchscreenWindow(manager, mPlugin);
            return new FrameState(manager);
        }

        protected override void TransitionToStart() {
            Manager.Core.ControlMode = mAvatar ? ControlMode.Delta : ControlMode.Absolute;
            mPlugin.Enabled = true;
        }

        protected override void TransitionToFinish() {
            Manager.Core.EnableUpdates = true;
        }

        protected override void TransitionFromStart() {
            Manager.Core.EnableUpdates = false;
            mPlugin.Enabled = false;
        }

        protected override void TransitionFromFinish() { }
    }
}
