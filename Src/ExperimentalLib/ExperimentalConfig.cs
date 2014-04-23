using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Config;
using log4net;

namespace Chimera.Experimental {
    public class ExperimentalConfig : ConfigFolderBase {
        public string ExperimentFile;
        public string FPSFolder;
        public string NodesFile;
        public string TargetsFile;
        public double TurnRate;
        public float MoveRate;
        public float DistanceThreshold;
        public float HeightOffset;
        public ControlMode Mode;
        public int StartWaitMS;
        public bool AutoStart;
        public bool AutoShutdown;
        public bool StartAtHome;

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

            TurnRate = Get("AvatarMovement", "TurnRate", .01, "How far the camera will turn each tick.");
            MoveRate = Get("AvatarMovement", "MoveRate", .03f, "How far the camera will move each tick.");
            DistanceThreshold = Get("AvatarMovement", "DistanceThreshold", .5f, "How far away from a target the position has to be before the target is considered hit.");
            HeightOffset = Get("AvatarMovement", "HeightOffset", 1f, "How much above the floor nodes the target is.");

            Mode = GetEnum<ControlMode>("AvatarMovement", "Mode", ControlMode.Delta, "What mode the system should be in for the run.", LogManager.GetLogger("Experiments"));
            StartWaitMS = Get("AvatarMovement", "StartWaitMS", 0, "How many MS to wait before starting the loop.");
            AutoStart = Get("AvatarMovement", "AutoStart", false, "Whether to start the loop as soon as the plugin is enabled.");
            AutoShutdown = Get("AvatarMovement", "AutoShutdown", false, "Whether to stop Chimera when the loop completes.");
            StartAtHome = Get("AvatarMovement", "StartAtHome", false, "Whether to teleport the avatar home before starting.");
        }
    }
}
