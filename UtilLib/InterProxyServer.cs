/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Armadillo ClientProxy.

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
using CookComputing.XmlRpc;
using System.Runtime.Remoting;
using System.Threading;
using OpenMetaverse.Packets;
using System.Net.Sockets;
using System.IO;
using System.Xml;
using GridProxy;
using System.Net;
using OpenMetaverse;

namespace UtilLib {
    [XmlRpcUrl("http://diana.apollo.cs.st-andrews.ac.uk")]
    public interface IMaster {
        [XmlRpcMethod]
        int Register(string uri, string address, int port, int xmlrpcPort);
    }

    public interface IMasterProxy : IMaster, IXmlRpcProxy { }

    public class InterProxyServer {
        /// <summary>
        /// All slaves current connected, indexed by end point.
        /// </summary>
        private Dictionary<IPEndPoint, string> slaves = new Dictionary<IPEndPoint, string>();
        /// <summary>
        /// UdpClient to send and receive packets from.
        /// </summary>
        private UdpClient socket;

        private IPEndPoint ep;

        /// <summary>
        /// Triggered whenever a slave connected to the master. Source is the name of the slave.
        /// </summary>
        public event EventHandler OnSlaveConnected;

        /// <summary>
        /// Triggered whenever a slave disconnects from the master. Source is the name of the slave.
        /// </summary>
        public event EventHandler OnSlaveDisconnected;
        public readonly static string DISCONNECT = "Disconnect";
        public readonly static string PING = "Ping";

        /// <summary>
        /// Start the master so that slaves can connect to it at the specified address and port.
        /// </summary>
        /// <param name="address">The address that slaves can connect to this master at.</param>
        /// <param name="port">The port that slaves can connect to this master on.</param>
        public InterProxyServer(string address, int port) {
            Start(address, port);
        }

        /// <summary>
        /// Start the master on localhost with a random port.
        /// </summary>
        public InterProxyServer() {
            Start();
        }

        /// <summary>
        /// Start the master on localhost with a specified port.
        /// </summary>
        /// <param name="port">The port to listen for connections for slaves on.</param>
        public InterProxyServer(int port) {
            Start(port);
        }

        /// <summary>
        /// The port that slaves can use to connect to this master.
        /// </summary>
        public int Port {
            get { return ((IPEndPoint) socket.Client.LocalEndPoint).Port; }
        }

        /// <summary>
        /// The address that slaves can use to connect to this master.
        /// </summary>
        public string Address {
            get { return ((IPEndPoint) socket.Client.LocalEndPoint).Address.ToString(); }
        }

        /// <summary>
        /// True if ready to receive connections from slaves.
        /// </summary>
        public bool Running {
            get { return socket.Client.IsBound; }
        }

        /// <summary>
        /// Names of the slaves that are connected.
        /// </summary>
        public string[] Slaves {
            get { return slaves.Values.ToArray(); }
        }

        /// <summary>
        /// Number of slaves that are connected.
        /// </summary>
        public int SlaveCount {
            get { return slaves.Count; }
        }

        #region IMaster Members

        #endregion

        public void BroadcastPacket(Packet packet) {
            byte[] bytes = packet.ToBytes();
            lock (slaves) {
                foreach (var slave in slaves.Keys)
                    socket.Send(bytes, bytes.Length, slave);
            }
            Logger.Log("Sent " + packet.Type + " packet to " + slaves.Count + " slaves.", Helpers.LogLevel.Debug);
        }

        /// <summary>
        /// Start the master so that slaves can connect into it.
        /// Will bind to localhost and whatever port is open.
        /// </summary>
        public void Start() {
            Start(Dns.GetHostName(), 0);
        }

        /// <summary>
        /// Start a proxy so that clients can connect to this master and be shadowed. Address is localhost, port is specified.
        /// </summary>
        /// <param name="port">The port that clients can use to connect to this proxy.</param>
        public void Start(int port) {
            Start(Dns.GetHostName(), port);
        }

