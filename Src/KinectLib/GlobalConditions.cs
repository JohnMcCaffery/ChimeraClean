﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuiLibDotNet;
using C = NuiLibDotNet.Condition;
using Chimera.Kinect.Axes;
using Chimera.Kinect.GUI;

namespace Chimera.Kinect {
    public static class GlobalConditions {
        private static bool mInit;
        private static Condition sActiveConditionR;
        private static Condition sActiveConditionL;

        private static KinectAxisConfig mConfig = new KinectAxisConfig();

        public static KinectAxisConfig Cfg {
            get { return mConfig; }
        }

        private static void Init() {
            if (mConfig.LimitArea)
                Nui.SelectSkeleton(mConfig.AreaX, mConfig.AreaY, mConfig.AreaRadius);
            Nui.Init();
            Nui.SetAutoPoll(true);

            mInit = true;
            Vector hipR = Nui.joint(Nui.Hip_Right);
            Vector handR = Nui.joint(Nui.Hand_Right);
            Vector handL = Nui.joint(Nui.Hand_Left);
            Condition heightThresholdR = Nui.y(handR) > Nui.y(hipR);
            Condition heightThresholdL = Nui.y(handL) > Nui.y(hipR);
            Condition heightThreshold = C.Or(heightThresholdL, heightThresholdR);

            Scalar dist = Nui.magnitude(Nui.joint(Nui.Shoulder_Centre) - Nui.joint(Nui.Hip_Centre));
            Condition distanceThresholdR = Nui.x(handR - Nui.joint(Nui.Hip_Right)) > dist;
            Condition distanceThresholdL = Nui.x(Nui.joint(Nui.Hip_Left) - handL) > dist;
            //Condition distanceThresholdR = Nui.magnitude(handR - hipR) > dist;
            //Condition distanceThresholdL = Nui.magnitude(hipL - handL) > dist;
            Condition distanceThreshold = C.Or(distanceThresholdL, distanceThresholdR);

            sActiveConditionR = C.Or(heightThresholdR, distanceThresholdR);
            sActiveConditionL = C.Or(heightThresholdL, distanceThresholdL);
        }

        public static Condition ActiveR {
            get {
                if (!mInit)
                    Init();
                return sActiveConditionR;
            }
        }
        public static Condition ActiveL {
            get {
                if (!mInit)
                    Init();
                return sActiveConditionL;
            }
        }
    }
}
