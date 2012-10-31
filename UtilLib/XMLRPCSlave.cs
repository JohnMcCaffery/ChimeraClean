/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Armadillo Proxy.

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
using Nwc.XmlRpc;
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

    public class XMLRPCSlave : MarshalByRefObject, ISlave {
        private Socket socket;
        private bool pinged = false;
        private string name;
        private int unprocessedPackets = 0;
        private int processedPackets = 0;
        private int receivedPackets = 0;
        private Nwc.XmlRpc.XmlRpcResponse masterResponse = null;

        public string Name {
            get { return name; }
            set { name = value; }
        }

        public bool Pinged {
            get { return pinged; }
        }

        public Nwc.XmlRpc.XmlRpcResponse MasterResponse {
            get { return masterResponse; }
        }

        public int ProcessedPackets {
            get { return processedPackets; }
        }

        public int UnprocessedPackets {
            get { return unprocessedPackets; }
        }

        public int ReceivedPackets {
            get { return receivedPackets; }
        }

        public event PacketDelegate OnPacketReceived;
        public event Action OnPing;

        public XMLRPCSlave() {
            RemotingConfiguration.Configure(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(XMLRPCSlave), "Slave.rem", WellKnownObjectMode.Singleton);
            RemotingServices.Marshal(this, "Slave.rem");
        }

        #region ISlave Members

        public string GetName() {
            return name;
        }

        public void Ping() {
            pinged = true;
            if (OnPing != null)
                OnPing();
        }

        public void SetLoginResponse(string content) {
            masterResponse = (Nwc.XmlRpc.XmlRpcResponse)(new XmlRpcResponseDeserializer()).Deserialize(content);
        }

        #endregion

        private byte[] receiveBuffer = new byte[8192];
        private byte[] zeroBuffer = new byte[8192];
        private EndPoint remoteEndPoint = (EndPoint)new IPEndPoint(IPAddress.Any, 0);
        Proxy proxy;

        private IPAddress GetAddress(string address) {
            IPAddress ip;
            if (IPAddress.TryParse(address, out ip)) 
                return ip;

            Uri uri = new Uri(address);
            if (uri.IsLoopback)
                return IPAddress.Loopback;
            switch (uri.HostNameType) {
                case UriHostNameType.Dns: return Dns.GetHostAddresses(address)[0];
                case UriHostNameType.IPv4: return IPAddress.Parse(address);
            }
            return IPAddress.Any;
        }

        public void Connect(string masterAddress, string listeningAddress, int masterXmlRpcPort, int port, int xmlRpcPort) {
            //socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //socket.Bind(new IPEndPoint(IPAddress.Parse(listeningIP), port));
            //socket.BeginReceiveFrom(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, ref remoteEndPoint, new AsyncCallback(ProcessPacket), null);

            IMasterProxy master = XmlRpcProxyGen.Create<IMasterProxy>();
            master.Url = "http://"+masterAddress+":"+masterXmlRpcPort+"/Master.rem";

            //new Thread(() => {
                int masterPort = master.Register(name, listeningAddress, port, xmlRpcPort);

                IPAddress masterIP = GetAddress(masterAddress);
                IPAddress listenIP = GetAddress(listeningAddress);

                string serverListenIPArg = "--proxy-remote-facing-address=" + listenIP.ToString();
                string serverListenPortArg = "--proxy-remote-facing-port=" + port;
                string clientLoginPortArg = "--proxy-login-port=0"; //Never going to be used. No one will ever connect to this proxy.
                string loginURIArg = "--proxy-remote-login-uri=http://http://localhost:9000";
                string[] args = { serverListenIPArg, serverListenPortArg, clientLoginPortArg, loginURIArg };
                ProxyConfig config = new ProxyConfig("Routing God", "jm726@st-andrews.ac.uk", args);
                proxy = new Proxy(config);
                proxy.ActiveCircuit = new IPEndPoint(masterIP, masterPort);

                foreach (PacketType pt in Enum.GetValues(typeof(PacketType)))
                    proxy.AddDelegate(pt, Direction.Incoming, ProcessPacket);

                proxy.Start();
                
            //}).Start();
        }

        private Packet ProcessPacket(Packet p, IPEndPoint ep) {
            receivedPackets++;
            try {
                if (OnPacketReceived != null)
                    OnPacketReceived(p, ep);
                processedPackets++;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                unprocessedPackets++;
            }
            return null;
        }

        private void ProcessPacket(IAsyncResult ar) {
            pinged = true;
            receivedPackets++;
            try {
                int length = socket.EndReceiveFrom(ar, ref remoteEndPoint);

                int end = length - 1;
                Packet packet = Packet.BuildPacket(receiveBuffer, ref end, zeroBuffer);

                new Thread(() => {
                    if (OnPacketReceived != null)
                        OnPacketReceived(packet, null);
                }).Start();
            } catch (Exception e) {
                Console.WriteLine("\n\n" + e.Message + "\n\n");
                unprocessedPackets++;
            } finally {
                socket.BeginReceiveFrom(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, ref remoteEndPoint, new AsyncCallback(ProcessPacket), null);
            }
        }

        public void Stop() {
            if (proxy != null)
                proxy.Stop();
        }
    }
}
