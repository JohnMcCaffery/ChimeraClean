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
using OpenMetaverse.Packets;
using System.Runtime.Remoting;
using GridProxy;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Configuration;
using OpenMetaverse;

namespace UtilLib {
    [XmlRpcUrl("http://diana.apollo.cs.st-andrews.ac.uk")]
    public interface ISlave {
        [XmlRpcMethod]
        string GetName();
        [XmlRpcMethod]
        void Ping();
        [XmlRpcMethod]
        void SetLoginResponse(string content);
    }

    public interface ISlaveProxy : ISlave, IXmlRpcProxy { }

    public class InterProxyClient {
        /// <summary>
        /// The socket on which the slave receives packets.
        /// </summary>
        private UdpClient socket = null;
        /// <summary>
        /// The name of this slave.
        /// </summary>
        private string name;
        /// <summary>
        /// How many packets the slave has received.
        /// </summary>
        private int receivedPackets = 0;
        /// <summary>
        /// The End Point of the Master server.
        /// </summary>
        private IPEndPoint masterEP;
        /// <summary>
        /// True if connected to the master.
        /// </summary>
        private bool connected = false;

        /// <summary>
        /// Create a new InterProxyClient
        /// </summary>
        public InterProxyClient() {
            socket = new UdpClient();
            socket.Client.Bind(new IPEndPoint(IPAddress.Any, 0));
            socket.BeginReceive(ReceivePacketDatagram, null);
            IPEndPoint ep = (IPEndPoint)socket.Client.LocalEndPoint;
            Logger.Log("Slave bound to " + socket.Client.LocalEndPoint + ".", Helpers.LogLevel.Info);

            Name = "Slave 1";
        }

        /// <summary>
        /// Connect a new InterProxyClient and have it automatically connect to a local server on 'port'.
        /// </summary>
        public InterProxyClient(int port) : this(Dns.GetHostName(), port) { }

        /// <summary>
        /// Create a new InterProxyClient and have it automatically connect to a local server on 'port'.
        /// </summary>
        /// <param name="address">The address of the master server to connect to.</param>
        /// <param name="port">The port on the master server to connect to.</param>
        public InterProxyClient(string address, int port) : this () {
            Connect(address, port);
        }

        /// <summary>
        /// The name this slave will show up with on the master.
        /// </summary>
        public string Name {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// How many packets this slave has received.
        /// </summary>
        public int ReceivedPackets {
            get { return receivedPackets; }
        }

        /// <summary>
        /// Whether the client is currently waiting for connections.
        /// </summary>
        public bool Running {
            get { return socket != null && socket.Client.IsBound; }
        }

        /// <summary>
        /// Whether the client is currently connected to a master server.
        /// </summary>
        public bool Connected {
            get { return connected; }
        }

        /// <summary>
        /// Triggered whenever a new packet is received.
        /// </summary>
        public event PacketDelegate OnPacketReceived;

        /// <summary>
        /// Triggered whenever the slave connects to the master.
        /// </summary>
        public event EventHandler OnConnected;

        /// <summary>
        /// Triggered whenever the slave disconnects from the master.
        /// </summary>
        public event EventHandler OnDisconnected;

        /// <summary>
        /// Triggered if the slave is unable to connect to the master.
        /// </summary>
        public event EventHandler OnConnectFailure;

        /// <summary>
        /// Triggered if the slave loses its connection to the master.
        /// </summary>
        public event EventHandler OnConnectionLost;

        /// <summary>
        /// Connect to the master server.
        /// </summary>
        /// <param name="address">The address of the master server.</param>
        /// <param name="port">The port for the master server.</param>
        public void Connect(string address, int port) {
            masterEP = null;
            try {
                foreach (var ip in Dns.GetHostEntry(address).AddressList)
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) 
                        masterEP = new IPEndPoint(ip, port);
            } catch (SocketException e) {
                Logger.Log("Slave unable to look up master address at " + address + ":" + port + "." + e.Message, Helpers.LogLevel.Info);
                masterEP = null;
                return;
            }
            if (masterEP == null) { 
                Logger.Log("Slave not able to look up master IP address found for " + address + ":" + port + ".", Helpers.LogLevel.Info);
                return;
            }
            
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(Name);
            socket.Send(bytes, bytes.Length, masterEP);
        }

        /// <summary>
        /// Connect to the a master server running locally.
        /// </summary>
        /// <param name="port">The port the master server is listening on.</param>
        public void Connect(int port) {
            Connect(Dns.GetHostName(), port);
        }

