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
using Chimera.Config;
using OpenMetaverse;

namespace Chimera.Kinect.Axes {
     public class StandAxis : KinectScaledAxis {
            private static readonly float DZ = .3f;
            private static readonly float SCALE = .05f;
            private static readonly float BACKDZ = DZ / 5f;
            private Condition mActive;
            private Scalar mRaw;
            private Scalar mValue;
            private KinectScaledAxisPanel mPanel;
            private StandConfig mStandConfig;

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

            public StandAxis(bool forward)
                : this(forward, AxisBinding.NotSet) { }

            public StandAxis(bool forward, AxisBinding binding)
                : base("Stand" + (forward ? "X" : "Y"), binding) {

                mStandConfig = new StandConfig();
                Vector handR = Nui.joint(Nui.Hand_Right);
                Vector handL = Nui.joint(Nui.Hand_Left);

                Vector zeroPoint = Vector.Create("ZeroPoint", mStandConfig.ZeroPosition.X, mStandConfig.ZeroPosition.Y, mStandConfig.ZeroPosition.Z);
                //How far pushing forward
                Vector diff = zeroPoint - Nui.joint(Nui.Hip_Centre);
                mRaw = forward ? Nui.z(diff) : Nui.x(diff);

                //Whether the push gesture could be active
                mActive = C.And(Nui.abs(mRaw) > Deadzone.Value, Nui.abs(mRaw) < mStandConfig.Maximum);
                //mActive = C.And(mActive, Nui.y(hand) > Nui.y(elbow));
                //The value for the push gesture
                mValue = Nui.ifScalar(mActive, mRaw, 0f);
            }
        }

     public class StandConfig : ConfigFolderBase {
         public Vector3 ZeroPosition = new Vector3(0f, 0f, 5f);
         public float Maximum = 10.0f;

         public StandConfig()
             : base("StandMovement") {
         }

         public override string Group {
             get { return "StandMovement"; }
         }

         protected override void InitConfig() {
             ZeroPosition = GetV("Positions", "ZeroPosition", ZeroPosition, "The neutral position for the user to stand where they will not move. Moving from this position moves the avatar.");
             Maximum = Get("Positions", "Maximum", Maximum, "The maximum distance from the center to read values.");
         }

     }

}

