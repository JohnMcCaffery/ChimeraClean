using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using OpenMetaverse.Packets;
using OpenMetaverse;
using System.Threading;
using GridProxy;

namespace UtilLib {
    public abstract class BackChannel {
        public readonly static string PING = "Ping";

        public readonly static string CONNECT = "Connect";

        public readonly static string DISCONNECT = "Disconnect";

        public readonly static byte[] PING_B = Encoding.ASCII.GetBytes(PING);

        public readonly static byte[] CONNECT_B = Encoding.ASCII.GetBytes(CONNECT);

        public readonly static byte[] DISCONNECT_B = Encoding.ASCII.GetBytes(DISCONNECT);

        /// <summary>
        /// Mapping of listeners for every different packet received.
        /// </summary>
        private readonly Dictionary<string, MessageDelegate> packetDelegates = new Dictionary<string,MessageDelegate>();

        /// <summary>
        /// UdpClient to send and receive packets from.
        /// </summary>
        private UdpClient socket;

        /// <summary>
        /// UdpClient to send ping packets which will check whether a connection is still alive.
        /// </summary>
        private UdpClient testConnectionSocket;

        /// <summary>
        /// Tracks whether the socket is currently bound.
        /// </summary>
        private bool bound;

        /// <summary>
        /// Buffer for zero coded packets.
        /// </summary>
        private byte[] zeroBuffer = new byte[8192];

        /// <summary>
        /// How many packets the slave has received.
        /// </summary>
        private int receivedPackets = 0;
        
        /// <summary>
        /// The port to bind to.
        /// </summary>
        private int port;

        /// <summary>
        /// Triggered whenever a new datagram packet is received.
        /// </summary>
        public event DataDelegate OnDataReceived;

        /// <summary>
        /// Triggered whenever a new packet is received.
        /// </summary>
        public event PacketDelegate OnPacketReceived;

        /// <summary>
        /// Triggered when a connection to a given end point is lost.
        /// </summary>
        public event Action<IPEndPoint> OnConnectionLost;

        /// <summary>
        /// Adds a listener for ping events.
        /// </summary>
        public BackChannel() {
            AddPacketDelegate(PING, (msg, source) => Send(msg, source));
        }

        /// <summary>
        /// True if the socket is bound.
        /// </summary>
        public bool Bound {
            get { return bound; }
        }

        /// <summary>
        /// The address that packets will be received on.
        /// "Not Bound" if not bound.
        /// </summary>
        public string Address {
            get { return bound ? ((IPEndPoint) socket.Client.LocalEndPoint).Address.ToString() : "Not Bound"; }
        }
        
        /// <summary>
        /// The port that packets will be received on.
        /// </summary>
        public int Port {
            get { return bound ? ((IPEndPoint) socket.Client.LocalEndPoint).Port : port; }
            set { port = bound ? Port : value; }
        }

        /// <summary>
        /// How many packets this slave has received.
        /// </summary>
        public int ReceivedPackets {
            get { return receivedPackets; }
        }

        /// <summary>
        /// Will be called whenever a send fails because a connection was forcibly closed by the remote host.
        /// </summary>
        protected abstract void ConnectionForciblyClosed();

        /// <summary>
        /// Turn a packet into an array of bytes. If necessary zerocde it. The returned array will be the correct length.
        /// </summary>
        /// <param name="packet">The packet to encode</param>
        protected byte[] GetBytes(OpenMetaverse.Packets.Packet packet) {
            byte[] bytes = packet.ToBytes();
            int length = bytes.Length;
            if (packet.Header.Zerocoded) {
                byte[] zerod = new byte[8192];
                length = Helpers.ZeroEncode(bytes, bytes.Length, zerod);
                bytes = zerod.Take(length).ToArray();
            }
            return bytes;
        }

        /// <summary>
        /// Send data to the specified end point.
        /// </summary>
        /// <param name="data">The data to send.</param>
        /// <param name="destination">The end point to send the data to.</param>
        public void Send(byte[] bytes, IPEndPoint destination) {
            try {
                socket.Send(bytes, bytes.Length, destination);
            } catch (SocketException e) {
                Logger.Log("Unable to send packet. " + e.Message, Helpers.LogLevel.Info);
            } catch (ObjectDisposedException e) {
                Logger.Log("Can't send packet through a closed socket.", Helpers.LogLevel.Info);
            }
        }

        /// <summary>
        /// Send a packet to the specified end point.
        /// </summary>
        /// <param name="packet">The packet to send.</param>
        /// <param name="destination">The end point to send the packet to.</param>
        public void Send(Packet packet, IPEndPoint destination) {
            Send(GetBytes(packet), destination);
        }

        /// <summary>
        /// Send a string to the specified end point.
        /// </summary>
        /// <param name="msg">The message to send.</param>
        /// <param name="destination">The destination to send the packet to.</param>
        public void Send(string msg, IPEndPoint destination) {
            Send(Encoding.ASCII.GetBytes(msg), destination);
        }

