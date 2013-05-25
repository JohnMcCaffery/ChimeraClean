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
using Chimera.Kinect.Interfaces;
using System.Windows.Forms;

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

        public class PushSingleAxis : KinectScaledAxis {
            private static readonly float DZ = .3f;
            private static readonly float SCALE = .05f;
            private static readonly float BACKDZ = DZ / 5f;
            private Condition mActive;
            private Scalar mRaw;
            private Scalar mValue;
            private KinectScaledAxisPanel mPanel;

            public override ConstrainedAxis Axis {
                get { return this; }
            }
            public override Condition Active {
                get { return mActive; }
            }
            public override Scalar RawScalar {
                get { return mRaw; }
            }

            public override float RawValue {
                get { return mRaw.Value; }
            }

            public override UserControl ControlPanel {
                get {
                    if (mPanel == null)
                        mPanel = new KinectScaledAxisPanel(this);
                    return mPanel;
                }
            }

            public PushSingleAxis(bool right, bool forward)
                : this(right, forward, AxisBinding.None) { }

            public PushSingleAxis(bool right, bool forward, AxisBinding binding)
                : base("Push" + (right ? "Right" : "Left") + (forward ? "+" : "-"), binding) {

                mMirror = false;

                Vector hand = Nui.joint(right ? Nui.Hand_Right : Nui.Hand_Left);
                Vector elbow = Nui.joint(right ? Nui.Elbow_Right : Nui.Elbow_Left);

                //How far pushing forward
                mRaw = Nui.z(hand - Nui.joint(Nui.Hip_Centre));
                if (forward)
                    mRaw *= -1f;

                //Whether the push gesture could be active
                mActive = right ? GlobalConditions.ActiveR : GlobalConditions.ActiveL;
                //mActive = C.And(mActive, Nui.y(hand) > Nui.y(elbow));
                //The value for the push gesture
                mValue = Nui.ifScalar(mActive, mRaw, 0f);

                AddListener();
            }
        }
    }
}
