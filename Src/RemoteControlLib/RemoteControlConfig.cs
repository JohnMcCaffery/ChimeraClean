using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Config;

namespace Chimera.RemoteControl {
    public class RemoteControlConfig : ConfigFolderBase {
        public string OpenSimExe;
        public string ClientAddress;
        public string Title;
        public int Port;

        public RemoteControlConfig()
            : base("RemoteControl") {
        }

        public override string Group {
            get { return "RemoteControl"; }
        }

        protected override void InitConfig() {
            OpenSimExe = Get(true, "OpenSimExe", "C:\\Users\\user\\Desktop\\opensim-0.7.5\\bin\\OpenSim.exe", "The executable that will run the OpenSim server.");
            ClientAddress = Get(true, "ClientAddress", "127.0.0.1", "The address of the client that is to be remote controlled.");
            Port = Get(true, "Port", 8050, "The port that the client will listen for remote control messages on.");
            Title = Get(true, "Title", "Remote Control", "The title that will appear at the top of the remote control window.");
        }
    }
}