        /// <summary>
        /// Start the master so that slaves can connect to it. Specifies the address and port to start it on.
        /// </summary>
        /// <param name="address">The address that slaves can connect to this master on.</param>
        /// <param name="port">The port that slaves can connect to this master on.</param>
        public void Start(string address, int port) {
            foreach (var ip in Dns.GetHostEntry(address).AddressList)
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) 
                    ep = new IPEndPoint(ip, port);

            if (socket == null)
                socket = new UdpClient();
            if (Running)
                socket.Close();
            try {
                socket.Client.Bind(ep);
                socket.BeginReceive(PacketReceived, null);
                Logger.Log("Master bound to " + socket.Client.LocalEndPoint + ".", Helpers.LogLevel.Info);
            } catch (SocketException e) {
                Logger.Log("Unable to bind master server to '" + ep.Address + ":" + ep.Port + "'", Helpers.LogLevel.Info);
            }
        }

        /// <summary>
        /// Process incoming packets from slaves. Incoming packets are either connection requests or disconnect notifiers.
        /// </summary>
        private void PacketReceived(IAsyncResult ar) {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
            bool disposing = false;
            try {
                byte[] bytes = socket.EndReceive(ar, ref ep);
                if (slaves.ContainsKey(ep)) 
                    SlaveDisconnected(ep);
                else {
                    string name = Encoding.ASCII.GetString(bytes);
                    lock(slaves)
                        slaves.Add(ep, name);

                    Logger.Log("Slave '" + name + "' connected from " + ep + ".", Helpers.LogLevel.Info);
                    socket.Send(bytes, bytes.Length, ep);

                    if (OnSlaveConnected != null)
                        OnSlaveConnected(name, null);
                }
                if (socket.Client != null && socket.Client.IsBound && !disposing)
                    socket.BeginReceive(PacketReceived, null);
            } catch (ObjectDisposedException e) {
                disposing = true;
                return;
            } catch (SocketException e) {
                if (e.Message.Equals("An existing connection was forcibly closed by the remote host"))
                    TestDisconnect();
                else
                    throw e;
            } finally {
            if (socket.Client != null && socket.Client.IsBound)
                socket.BeginReceive(PacketReceived, null);
            }
        }

        /// <summary>
        /// Remove a slave from the list of slaves connected.
        /// </summary>
        private void SlaveDisconnected(IPEndPoint ep) {
                string name = slaves[ep];
                lock (slaves)
                    slaves.Remove(ep);
                Logger.Log("Slave '" + name + "' at " + ep + " disconnected.", Helpers.LogLevel.Info);
                if (OnSlaveDisconnected != null)
                    OnSlaveDisconnected(name, null);
        }

        /// <summary>
        /// Test which slave has been disconnected and remove it from the list of slaves.
        /// </summary>
        private void TestDisconnect() {
            IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, 0);
            UdpClient testClient = new UdpClient(ep);
            IPEndPoint testEP = new IPEndPoint(IPAddress.Any, 0);
            byte[] bytes = Encoding.ASCII.GetBytes(PING);
            List<IPEndPoint> toRemove = new List<IPEndPoint>();
            lock (slaves) {
                foreach (IPEndPoint slave in slaves.Keys) {
                    testClient.BeginReceive(ar => {
                        try {
                            testClient.EndReceive(ar, ref testEP);
                        } catch (SocketException e) {
                            if (e.Message.Equals("An existing connection was forcibly closed by the remote host"))
                                toRemove.Add(slave);
                        }
                    }, ep);
                    testClient.Send(bytes, bytes.Length, slave);
                    testClient.Send(bytes, bytes.Length, ep);
                    Thread.Sleep(500);
                }
            }
            foreach (var slave in toRemove)
                SlaveDisconnected(slave);
            testClient.Close();
        }

        /// <summary>
        /// Stop the master server, unbinding all ports it had bound.
        /// </summary>
        public void Stop() {
            byte[] close = Encoding.ASCII.GetBytes(DISCONNECT);
            lock (slaves)
                foreach (var slave in slaves.Keys)
                    socket.Send(close, close.Length, slave);
            socket.Close();
        }
    }
}
