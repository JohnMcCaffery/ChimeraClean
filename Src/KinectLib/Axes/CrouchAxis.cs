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
using System.Windows.Forms;

namespace Chimera.Kinect.Axes {
    public class CrouchAxis : KinectScaledAxis {
        private Condition mActive = C.Create("CrouchActive", true);
        private Scalar mRaw;
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

        public override float KinectRawValue {
            get { return mRaw.Value; }
        }

        public override UserControl ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new KinectScaledAxisPanel(this);
                return mPanel;
            }
        }

        public CrouchAxis()
            : this(AxisBinding.NotSet) {
        }

        public CrouchAxis(AxisBinding binding)
            : base("Crouch", binding) {

            Scalar hip = Nui.y(Nui.joint(Nui.Hip_Centre));
            Scalar feet = Nui.y((Nui.joint(Nui.Ankle_Left) + Nui.joint(Nui.Ankle_Right))) / 2f;

            //How far pushing forward
            Scalar raw = feet - hip;
            Scalar anchor = Nui.smooth(Nui.magnitude(Nui.joint(Nui.Shoulder_Centre) - Nui.joint(Nui.Hip_Centre)), 50);

            mRaw = (anchor * 3) + raw;
            mRaw *= -1f;
        }
    }
}
