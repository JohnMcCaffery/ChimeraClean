using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GridProxy;
using OpenMetaverse.Packets;
using System.Net;

namespace UtilLib {
    public abstract class Master {
        protected Proxy clientProxy;

        protected readonly InterProxyServer masterServer = new InterProxyServer();

        private string proxyLoginURI;

        private string proxyAddress;

        private bool proxyLoggedIn;

        private bool proxyStarted;
        private int proxyPort;

        /// <summary>
        /// Triggered whenever a client logs in to the proxy.
        /// </summary>
        public event EventHandler OnClientLoggedIn;

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
        /// Triggered whenever the client proxy starts up.
        /// </summary>
        public event EventHandler OnProxyStarted;

        /// <summary>
        /// Address that clients should use to connect to the proxy.
        /// </summary>
        public string ProxyAddress {
            get { return proxyAddress; }
        }

        /// <summary>
        /// Port that clients should use to connect to the proxy.
        /// </summary>
        public int ProxyPort {
            get { return proxyPort; }
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
        /// True if the proxy is ready to receive a connection from a client.
        /// </summary>
        public bool ProxyRunning {
            get { return proxyStarted; }
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
        /// The address of the server the proxy is proxying.
        /// </summary>
        public string ProxyLoginURI {
            get { return proxyLoginURI; }
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
        /// Start a proxy so that clients can connect to this master and be shadowed.
        /// </summary>
        public void StartProxy() {
            StartProxy("http://localhost:9000", "127.0.0.1", 80808);
        }

        /// <summary>
        /// Start a proxy so that clients can connect to this master and be shadowed. Specifies the masterAddress and masterPort to start on.
        /// </summary>
        /// <param name="masterAddress">The masterAddress that clients can use to connect to this proxy.</param>
        /// <param name="masterPort">The masterPort that clients can use to connect to this proxy.</param>
        public void StartProxy(string loginURI, string clientAddress, int clientPort) {
            if (clientProxy != null)
                clientProxy.Stop();
            string file = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            proxyLoginURI = loginURI;
            proxyPort = clientPort;
            proxyAddress = clientAddress;

            string portArg = "--proxy-login-masterPort=" + clientPort;
            string listenIPArg = "--proxy-proxyAddress-facing-masterAddress=" + clientAddress;
            string loginURIArg = "--proxy-remote-login-uri=" + loginURI;
            string[] args = { portArg, listenIPArg, loginURIArg };
            ProxyConfig config = new ProxyConfig("Routing God", "jm726@st-andrews.ac.uk", args);
            clientProxy = new Proxy(config);
            clientProxy.AddLoginResponseDelegate(response => {
                proxyLoggedIn = true;
                if (OnClientLoggedIn != null)
                    OnClientLoggedIn(clientProxy, null);
                return response;
            });
            foreach (PacketType pt in Enum.GetValues(typeof(PacketType))) {
                clientProxy.AddDelegate(pt, Direction.Incoming, ReceiveIncomingPacket);
                clientProxy.AddDelegate(pt, Direction.Outgoing, ReceiveOutgoingPacket);
            }

            clientProxy.Start();
            proxyLoggedIn = true;

            if (OnProxyStarted != null)
                OnProxyStarted(clientProxy, null);
        }

        /// <summary>
        /// Stop the master, closing any ports it had opened to listen.
        /// </summary>
        public void Stop() {
            if (ProxyRunning)
                clientProxy.Stop();
            if (MasterRunning)
                masterServer.Stop();
        }

        /// <summary>
        /// Called whenever a packet is received from the server.
        /// </summary>
        /// <param name="p">The packet that was received.</param>
        /// <param name="ep">The end point the packet was received from.</param>
        /// <returns>The packet which is to be forwarded on to the client.</returns>
        protected abstract Packet ReceiveIncomingPacket(Packet p, IPEndPoint ep);

        /// <summary>
        /// Called whenever a packet is received from the client.
        /// </summary>
        /// <param name="p">The packet that was received.</param>
        /// <param name="ep">The end point the packet was received from.</param>
        /// <returns>The packet which is to be forwarded on to the server.</returns>
        protected abstract Packet ReceiveOutgoingPacket(Packet p, IPEndPoint ep);
    }
}
