using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;
using System.Runtime.Remoting;
using System.Threading;
using OpenMetaverse.Packets;
using System.Net.Sockets;
using Nwc.XmlRpc;
using System.IO;
using System.Xml;
using GridProxy;
using System.Net;

namespace UtilLib {
    [XmlRpcUrl("http://diana.apollo.cs.st-andrews.ac.uk")]
    public interface IMaster {
        [XmlRpcMethod]
        int Register(string uri, string address, int port, int xmlrpcPort);
    }

    public interface IMasterProxy : IMaster, IXmlRpcProxy { }

    public class XMLRPCMaster : MarshalByRefObject, IMaster {
        private List<IPEndPoint> slaveEPs = new List<IPEndPoint>();
        private List<ISlave> slaves = new List<ISlave>();
        private Proxy proxy;
        //private UdpClient udpClient = new UdpClient();

        public event EventHandler OnSlaveConnected;

        public XMLRPCMaster(string listenAddress, int port) {
            RemotingConfiguration.Configure(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(XMLRPCMaster), "Master.rem", WellKnownObjectMode.Singleton);
            RemotingServices.Marshal(this, "Master.rem");

            string portArg = "--proxy-login-port=0"; //No one will ever log in
            string clientListenIPArg = "--proxy-client-facing-address="+listenAddress; //No client will ever send packets to the server so this is irrelevant
            string clientPortArg = "--proxy-client-facing-port=0"; //No client will ever send packets to the server so this is irrelevant
            string serverListenIPArg = "--proxy-server-facing-address="+listenAddress; //This is the address packets sent by this master will appear from
            string serverPortArg = "--proxy-server-facing-port="+port; //This is the port packets sent by this master will appear from
            string loginURIArg = "--proxy-remote-login-uri=http://"+listenAddress+":0"; //This proxy will never log in.
            string[] args = { portArg, clientListenIPArg, clientPortArg, serverListenIPArg, serverPortArg, loginURIArg };
            ProxyConfig config = new ProxyConfig("Routing God", "jm726@st-andrews.ac.uk", args);
            proxy = new Proxy(config);
            proxy.Start();
        }

        #region IMaster Members

        //private <IPEndPoint slaveEPs;

        public int Register(string name, string address, int port, int xmlrpcPort) {
            ISlaveProxy slave = XmlRpcProxyGen.Create<ISlaveProxy>();
            slave.Url = "http://"+address+":"+xmlrpcPort+"/Slave.rem";

            lock (slaves) {
                slaveEPs.Add(new IPEndPoint(IPAddress.Parse(address), port));
                slaves.Add(slave);
            }

            new Thread(() => {
                try {
                    slave.Ping();
                    if (OnSlaveConnected != null)
                        OnSlaveConnected(name, null);
                } catch (Exception e) {
                    Console.WriteLine(e.Message + "\n" + e.StackTrace);
                }
            }).Start();

            return proxy.ServerSoucePort;
        }

        #endregion

        public void BroadcastPacket(Packet packet) {
            foreach (var ep in slaveEPs)
                proxy.SendPacket(packet, ep, false);
        }

        public void SetLoginResponse(Nwc.XmlRpc.XmlRpcResponse response) {
            new Thread(() => {
                string res = null;
                while (res == null) {
                    try {
                        res = XmlRpcResponseSerializer.Singleton.Serialize(response);
                    } catch (Exception e) {
                        res = null;
                    }
                }
                lock (slaves) {
                    foreach (var slave in slaves) {
                        slave.SetLoginResponse(res);
                    }
                }
            }).Start();
        }

        public void Stop() {
            proxy.Stop();
        }
    }
}
