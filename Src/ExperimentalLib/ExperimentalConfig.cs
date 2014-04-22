using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Config;

namespace Chimera.Experimental {
    public class ExperimentalConfig : ConfigFolderBase {
        public string ExperimentFile;
        public string FPSFolder;
        public string NodesFile;
        public string TargetsFile;
        public double YawRate;
        public double PitchRate;
        public float MoveRate;
        public float DistanceThreshold;
        public float HeightOffset;

        public ExperimentalConfig()
            : base("Experiments") { }

        public override string Group {
            get { return "Experiments"; }
        }

        protected override void InitConfig() {
            ExperimentFile = GetFileSection("MovementTracker", "File", null, "The xml file which defines the experiment.");
            FPSFolder = GetFolderSection("MovementTracker", "FPSFolder", "FPS", "The folder where FPS results will be written to.");

            NodesFile = GetFileSection("AvatarMovement", "NodesFile", "Experiments/Cathedral.xml", "The xml file where the nodes which are potential targets for navigating to are stored.");
            TargetsFile = GetFileSection("AvatarMovement", "TargetsFile", "Experiments/CathedralRoute.xml", "The xml file where the nodes which make up a route are stored.");

            YawRate = Get("AvatarMovement", "YawRate", .01, "How far the camera will turn each tick.");
            PitchRate = Get("AvatarMovement", "PitchRate", .01, "How far the camera will pitch each tick.");
            MoveRate = Get("AvatarMovement", "MoveRate", .03f, "How far the camera will move each tick.");
            DistanceThreshold = Get("AvatarMovement", "DistanceThreshold", .5f, "How far away from a target the position has to be before the target is considered hit.");
            HeightOffset = Get("AvatarMovement", "HeightOffset", 1f, "How much above the floor nodes the target is.");
        }
    }
}
