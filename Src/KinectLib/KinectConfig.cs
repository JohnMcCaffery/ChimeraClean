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

        protected override void InitConfig() {
            AddKey(true, "KinectPosition");
            AddKey(true, "KinectPitch");
            AddKey(true, "KinectYaw");

            Position = Get(true, "KinectPosition", Vector3.Zero);
            Pitch = Get(true, "KinectPitch", 0.0);
            Yaw = Get(true, "KinectYaw", 0.0);
        }
    }
}
