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

        public KinectAxisConfig()
            : base("KinectMovement") {
                LimitArea = Get("LimitArea", false, "If the kinect area should be limited to the specified area.");
                AreaX = Get("AreaX", 0.0f, "The X coordinate of the area center.");
                AreaY = Get("AreaY", 5.0f, "The X coordinate of the area center.");
                AreaRadius = Get("AreaRadius", 2.0f, "The radius of the area.");
                CursorSmoothing = Get("SimpleKinectCursor", "Smoothing", 5.0f, "The number of frames for cdursor smoothing.");
        }
    }
}
