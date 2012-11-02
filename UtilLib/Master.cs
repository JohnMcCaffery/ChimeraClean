using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GridProxy;
using OpenMetaverse.Packets;
using System.Net;

namespace UtilLib {
    public abstract class Master : ProxyManager {

        protected readonly InterProxyServer masterServer = new InterProxyServer();

        /// <summary>
        /// Triggered whenever a slave disconnects.
        /// </summary>
        public event EventHandler OnSlaveDisconnected {
            add { masterServer.OnSlaveDisconnected += value; }
            remove { masterServer.OnSlaveDisconnected -= value; }
        }

        /// <summary>
        /// Triggered whenever a slave connects.
        /// </summary>
        public event EventHandler OnSlaveConnected {
            add { masterServer.OnSlaveConnected += value; }
            remove { masterServer.OnSlaveConnected -= value; }
        }

        /// <summary>
        /// Address that slaves should use to connect to this master.
        /// </summary>
        public string MasterAddress {
            get { return masterServer.Address; }
        }

        /// <summary>
        /// Port that slaves should use to connect to this master.
        /// </summary>
        public int MasterPort {
            get { return masterServer.Port; }
        }

        /// <summary>
        /// True if the master is ready to receive connections from slaves.
        /// </summary>
        public bool MasterRunning {
            get { return masterServer != null && masterServer.Running; }
        }

        /// <summary>
        /// Number of slaves that are connected.
        /// </summary>
        public int SlaveCount {
            get { return masterServer.SlaveCount; }
        }

        /// <summary>
        /// Names of the slaves that are connected.
        /// </summary>
        public string[] Slaves {
            get { return masterServer.Slaves; }
        }

        /// <summary>
        /// Start the master so that slaves can connect into it.
        /// </summary>
        public void StartMaster() {
            masterServer.Start();
        }

        /// <summary>
        /// Start the master so that slaves can connect into it.
        /// </summary>
        /// <param name="masterPort">The masterPort that clients should connect to this master on.</param>
        public void StartMaster(int port) {
            masterServer.Start(port);
        }

        /// <summary>
        /// Stop the master, closing any ports it had opened to listen.
        /// </summary>
        public void Stop() {
            StopProxy();
            if (MasterRunning)
                masterServer.Stop();
        }
    }
}
