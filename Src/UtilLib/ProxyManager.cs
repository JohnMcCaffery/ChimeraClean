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
using GridProxyConfig = GridProxy.ProxyConfig;

namespace UtilLib {
    public abstract class ProxyManager {

        private Process client;
        protected Proxy clientProxy;
        private readonly string proxyAddress = "127.0.0.1";
        private Init.Config proxyConfig;
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
        /// The configuration used to start the proxy.
        /// </summary>
        public Init.Config ProxyConfig {
            get {
                if (proxyConfig == null)
                    proxyConfig = new Init.Config();
                return proxyConfig; 
            }
            set { proxyConfig = value; }
        }

        /// <summary>
        /// Create a ProxyManager specifying a configuration.
        /// </summary>
        /// <param name="config">The configuration for the proxy manager.</param>
        protected ProxyManager(Init.Config config) {
            proxyConfig = config;
        }

        /// <summary>
        /// Create a ProxyManager without specifying a configuration.
        /// </summary>
        protected ProxyManager() {
            proxyConfig = null;
        }

        /// <summary>
        /// Bind a proxy with the configuration specified in the passed config.
        /// </summary>
        /// <param name="config">The configuration to set up the proxy with.</param>
        /// <returns>True if the proxy was successfully started.</returns>
        public bool StartProxy(Init.Config config) {
            proxyConfig = config;
            return StartProxy();
        }

        /// <summary>
        /// Bind a proxy with the details specified in ProxyConfig.
        /// </summary>
        /// <returns>True if the proxy was successfully started.</returns>
        public bool StartProxy() {
            if (proxyConfig == null)
                throw new Exception("Unable to start proxy. No configuration specified.");
            if (proxyConfig.ProxyLoginURI == null)
                throw new Exception("Unable to start proxy. No login URI specified in the configuration.");
            if (clientProxy != null)
                clientProxy.Stop();
            string file = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            string portArg = "--proxy-login-port=" + proxyConfig.ProxyPort;
            string listenIPArg = "--proxy-proxyAddress-facing-address=" + proxyAddress;
            string loginURIArg = "--proxy-remote-login-uri=" + proxyConfig.ProxyLoginURI;
            string[] args = { portArg, listenIPArg, loginURIArg };
            GridProxyConfig config = new GridProxyConfig("Routing God", "jm726@st-andrews.ac.uk", args);
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

        private object startLock = new object();

        /// <summary>
        /// Start a client process to connect to the proxy. If the proxy is not already running it is started.
        /// </summary>
        /// <returns>True if the client was successfully started</returns>
        public bool StartClient() {
            if (proxyConfig == null)
                throw new Exception("Unable to start client. No configuration specified.");
            if (proxyConfig.ClientExecutable == null)
                throw new Exception("Unable to start client. No executable specified.");
            if (!ProxyRunning) 
                StartProxy();

            client = new Process();
            client.StartInfo.FileName = proxyConfig.ClientExecutable;
            if (proxyConfig.UseGrid)
                client.StartInfo.Arguments = "--grid " + proxyConfig.LoginGrid;
            else
                client.StartInfo.Arguments = "--loginURI http://localhost:" + proxyConfig.ProxyPort;
            if (proxyConfig.AutoLoginClient)
                client.StartInfo.Arguments += " --login " + proxyConfig.LoginFirstName + " " + proxyConfig.LoginLastName + " " + proxyConfig.LoginPassword;
            try {
                Logger.Log("Starting client '" + client.StartInfo.FileName + "' with arguments '" + client.StartInfo.Arguments + "'.", Helpers.LogLevel.Info);
                if (!client.Start())
                    return false;

                lock (startLock)
                    Monitor.Wait(startLock, 10000);

                return clientLoggedIn;

            } catch (Win32Exception e) {
                Logger.Log("Unable to start client from " + client.StartInfo.FileName + ". " + e.Message, Helpers.LogLevel.Info);
                return false;
            }
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
