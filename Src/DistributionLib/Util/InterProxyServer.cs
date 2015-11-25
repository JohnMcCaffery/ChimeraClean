/*************************************************************************
Copyright (c) 2012 John McCaffery 
This file is part of Armadillo SlaveProxy.
Routing Project is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.
Routing Project is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.
You should have received a copy of the GNU General Public License
along with Routing Project.  If not, see <http://www.gnu.org/licenses/>.
**************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using System.Threading;
using OpenMetaverse.Packets;
using System.Net.Sockets;
using System.IO;
using System.Xml;
using GridProxy;
using System.Net;
using OpenMetaverse;
using log4net;

namespace UtilLib {
    public class InterProxyServer : BackChannel {
        /// <summary>
        /// All slaves current connected, indexed by end point.
        /// </summary>
        private Dictionary<IPEndPoint, string> slaves = new Dictionary<IPEndPoint, string>();

        /// <summary>
        /// Triggered whenever a slave connected to the master. Source is the name of the slave.
        /// </summary>
        public event System.Action<string, IPEndPoint> OnSlaveConnected;

        /// <summary>
        /// Triggered whenever a slave disconnects from the master. Source is the name of the slave.
        /// </summary>
        public event System.Action<string> OnSlaveDisconnected;

        /// <summary>
        /// Bind the master so that slaves can connect to it at the specified masterAddress and masterPort.
        /// </summary>
        /// <param name="masterAddress">The masterAddress that slaves can connect to this master at.</param>
        /// <param name="masterPort">The masterPort that slaves can connect to this master on.</param>
        public InterProxyServer(int port)
            : this() {
            Start(port);
        }

        /// <summary>
        /// Bind the master on localhost with a random masterPort.
        /// </summary>
        public InterProxyServer() : base (LogManager.GetLogger("ServerLogger")) {
            AddPacketDelegate(CONNECT, HandleSlaveConnected);
            AddPacketDelegate(DISCONNECT, HandleSlaveDisconnected);
        }

        private void HandleSlaveConnected(string msg, IPEndPoint source) {
            string[] split = msg.Split(new char[] { ' ' }, 2);
            if (split.Length == 2) {
                string name = split[1];
                lock (slaves) {
                    if (slaves.Values.Contains(name)) {
                        Logger.Info("Master received connect from already registered slave '" + name + "' from " + source + "'. Rejected.");
                        Send(REJECT + " '" + name + "' already bound.", source);
                    } else {
                        Logger.Info("Master registered new slave '" + name + "' at " + source + ".");
                        slaves.Add(source, split[1]);
                        Send(name, source);
                        if (OnSlaveConnected != null)
                            OnSlaveConnected(name, source);
                    }
                }
            }
        }

        private void HandleSlaveDisconnected(string msg, IPEndPoint source) {
            DisconnectSlave(source);
        }

        protected override void ConnectionForciblyClosed() {
            List<IPEndPoint> toRemove = new List<IPEndPoint>();
            lock (slaves) {
                foreach (IPEndPoint slave in slaves.Keys)
                    if (!CheckConnection(slave, 5))
                        toRemove.Add(slave);
            }
            foreach (IPEndPoint slave in toRemove)
                DisconnectSlave(slave);
        }

        /// <summary>
        /// Send a packet to all connected slaves.
        /// </summary>
        /// <param name="packet">The packet to broadcast</param>
        public void BroadcastPacket(Packet packet) {
            byte[] bytes = GetBytes(packet);
            lock (slaves) {
                foreach (var slave in slaves.Keys)
                    Send(bytes, slave);
            }
            Logger.Debug("Master sent " + packet.Type + " packet to " + slaves.Count + " slaves.");
        }

        /// <summary>
        /// Bind the master so that slaves can connect into it.
        /// Will bind to localhost and whatever masterPort is open.
        /// </summary>
        public bool Start() {
            if (Bind(Port)) {
                Logger.Info("Master bound to " + Address + ":" + Port);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Bind a proxy so that clients can connect to this master and be shadowed. Address is localhost, masterPort is specified.
        /// </summary>
        /// <param name="masterPort">The masterPort that clients can use to connect to this proxy.</param>
        public bool Start(int port) {
            Port = port;
            return Start();
        }

        /// <summary>
        /// Remove a slave from the list of slaves connected.
        /// </summary>
        private void DisconnectSlave(IPEndPoint ep) {
            lock (slaves) {
                if (slaves.ContainsKey(ep)) {
                    string name = slaves[ep];
                    slaves.Remove(ep);
                    Logger.Info("Master saw slave '" + name + "' at " + ep + " disconnect.");
                    if (OnSlaveDisconnected != null)
                        OnSlaveDisconnected(name);
                }

            }
        }

        /// <summary>
        /// Disconnect the master server, unbinding all ports it had bound.
        /// </summary>
        public void Stop() {
            lock (slaves)
                foreach (var slave in slaves.Keys)
                    Send(DISCONNECT_B, slave);
            Unbind();
            Logger.Info("Master closed. " + slaves.Count + " slaves notified.");
            slaves.Clear();
        }
    }
}