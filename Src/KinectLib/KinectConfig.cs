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
using Chimera.Util;
using OpenMetaverse;
using Chimera.Config;

namespace Chimera.Kinect {
    class KinectConfig : ConfigFolderBase {
        public Vector3 Position;
        public double Pitch;
        public double Yaw;

        public override string Group {
            get { return "EyeTracker"; }
        }

        public KinectConfig()
            : base("EyeTracker") {
        }

        protected override void InitConfig() {
            Position = GetV(true, "Position", Vector3.Zero, "The position of the kinect in real world coordinates (mm).");
            Pitch = Get(true, "Pitch", 0, "The pitch of where the kinect is looking in real space.");
            Yaw = Get(true, "Yaw", 180.0, "the yaw of where the kinect is looking in real space.");
        }
    }
}
