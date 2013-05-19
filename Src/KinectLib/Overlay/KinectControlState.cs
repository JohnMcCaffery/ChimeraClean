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
using Chimera.Overlay.Drawables;
using System.Drawing;
using Chimera.Overlay.Triggers;
using Chimera.Util;
using OpenMetaverse;

namespace Chimera.Kinect.Overlay {
    public class KinectControlState : State {
        private KinectMovementPlugin mInput;
        private bool mAvatar;
        private List<CursorTrigger> mClickTriggers = new List<CursorTrigger>();
        private Rotation mStartOrientation;
        private Vector3 mStartPosition;

        public override IWindowState CreateWindowState(WindowOverlayManager manager) {
            return new KinectControlWindowState(manager);
        }

        public KinectControlState(string name, StateManager manager, bool avatar)
            : base(name, manager) {

            mInput = manager.Coordinator.GetPlugin<KinectMovementPlugin>();
            mAvatar = avatar;

            mStartOrientation = new Rotation(manager.Coordinator.Orientation);
            mStartPosition = manager.Coordinator.Position;

            mInput.FlyEnabled = false;
            mInput.WalkEnabled = false;
            mInput.YawEnabled = false;
        }

        protected override void TransitionToFinish() {
            mInput.FlyEnabled = true;
            mInput.WalkEnabled = true;
            mInput.YawEnabled = true;
            mInput.Enabled = true;
        }

        protected override void TransitionFromStart() { 
            mInput.FlyEnabled = false;
            mInput.WalkEnabled = false;
            mInput.YawEnabled = false;       
        }

        public override void TransitionToStart() {
            Manager.Coordinator.ControlMode = mAvatar ? ControlMode.Delta : ControlMode.Absolute;
            if (!mAvatar) {
                Manager.Coordinator.EnableUpdates = true;
                Manager.Coordinator.Update(mStartPosition, Vector3.Zero, mStartOrientation, Rotation.Zero);
                Manager.Coordinator.EnableUpdates = false;
            }
            Manager.Coordinator.EnableUpdates = true; 
            foreach (var manager in Manager.OverlayManagers)
                manager.ControlPointer = false;
        }

        public override void TransitionFromFinish() { }
    }
}
