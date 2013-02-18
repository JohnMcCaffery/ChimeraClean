/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
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
using log4net;
using OpenMetaverse.StructuredData;
using System.Collections;

namespace UtilLib {
    public class ProxyManager {
        private static readonly string proxyAddress = "127.0.0.1";

        private Process client;
        private ILog logger;
        protected Proxy clientProxy;
        private Init.Config proxyConfig;
        private bool clientLoggedIn;
        private bool proxyStarted;
        private UUID secureSessionID = UUID.Zero;
        private UUID sessionID = UUID.Zero;
        private UUID agentID = UUID.Zero;
        private string firstName = "NotLoggedIn";
        private string lastName = "NotLoggedIn";

        /// <summary>
        /// Triggered whenever the client proxy starts up.
        /// </summary>
        public event EventHandler OnProxyStarted;

        /// <summary>
        /// Triggered whenever a client logs in to the proxy.
        /// </summary>
        public event EventHandler OnClientLoggedIn;

        /// <summary>
        /// The logger to use when writing to the logs.
        /// </summary>
        protected ILog Logger {
            get { return logger; }
            set { logger = value; }
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
        /// The session ID the client received when it logged in to the proxy.
        /// </summary>
        public UUID SessionID {
            get { return sessionID; }
        }

        /// <summary>
        /// The secure session ID the client received when it logged in to the proxy.
        /// </summary>
        public UUID SecureSessionID {
            get { return secureSessionID; }
        }

        /// <summary>
        /// The UUID of the agent currently logged in to the proxy.
        /// </summary>
        public UUID AgentID {
            get { return agentID; }
        }

        /// <summary>
        /// The first name of the agent currently logged in to the proxy.
        /// </summary>
        public string FirstName {
            get { return firstName; }
        }

        /// <summary>
        /// The last name of the agent currently logged in to the proxy.
        /// </summary>
        public string LastName {
            get { return lastName; }
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
        /// Create a ProxyManager without specifying a configuration.
        /// </summary>
        public ProxyManager() {
            proxyConfig = null;
        }
        
        /// <summary>
        /// Create a ProxyManager specifying a configuration.
        /// </summary>
        /// <param name="config">The configuration for the proxy manager.</param>
        public ProxyManager(Init.Config config, ILog logger) {
            proxyConfig = config;
            this.logger = logger;
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
            string proxyCaps = "--proxy-caps=false";
            string[] args = { portArg, listenIPArg, loginURIArg, proxyCaps };
            GridProxyConfig config = new GridProxyConfig("Routing God", "jm726@st-andrews.ac.uk", args);
            try {
                clientProxy = new Proxy(config);
                clientProxy.AddLoginResponseDelegate(response => {
                    clientLoggedIn = true;
                    Hashtable t = (Hashtable)response.Value;

                    sessionID = UUID.Parse(t["session_id"].ToString());
                    secureSessionID = UUID.Parse(t["secure_session_id"].ToString());
                    agentID = UUID.Parse(t["agent_id"].ToString());
                    firstName = t["first_name"].ToString();
                    lastName = t["last_name"].ToString();

                    lock(startLock)
                        Monitor.PulseAll(startLock);

                    new Thread(() => {
                        if (OnClientLoggedIn != null)
                            OnClientLoggedIn(clientProxy, null);
                    }).Start();
                });
                foreach (PacketType pt in Enum.GetValues(typeof(PacketType))) {
                    clientProxy.AddDelegate(pt, Direction.Incoming, ReceiveIncomingPacket);
                    clientProxy.AddDelegate(pt, Direction.Outgoing, ReceiveOutgoingPacket);
                }

                clientProxy.Start();
                proxyStarted = true;
            } catch (NullReferenceException e) {
                Logger.Info("Unable to start proxy. " + e.Message);
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
            if (proxyConfig.ViewerExecutable == null)
                throw new Exception("Unable to start client. No executable specified.");
            if (!ProxyRunning) 
                StartProxy();

            client = new Process();
            client.StartInfo.FileName = proxyConfig.ViewerExecutable;
            client.StartInfo.WorkingDirectory = proxyConfig.ViewerWorkingDirectory;
            if (proxyConfig.UseGrid)
                client.StartInfo.Arguments = "--grid " + proxyConfig.LoginGrid;
            else
                client.StartInfo.Arguments = "--loginuri http://localhost:" + proxyConfig.ProxyPort;
            if (proxyConfig.AutoLoginClient)
                client.StartInfo.Arguments += " --login " + proxyConfig.LoginFirstName + " " + proxyConfig.LoginLastName + " " + proxyConfig.LoginPassword;
            try {
                Logger.Info("Starting client '" + client.StartInfo.FileName + "' with arguments '" + client.StartInfo.Arguments + "'.");
                if (!client.Start())
                    return false;

                //lock (startLock)
                    //Monitor.Wait(startLock, 10000);

                return clientLoggedIn;

            } catch (Win32Exception e) {
                Logger.Info("Unable to start client from " + client.StartInfo.FileName + ". " + e.Message);
                return false;
            }
        }

        /// <summary>
        /// Called whenever a packet is received from the client.
        /// </summary>
        /// <param name="p">The packet that was received.</param>
        /// <param name="ep">The end point the packet was received from.</param>
        /// <returns>The packet which is to be forwarded on to the server.</returns>
        protected virtual Packet ReceiveOutgoingPacket(Packet p, IPEndPoint ep) { return p; }

        /// <summary>
        /// Called whenever a packet is received from the server.
        /// </summary>
        /// <param name="p">The packet that was received.</param>
        /// <param name="ep">The end point the packet was received from.</param>
        /// <returns>The packet which is to be forwarded on to the client.</returns>
        protected virtual Packet ReceiveIncomingPacket(Packet p, IPEndPoint ep) { return p; }

        /// <summary>
        /// Disconnect the client proxy.
        /// </summary>
        public void StopProxy() {
            if (proxyStarted)
                clientProxy.Stop();
            proxyStarted = false;
        }

        /// <summary>
        /// Stop all functionality coming from this proxy manager. Should be overriden by any inheriting classes.
        /// </summary>
        public virtual void Stop() {
            StopProxy();
        }
    }
}