        /// <summary>
        /// Add a delegate which controls how packets with certain data will be handled.
        /// </summary>
        /// <param name="identifier">If the data in a packet starts with this string the delegate will be called.</param>
        /// <param name="handler">The delegate that is called if the packet data starts with identifier.</param>
        protected void AddPacketDelegate(string identifier, MessageDelegate handler) {
            packetDelegates.Add(identifier, handler);
        }

        /// <summary>
        /// Disconnect the master server, unbinding all ports it had bound.
        /// </summary>
        protected void Unbind() {
            try {
                if (bound) {
                    bound = false;
                    socket.Close();
                    testConnectionSocket.Close();
                }
            } catch (Exception e) {
            }
        }

        /// <summary>
        /// Test which slave has been disconnected and remove it from the list of slaves.
        /// </summary>
        /// <param name="ep">The end point to check the connection with.</param>
        /// <param name="count">How many more attempts to make before stopping checking.</param>
        protected bool CheckConnection(IPEndPoint ep, int count) {
            if (count == 0) 
                return false;

            IPEndPoint testEP = new IPEndPoint(IPAddress.Any, 0);
            bool pingReceived = false;
            object pingLock = new object();
            if (testConnectionSocket.Client != null)
                try {
                    testConnectionSocket.BeginReceive(ar => {
                        try {
                            byte[] data = testConnectionSocket.EndReceive(ar, ref testEP);
                            pingReceived = Encoding.ASCII.GetString(data).Equals(InterProxyServer.PING);
                            lock (pingLock)
                                Monitor.PulseAll(pingLock);
                        } catch (ObjectDisposedException e) {
                        } catch (SocketException e) { }
                    }, ep);
                } catch (ObjectDisposedException e) {
                    Logger.DebugLog("BackChannel unable to test connection. TestConnectionSocket disposed.");
                }
            testConnectionSocket.Send(PING_B, PING_B.Length, ep);
            lock (pingLock)
                Monitor.Wait(pingLock, 1000);

            if (pingReceived)
                return true;
            else
                return CheckConnection(ep, --count);
        }

        /// <summary>
        /// Process incoming packets from slaves. Incoming packets are either connection requests or disconnect notifiers.
        /// </summary>
        private void PacketReceived(IAsyncResult ar) {
            if (socket == null)
                return;
            IPEndPoint source = new IPEndPoint(IPAddress.Any, 0);
            bool disposing = false;
            try {
                byte[] bytes = socket.EndReceive(ar, ref source);
                if (OnDataReceived != null)
                    OnDataReceived(bytes, bytes.Length, source);
                string msg = Encoding.ASCII.GetString(bytes);
                string key = packetDelegates.Keys.SingleOrDefault(str => msg.StartsWith(str));
                if (key != null)
                    packetDelegates[key](msg, source);
                else
                    ProcessPacket(bytes, source);
            } catch (ObjectDisposedException e) {
                disposing = true;
                return;
            } catch (SocketException e) {
                if (e.Message.Equals("An existing connection was forcibly closed by the remote host"))
                    ConnectionForciblyClosed();
                else
                    throw e;
            } finally {
                if (!disposing && socket.Client != null && socket.Client.IsBound)
                    socket.BeginReceive(PacketReceived, null);
            }
        }

        private void ProcessPacket(byte[] bytes, IPEndPoint source) {
            try {
                int length = bytes.Length - 1;
                Packet p = Packet.BuildPacket(bytes, ref length, zeroBuffer);
                Logger.Log("Received " + p.Type + " packet from " + source + ".", Helpers.LogLevel.Debug);
                try {
                    receivedPackets++;
                    if (OnPacketReceived != null)
                        OnPacketReceived(p, source);
                } catch (Exception e) {
                    Logger.Log("Problem in packet received delegate. " + e.Message + "\n" + e.StackTrace, OpenMetaverse.Helpers.LogLevel.Info);
                }
            } catch (Exception e) {
                Logger.Log("Problem unpacking packet from " + source + ". " + e.Message, OpenMetaverse.Helpers.LogLevel.Info);
            }
        }

        protected bool Bind() {
            return Bind(port);
        }

        /// <summary>
        /// Bind the UDP socket to a specific port.
        /// </summary>
        /// <param name="port">The port to bind the socket to.</param>
        protected bool Bind(int port) {
            try {
                socket = new UdpClient(port);
                testConnectionSocket = new UdpClient(0);
                socket.BeginReceive(PacketReceived, null);
                bound = true;
                return true;
            } catch (SocketException e) {
                return false;
            }
        }
    }

    /// <summary>
    /// Callback for a matching message is received.
    /// </summary>
    /// 
    /// <param name="msg">The message received, as a string.</param>
    /// <param name="source">The source of the data.</param>
    public delegate void MessageDelegate(string msg, IPEndPoint source);


    /// <summary>
    /// Callback for when a datagram packet.
    /// </summary>
    /// <param name="data">The data contained in the packet.</param>
    /// <param name="length">The length of the data.</param>
    /// <param name="source">The source of the data.</param>
    public delegate void DataDelegate(byte[] data, int length, IPEndPoint source);
}
