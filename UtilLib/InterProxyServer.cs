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
        public readonly static byte[] PING_B = Encoding.ASCII.GetBytes(PING);

        /// <summary>
        /// Start the master so that slaves can connect to it at the specified masterAddress and masterPort.
        /// </summary>
        /// <param name="masterAddress">The masterAddress that slaves can connect to this master at.</param>
        /// <param name="masterPort">The masterPort that slaves can connect to this master on.</param>
        public InterProxyServer(string address, int port) {
            Start(address, port);
        }

        /// <summary>
        /// Start the master on localhost with a random masterPort.
        /// </summary>
        public InterProxyServer() {
            Address = Dns.GetHostName();
        }

        /// <summary>
        /// Start the master on localhost with a specified masterPort.
        /// </summary>
        /// <param name="masterPort">The masterPort to listen for connections for slaves on.</param>
        public InterProxyServer(int port) : this() {
            Port = port;
        }

        /// <summary>
        /// The masterPort that slaves can use to connect to this master.
        /// </summary>
        public int Port {
            get {
                if (ep == null)
                    Address = Dns.GetHostName();
                return ep.Port; }
            set {
                if (ep == null)
                    Address = Dns.GetHostName();
                ep.Port = value; 
            }
        }

        /// <summary>
        /// The masterAddress that slaves can use to connect to this master.
        /// </summary>
        public string Address {
            get { 
                if (ep == null)
                    Address = Dns.GetHostName();
                return ep.Address.ToString(); 
            }
            set {
                int port = ep != null ? ep.Port : 0;
                foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        ep = new IPEndPoint(ip, port);
            }
        }

        /// <summary>
        /// True if ready to receive connections from slaves.
        /// </summary>
        public bool Running {
            get { return socket != null && socket.Client != null && socket.Client.IsBound; }
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

        public void BroadcastPacket(Packet packet) {
            byte[] bytes = packet.ToBytes();
            int length = bytes.Length;
            if (packet.Header.Zerocoded) {
                byte[] zerod = new byte[8192];
                length = Helpers.ZeroEncode(bytes, bytes.Length, zerod);
                bytes = zerod;
            }
            lock (slaves) {
                foreach (var slave in slaves.Keys)
                    socket.Send(bytes, length, slave);
            }
            Logger.Log("Master sent " + packet.Type + " packet to " + slaves.Count + " slaves.", Helpers.LogLevel.Debug);
        }

        /// <summary>
        /// Start the master so that slaves can connect into it.
        /// Will bind to localhost and whatever masterPort is open.
        /// </summary>
        public void Start() {
            if (ep == null)
                Logger.Log("Master unable to start. No End Point specified.", Helpers.LogLevel.Info);
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
        /// Start a proxy so that clients can connect to this master and be shadowed. Address is localhost, masterPort is specified.
        /// </summary>
        /// <param name="masterPort">The masterPort that clients can use to connect to this proxy.</param>
        public void Start(int port) {
            Port = port;
            Start();
        }

        /// <summary>
        /// Start the master so that slaves can connect to it. Specifies the masterAddress and masterPort to start it on.
        /// </summary>
        /// <param name="masterAddress">The masterAddress that slaves can connect to this master on.</param>
        /// <param name="masterPort">The masterPort that slaves can connect to this master on.</param>
        public void Start(string address, int port) {
            Address = address;
            Port = port;
            Start();
        }

        /// <summary>
        /// Process incoming packets from slaves. Incoming packets are either connection requests or disconnect notifiers.
        /// </summary>
        private void PacketReceived(IAsyncResult ar) {
            if (socket == null)
                return;
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
            bool disposing = false;
            try {
                byte[] bytes = socket.EndReceive(ar, ref ep);
                string msg = Encoding.ASCII.GetString(bytes);
                if (msg.Equals(DISCONNECT) && slaves.ContainsKey(ep)) 
                    SlaveDisconnected(ep);
                else if (msg.Equals(PING)) {
                    socket.Send(PING_B, PING_B.Length, ep);
                } else if (!msg.Equals(DISCONNECT)) {
                    lock(slaves)
                        slaves.Add(ep, msg);

                    Logger.Log("Master saw slave '" + msg + "' connect from " + ep + ".", Helpers.LogLevel.Info);
                    socket.Send(bytes, bytes.Length, ep);

                    if (OnSlaveConnected != null)
                        OnSlaveConnected(msg, null);
                }
            } catch (ObjectDisposedException e) {
                disposing = true;
                return;
            } catch (SocketException e) {
                if (e.Message.Equals("An existing connection was forcibly closed by the remote host"))
                    TestDisconnect();
                else
                    throw e;
            } finally {
                if (!disposing && socket.Client != null && socket.Client.IsBound)
                    socket.BeginReceive(PacketReceived, null);
            }
        }

        /// <summary>
        /// Remove a slave from the list of slaves connected.
        /// </summary>
        private void SlaveDisconnected(IPEndPoint ep) {
            lock (slaves) {
                if (slaves.ContainsKey(ep)) {
                    string name = slaves[ep];
                    slaves.Remove(ep);
                    Logger.Log("Master saw slave '" + name + "' at " + ep + " disconnect.", Helpers.LogLevel.Info);
                    if (OnSlaveDisconnected != null)
                        OnSlaveDisconnected(name, null);
                }

            }
        }

        /// <summary>
        /// Test which slave has been disconnected and remove it from the list of slaves.
        /// </summary>
        private void TestDisconnect() {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
            UdpClient testClient = new UdpClient(ep);
            IPEndPoint testEP = new IPEndPoint(IPAddress.Any, 0);
            List<IPEndPoint> toRemove = new List<IPEndPoint>();
            bool pingReceived = false;
            object pingLock = new object();
            lock (slaves) {
                foreach (IPEndPoint slave in slaves.Keys) {
                    testClient.BeginReceive(ar => {
                        try {
                            byte[] data = testClient.EndReceive(ar, ref testEP);
                            pingReceived = Encoding.ASCII.GetString(data).Equals(InterProxyServer.PING);
                            lock (pingLock)
                                Monitor.PulseAll(pingLock);
                        } catch (ObjectDisposedException e) {
                        } catch (SocketException e) { }
                    }, ep);
                    testClient.Send(PING_B, PING_B.Length, slave);
                    lock (pingLock)
                        Monitor.Wait(pingLock, 1000);
                    if (!pingReceived)
                        toRemove.Add(slave);
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
            socket = null;
            slaves.Clear();
        }
    }
}
