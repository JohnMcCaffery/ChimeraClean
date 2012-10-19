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

namespace UtilLib {
    [XmlRpcUrl("http://diana.apollo.cs.st-andrews.ac.uk")]
    public interface IMaster {
        [XmlRpcMethod]
        void Register(string uri, string address, int port, int xmlrpcPort);
    }

    public interface IMasterProxy : IMaster, IXmlRpcProxy { }

    public class XMLRPCMaster : MarshalByRefObject, IMaster {
        private List<UdpClient> slaveEPs = new List<UdpClient>();
        private List<ISlave> slaves = new List<ISlave>();

        public event EventHandler OnSlaveConnected;

        public XMLRPCMaster() {
            RemotingConfiguration.Configure(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(XMLRPCMaster), "Master.rem", WellKnownObjectMode.Singleton);
            RemotingServices.Marshal(this, "Master.rem");
        }

        #region IMaster Members

        public void Register(string name, string address, int port, int xmlrpcPort) {

            ISlaveProxy slave = XmlRpcProxyGen.Create<ISlaveProxy>();
            slave.Url = "http://"+address+":"+xmlrpcPort+"/Slave.rem";

            lock (slaves) {
                slaveEPs.Add(new UdpClient(address, port));
                slaves.Add(slave);
            }

            new Thread(() => {
                slave.Ping();
                if (OnSlaveConnected != null)
                    OnSlaveConnected(name, null);
            }).Start();
        }

        #endregion

        public void BroadcastPacket(Packet packet) {
            //new Thread(() => {
            //if (slaves.Count > 0) {
                byte[] buffer = packet.ToBytes();
                int end = buffer.Length - 1;
                lock (slaves)
                    foreach (UdpClient slave in slaveEPs)
                        slave.Send(buffer, buffer.Length);
                Packet p = Packet.BuildPacket(buffer, ref end, new byte[8192]);
            //}
            //}).Start();
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
    }
}
