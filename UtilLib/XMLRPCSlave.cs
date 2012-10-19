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

        public void Connect(string masterURI, string listeningIP, int port, int xmlRpcPort) {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(new IPEndPoint(IPAddress.Parse(listeningIP), port));
            socket.BeginReceiveFrom(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, ref remoteEndPoint, new AsyncCallback(ProcessPacket), null);

            IMasterProxy master = XmlRpcProxyGen.Create<IMasterProxy>();
            master.Url = masterURI;

            new Thread(() => master.Register(name, listeningIP, port, xmlRpcPort)).Start();
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
    }
}
