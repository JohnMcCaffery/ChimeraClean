using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;
using OpenMetaverse;

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
