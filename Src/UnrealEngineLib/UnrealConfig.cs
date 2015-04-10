using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Config;
using System.IO;
using Chimera;
using log4net;

namespace UnrealEngineLib {
    public class UnrealConfig : ConfigFolderBase {
        public static readonly int DEFAULT_LISTEN_PORT = 5001;
        public static readonly int DEFAULT_UNREAL_PORT = 5000;
        public static readonly string DEFAULT_UNREAL_EXE = "E:/Engines/Unreal Editor/ChimeraLinkTest/WindowsNoEditor/ChimeraLinkTest.exe";
        public static readonly string UNREAL_INITIALISED_STR = "Unreal Started";
        public static readonly string UNREAL_SHUTDOWN_STR = "Unreal Stopping";
        public static readonly string UNREAL_INITIALISED_ACK = "C# Started";
        public static readonly string SHUTDOWN_STR = "ExitUnreal";
        public static int CURRENT_PORT = DEFAULT_UNREAL_PORT;

        public string UnrealExecutable;
        public string UnrealWorkingDirectory;

        public string UnrealInitialisedAck;
        public string UnrealInitialisedStr;
        public string UnrealShutdownStr;

        public string ShutdownStr;
        public string UnrealArguments;
        public bool AutoStartUnreal;
        public bool AutoStartServer;
        public bool AutoRestartUnreal;
        public int ListenPort;
        public int UnrealPort;

        public Fill Fill = Fill.Windowed;

        public bool GetLocalID;

        public override string Group {
            get { return "Unreal"; }
        }

        public UnrealConfig(params string[] args)
            : base("Unreal", IGNORE_FRAME, args) {
        }

        public UnrealConfig(string frameName, params string[] args)
            : base("Unreal", frameName, args) {
        }

        protected override void InitConfig() {
            string folder = Environment.CurrentDirectory.Replace("\\Configs", "") + "\\";
            string file = GetFile("UnrealExe", DEFAULT_UNREAL_EXE, "The executable that runs unreal.",
                "E:/Engines/Unreal Editor/ChimeraLinkTest/WindowsNoEditor/ChimeraLinkTest.exe");

            UnrealExecutable = Path.GetFullPath(Path.Combine(folder, file));

            string defaultWD = new Uri(folder).MakeRelativeUri(new Uri(Path.GetDirectoryName(UnrealExecutable))).OriginalString;
            UnrealWorkingDirectory = GetFolder("WorkingDirectory", defaultWD, "The working directory for the unreal executable.");

            UnrealInitialisedAck = GetStr("UnrealInitialisedAck", UNREAL_INITIALISED_ACK, "The string to send to unreal to acknowledge receipt of it's startup message.");
            UnrealInitialisedStr = GetStr("UnrealInitialisedStr", UNREAL_INITIALISED_STR, "The string that will be received from Unreal when it has started up and started a socket.");
            UnrealShutdownStr = GetStr("UnrealShutdownStr", UNREAL_SHUTDOWN_STR, "The string that will be received from Unreal if it is shut down.");
            ShutdownStr = GetStr("ShutdownStr", SHUTDOWN_STR, "The string that can be sent to Unreal to request it shut down.");

            AutoStartUnreal = Get("AutoStarUnreal", false, "Whether to automatically start unreal at startup.");
            AutoStartServer = Get("AutoStarServer", false, "Whether to automatically restart the TCP server which listens for connections at startup.");

            AutoRestartUnreal = Get("AutoRestart", false, "Whether to automatically restart unreal if the process exits.");

            Fill = GetFrameEnum<Fill>("Fill", Fill.Windowed, "What mode to set the window to.", LogManager.GetLogger(Frame + "Unreal"));

            UnrealPort = GetFrame("UnrealPort", CURRENT_PORT++, "The port that unreal is listening from.");
            ListenPort = GetFrame("ListenPort", CURRENT_PORT++, "The port chimera will listen for connections from Unreal from.");

            //EnableWindowPackets = Init.Get(generalConfig, "EnableWindowPackets", true);
            //UseSetFollowCamPackets = !enableWindowPackets || Get(generalConfig, "UseSetFollowCamPackets", false);
            //ControlCamera = Init.Get(sectionConfig, "ControlCamera", true);
        }

    }
}
