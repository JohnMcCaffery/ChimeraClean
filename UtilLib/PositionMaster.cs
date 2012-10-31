using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GridProxy;
using OpenMetaverse.Packets;

namespace UtilLib {
    public class CameraMaster {
        public event EventHandler OnSlaveConnected;

        public event EventHandler OnSlaveDisconnected;

        /// <summary>
        /// Triggered whenever a packet is received from the connected clientAddress.
        /// </summary>
        public event PacketDelegate OnPacketReceived;

        /// <summary>
        /// Triggered whenever a packet is received and processed from the connected clientAddress.
        /// </summary>
        public event EventHandler OnPacketProcessed;

        /// <summary>
        /// Triggered whenever a packet is created and sent to all connected slaves.
        /// </summary>
        public event EventHandler OnPacketGenerated;

        /// <summary>
        /// Triggered whenever a packet from the connected clientAddress is forwarded to all connected slaves.
        /// </summary>
        public event EventHandler OnPacketForwarded;

        /// <summary>
        /// Creates a master with no specified address and port. Address is localhost. Port will be generated when the master is started.
        /// </summary>
        public CameraMaster() {
            throw new System.NotImplementedException();
        }

        /// <param name="slavePort">Create a master with the port that slaves can use to connect to this master specified. Address is localhost. Clients can connect on localhost with a random port that is specified when the proxy is started.</param>
        public CameraMaster(int slavePort) {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Create a master with an address and port specified for clients to connect to. Slaves can connect using localhost and a random port selected when the master is started.
        /// </summary>
        /// <param name="clientAddress">The address that clients should use to connect to this proxy.</param>
        /// <param name="clientPort">The port that clients should use to connect to this proxy.</param>
        public CameraMaster(string clientAddress, int clientPort) {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Create a master with an address and port specified for clients to connect to and a port specified for slaves to connect to.
        /// </summary>
        /// <param name="clientAddress">The address that clients should use to connect to this proxy.</param>
        /// <param name="clientPort">The port that clients should use to connect to this proxy.</param>
        /// <param name="slavePort">The port that slaves should use to connect to this proxy.</param>
        public CameraMaster(string clientAddress, int clientPort, int slavePort) {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Server that will communicate with all slaves that want to run off this master.
        /// </summary>
        private InterProxyServer InterProxyServer {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        /// <summary>
        /// Proxy that can communicate with a clientAddress to get AgentUpdate information.
        /// </summary>
        public GridProxy.Proxy ClientProxy {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        /// <summary>
        /// Names of the slaves that are connected.
        /// </summary>
        public string[] Slaves {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        /// <summary>
        /// Number of slaves that are connected.
        /// </summary>
        public int SlaveCount {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        /// <summary>
        /// Port that slaves should use to connect to this master.
        /// </summary>
        public int SlavePort {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        /// <summary>
        /// Port that clients should use to connect to the proxy.
        /// </summary>
        public int ClientPort {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        /// <summary>
        /// Address that slaves should use to connect to this master.
        /// </summary>
        public string SlaveAddress {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        /// <summary>
        /// Address that clients should use to connect to the proxy.
        /// </summary>
        public string ClientAddress {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        public Rotation Rotation {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        /// <summary>
        /// How many packets have been received from the connected clientAddress.
        /// </summary>
        public int PacketsReceieved {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        /// <summary>
        /// How many packets received from the connected clientAddress were forwarded to all slaves.
        /// </summary>
        public int PacketsForwarded {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        /// <summary>
        /// How many packets were generated and sent to all connected slaves.
        /// </summary>
        public int PacketsGenerated {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        /// <summary>
        /// How many packets were recived and processed from the connected clientAddress.
        /// </summary>
        public int PacketsProcessed {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        /// <summary>
        /// True if the proxy is ready to receive a connection from a client.
        /// </summary>
        public bool ProxyRunning {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        /// <summary>
        /// True if the master is ready to receive connections from slaves.
        /// </summary>
        public bool MasterRunning {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        /// <summary>
        /// Which packet types will be forwarded.
        /// </summary>
        public HashSet<PacketType> PacketTypesToForward {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        /// <summary>
        /// Start a proxy so that clients can connect to this master and be shadowed.
        /// </summary>
        public void StartProxy() {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Start a proxy so that clients can connect to this master and be shadowed. Specifies the address and port to start on.
        /// </summary>
        /// <param name="address">The address that clients can use to connect to this proxy.</param>
        /// <param name="port">The port that clients can use to connect to this proxy.</param>
        public void StartProxy(string address, int port) {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Start the master so that slaves can connect into it.
        /// </summary>
        public void StartMaster() {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Stop the master, closing any ports it had opened to listen.
        /// </summary>
        public void Stop() {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Start the master so that slaves can connect into it.
        /// </summary>
        /// <param name="port">The port that clients should connect to this master on.</param>
        public void StartMaster(int port) {
            throw new System.NotImplementedException();
        }
    }
}
