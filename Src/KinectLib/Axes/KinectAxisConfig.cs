using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;
using Chimera.Interfaces;
using System.IO;

namespace Chimera.Kinect.Axes {
    public class KinectAxisConfig : ConfigBase {
        public KinectAxisConfig()
            : base("Movement", Path.GetFullPath("../Config/Kinect.ini"), new string[0]) {
        }

        public override string Group {
            get { return "Kinect Movement"; }
        }

        protected override void InitConfig() {
            Get(false, "Deadzone|X|", .1f, "The deadzone for axis |X|.");
            Get(false, "Scale|X|", .1f, "The scale factor for axis |X|.");
        }

        public float GetDeadzone(string name) {
            return Get(false, "Deadzone" + name, .1f, "");
        }

        public float GetScale(string name) {
            return Get(false, "Scale" + name, 1f, "");
        }
    }
}
