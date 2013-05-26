using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Xml;

namespace Chimera.Overlay.States {
    public class ExploreStateFactory : IStateFactory {

        public State Create(OverlayPlugin manager, System.Xml.XmlNode node) {
            return new ExploreState(manager, node);
        }

        public State Create(OverlayPlugin manager, System.Xml.XmlNode node, System.Drawing.Rectangle clip) {
            return Create(manager, node);
        }

        public string Name {
            get { return "Explore"; }
        }
    }


    public class ExploreState : State {
        private bool mAvatar;

        public ExploreState(string name, OverlayPlugin manager)
            : base(name, manager) {
        }

        public ExploreState(OverlayPlugin manager, XmlNode node)
            : base(GetName(node, "explore state"), manager) {

            mAvatar = GetBool(node, false, "Avatar");
        }

        public override IWindowState CreateWindowState(WindowOverlayManager manager) {
            return new WindowState(manager);
        }

        public override void TransitionToStart() {
            Manager.Coordinator.EnableUpdates = true;
            Manager.Coordinator.ControlMode = mAvatar ? ControlMode.Delta : ControlMode.Absolute;
        }

        protected override void TransitionToFinish() { }

        protected override void TransitionFromStart() { }

        public override void TransitionFromFinish() { }
    }
}
