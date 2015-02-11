using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Config;

namespace DistributionLib.Config {
    public class DistributedConfig : ConfigFolderBase {
        public int Port;
        public bool AutoStartServer;
        public bool AutoStartClient;
        public string Address;
        public string ClientName;

        public override string Group {
            get { return "Distributed"; }
        }

        public DistributedConfig()
            : base("Distributed") {
        }

        protected override void InitConfig() {
            Port = Get("Port", 5000, "The port which the server will listen for connections from clients on.");
            Address = GetStr("Address", "localhost", "The address of the machine on which the server is running.");
            ClientName = GetStr("ClientName", "Client", "The name by which the client will be identified.");
            AutoStartServer = Get("AutoStartServer", false, "Whether the server should start listening on Port as soon as Chimera is launched.");
            AutoStartClient = Get("AutoStartClient", false, "Whether the client should start attempting to connect to the server on Port as soon as Chimera is launched.");
        }
    }
}
