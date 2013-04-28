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
using Chimera.GUI.Controls.Plugins;
using Chimera.Kinect.GUI.Axes;

namespace Chimera.Kinect.Axes {
    public class PushAxis : SplitAxis {
        public PushAxis(bool right, AxisBinding binding)
            : base(
            "Push" + (right ? "Right" : "Left"), 
            binding,
            new PushSingleAxis(right, true), 
            new PushSingleAxis(right, false)) { }

        public PushAxis(bool right)
            : this(right, AxisBinding.None) {
        }

        public class PushSingleAxis : ConstrainedAxis {
            private static readonly float DZ = .3f;
            private static readonly float SCALE = .05f;
            private static readonly float BACKDZ = DZ / 5f;
            private Scalar mDiff;
            private Scalar mValue;
            private PushAxisPanel mPanel;

            public Scalar Diff {
                get { return mDiff; }
            }

            public override System.Windows.Forms.UserControl ControlPanel {
                get {
                    if (mPanel == null)
                        mPanel = new PushAxisPanel(this);
                    return mPanel;
                }
            }

            public PushSingleAxis(bool right, bool forward)
                : this(right, forward, AxisBinding.None) {
            }

            public PushSingleAxis(bool right, bool forward, AxisBinding binding)
                : base("Push " + (right ? "Right" : "Left") + (forward ? "+" : "-"), forward ? DZ : BACKDZ, SCALE, binding) {

                mMirror = false;

                //How far pushing forward
                mDiff = Nui.z(Nui.joint(right ? Nui.Hand_Right : Nui.Hand_Left) - Nui.joint(Nui.Hip_Centre));
                if (forward)
                    mDiff *= -1f;

                //Whether the push gesture could be active
                Condition active = right ? GlobalConditions.ActiveR : GlobalConditions.ActiveL;
                //The value for the push gesture
                mValue = Nui.ifScalar(active, mDiff, 0f);

                Nui.Tick += new ChangeDelegate(Nui_Tick);
            }

            void Nui_Tick() {
                SetRawValue(mValue.Value);
            }
        }
    }
}
