﻿/*************************************************************************
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
using SlimDX.XInput;
using Chimera.Plugins;
using Chimera;
using Chimera.Interfaces;

namespace Joystick {
    public class TriggerAxis : ConstrainedAxis, ITickListener {
        private bool mLeftUp;
        
        public TriggerAxis(bool leftUp, AxisBinding binding)
            : base("Trigger", 0, .0005f, binding) {

            mLeftUp = leftUp;
        }
       
        public TriggerAxis(bool leftUp)
            : this(leftUp, AxisBinding.NotSet) {
        }

        public TriggerAxis() : this (false) { }


        public void Init(ITickSource source) {
            GamepadManager.Init(source);
        }

        protected override float RawValue {
            get {
                if (!GamepadManager.Initialised)
                    return 0f;

                Gamepad g = GamepadManager.Gamepad;

                float l = g.LeftTrigger * (mLeftUp ? 1f : -1f);
                float r = g.RightTrigger * (mLeftUp ? -1f : 1f);

                return l + r;
            }
        }
    }
}
