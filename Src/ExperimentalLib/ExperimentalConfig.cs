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

        //General
        public string RunInfo;
        public bool IncludeTimestamp;
        public bool SaveResults;
        public bool OneSecMininum;
        public string TimestampFormat = "yyyy.MM.dd-HH.mm.ss";
        public string[] OutputKeys;

        //Avatar Movement
        public string NodesFile;
        public string TargetsFile;
        public string MapFile;

        public ControlMode Mode;
        public int StartWaitMS;
        public bool AutoStart;
        public bool AutoShutdown;
        public bool StartAtHome;

        public double TurnRate;
        public float MoveRate;
        public float DistanceThreshold;
        public float HeightOffset;

        //Recorder Bot
        public bool ProcessOnFinish;
        public bool TeleportToStart;
        public bool UpdateStatsGUI;

        //Movement Tracker
        public string ExperimentFile;
        public string FPSFolder;

        //Non config
        public DateTime Timestamp;
        public string IDS;

        public ExperimentalConfig()
            : base("Experiments") { }

        public override string Group {
            get { return "Experiments"; }
        }

        protected override void InitConfig() {
            //General
            ExperimentName = GetStr("ExperimentName", "Experiment", "The name of the experiment. Controls the folder where the results will be written to.");
            RunInfo = GetStr("RunInfo", Mode.ToString(), "The name of the specific run happening.");
            OneSecMininum = Get("LimitFrequency", true, "Whether to limit the maximum log frequency to one log per second. Viewer timestamps only go to the second so finer grained logging is not possible.");
            TimestampFormat = GetStr("TimestampFormat", TimestampFormat, "The format that all timestamps will be saved as. Should match second life's log's timestamps.");
            IncludeTimestamp = Get("IncludeTimestamp", true, "Whether to include a timestamp in file names when saving results.");
            ProcessOnFinish = Get("ProcessResults", false, "Whether to process the log files to  <ExperimentName>/RunInfo(-<Timestamp>).csv file when closing.");
            string outputKeysStr = GetSection("Recorder", "OutputKeys", "CFPS,SFPS,FT", "The columns the output table should have. Each column is separted by a comma. Valid keys are: CFPS, SFPS, FT, PingTime.");
            OutputKeys = outputKeysStr.Split(',');

            //Movement Tracker
            ExperimentFile = GetFileSection("MovementTracker", "File", null, "The xml file which defines the experiment.");
            FPSFolder = GetFolderSection("MovementTracker", "FPSFolder", "FPS", "The folder where FPS results will be written to.");

            //Avatar movement
            NodesFile = GetFileSection("AvatarMovement", "NodesFile", "Experiments/Cathedral.xml", "The xml file where the nodes which are potential targets for navigating to are stored.");
            TargetsFile = GetFileSection("AvatarMovement", "TargetsFile", "Experiments/CathedralRoute.xml", "The xml file where the nodes which make up a route are stored.");
            MapFile = GetFileSection("AvatarMovement", "MapFile", null, "The file where the map image one which the route is to be drawn on is stored.");

            Mode = GetEnum<ControlMode>("AvatarMovement", "Mode", ControlMode.Delta, "What mode the system should be in for the run.", LogManager.GetLogger("Experiments"));
            StartWaitMS = Get("AvatarMovement", "StartWaitMS", 0, "How many MS to wait before starting the loop.");
            AutoStart = Get("AvatarMovement", "AutoStart", false, "Whether to start the loop as soon as the plugin is enabled.");
            AutoShutdown = Get("AvatarMovement", "AutoShutdown", false, "Whether to stop Chimera when the loop completes.");
            StartAtHome = Get("AvatarMovement", "StartAtHome", false, "Whether to teleport the avatar home before starting. Overrides TeleportToStart if set.");
            SaveResults = Get("AvatarMovement", "SaveFPS", true, "Whether to save the log 'Experiments/<ExperimentName>/<Timestamp>-RunInfo(-Frame).log'.");
            TeleportToStart = Get("AvatarMovement", "TeleportToStart", false, "<CURRENTLY DOES NOT WORK> Whether to use the map dialog to teleport the avatar to the start location specified in RecorderBot / StartIsland/StartLocation. Won't work if StartAtHome is enabled.");

            TurnRate = Get("AvatarMovement", "TurnRate", .01, "How far the camera will turn each tick.");
            MoveRate = Get("AvatarMovement", "MoveRate", .03f, "How far the camera will move each tick.");
            HeightOffset = Get("AvatarMovement", "HeightOffset", 1f, "How much above the floor nodes the target is.");
            DistanceThreshold = Get("AvatarMovement", "DistanceThreshold", .5f, "How far away from a target the position has to be before the target is considered hit.");

            //Recorder Bot
            FirstName = GetSection("RecorderBot", "FirstName", "Recorder", "The first name of the bot that will be logged in to track server stats.");
            LastName = GetSection("RecorderBot", "LastName", "Bot", "The last name of the bot that will be logged in to track server stats.");
            Password = GetSection("RecorderBot", "Password", "password", "The password for the bot that will be logged in to track server stats.");

            AutoLogin = Get("RecorderBot", "AutoLogin", false, "Whether the bot should automatically log in as soon as the plugin is enabled.");
            StartLocation = GetV("RecorderBot", "StartLocation", new Vector3(128f, 128f, 24f), "Where on the island the bot should be logged in to.");
            StartIsland = GetSection("RecorderBot", "StartIsland", "Cathedral 1", "Which island the bot should log in to.");
            UpdateStatsGUI = Get("RecorderBot", "UpdateStatsGUI", false, "Whether to regularly update the Recorder's GUI with Recorder bot stats.");
        }
        internal string GetLogFileName() {
            return GetLogFileName(new CoreConfig().Frames[0]);
        }

        internal string GetLogFileName(string frameName) {
            string dir = Path.GetFullPath(Path.Combine("Experiments", ExperimentName));

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string filename = RunInfo;
            if (filename == null)
                filename = "";

            if (new CoreConfig().Frames.Length > 1)
                filename += (filename.Length == 0 ? "" : "-") + frameName;

            if (IncludeTimestamp || filename.Length == 0) {
                string time = Timestamp.ToString(TimestampFormat);
                filename = time + (filename.Length == 0 ? "" : "-") + filename;
            } 
            return Path.Combine(dir, filename + ".log");
        }

        public void SetupFPSLogs(Core core, ILog logger) {
            //string runInfo = RunInfo.Clone().ToString();
            Timestamp = DateTime.Now;

            IDS = core.Frames.Select(f => (f.Output as OpenSimController).ProxyController.SessionID).
                Aggregate("", (a, id) => a + "," + id);

            foreach (var frame in core.Frames) {
                OpenSimController OSOut = frame.Output as OpenSimController;
                if (OSOut != null && OSOut.ViewerController.Started) {
                    OSOut.ViewerController.PressKey("s", true, true, true);

                    //Select the correct setting
                    OSOut.ViewerController.SendString("UserL");
                    Thread.Sleep(500);
                    OSOut.ViewerController.PressKey("{TAB}");
                    Thread.Sleep(500);
                    OSOut.ViewerController.PressKey("{TAB}");
                    Thread.Sleep(500);
                    OSOut.ViewerController.PressKey("{TAB}");
                    //Delete the old value
                    OSOut.ViewerController.PressKey("{DEL}");

                    //Create temp file (If the new log file has the same name as the one that was originally entered logging won't work)
                    OSOut.ViewerController.SendString("Blah");
                    OSOut.ViewerController.PressKey("{ENTER}");

                    string file = GetLogFileName(frame.Name);

                    //Set the filename
                    OSOut.ViewerController.SendString(file);
                    Thread.Sleep(500);
                    logger.Info("Saving viewer log to " + file + ".");

                    //Save filename and close window
                    OSOut.ViewerController.PressKey("{ENTER}");
                    Thread.Sleep(500);
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
                    OSOut.ViewerController.PressKey("{ENTER}");
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
