using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Xml;
using OpenMetaverse;
using Chimera.Util;

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
        private bool mSetPosition;
        private bool mSetOrientation;
        private Vector3 mStartPosition;
        private Rotation mStartOrientation;

        public ExploreState(string name, OverlayPlugin manager)
            : base(name, manager) {
        }

        public ExploreState(OverlayPlugin manager, XmlNode node)
            : base(GetName(node, "explore state"), manager) {

            mAvatar = GetBool(node, false, "Avatar");

            mSetOrientation = node.Attributes["Pitch"] != null || node.Attributes["Yaw"] != null;
            mSetPosition = node.Attributes["X"] != null && node.Attributes["Y"] != null && node.Attributes["Z"] != null;

            mStartOrientation = new Rotation(GetDouble(node, 0.0, "Pitch"), GetDouble(node, 0.0, "Yaw"));
            mStartPosition = new Vector3(GetFloat(node, 0f, "X"), GetFloat(node, 0f, "Y"), GetFloat(node, 0f, "Z"));
        }

        public override IWindowState CreateWindowState(WindowOverlayManager manager) {
            return new WindowState(manager);
        }

        protected override void TransitionToStart() {
            Manager.Coordinator.EnableUpdates = true;
            Manager.Coordinator.ControlMode = mAvatar ? ControlMode.Delta : ControlMode.Absolute;
            Vector3 pos = mSetPosition ? mStartPosition : Manager.Coordinator.Position;
            Rotation rot = mSetOrientation ? mStartOrientation : Manager.Coordinator.Orientation;
            if (mSetPosition || mSetOrientation)
                Manager.Coordinator.Update(pos, Vector3.Zero, rot, Rotation.Zero);
        }

        protected override void TransitionToFinish() {
            TransitionToStart();
        }

        protected override void TransitionFromStart() { }

        protected override void TransitionFromFinish() { }
    }
}
