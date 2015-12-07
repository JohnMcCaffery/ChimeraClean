using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;
using Chimera.Interfaces;
using System.IO;
using Chimera.Plugins;
using Chimera.Config;

namespace Chimera.Kinect.Axes {
    public class KinectAxisConfig : AxisConfig {
        public bool LimitArea;
        public float AreaX;
        public float AreaY;
        public float AreaRadius;
        public float CursorSmoothing;

        public int RetryAttempts;
        public int InitialRetryWait;
        public float RetryWaitMultiplier;

        public KinectAxisConfig()
            : base("KinectMovement") {

        }

        protected override void InitConfig() {
            base.InitConfig();
            LimitArea = Get("LimitArea", false, "If the kinect area should be limited to the specified area.");
            AreaX = Get("AreaX", 0.0f, "The X coordinate of the area center.");
            AreaY = Get("AreaY", 5.0f, "The X coordinate of the area center.");
            AreaRadius = Get("AreaRadius", 2.0f, "The radius of the area.");
            CursorSmoothing = Get("SimpleKinectCursor", "Smoothing", 5.0f, "The number of frames for cdursor smoothing.");

            //Don't know if the true is necessary or not
            RetryAttempts = Get("RetryAttempts", 10, "How many times to retry to get a connection to the Kinect.");
            RetryWaitMultiplier = Get("RetryWaitMultiplier", 1.5f, "How long to extend the wait by each retry attempt.");
            InitialRetryWait = Get("InitialRetryWaitMS", 2000, "How long to wait before retrying to connect to the Kinect on the first attempt.");
            //RetryAttempts = Get(true, "RetryAttempts", 10, "How many times to retry to get a connection to the Kinect.");
            //RetryWaitMultiplier = Get(true, "RetryWaitMultiplier", 1.5f, "How long to extend the wait by each retry attempt.");
            //InitialRetryWait = Get(true, "InitialRetryWaitMS", 2000, "How long to wait before retrying to connect to the Kinect on the first attempt.");
        }
    }
}
