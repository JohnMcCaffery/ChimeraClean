using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;
using Chimera.Interfaces;

namespace Chimera.Kinect.Axes {
    public class KinectAxisConfig : ConfigBase {
        public KinectAxisConfig()
            : base("Movement", "Config/Kinect.ini") {
        }

        public override string Group {
            get { return "Kinect Movement"; }
        }

        protected override void InitConfig() {
            Get(false, "|X|Deadzone", .1f, "The deadzone for axis |X|.");
            Get(false, "|X|Scale", .1f, "The scale factor for axis |X|.");
        }

        public float GetDeadzone(string name) {
            return Get(false, name + "Deadzone", .1f, "");
        }

        public float GetScale(string name) {
            return Get(false, name + "Scale", .1f, "");
        }
    }
}
