using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay.States {
    public class BlankStateFactory : IStateFactory {
        public State Create(OverlayPlugin manager, XmlNode node) {
            return new BlankState(manager, node);
        }

        public State Create(OverlayPlugin manager, XmlNode node, System.Drawing.Rectangle clip) {
            return Create(manager, node);
        }

        public string Name {
            get { return "Blank"; }
        }
    }

    public class BlankState : State {
        public BlankState(OverlayPlugin plugin, XmlNode node)
            : base(GetName(node, "Blank State"), plugin) {
        }

        public override Chimera.Interfaces.Overlay.IWindowState CreateWindowState(WindowOverlayManager manager) {
            return new WindowState(manager);
        }

        protected override void TransitionToStart() { }

        protected override void TransitionToFinish() { }

        protected override void TransitionFromStart() { }

        protected override void TransitionFromFinish() { }
    }
}
