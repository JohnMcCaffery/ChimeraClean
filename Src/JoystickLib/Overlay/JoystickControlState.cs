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

namespace Chimera.Joystick.Overlay
{
    public class JoystickControlStateFactory : IStateFactory
    {
        #region IFactory<State> Members

        public State Create(OverlayPlugin manager, XmlNode node)
        {
            return new JoystickControlState(manager, node);
        }

        public State Create(OverlayPlugin manager, XmlNode node, Rectangle clip)
        {
            return Create(manager, node);
        }

        #endregion

        #region IFactory Members

        public string Name
        {
            get { return "JoystickControl"; }
        }

        #endregion
    }

    public class JoystickControlState : State
    {
        private bool mAvatar;
        private List<CursorTrigger> mClickTriggers = new List<CursorTrigger>();
        private Rotation mStartOrientation;
        private Vector3 mStartPosition;

        public JoystickControlState(string name, OverlayPlugin manager, bool avatar)
            : base(name, manager)
        {
            mAvatar = avatar;

            mStartOrientation = new Rotation(manager.Core.Orientation);
            mStartPosition = manager.Core.Position;
        }

        public JoystickControlState(OverlayPlugin manager, XmlNode node)
            : base(GetName(node, "joystick movement state"), manager, node)
        {

            mAvatar = GetBool(node, true, "Avatar");
        }

        protected override void TransitionToFinish()
        {
        }

        protected override void TransitionFromStart()
        {
        }

        protected override void TransitionToStart()
        {
            Manager.Core.ControlMode = mAvatar ? ControlMode.Delta : ControlMode.Absolute;
            if (!mAvatar)
            {
                Manager.Core.EnableUpdates = true;
            }
            Manager.Core.EnableUpdates = true;
            Manager.ControlPointers = false;
        }

        protected override void TransitionFromFinish() { }
    }
}
