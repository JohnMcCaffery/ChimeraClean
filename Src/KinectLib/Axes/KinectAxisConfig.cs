using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;
using Chimera.Interfaces;
using System.IO;
using Chimera.Plugins;

namespace Chimera.Kinect.Axes {
    public class KinectAxisConfig : AxisBasedDelta.AxisConfig {
        public int RetryAttempts;
        public int InitialRetryWait;
        public float RetryWaitMultiplier;

        public KinectAxisConfig()
            : base("KinectMovement") {

        }

        protected override void InitConfig() {
            base.InitConfig();

            RetryAttempts = Get(true, "RetryAttempts", 10, "How many times to retry to get a connection to the Kinect.");
            RetryWaitMultiplier = Get(true, "RetryWaitMultiplier", 1.5f, "How long to extend the wait by each retry attempt.");
            InitialRetryWait = Get(true, "InitialRetryWaitMS", 2000, "How long to wait before retrying to connect to the Kinect on the first attempt.");
        }
    }
}
