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
using SlimDX.XInput;
using Chimera.Plugins;
using Chimera;
using Chimera.Interfaces;

namespace Joystick {
    public class ThumbstickAxisLX : ThumbstickAxis {
        public ThumbstickAxisLX() : base(true, true, AxisBinding.NotSet) {
        }
    }
    public class ThumbstickAxisLY : ThumbstickAxis {
        public ThumbstickAxisLY() : base(true, false, AxisBinding.NotSet) {
        }
    }
    public class ThumbstickAxisRX : ThumbstickAxis {
        public ThumbstickAxisRX() : base(false, true, AxisBinding.NotSet) {
        }
    }
    public class ThumbstickAxisRY : ThumbstickAxis {
        public ThumbstickAxisRY() : base(false, false, AxisBinding.NotSet) {
        }
    }

    public class ThumbstickAxis : ConstrainedAxis, ITickListener {
        private bool mLeft;
        private bool mX;

        private static string MakeName(bool left, bool x) {
            return (left ? "Left" : "Right") + "Thumbstick" + (x ? "X" : "Y");
        }

        public ThumbstickAxis(bool left, bool x, AxisBinding binding)
            : base(MakeName(left, x), short.MaxValue / 3f, .00005f, binding) {

            mLeft = left;
            mX = x;
        }

        public ThumbstickAxis(bool left, bool x)
            : this (left, x, AxisBinding.NotSet) {
        }

        public void Init(ITickSource source) {
            GamepadManager.Init(source);
        }

        protected override float RawValue {
            get {
                if (!GamepadManager.Initialised)
                    return 0f;

                Gamepad g = GamepadManager.Gamepad;
                if (mLeft)
                    return mX ? g.LeftThumbX : g.LeftThumbY;
                else
                    return mX ? g.RightThumbX : g.RightThumbY;
            }
        }
    }
}
