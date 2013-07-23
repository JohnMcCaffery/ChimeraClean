/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay;
using Chimera.Overlay.Features;
using System.Drawing;
using Chimera.Overlay.Triggers;
using Chimera.Util;
using OpenMetaverse;
using System.Xml;

namespace Chimera.Kinect.Overlay {
    public class KinectControlStateFactory : IStateFactory {
        #region IFactory<State> Members

        public State Create(OverlayPlugin manager, XmlNode node) {
            return new KinectControlState(manager, node);
        }

        public State Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }

        #endregion

        #region IFactory Members

        public string Name {
            get { return "KinectControl"; }
        }

        #endregion
    }

    public class KinectControlState : State {
        private KinectMovementPlugin mInput;
        private bool mAvatar;
        private List<CursorTrigger> mClickTriggers = new List<CursorTrigger>();
        private Rotation mStartOrientation;
        private Vector3 mStartPosition;
        private bool mSetPosition;

        public override IFrameState CreateWindowState(FrameOverlayManager manager) {
            return new KinectControlWindowState(manager);
        }

        public KinectControlState(string name, OverlayPlugin manager, bool avatar)
            : base(name, manager) {

            mInput = manager.Core.GetPlugin<KinectMovementPlugin>();
            mAvatar = avatar;

            mStartOrientation = new Rotation(manager.Core.Orientation);
            mStartPosition = manager.Core.Position;
        }

        public KinectControlState(OverlayPlugin manager, XmlNode node)
            : base(GetName(node, "kinect movement state"), manager, node) {

            mInput = manager.Core.GetPlugin<KinectMovementPlugin>();
            mAvatar = GetBool(node, true, "Avatar");

            double pitch = GetDouble(node, manager.Core.Orientation.Pitch);
            double yaw = GetDouble(node, manager.Core.Orientation.Yaw);
            float x = GetFloat(node, manager.Core.Position.X, "X");
            float y = GetFloat(node, manager.Core.Position.Y, "Y");
            float z = GetFloat(node, manager.Core.Position.Z, "Z");
            mStartOrientation = new Rotation(pitch, yaw);
            mStartPosition = new Vector3(x, y, z);

            mSetPosition = (node.Attributes["X"] !=  null && node.Attributes["Y"] != null && node.Attributes["Z"] != null) || node.Attributes["Pitch"] != null || node.Attributes["Yaw"] != null;
        }

        protected override void TransitionToFinish() {
            mInput.Enabled = true;
        }

        protected override void TransitionFromStart() {
            mInput.Enabled = false;
        }

        protected override void TransitionToStart() {
            Manager.Core.ControlMode = mAvatar ? ControlMode.Delta : ControlMode.Absolute;
            if (!mAvatar) {
                Manager.Core.EnableUpdates = true;
                if (mSetPosition)
                    Manager.Core.Update(mStartPosition, Vector3.Zero, mStartOrientation, Rotation.Zero);
            }
            Manager.Core.EnableUpdates = true;
            Manager.ControlPointers = false;
        }

        protected override void TransitionFromFinish() { }
    }
}