        /// <summary>
        /// Receive a UDP datagram from the server containing a packet or some control information.
        /// </summary>
        private void ReceivePacketDatagram(IAsyncResult ar) {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
            byte[] bytes = null;
            bool disposing = false;
            try {
                bytes = socket.EndReceive(ar, ref ep);
            } catch (ObjectDisposedException) {
                disposing = true;
                return;
            } catch (SocketException e) {
                if (e.Message.Equals("An existing connection was forcibly closed by the remote host")) {
                    if (!connected && masterEP != null) {
                        Logger.Log("Slave unable to connect to master at " + masterEP + ".", Helpers.LogLevel.Info);
                        if (OnConnectionLost != null)
                            OnConnectionLost(this, null);
                        masterEP = null;
                        return;
                    }
                } else 
                    throw e;

            } catch (Exception e) {
                Logger.Log("Slave had problem receiving packet. " + e.Message, Helpers.LogLevel.Info);
                return;
            } finally {
                if (socket.Client != null && !disposing)
                    socket.BeginReceive(ReceivePacketDatagram, null);
            }

            //Packet received from unknown host
            if (!ep.Equals(masterEP)) {
                Logger.Log("Slave received packet from unknown host " + ep + ".", Helpers.LogLevel.Info);
                return;
            }

            string msg = Encoding.ASCII.GetString(bytes);

            //Ping
            if (ep.Equals(masterEP) && msg.Equals(InterProxyServer.PING)) {
                socket.Send(InterProxyServer.PING_B, InterProxyServer.PING_B.Length, masterEP);
                return;
            }

            //Connect
            if (msg.Equals(Name)) {
                Logger.Log("Slave '" + Name + "' successfully connected to master at " + masterEP + ".", Helpers.LogLevel.Info);
                socket.BeginReceive(ReceivePacketDatagram, null);
                connected = true;
                if (OnConnected != null)
                    OnConnected(this, null);
                Thread pingThread = new Thread(TestDisconnect);
                pingThread.Name = "PingThread";
                pingThread.Start();
                return;
            }

            //Disconnect
            if (msg.Equals(InterProxyServer.DISCONNECT)) {
                Logger.Log("Slave disconnecting. Master signalled disconnect.", Helpers.LogLevel.Info);
                masterEP = null;
                Disconnect();
                return;
            }

            //Actual Packet
            try {
                int end = bytes.Length - 1;
                Packet packet = Packet.BuildPacket(bytes, ref end, new byte[8996]);
                
                Logger.Log("Slave received " + packet.Type + " from master at " + masterEP + ".", Helpers.LogLevel.Debug);

                if (OnPacketReceived != null)
                    OnPacketReceived(packet, null);
            } catch (Exception e) {
                Logger.Log("Slave had problem processing packet from " + ep + ". " + e.Message, Helpers.LogLevel.Info);
                return;
            }
        }

        /// <summary>
        /// Process a packet received from the server.
        /// </summary>
        private Packet ProcessPacket(Packet p, IPEndPoint ep) {
            receivedPackets++;
            try {
                if (OnPacketReceived != null)
                    OnPacketReceived(p, ep);
            } catch (Exception e) {
                Logger.Log("Slave unable to process " + p.Type + ". " + e.Message, Helpers.LogLevel.Info);
            }
            return null;
        }


        /// <summary>
        /// Disconnect the client from the server.
        /// </summary>
        public void Disconnect() {
            if (Connected) {
                IPEndPoint oldMaster = masterEP;
                connected = false;
                masterEP = null;
                if (oldMaster != null) {
                    byte[] bytes = ASCIIEncoding.ASCII.GetBytes("Disconnect");
                    socket.Send(bytes, bytes.Length, oldMaster);
                    Logger.Log("Slave disconnected from master at " + oldMaster + ".", Helpers.LogLevel.Info);
                } 
                if (OnDisconnected != null)
                    OnDisconnected(this, null);
            }
        }

        /// <summary>
        /// Stop the client, this will kill the UDP client.
        /// </summary>
        public void Stop() {
            if (socket != null && socket.Client.IsBound) {
                Disconnect();
                socket.Close();
            }
        }

        /// <summary>
        /// Test which slave has been disconnected and remove it from the list of slaves.
        /// </summary>
        private void TestDisconnect() {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
            UdpClient testClient = new UdpClient(ep);
            IPEndPoint testEP = new IPEndPoint(IPAddress.Any, 0);
            bool pingReceived = false;
            object pingLock = new object();
            AsyncCallback requestCallback = ar => {
                try {
                    byte[] data = testClient.EndReceive(ar, ref testEP);
                    pingReceived = Encoding.ASCII.GetString(data).Equals(InterProxyServer.PING);
                    lock (pingLock)
                        Monitor.PulseAll(pingLock);
                } catch (ObjectDisposedException e) {
                } catch (SocketException e) { }
            };
            while (Connected) {
                pingReceived = false;
                testClient.BeginReceive(requestCallback, null);
                testClient.Send(InterProxyServer.PING_B, InterProxyServer.PING_B.Length, masterEP);

                if (Connected) lock (pingLock)
                    Monitor.Wait(pingLock, 1000);

                if (!pingReceived) {
                    Logger.Log("Slave unable to ping master at " + masterEP + ". Assuming connection dead.", Helpers.LogLevel.Info);
                    masterEP = null;
                    Disconnect();
                }

            }
            testClient.Close();
        }
    }
}
