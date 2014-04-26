using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Config;
using log4net;
using Chimera.OpenSim;
using System.IO;
using Chimera.OpenSim.Interfaces;
using OpenMetaverse;
using System.Threading;

namespace Chimera.Experimental {
    public class ExperimentalConfig : ConfigFolderBase, IOpensimBotConfig {
        public string ExperimentName;

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
        public bool SaveResults;

        public bool UpdateStatsGUI;

        public DateTime Timestamp;

        public string TimestampFormat = "yyyy.MM.dd-HH.mm.ss";
        public string[] OutputKeys;

        public ExperimentalConfig()
            : base("Experiments") { }

        public override string Group {
            get { return "Experiments"; }
        }

        protected override void InitConfig() {
            ExperimentName = GetStr("ExperimentName", "Experiment", "The name of the experiment. Controls the folder where the results will be written to.");

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
            SaveResults = Get("AvatarMovement", "SaveFPS", true, "Whether to save the log 'Experiments/<ExperimentName>/<Timestamp>-Mode-Frame.log'.");

            TimestampFormat = GetStr("TimestampFormat", TimestampFormat, "The format that all timestamps will be saved as. Should match second life's log's timestamps.");

            FirstName = GetSection("RecorderBot", "FirstName", "Recorder", "The first name of the bot that will be logged in to track server stats.");
            LastName = GetSection("RecorderBot", "LastName", "Bot", "The last name of the bot that will be logged in to track server stats.");
            Password = GetSection("RecorderBot", "Password", "password", "The password for the bot that will be logged in to track server stats.");

            StartLocation = GetV("RecorderBot", "StartLocation", new Vector3(128f, 128f, 24f), "Where on the island the bot should be logged in to.");
            StartIsland = GetSection("RecorderBot", "StartIsland", "Cathedral 1", "Which island the bot should log in to.");
            AutoLogin = Get("RecorderBot", "AutoLogin", false, "Whether the bot should automatically log in as soon as the plugin is enabled.");
            UpdateStatsGUI = Get("RecorderBot", "UpdateStatsGUI", false, "Whether to regularly update the Recorder's GUI with Recorder bot stats.");

            string outputKeysStr = GetSection("Recorder", "OutputKeys", "CFPS,SFPS,FT", "The columns the output table should have. Each column is separted by a comma. Valid keys are: CFPS, SFPS, FT, PingTime.");
            OutputKeys = outputKeysStr.Split(',');
        }

        public void SetupFPSLogs(Core core, string specific, ILog logger) {
            Timestamp = DateTime.Now;
            string time = Timestamp.ToString(TimestampFormat);

            string dir = Path.GetFullPath(Path.Combine("Experiments", ExperimentName));

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            foreach (var frame in core.Frames) {
                OpenSimController OSOut = frame.Output as OpenSimController;
                if (OSOut != null && OSOut.ViewerController.Started) {
                    OSOut.ViewerController.PressKey("s", true, true, true);

                    //Select the correct setting
                    OSOut.ViewerController.SendString("UserL");
                    OSOut.ViewerController.PressKey("{TAB}");
                    OSOut.ViewerController.PressKey("{TAB}");
                    OSOut.ViewerController.PressKey("{TAB}");
                    //Delete the old value
                    OSOut.ViewerController.PressKey("{DEL}");

                    //Set the filename
                    string file = Path.Combine(dir, time + "-" + specific + frame.Name + ".log");
                    OSOut.ViewerController.SendString(file);
                    logger.Info("Saving viewer log to " + file + ".");

                    //Save filename and close window
                    OSOut.ViewerController.PressKey("{TAB}");
                    OSOut.ViewerController.PressKey("W", true, false, false);
                }
            }
        }
        public void StopRecordingLog(Core core) {
            foreach (var frame in core.Frames) {
                OpenSimController OSOut = frame.Output as OpenSimController;
                if (OSOut != null && OSOut.ViewerController.Started) {

                    //Select the correct setting
                    OSOut.ViewerController.SendString("UserL");
                    OSOut.ViewerController.PressKey("{TAB}");
                    OSOut.ViewerController.PressKey("{TAB}");
                    OSOut.ViewerController.PressKey("{TAB}");
                    //Delete the test filename
                    OSOut.ViewerController.PressKey("{DEL}");
                    //Save filename and close window
                    OSOut.ViewerController.PressKey("{TAB}");
                    OSOut.ViewerController.PressKey("W", true, false, false);
                }
            }
        }


        public string FirstName {
            get;
            set;
        }

        public string LastName {
            get;
            set;
        }

        public string Password {
            get;
            set;
        }

        public OpenMetaverse.Vector3 StartLocation {
            get;
            set;
        }

        public string StartIsland {
            get;
            set;
        }

        public bool AutoLogin {
            get;
            set;
        }
    }
}
