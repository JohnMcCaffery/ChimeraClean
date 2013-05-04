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
    class KinectConfig : ConfigBase {
        public Vector3 Position;
        public double Pitch;
        public double Yaw;
        public bool Autostart;
        public bool Enabled;
        public bool EnableHead;

        public override string Group {
            get { return "Kinect"; }
        }

        protected override void InitConfig() {
            AddCommandLineKey(true, "KinectPosition");
            AddCommandLineKey(true, "KinectPitch");
            AddCommandLineKey(true, "KinectYaw");

            Position = GetV(true, "KinectPosition", Vector3.Zero, "The position of the kinect in real world coordinates (mm).");
            Pitch = Get(true, "KinectPitch", 0.0, "The pitch of where the kinect is looking in real space.");
            Yaw = Get(true, "KinectYaw", 0.0, "the yaw of where the kinect is looking in real space.");
            Autostart = Get(true, "KinectAutostart", false, "Whether to start the kinect when the system starts.");
            Enabled = Get(true, "KinectEnabled", true, "Whether to start with kinect input controlling the system.");
            EnableHead = Get(true, "KinectHeadEnabled", false, "Whether to start with the kinect mapping the user's head position into the 'eye' position for calculating views.");
        }
    }
}
