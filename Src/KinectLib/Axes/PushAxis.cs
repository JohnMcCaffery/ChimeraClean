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
using Chimera.Plugins;
using Chimera.Interfaces;
using NuiLibDotNet;
using C = NuiLibDotNet.Condition;

namespace Chimera.Kinect.Axes {
    public class PushAxis : ConstrainedAxis {
        private static readonly float DZ = .3f;
        private static readonly float SCALE = .05f;
        private static readonly float BACKDZ = DZ / 5f;
        private static Scalar mValue;

        public PushAxis(bool right, bool forward)
            : this(right, forward, AxisBinding.None) {
        }

        public PushAxis(bool right, bool forward, AxisBinding binding)
            : base("Push " + (right ? "Left" : "Right"), forward ? DZ : BACKDZ, SCALE, binding) {

            //How far pushing forward
            Scalar diff = Nui.z(Nui.joint(right ? Nui.Hand_Right : Nui.Hand_Left) - Nui.joint(Nui.Hip_Centre)) - .15f;
            //Whether the push gesture could be active
            Condition active = right ? GlobalConditions.ActiveR : GlobalConditions.ActiveL;
            //The value for the push gesture
            mValue = Nui.ifScalar(active, right ? diff * -1f : diff, 0f);

            Nui.Tick += new ChangeDelegate(Nui_Tick);
        }

        void Nui_Tick() {
            SetRawValue(mValue.Value);
        }
    }
}
