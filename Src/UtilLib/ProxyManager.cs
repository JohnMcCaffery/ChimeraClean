using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GridProxy;
using OpenMetaverse.Packets;
using System.Net;
using System.Net.Sockets;
using OpenMetaverse;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;

namespace UtilLib {
    public abstract class ProxyManager {
        private Process client;
        private string clientExe = "C:\\Program Files (x86)\\Firestorm-Release\\Firestorm-Release.exe";
        private string firstName = null;
        private string lastName = null;
        private string password = null;
        private string grid = null;

        protected Proxy clientProxy;
        private readonly string proxyAddress = "127.0.0.1";
        private string proxyLoginURI = "http://diana.cs.st-andrews.ac.uk:8002";
        private int proxyPort = 8080;
        private bool clientLoggedIn;
        private bool proxyStarted;

        /// <summary>
        /// Triggered whenever the client proxy starts up.
        /// </summary>
        public event EventHandler OnProxyStarted;

        /// <summary>
        /// Triggered whenever a client logs in to the proxy.
        /// </summary>
        public event EventHandler OnClientLoggedIn;

        /// <summary>
        /// The executable that will run the client.
        /// </summary>
        public string ClientExecutable {
            get { return clientExe; }
            set { clientExe = value; }
        }

        /// <summary>
        /// The first name to login with.
        /// </summary>
        public string FirstName {
            get { return firstName; }
            set { firstName = value; }
        }

        /// <summary>
        /// The last name to login with.
        /// </summary>
        public string LastName {
            get { return lastName; }
            set { lastName = value; }
        }

        /// <summary>
        /// The password to login with.
        /// </summary>
        public string Password {
            get { return password; }
            set { password = value; }
        }

        /// <summary>
        /// The name of the grid the client should connect to.
        /// </summary>
        public string Grid {
            get { return grid; }
            set { grid = value; }
        }

        /// <summary>
        /// The address of the server the proxy is proxying.
        /// </summary>
        public string ProxyLoginURI {
            get { return proxyLoginURI; }
            set { proxyLoginURI = value; }
        }

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
            set { proxyPort = value; }
        }

        /// <summary>
        /// True if the proxy is ready to receive a connection from a client.
        /// </summary>
        public bool ProxyRunning {
            get { return proxyStarted; }
        }

        /// <summary>
        /// True if there is a client logged in to the proxy.
        /// </summary>
        public bool ClientLoggedIn {
            get { return clientLoggedIn; }
        }

        /// <summary>
        /// Bind a proxy so that clients can connect to this master and be shadowed.
        /// </summary>
        public bool StartProxy() {
            if (clientProxy != null)
                clientProxy.Stop();
            string file = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            string portArg = "--proxy-login-port=" + proxyPort;
            string listenIPArg = "--proxy-proxyAddress-facing-address=" + proxyAddress;
            string loginURIArg = "--proxy-remote-login-uri=" + proxyLoginURI;
            string[] args = { portArg, listenIPArg, loginURIArg };
            ProxyConfig config = new ProxyConfig("Routing God", "jm726@st-andrews.ac.uk", args);
            try {
                clientProxy = new Proxy(config);
                clientProxy.AddLoginResponseDelegate(response => {
                    clientLoggedIn = true;

                    lock(startLock)
                        Monitor.PulseAll(startLock);

                    if (OnClientLoggedIn != null)
                        OnClientLoggedIn(clientProxy, null);
                    return response;
                });
                foreach (PacketType pt in Enum.GetValues(typeof(PacketType))) {
                    clientProxy.AddDelegate(pt, Direction.Incoming, ReceiveIncomingPacket);
                    clientProxy.AddDelegate(pt, Direction.Outgoing, ReceiveOutgoingPacket);
                }

                clientProxy.Start();
                proxyStarted = true;
            } catch (NullReferenceException e) {
                Logger.Log("Unable to start proxy. " + e.Message, Helpers.LogLevel.Info);
                return false;
            }

            if (OnProxyStarted != null)
                OnProxyStarted(clientProxy, null);

            return proxyStarted;
        }

        /// <summary>
        /// Bind a proxy so that clients can connect to this master and be shadowed. Specifies the masterAddress and masterPort to start on.
        /// </summary>
        /// <param name="masterAddress">The masterAddress that clients can use to connect to this proxy.</param>
        /// <param name="masterPort">The masterPort that clients can use to connect to this proxy.</param>
        public bool StartProxy(string loginURI, int port) {
            proxyLoginURI = loginURI;
            proxyPort = port;
            return StartProxy();
        }

        private object startLock = new object();

        public bool StartClient() {
            if (firstName == null)
                throw new Exception("Unable to start client. No first name specified.");
            if (lastName == null)
                throw new Exception("Unable to start client. No last name specified.");
            if (password == null)
                throw new Exception("Unable to start client. No password specified.");
            client = new Process();
            client.StartInfo.FileName = clientExe;
            if (grid == null)
                client.StartInfo.Arguments = "--loginURI http://localhost:" + proxyPort;
            else
                client.StartInfo.Arguments = "--grid " + grid;
            client.StartInfo.Arguments += " --login " + firstName + " " + lastName + " " + password;
            try {
                if (!client.Start())
                    return false;

                lock (startLock)
                    Monitor.Wait(startLock, 10000);

                return clientLoggedIn;

            } catch (Win32Exception e) {
                Logger.Log("Unable to start client from " + clientExe + ". " + e.Message, Helpers.LogLevel.Info);
                return false;
            }
        }

        public bool StartClient(string clientExe, string firstName, string lastName, string password) {
            this.clientExe = clientExe;
            this.firstName = firstName;
            this.lastName = lastName;
            this.password = password;
            return StartClient();
        }

        public bool StartClient(string clientExe, string firstName, string lastName, string password, string grid) {
            this.grid = grid;
            return StartClient(clientExe, firstName, lastName, password);
        }

        /// <summary>
        /// Called whenever a packet is received from the client.
        /// </summary>
        /// <param name="p">The packet that was received.</param>
        /// <param name="ep">The end point the packet was received from.</param>
        /// <returns>The packet which is to be forwarded on to the server.</returns>
        protected abstract Packet ReceiveOutgoingPacket(Packet p, IPEndPoint ep);

        /// <summary>
        /// Called whenever a packet is received from the server.
        /// </summary>
        /// <param name="p">The packet that was received.</param>
        /// <param name="ep">The end point the packet was received from.</param>
        /// <returns>The packet which is to be forwarded on to the client.</returns>
        protected abstract Packet ReceiveIncomingPacket(Packet p, IPEndPoint ep);

        /// <summary>
        /// Disconnect the client proxy.
        /// </summary>
        public void StopProxy() {
            if (proxyStarted)
                clientProxy.Stop();
            proxyStarted = false;
        }
    }
}
