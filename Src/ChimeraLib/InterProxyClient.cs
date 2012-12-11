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
using CookComputing.XmlRpc;
using OpenMetaverse.Packets;
using System.Runtime.Remoting;
using GridProxy;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Configuration;
using OpenMetaverse;
using log4net;
using System.Net.NetworkInformation;

namespace UtilLib {
    public class InterProxyClient : BackChannel {
        /// <summary>
        /// The name of this slave.
        /// </summary>
        private string name;
        /// <summary>
        /// The End Point of the Master server.
        /// </summary>
        private IPEndPoint masterEP;
        /// <summary>
        /// True if connected to the master.
        /// </summary>
        private bool connected = false;

        private bool rejected = false;

        private readonly object connectLock = new object();

        private readonly object testLock = new object();

        public string MasterAddress { get { return masterEP == null ? "Not Connected" : masterEP.Address.ToString(); } }

        /// <summary>
        /// Create a new InterProxyClient
        /// </summary>
        public InterProxyClient(string name) : base (LogManager.GetLogger(name)) {
            Bind();
            Logger.Info("Slave bound to " + Address + ":" + Port + ".");
            Name = name;

            AddPacketDelegate(DISCONNECT, HandleDisconnect);
            AddPacketDelegate(REJECT, RejectHandler);
            AddPacketDelegate(ACCEPT, ConnectHandler);
        }

        private void HandleDisconnect(string msg, IPEndPoint source) {
            if (source.Equals(masterEP)) {
                Logger.Info("Slave disconnected. Master at " + masterEP + " signalled disconnect.");
                connected = false;
                if (OnDisconnected != null)
                    OnDisconnected(this, null);
            }
        }

        /// <summary>
        /// The name this slave will show up with on the master.
        /// </summary>
        public string Name {
            get { return name; }
            set { 
                name = value;
                Logger = LogManager.GetLogger(Name);
            }
        }

        /// <summary>
        /// Whether the client is currently connected to a master server.
        /// </summary>
        public bool Connected {
            get { return connected; }
        }

        /// <summary>
        /// Triggered whenever the slave connects to the master.
        /// </summary>
        public event EventHandler OnConnected;

        /// <summary>
        /// Triggered whenever the slave disconnects from the master.
        /// </summary>
        public event EventHandler OnDisconnected;

        /// <summary>
        /// Called if the slave tried to connect to the master but failed.
        /// </summary>
        public event EventHandler OnUnableToConnect;

        protected override void ConnectionForciblyClosed() {
            if (connected) {
                Logger.Info("Connection to master at " + masterEP + " lost.");
                DisconnectUtil();
            } else {
                Logger.Info("Unable to send packet to master at " + masterEP + ".");
            }
        }

        /// <summary>
        /// Connect to the master server.
        /// </summary>
        /// <param name="masterAddress">The masterAddress of the master server.</param>
        /// <param name="masterPort">The masterPort for the master server.</param>
        public bool Connect(string address, int port) {
            try {
                IPAddress local = AsLocalIP(address);
                if (local != null)
                    masterEP = new IPEndPoint(local, port);
                else {
                    foreach (var ip in Dns.GetHostEntry(address).AddressList)
                        if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            masterEP = new IPEndPoint(ip, port);
                    if (IPAddress.IsLoopback(masterEP.Address))
                        masterEP.Address = GetLocal();
                }
            } catch (SocketException e) {
                Logger.Info("Slave unable to look up master address at " + address + "." + e.Message);
                if (OnUnableToConnect != null)
                    OnUnableToConnect("Slave unable to look up master address at " + address + "." + e.Message, null);
                masterEP = null;
                return false;
            }
            if (masterEP == null) { 
                Logger.Info("Slave not able to look up master IP masterAddress found for " + address + ":" + port + ".");
                if (OnUnableToConnect != null)
                    OnUnableToConnect("Slave not able to look up master IP masterAddress found for " + address + ":" + port + ".", null);
                return false;
            }

            int attempt = 1;
            rejected = false;
            while (!connected && !rejected && attempt <= 5) {
                Logger.Debug("Attempting to connect to " + masterEP + ". Attempt " + attempt + ".");
                Send(CONNECT + " " + Name, masterEP);
                lock (connectLock)
                    Monitor.Wait(connectLock, 1000);
                attempt++;
            }
            if (!connected && !rejected) {
                Logger.Info("Slave unable to connect to " + masterEP + ". No reply received.");
                if (OnUnableToConnect != null)
                    OnUnableToConnect("Unable to connect to " + masterEP + ". No reply received.", null);
            }

            return connected;
        }

        private void RejectHandler(string msg, IPEndPoint source) {
            Logger.Info("Slave '" + Name + "' unable to register with master at " + source + ". " + msg);
            if (OnUnableToConnect != null)
                OnUnableToConnect("Slave '" + Name + "' unable to register with master at " + source + ". " + msg, null);

            rejected = true;
            connected = false;
            lock (connectLock)
                Monitor.PulseAll(connectLock);
        }

        private void ConnectHandler(string msg, IPEndPoint source) {
            Logger.Info("Slave '" + Name + "' connected to master at " + source + ".");
            masterEP = source;
            connected = true;
            if (OnConnected != null)
                OnConnected(this, null);
            Thread checkThread = new Thread(TestDisconnect);
            checkThread.Name = "Client check thread.";
            checkThread.Start();
            lock (connectLock)
                Monitor.PulseAll(connectLock);
            
        }

        /// <summary>
        /// Connect to the a master server running locally.
        /// </summary>
        /// <param name="masterPort">The masterPort the master server is listening on.</param>
        public bool Connect(int port) {
            return Connect(Dns.GetHostName(), port);
        }

        /// <summary>
        /// Disconnect the client, this will kill the UDP client.
        /// </summary>
        public void Stop() {
            Disconnect();
            Unbind();
        }

        /// <summary>
        /// Test whether the master server has disappeared.
        /// </summary>
        private void TestDisconnect() {
            while (Connected) {
                if (!CheckConnection(masterEP, 5)) {
                    Logger.Info("Connection lost with master at " + masterEP + ".");
                    DisconnectUtil();
                }
                lock (testLock)
                    Monitor.Wait(testLock, 1000);
            }
        }

        /// <summary>
        /// Disconnect from the master server.
        /// The port binding will not be released.
        /// </summary>
        public void Disconnect() {
            if (connected) {
                Send(DISCONNECT_B, masterEP);
                DisconnectUtil();
                Logger.Info("Slave '" + Name + "' disconnected from master at " + masterEP);
            } else {
                Logger.Info("Slave '" + Name + "' cannot disconnect. Not currently connected.");
            }
        }

        private void DisconnectUtil() {
                connected = false;
                if (OnDisconnected != null)
                    OnDisconnected(this, null);
                lock (testLock)
                    Monitor.PulseAll(testLock);
        }
    }
}
